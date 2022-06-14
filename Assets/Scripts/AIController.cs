using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleBehaviour;

public class AIController : MonoBehaviour
{
    [HideInInspector] public RaceMonitor raceMonitor;
    WheelVehicle wheelVehicle;
    [HideInInspector]public Circuit circuit;
    [HideInInspector]public CheckPointManager cpm;

    public float steeringSensitivity = .01f;
    [HideInInspector] public Vector3 target;
    [HideInInspector] public GameObject targetWaypoint;
    [HideInInspector] public int currentWP = 0;

    // Start is called before the first frame update
    void Start()
    {
        wheelVehicle = GetComponent<WheelVehicle>();
        raceMonitor = FindObjectOfType<RaceMonitor>();
        cpm = GetComponent<CheckPointManager>();
        circuit = GameObject.FindGameObjectWithTag("Circuit").GetComponent<Circuit>();

        target = circuit.wayPoints[currentWP].transform.position;
        targetWaypoint = circuit.wayPoints[currentWP];
    }

    // Update is called once per frame
    void Update()
    {
        if (!raceMonitor.raceStarted) return;

        if (cpm.lap == raceMonitor.totalLaps + 1)
        {
            wheelVehicle.GO(0, Random.Range(-40, 40), false, false, false);
            return;
        }
        Vector3 localTarget = wheelVehicle._rb.gameObject.transform.InverseTransformPoint(target);
        float distanceToTarget = Vector3.Distance(target, wheelVehicle._rb.gameObject.transform.position);

        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        float steer = Mathf.Clamp(targetAngle * steeringSensitivity, -40, 40) * Mathf.Sign(wheelVehicle.Speed);
        float accel = 1f;
        float brake = 0;

        wheelVehicle.GO(accel, steer, false, false, false);

        if(distanceToTarget < 7.5f)
        {
            currentWP++;
            if (currentWP >= circuit.wayPoints.Length) 
                currentWP = 0;
            target = circuit.wayPoints[currentWP].transform.position;
            targetWaypoint = circuit.wayPoints[currentWP];
        }

        if (distanceToTarget < 24 && wheelVehicle.Speed > 54) wheelVehicle.Handbrake = true;
        else wheelVehicle.Handbrake = false;
    }
}
