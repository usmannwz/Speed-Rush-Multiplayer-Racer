using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using VehicleBehaviour.Utils;
using VehicleBehaviour;


using Photon.Realtime;
using Photon.Pun;

public class RaceMonitor : MonoBehaviourPunCallbacks
{
    Admob admob;
    PlayerProfile playerProfile;
    public GameObject[] carsPrefab;
    public GameObject[][] colorCars = new GameObject[4][];
    public GameObject[] truckPrefabs;
    public GameObject[] superCarPrefabs;
    public GameObject[] monsterTruckPrefabs;
    public GameObject[] vanPrefabs;
    public Transform[] spawnPos;

    public GameObject[] countDownItems;

    [Header("PreRaceUI")]
    public Button exitRace;
    public Button startRaceButton;
    public Toggle toggle_autoFill;
    public GameObject waitingText;
    public Text playersCount;
    public Text playersLog;


    [Header("UI Panels")]
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject racePanel;
    public GameObject leavePanel;

    [Header("Game Over Screen")]
    public GameObject victoryHeader;
    public GameObject defeatHeader;
    public Text positionText;

    bool autoFill;

    [HideInInspector] public bool raceStarted, raceEnded;

    public int totalLaps = 1;
    int playerCarIndex;
    int playerCarColorIndex;

    private void Awake()
    {
        totalLaps = PlayerPrefs.GetInt("TotalLaps");
    }

    // Start is called before the first frame update
    void Start()
    {
        colorCars[0] = truckPrefabs;
        colorCars[1] = superCarPrefabs;
        colorCars[2] = monsterTruckPrefabs;
        colorCars[3] = vanPrefabs;

        admob = FindObjectOfType<Admob>();
        playerProfile = FindObjectOfType<PlayerProfile>();

        racePanel.SetActive(false);
        exitRace.gameObject.SetActive(true);
        startRaceButton.gameObject.SetActive(false);
        toggle_autoFill.gameObject.SetActive(false);
        waitingText.SetActive(false);
        playersCount.gameObject.SetActive(false);


        SpawnCars();
    }

    // Update is called once per frame
    void Update()
    {

        if (!raceStarted && PhotonNetwork.IsConnected) playersCount.text = ("Total Players: ") + PhotonNetwork.CurrentRoom.PlayerCount.ToString() + ("/4");

    }

    public void BeginGame()
    {
        if (autoFill)
        {
            string[] aiNames = { "Liam", "Noah", "Oliver", "Elijah", "William", "James", "Benjamin", "Lucas", "Henry", "Alexander", "Mason", "Michael" };
            int numAICars = PhotonNetwork.CurrentRoom.MaxPlayers - PhotonNetwork.CurrentRoom.PlayerCount;
            for (int i = PhotonNetwork.CurrentRoom.PlayerCount; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
            {
                Vector3 startPos = spawnPos[i].position;
                Quaternion startRot = spawnPos[i].rotation;
                int rand = Random.Range(0, carsPrefab.Length);

                object[] instanceData = new object[1];
                instanceData[0] = (string)aiNames[Random.Range(0, aiNames.Length)];

                GameObject AICar = PhotonNetwork.Instantiate(carsPrefab[rand].name, startPos, startRot, 0, instanceData);
                AICar.GetComponent<AIController>().enabled = true;
                AICar.GetComponent<WheelVehicle>().enabled = true;
                AICar.GetComponent<WheelVehicle>().networkName = (string)instanceData[0];
                AICar.GetComponent<PlayerController>().enabled = false;
            }
        }


        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("StartGame", RpcTarget.All, null);
        }
    }


    [PunRPC]
    public void StartGame()
    {

        if (PhotonNetwork.IsConnected) playerProfile.SetTotalMatches("TotalMatches_M");
        else playerProfile.SetTotalMatches("TotalMatches_S");

        racePanel.SetActive(true);
        startRaceButton.gameObject.SetActive(false);
        exitRace.gameObject.SetActive(false);
        toggle_autoFill.gameObject.SetActive(false);
        waitingText.SetActive(false);
        playersCount.gameObject.SetActive(false);
        StartCoroutine(StartCountDown());
    }

