using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointManager : MonoBehaviour
{
    private RaceMonitor raceMonitor;

    [HideInInspector] public GameObject[] checkpoints_p;
    [HideInInspector]public GameObject cpContainerObject;

    [HideInInspector] public int lap = 0;
    [HideInInspector] public int checkpoint_pos = -1;
    int CheckPoint_m; // For Main CheckPoint counter

    [HideInInspector] public int checkPointCount_p, checkPointCount_m;
    [HideInInspector] public int lastCP_p;
    [HideInInspector] public int nextCP_m;

    [HideInInspector] public bool isColliding;

    [HideInInspector] public float timeEntered = 0;

    public Text lapCounterText;


    Vector3 mPrevPos;

    private void Awake()
    {
        cpContainerObject = GameObject.FindGameObjectWithTag("CheckPointsContainer");
        
        raceMonitor = FindObjectOfType<RaceMonitor>();
        raceMonitor.racePanel.SetActive(true);
        lapCounterText = GameObject.FindGameObjectWithTag("LapCounterText").GetComponent<Text>();
        raceMonitor.racePanel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

        checkPointCount_p = GameObject.FindGameObjectsWithTag("CheckPoint_P").Length;
        checkpoints_p = new GameObject[checkPointCount_p];

        checkPointCount_m = GameObject.FindGameObjectsWithTag("CheckPoint_m").Length;
        nextCP_m = 0;
        
        for (int i = 0; i < checkPointCount_p; i++)
        {
            checkpoints_p[i] = cpContainerObject.transform.GetChild(i).gameObject;
        }

        mPrevPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit[] hits = Physics.RaycastAll(new Ray(mPrevPos, (transform.position - mPrevPos).normalized), (transform.position - mPrevPos).magnitude);
        for (int i = 0; i < hits.Length; i++)
        {
            Collider other = hits[i].collider;

            if (other.tag == "CheckPoint_m")
            {

                if (int.Parse(other.gameObject.name) == nextCP_m)
                {

                    if (int.Parse(other.gameObject.name) == 0)
                    {
                        Debug.Log("Lap++");
                        lap++;

                        //if PhotonView.isConnected
                        // TargetRPC(UpdateLapsUI)
                        //else
                        if (gameObject.GetComponent<PlayerController>().isActiveAndEnabled) UpdateLapsUI(lap);

                        checkpoint_pos = 0;
                    }
                    nextCP_m++;

                    if (!gameObject.GetComponent<AIController>().isActiveAndEnabled)  // Turn Checkpoint Green Only if Car is Player Car
                    {
                        other.gameObject.GetComponent<CheckPoint>().TurnGreen();
                    }


                    if (int.Parse(other.gameObject.name) == checkPointCount_m - 1)
                    {
                        Debug.Log("lastCP_p Checkpoint Checked");
                        nextCP_m = 0;
                        if (!gameObject.GetComponent<AIController>().isActiveAndEnabled)
                            StartCoroutine(ResetCheckpoints(other));
                    }
                }
                Debug.Log("Checkpoint_m Triggered");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isColliding) return;
        isColliding = true;
        if (other.tag == "CheckPoint_P")
        {
            timeEntered = Time.time;

            int currCPNumber_pos = int.Parse(other.gameObject.name);

            lastCP_p = currCPNumber_pos;
            checkpoint_pos = currCPNumber_pos;

            //if (checkpoint_pos == 0) lap++;
        }

        //if(other.tag == "CheckPoint_m")
        //{

        //    if(int.Parse(other.gameObject.name) == nextCP_m)
        //    {

        //        if (int.Parse(other.gameObject.name) == 0)
        //        {
        //            Debug.Log("Lap++");
        //            lap++;

        //            //if PhotonView.isConnected
        //            // TargetRPC(UpdateLapsUI)
        //            //else
        //             if(gameObject.GetComponent<PlayerController>().isActiveAndEnabled) UpdateLapsUI(lap);

        //            checkpoint_pos = 0;
        //        }
        //        nextCP_m++;

        //        if (!gameObject.GetComponent<AIController>().isActiveAndEnabled)  // Turn Checkpoint Green Only if Car is Player Car
        //        {
        //            other.gameObject.GetComponent<CheckPoint>().TurnGreen();
        //        }


        //        if (int.Parse(other.gameObject.name) == checkPointCount_m - 1)
        //        {
        //            Debug.Log("lastCP_p Checkpoint Checked");
        //            nextCP_m = 0;
        //            if (!gameObject.GetComponent<AIController>().isActiveAndEnabled) 
        //                StartCoroutine(ResetCheckpoints(other));
        //        }
        //    }
        //    Debug.Log("Checkpoint_m Triggered");
        //}

    }

    public void UpdateLapsUI(int currentLap)
    {
        lapCounterText.text = ("Lap:") + currentLap + ("/") + PlayerPrefs.GetInt("TotalLaps");
    }



    IEnumerator ResetCheckpoints(Collider other)
    {
        yield return new WaitForSeconds(2);
        other.gameObject.GetComponent<CheckPoint>().ResetCheckPointsColor();
    }

    private void OnTriggerExit(Collider other)
    {
        if (isColliding)
            isColliding = false;
    }
}
