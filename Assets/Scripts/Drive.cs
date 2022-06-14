using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{
    public WheelCollider[] WCs;
    public GameObject[] Wheels;
    public float torque = 200;
    public float maxSteerAngle = 30;
    public float maxBrakeTorque = 500;

    public Transform centerOfMass;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
    }

    void Go(float accel, float steer, float brake)
    {
        accel = Mathf.Clamp(accel, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
        brake = Mathf.Clamp(brake, 0, 1) * maxBrakeTorque;

        float thrustTorque = accel * torque;

        for (int i = 0; i < 4; i++)
        {
            WCs[i].motorTorque = thrustTorque;

            if (i < 2)
                WCs[i].steerAngle = steer;
            else
                WCs[i].brakeTorque = brake;

            Quaternion quat;
            Vector3 position;
            WCs[i].GetWorldPose(out position, out quat);
            Wheels[i].transform.position = position;
            Wheels[i].transform.rotation = quat;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float a = Input.GetAxis("Vertical");
        float s = Input.GetAxis("Horizontal");
        float b = Input.GetAxis("Jump");

        Go(a,s,b);
    }
}
