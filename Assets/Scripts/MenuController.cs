using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class MenuController : MonoBehaviourPunCallbacks
{
    byte maxPlayers = 4;
    bool isConnecting;
    public Text feedbackText;
    string gameVersion;

    public GameObject panel_MapSelection;
    public GameObject panel_SinglePlayersModes;
    public GameObject panel_Main;
    public GameObject panel_PlayerProfile;

    public InputField playerNameInputField;
    public Text playerNameText;
    public Text lapCountText;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PlayerPrefs.HasKey("aaa"))
        {
            PlayerPrefs.SetInt("TotalLaps", 1);

            PlayerPrefs.SetInt("aaa", 0);
        }

        if (PlayerPrefs.HasKey("PlayerName"))
        {
            playerNameInputField.text = PlayerPrefs.GetString("PlayerName");
            playerNameText.text = playerNameInputField.text;
        }
        else
        {
            playerNameText.text = "Guest" + Random.Range(1000000, 9000000);
            SetName(playerNameText.text);
        }

        lapCountText.text = PlayerPrefs.GetInt("TotalLaps").ToString();
    }

    public void MultiPlayer()
    {
        feedbackText.text = "";
        isConnecting = true;
        if (PhotonNetwork.IsConnected)
        {
            feedbackText.text += "\nJoining Room...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            feedbackText.text += "\nConnecting...";
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //public void SinglePlayer()
    //{
    //    SceneManager.LoadScene("SampleScene");
    //}

    public void SetName(string name)
    {
        PlayerPrefs.SetString("PlayerName", name);
        playerNameText.text = name;
    }

    public void OpenSinglePlayerModes()
    {
        panel_SinglePlayersModes.SetActive(!panel_SinglePlayersModes.gameObject.activeInHierarchy);
    }

    public void OpenMapsPanel()
    {
        panel_MapSelection.SetActive(true);
    }

    public void CloseMapsPanel()
    {
        panel_MapSelection.SetActive(false);
    }

    public void PlayMap(string mapName)
    {
        SceneManager.LoadScene(mapName);
    }

    public void PlayRandomMap()
    {
        int rand = Random.Range(0, SceneManager.sceneCountInBuildSettings - 3);

        SceneManager.LoadScene("Random Map " + rand);
    }

    public void IncreaseLapCount()
    {
        if (PlayerPrefs.GetInt("TotalLaps") > 3 ) return;
        PlayerPrefs.SetInt("TotalLaps", PlayerPrefs.GetInt("TotalLaps") + 1);
        lapCountText.text = PlayerPrefs.GetInt("TotalLaps").ToString();
    }

    public void DecreaseLapCount()
    {
        if (PlayerPrefs.GetInt("TotalLaps") < 2) return;
        PlayerPrefs.SetInt("TotalLaps", PlayerPrefs.GetInt("TotalLaps") - 1);
        lapCountText.text = PlayerPrefs.GetInt("TotalLaps").ToString();
    }

    public void OpenPlayerProfile()
    {
        panel_PlayerProfile.SetActive(true);
        panel_Main.SetActive(false);

        panel_SinglePlayersModes.SetActive(false);
        panel_MapSelection.SetActive(false);
    }

    public void ExitPlayerProfile()
    {
        panel_Main.SetActive(true);
        panel_PlayerProfile.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    #region Network Callbacks

    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            feedbackText.text += "\nOnConnectedToMaster";
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        feedbackText.text += "\nFailed To Join Random Room";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayers});
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        feedbackText.text += "\nDisconnected Because" + cause;
        isConnecting = false;
    }

    public override void OnJoinedRoom()
    {
        feedbackText.text += "\nJoined Room with" + PhotonNetwork.CurrentRoom.PlayerCount + "Players";
        PhotonNetwork.LoadLevel("Green Map");
    }


    #endregion
}
