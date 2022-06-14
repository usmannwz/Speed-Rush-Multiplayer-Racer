using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private MeshRenderer rend;

    public Material greenMaterial, orangeMaterial;


    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        rend.material = orangeMaterial;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TurnGreen()
    {
        rend.material = greenMaterial;
    }

    public void TurnOrange()
    {
        rend.material = orangeMaterial;
    }

    public void ResetCheckPointsColor()
    {
        GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint_m");
        foreach (GameObject cp in checkPoints)
        {
            cp.GetComponent<MeshRenderer>().material = orangeMaterial;
        }
    }
}
