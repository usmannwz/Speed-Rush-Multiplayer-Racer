using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardUI : MonoBehaviour
{
    public Text fisrt;
    public Text second;
    public Text third;
    public Text fourth;


    private void Start()
    {
        LeaderBoard.Reset();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        List<string> places = LeaderBoard.GetPlaces();
        if(places.Count > 0) fisrt.text = places[0];
        if (places.Count > 1) second.text = places[1];
        if (places.Count > 2) third.text = places[2];
        if (places.Count > 3) fourth.text = places[3];
    }
}
