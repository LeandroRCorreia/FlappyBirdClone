 using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class GameSaver : MonoBehaviour
{
    private string fileDataScorePath => $"{Application.persistentDataPath}/BestScores.json" ;
    public int BestScore {get;private set;} = 0;
    public int CurrentMedalIndex {get;private set;} = -1;
    private int worstScore = 0;

    private const int lastScoreIndex = 2;

    public bool IsNewScore {get; private set;} = false;

    private void Start() 
    {
        if (!File.Exists(fileDataScorePath)) CreateBestScoreFile();
        LoadScores();
    }

    private void CreateBestScoreFile()
    {
        List<int> randomMinScoreValues = new List<int>()
        {
            Random.Range(8, 20),
            Random.Range(8, 20),
            Random.Range(8, 20)
        };
        randomMinScoreValues.Sort((x, y) => y.CompareTo(x));

        using (FileStream file = new FileStream(fileDataScorePath, FileMode.Create, FileAccess.Write))
        using (StreamWriter writer = new StreamWriter(file))
        using(JsonWriter jsonWriter = new JsonTextWriter(writer))
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            jsonSerializer.Serialize(jsonWriter, randomMinScoreValues);

        }
    }

    private void LoadScores()
    {
        var scoreData = GetScoreData();
        BestScore = scoreData[0];
        worstScore = scoreData[lastScoreIndex];
    }

    public void SaveGame(int bestScore, int lastScore)
    {
        if(lastScore == 0) 
        {
            CurrentMedalIndex = -1;
            return;
        }
        IsNewScore = lastScore > worstScore ? true : false;
        if(!IsNewScore)
        {
            CurrentMedalIndex = 3;
            return;
        } 

        PlaceNewScore(lastScore);
        LoadScores();
    }

    private void PlaceNewScore(int lastScore)
    {
        List<int> deserBestScoreData = GetScoreData();

        for (int i = 0; i < deserBestScoreData.Count; i++)
        {
            if (lastScore > deserBestScoreData[i])
            {
                SaveFileScoreData(lastScore, in deserBestScoreData, i);
                break;
            }
        }

    }

    private List<int> GetScoreData()
    {
        List<int> deserBestScoreData;
        using (FileStream file = new FileStream(fileDataScorePath, FileMode.Open, FileAccess.Read))
        using (StreamReader reader = new StreamReader(file))
        using (JsonReader jsonReader = new JsonTextReader(reader))
        {
            JsonSerializer ser = new JsonSerializer();
            deserBestScoreData = ser.Deserialize<List<int>>(jsonReader);
        }

        return deserBestScoreData;
    }

    private void SaveFileScoreData(int lastScore, in List<int> dataScore, int indexScore)
    {
        dataScore.Insert(indexScore, lastScore);
        dataScore.RemoveAt(lastScoreIndex + 1);
        CurrentMedalIndex = indexScore;
        File.Delete(fileDataScorePath);
        using(FileStream file = new FileStream(fileDataScorePath, FileMode.OpenOrCreate, FileAccess.Write))
        using(StreamWriter writer = new StreamWriter(file))
        using(JsonWriter jsonWriter = new JsonTextWriter(writer))
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            jsonSerializer.Serialize(jsonWriter, dataScore);

        }

    }

    private void ClearFileScoreData()
    {
        if(File.Exists(fileDataScorePath))
        {
            File.Delete(fileDataScorePath);
            CreateBestScoreFile();
        }

    }

}
