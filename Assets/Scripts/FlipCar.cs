using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlipCar : MonoBehaviour
{
    RaceMonitor raceMonitor;
    CheckPointManager cpManager;
    [HideInInspector]public Circuit circuit;

    private AIController ai;
    Rigidbody rb;
    float lastTimeChecked;

    [HideInInspector] public Button btn_ResetCar;

    private void Awake()
    {
        circuit = GameObject.FindGameObjectWithTag("Circuit").GetComponent<Circuit>();
        
        raceMonitor = FindObjectOfType<RaceMonitor>();

        raceMonitor.pausePanel.SetActive(true);
        btn_ResetCar = GameObject.FindGameObjectWithTag("ResetCarButton").GetComponent<Button>();
        raceMonitor.pausePanel.SetActive(false);

        cpManager = GetComponent<CheckPointManager>();
        ai = GetComponent<AIController>();
        rb = GetComponent<Rigidbody>();

        if (!ai.isActiveAndEnabled) btn_ResetCar.onClick.AddListener(ManualFlip);
    }


    // Update is called once per frame
    void Update()
    {
        if (!raceMonitor.raceStarted)
        {
            lastTimeChecked = Time.time;
            return;
        }

        if (cpManager.lap == raceMonitor.totalLaps + 1) return;

        if (transform.up.y > 0.45f && rb.velocity.magnitude > 1)
            lastTimeChecked = Time.time;
        if (Time.time > lastTimeChecked + 3)
            AutoFlip();
    }

    void AutoFlip()
    {
        if (ai.isActiveAndEnabled)
        {
            if (ai.currentWP == 0)
            {
                this.transform.position = circuit.wayPoints[circuit.wayPoints.Length - 1].transform.position;
            }
            else
                this.transform.position = circuit.wayPoints[ai.currentWP - 1].transform.position;

            this.transform.LookAt(ai.targetWaypoint.transform, Vector3.up);
            this.transform.position += Vector3.up;
        }
        // else
        // {
        //     this.transform.position = cpManager.checkpoints_p[cpManager.lastCP_p].transform.position;
        //     if(cpManager.checkpoint_pos == cpManager.checkPointCount_p - 1) 
        //         this.transform.LookAt(cpManager.checkpoints_p[0].transform, Vector3.up);
        //     else this.transform.LookAt(cpManager.checkpoints_p[cpManager.lastCP_p + 1].transform, Vector3.up);
        // }
    }

    public void ManualFlip(){

        this.transform.position = cpManager.checkpoints_p[cpManager.lastCP_p].transform.position;
        if(cpManager.checkpoint_pos == cpManager.checkPointCount_p - 1) 
            this.transform.LookAt(cpManager.checkpoints_p[0].transform, Vector3.up);
        else this.transform.LookAt(cpManager.checkpoints_p[cpManager.lastCP_p + 1].transform, Vector3.up);
        
        this.transform.position += Vector3.up;

        raceMonitor.pausePanel.SetActive(false);
    }
}
