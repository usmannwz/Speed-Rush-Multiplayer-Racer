using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float spinSpeed;

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, spinSpeed, 0);
    }
}
