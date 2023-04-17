using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameMode))]
public class GameSaver : MonoBehaviour
{
    private GameMode gameMode;

    private const string LAST_SCORE_KEY = "LSK";
    private const string BEST_SCORE_KEY = "BSK";
    private const string SILVER_SCORE_KEY = "SSK";
    private const string BRONZE_SCORE_KEY = "BBSK";

    public int LastScoreData
    {
        get { return PlayerPrefs.GetInt(LAST_SCORE_KEY, 0);}
        private set { PlayerPrefs.SetInt(LAST_SCORE_KEY, value); }
    }

    public int BestScoreData
    {
        get { return PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);}
        private set { PlayerPrefs.SetInt(BEST_SCORE_KEY, value); }
    }

    public int SilverMedalScore
    {
        get { return PlayerPrefs.GetInt(SILVER_SCORE_KEY, 0);}
        private set { PlayerPrefs.SetInt(SILVER_SCORE_KEY, value); }
    }

    public int BronzeMedalScore
    {
        get { return PlayerPrefs.GetInt(BRONZE_SCORE_KEY, 0);}
        private set { PlayerPrefs.SetInt(BRONZE_SCORE_KEY, value); }
    }

    [field:SerializeField] public List<int> topThreeScores = new List<int>();

    public int CurrentMedalIndex {get; private set;}

    private void Awake() 
    {
        gameMode = GetComponent<GameMode>();
        LoadMedalsValue();
    }

    private void LoadMedalsValue()
    {
        topThreeScores.Clear();
        topThreeScores.Add(BestScoreData);
        topThreeScores.Add(SilverMedalScore);
        topThreeScores.Add(BronzeMedalScore);
    }

    public void SaveGame(int bestScoreData, int lastScore)
    {
        CurrentMedalIndex = GetScorePlaceIndex(lastScore);
        BestScoreData = bestScoreData;
        LastScoreData = lastScore;
        SaveNewScoreData(lastScore);
    }

    public void SaveNewScoreData(int score)
    {
        
        if(CurrentMedalIndex <= 3 && CurrentMedalIndex > -1)
        {
            topThreeScores.Insert(CurrentMedalIndex, score);
            topThreeScores.RemoveAt(3);
            topThreeScores.Sort((x, y) => y.CompareTo(x));
            BestScoreData = topThreeScores[0];
            SilverMedalScore = topThreeScores[1];
            BronzeMedalScore = topThreeScores[2];
        }

    }

    private int GetScorePlaceIndex(int score)
    {
        if(score == 0) return -1;

        var placeScore = -1;
        topThreeScores.Sort((x, y) => y.CompareTo(x));
        for (int i = 0; i < topThreeScores.Count; i++)
        {
            if (score >= topThreeScores[i])
            {
                placeScore = i;
                break;
            }

        }
        
        return placeScore >= 0 ? placeScore : 3;
    }

}