    IEnumerator StartCountDown()
    {
        yield return new WaitForSeconds(2);

        foreach (GameObject item in countDownItems)
        {
            item.SetActive(true);
            yield return new WaitForSeconds(1);
            item.SetActive(false);
        }
        raceStarted = true;
    }

    void SpawnCars()
    {
        playerCarColorIndex = PlayerPrefs.GetInt("Color");
        playerCarIndex = PlayerPrefs.GetInt("PlayerCar");
        int playerSpawnPos = Random.Range(0, spawnPos.Length);                   // Random start pos for Player Car
        Vector3 startPos = spawnPos[playerSpawnPos].position;
        Quaternion startRot = spawnPos[playerSpawnPos].rotation;
        GameObject pCar = null;

        if (PhotonNetwork.IsConnected)
        {
            startPos = spawnPos[PhotonNetwork.CurrentRoom.PlayerCount - 1].position;
            startRot = spawnPos[PhotonNetwork.CurrentRoom.PlayerCount - 1].rotation;

            if (NetworkedPlayer.localPlayerInstance == null)
            {
                pCar = PhotonNetwork.Instantiate(colorCars[playerCarIndex][playerCarColorIndex].name, startPos, startRot, 0);
            }

            if (PhotonNetwork.IsMasterClient)
            {
                startRaceButton.gameObject.SetActive(true);
                toggle_autoFill.gameObject.SetActive(true);
            }
            else
                waitingText.SetActive(true);
            playersCount.gameObject.SetActive(true);
        }
        else
        {
            pCar = Instantiate(colorCars[playerCarIndex][playerCarColorIndex]);
            pCar.transform.position = startPos;
            pCar.transform.rotation = startRot;


            foreach (Transform t in spawnPos)
            {
                if (t == spawnPos[playerSpawnPos]) continue;
                GameObject car = Instantiate(colorCars[Random.Range(0, carsPrefab.Length)][Random.Range(0, truckPrefabs.Length)]);
                car.transform.position = t.position;
                car.transform.rotation = t.rotation;
            }

            StartGame();
        }

        CameraFollow.target = pCar.GetComponent<WheelVehicle>()._rb.transform;  // Set camera target to Player Car
        pCar.GetComponent<AIController>().enabled = false;
        pCar.GetComponent<WheelVehicle>().enabled = true;
        pCar.GetComponent<PlayerController>().enabled = true;


    }


    public void EndRace()
    {
        racePanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void ShowResults(int pos)
    {
        positionText.text += pos;

        if (pos == 1)
        {
            victoryHeader.SetActive(true);
            if (PhotonNetwork.IsConnected) playerProfile.SetWins("Wins_M");
            else playerProfile.SetWins("Wins_S");
        }
        else
        {
            Debug.Log(pos);
            defeatHeader.SetActive(true);
            if (PhotonNetwork.IsConnected) playerProfile.SetDefeats("Defeats_M");
            else playerProfile.SetDefeats("Defeats_S");
        }
    }

    public void RestartRace()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        if (raceEnded)
        {
            PhotonNetwork.LeaveRoom();
            //admob.ShowInterstitial();
            SceneManager.LoadScene("Main Menu");
        }
        else leavePanel.SetActive(true);
    }

    public void ReturnHome()
    {

    }

    public void LeaveGame()
    {
        if (PhotonNetwork.IsConnected) playerProfile.SetDefeats("Defeats_M");
        else playerProfile.SetDefeats("Defeats_S");

        if(PhotonNetwork.IsConnected) PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Main Menu");
    }

    public void CloseLeavePanel() 
    {
        leavePanel.SetActive(false);
    }

    public void ToggleAutoFill(bool autoFill)
    {
        this.autoFill = autoFill;
    }

    public void OpenPausePanel()
    {
        pausePanel.SetActive(true);
    }

    public void ClosePausePanel()
    {
        pausePanel.SetActive(false);
    }
}
