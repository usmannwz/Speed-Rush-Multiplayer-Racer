using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

struct PlayerStats
{
    public string name;
    public int position;
    public float time;

    public PlayerStats(string n, int p, float t)
    {
        name = n;
        position = p;
        time = t;
    }
}


public class LeaderBoard
{
    static Dictionary<int, PlayerStats> lb = new Dictionary<int, PlayerStats>();
    static int carsRegistered = -1;


    public static void Reset()
    {
        lb.Clear();
        carsRegistered = -1;
    }

    public static int RegisterCar(string name)
    {
        carsRegistered++;
        lb.Add(carsRegistered, new PlayerStats(name, 0, 0));
        return carsRegistered;
    }

    public static void SetPosition(int rego, int lap, int nextCP_m, int checkpoint, float time)
    {
        if (nextCP_m == 0) nextCP_m = 10;
        int position = lap * 10000 + checkpoint + (nextCP_m * 300);
        lb[rego] = new PlayerStats(lb[rego].name, position, time);
    }

    public static string GetPositionString(int rego)
    {
        int index = 0;
        foreach(KeyValuePair<int, PlayerStats> pos in lb.OrderByDescending(key => key.Value.position).ThenBy(key => key.Value.time))
        {
            index++;
            if(pos.Key == rego)
            {
                switch (index)
                {
                    case 1: return "1";
                    case 2: return "2";
                    case 3: return "3";
                    case 4: return "4";
                }
            }
        }
        return "Unknown";
    }

    public static int GetPositionInt(int rego)
    {
        int index = 0;
        foreach (KeyValuePair<int, PlayerStats> pos in lb.OrderByDescending(key => key.Value.position).ThenBy(key => key.Value.time))
        {
            index++;
            if (pos.Key == rego)
            {
                return index;
            }
        }
        return 0;
    }

    public static List<string> GetPlaces()
    {
        List<string> places = new List<string>();

        foreach (KeyValuePair<int, PlayerStats> pos in lb.OrderByDescending(key => key.Value.position).ThenBy(key => key.Value.time))
        {
            places.Add(pos.Value.name);
        }
            return places;
    }
}
