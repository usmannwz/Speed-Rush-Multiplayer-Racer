using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleBehaviour;

public class PlayerController : MonoBehaviour
{
    RaceMonitor raceMonitor;
    private WheelVehicle wheelVehicle;
    CheckPointManager cpm;

    Rigidbody _rb;

    [SerializeField] AnimationCurve turnInputCurve = AnimationCurve.Linear(-1.0f, -1.0f, 1.0f, 1.0f);

    float speed = 0.0f;
    float steerAngle = 40.0f;

    bool drift;
    bool raceEnded = false;

    // Input names to read using GetAxis
    [SerializeField] string throttleInput = "Throttle";
    [SerializeField] string brakeInput = "Brake";
    [SerializeField] string turnInput = "Horizontal";
    [SerializeField] string jumpInput = "Jump";
    [SerializeField] string driftInput = "Drift";
    [SerializeField] string boostInput = "Boost";

    // Start is called before the first frame update
    void Start()
    {
        raceMonitor = FindObjectOfType<RaceMonitor>();
        wheelVehicle = GetComponent<WheelVehicle>();
        cpm = GetComponent<CheckPointManager>();
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!raceMonitor.raceStarted) return;
        if (cpm.lap == raceMonitor.totalLaps + 1 && !raceEnded)
        {
            wheelVehicle.GO(0, Random.Range(-40, 40), false, false, false);
            raceMonitor.EndRace();
            raceMonitor.raceEnded = true;
            int pos = LeaderBoard.GetPositionInt(wheelVehicle.playerNamePrefab.GetComponent<NameUIController>().carRego);
            raceMonitor.ShowResults(pos);
            raceEnded = true;
            return;
        }

        // Mesure current speed
        speed = transform.InverseTransformDirection(_rb.velocity).z * 3.6f;

        // Get all the inputs!
        float throttle = 0;
        // Accelerate & brake
        if (throttleInput != "" && throttleInput != null)
        {
            throttle = SimpleInput.GetAxis(throttleInput) /*- SimpleInput.GetAxis(brakeInput)*/;
        }
        // Boost
        bool boosting = (Input.GetAxis(boostInput) > 0.5f);
        // Turn
        float steering = turnInputCurve.Evaluate(SimpleInput.GetAxis(turnInput)) * steerAngle;
        // Dirft
        //drift = Input.GetAxis(driftInput) > 0 && _rb.velocity.sqrMagnitude > 100;
        // Jump
        bool jumping = Input.GetAxis(jumpInput) != 0;

        wheelVehicle.GO(throttle, steering, drift, jumping, boosting);
        
    }

    public void HandbrakeDown()
    {
        drift = true;
    }

    public void HandbrakeUp()
    {
        drift = false;
    }
}
