using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCar : MonoBehaviour
{
    public GameObject[] cars;
    public GameObject[][] colorCars = new GameObject[4][];
    public GameObject[] trucks;
    public GameObject[] superCars;
    public GameObject[] monsterTrucks;
    public GameObject[] vans;

    public float rotateSpeed;
    int currentCar;

    // Start is called before the first frame update
    void Start()
    {
        colorCars[0] = trucks;
        colorCars[1] = superCars;
        colorCars[2] = monsterTrucks;
        colorCars[3] = vans;

        SpawnCars();

        if (PlayerPrefs.HasKey("PlayerCar"))
            currentCar = PlayerPrefs.GetInt("PlayerCar");

        this.transform.LookAt(cars[currentCar].transform.position);
    }

    public void SpawnCars()
    {
        for (int i = 0; i < cars.Length; i++)
        {

            GameObject car = null;

            for (int j = 0; j < trucks.Length; j++)
            {
                switch (i)
                {
                    case 0:
                        car = trucks[PlayerPrefs.GetInt("truckColor")];
                        break;
                    case 1:
                        car = superCars[PlayerPrefs.GetInt("superCarColor")];
                        break;
                    case 2:
                        car = monsterTrucks[PlayerPrefs.GetInt("monsterTruckColor")];
                        break;
                    case 3:
                        car = vans[PlayerPrefs.GetInt("vanColor")];
                        break;
                    default:
                        break;
                }
            }

            Vector3 pos = cars[i].transform.GetChild(0).position;
            Quaternion rot = cars[i].transform.GetChild(0).rotation;

            Destroy(cars[i].transform.GetChild(0).gameObject);

            Instantiate(car, pos, rot, cars[i].transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextCar()
    {
        currentCar++;
        if (currentCar > cars.Length - 1) currentCar = 0;

        PlayerPrefs.SetInt("PlayerCar", currentCar);

        StartCoroutine(RotateCamera(currentCar));
    }

    public void PreviousCar()
    {
        currentCar--;
        if (currentCar < 0) currentCar = cars.Length - 1;

        PlayerPrefs.SetInt("PlayerCar", currentCar);

        StartCoroutine( RotateCamera(currentCar));
    }

    IEnumerator RotateCamera(int index)
    {
        float progress = 0;

        while (progress < 1.25f)
        {
            Quaternion lookDir = Quaternion.LookRotation(cars[currentCar].transform.position - this.transform.position);
            this.transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, Time.deltaTime * rotateSpeed);

            yield return new WaitForEndOfFrame();
            progress += Time.deltaTime;

            Debug.Log(progress);
        }
    }

    public void SaveCarColor()
    {
        switch (currentCar)
        {
            case 0:
                PlayerPrefs.SetInt("truckColor", PlayerPrefs.GetInt("Color"));
                break;
            case 1:
                PlayerPrefs.SetInt("superCarColor", PlayerPrefs.GetInt("Color"));
                break;
            case 2:
                PlayerPrefs.SetInt("monsterTruckColor", PlayerPrefs.GetInt("Color"));
                break;
            case 3:
                PlayerPrefs.SetInt("vanColor", PlayerPrefs.GetInt("Color"));
                break;
        }
    }



    public void ChangeColor(int index)
    {
        PlayerPrefs.SetInt("Color", index);

        Vector3 pos = cars[currentCar].transform.GetChild(0).position;
        Quaternion rot = cars[currentCar].transform.GetChild(0).rotation;

        Destroy(cars[currentCar].transform.GetChild(0).gameObject);

        GameObject colorCar = Instantiate(colorCars[currentCar][PlayerPrefs.GetInt("Color")], pos, rot, cars[currentCar].transform);


        SaveCarColor();

        //cars[currentCar] = colorCar;
    }
}
