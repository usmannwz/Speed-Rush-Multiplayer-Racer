using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerProfile : MonoBehaviour
{
    public static PlayerProfile instance;

    public Sprite[] playerImages;
    public GameObject playerImage;

    public Text wins_S, wins_M, defeats_S, defeats_M, matches_S, matches_M;

    private void Awake()
    {
        MakeSingleton();

        if (!PlayerPrefs.HasKey("FirstTime"))
        {
            PlayerPrefs.SetInt("TotalMatches_S", 0);
            PlayerPrefs.SetInt("TotalMatches_M", 0);
            PlayerPrefs.SetInt("Wins_S", 0);
            PlayerPrefs.SetInt("Wins_M", 0);
            PlayerPrefs.SetInt("Defeats_S", 0);
            PlayerPrefs.SetInt("Defeats_M", 0);

            PlayerPrefs.SetInt("PlayerImage", 0);

            PlayerPrefs.SetInt("FirstTime", 0);
        }

        playerImage.GetComponent<Image>().sprite = playerImages[PlayerPrefs.GetInt("PlayerImage")];

        wins_S.text = PlayerPrefs.GetInt("Wins_S").ToString();
        wins_M.text = PlayerPrefs.GetInt("Wins_M").ToString();
        defeats_S.text = PlayerPrefs.GetInt("Defeats_S").ToString();
        defeats_M.text = PlayerPrefs.GetInt("Defeats_M").ToString();
        matches_S.text = PlayerPrefs.GetInt("TotalMatches_S").ToString();
        matches_M.text = PlayerPrefs.GetInt("TotalMatches_M").ToString();
    }

    public void MakeSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void SetPlayerImage(int index)
    {
        PlayerPrefs.SetInt("PlayerImage", index);
        playerImage.GetComponent<Image>().sprite = playerImages[PlayerPrefs.GetInt("PlayerImage")];
    }


    public void SetTotalMatches(string match)
    {
        PlayerPrefs.SetInt(match, GetTotalMatches(match) + 1);
    }

    public int GetTotalMatches(string match)
    {
        return PlayerPrefs.GetInt(match);
    }

    public void SetWins(string wins)
    {
        PlayerPrefs.SetInt(wins, GetTotalMatches(wins) + 1);
    }

    public int GetWins(string wins)
    {
        return PlayerPrefs.GetInt(wins);
    }
    public void SetDefeats(string defeats)
    {
        PlayerPrefs.SetInt(defeats, GetTotalMatches(defeats) + 1);
    }

    public int Getdefeats(string defeats)
    {
        return PlayerPrefs.GetInt(defeats);
    }

    public void ResetStats()
    {
        PlayerPrefs.SetInt("TotalMatches_S",0);
        PlayerPrefs.SetInt("TotalMatches_M",0);
        PlayerPrefs.SetInt("Wins_S",0);
        PlayerPrefs.SetInt("Wins_M",0);
        PlayerPrefs.SetInt("Defeats_S",0);
        PlayerPrefs.SetInt("Defeats_M",0);

        // Change String Value
        wins_S.text = PlayerPrefs.GetInt("Wins_S").ToString();
        wins_M.text = PlayerPrefs.GetInt("Wins_M").ToString();
        defeats_S.text = PlayerPrefs.GetInt("Defeats_S").ToString();
        defeats_M.text = PlayerPrefs.GetInt("Defeats_M").ToString();
        matches_S.text = PlayerPrefs.GetInt("TotalMatches_S").ToString();
        matches_M.text = PlayerPrefs.GetInt("TotalMatches_M").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
