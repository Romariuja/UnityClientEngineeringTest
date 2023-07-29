using System;
using System.Collections.Generic;
using System.IO;
using Completed;
using UnityEngine;

public class LocalScoreManager : MonoBehaviour
{
    [SerializeField]
    private const string EXT = "_Score.json";
    
    // Define a custom data structure to store player scores
    [Serializable]
    public class PlayerScore
    {
        public string playerName;
        public int score;
    }
    
    [SerializeField]
    private ScoreView scoreViewPrefab;
    private List<PlayerScore> localScores = new List<PlayerScore>();
    private const int maxScoresToShow = 10;
    private Stack<ScoreView> scoresViewStack = new Stack<ScoreView>();

    private void Start()
    {
        GameManager.instance.onGameOver += SubmitNewScore;
       
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.instance.onGameOver -= SubmitNewScore;
    }

    // Example method for submitting a score
    private void SubmitNewScore(string playerName, int score)       
    {
        
        AddScore(playerName, score);
        GetHighScores();
        for (int i = 0; i < maxScoresToShow; i++)
        {
            ScoreView scoreView = GameObject.Instantiate(scoreViewPrefab, transform);
            scoreView.gameObject.SetActive(false);
            scoresViewStack.Push(scoreView);
        }

        RefreshScoreBoard();
    }
    public void RefreshScoreBoard()
    {
        gameObject.SetActive(true);
        foreach (var scoreModel in localScores)
        {
            ScoreView scoreview = GetScoreFromStack();
            scoreview.gameObject.SetActive(true);
            scoreview.playerNameText.text = scoreModel.playerName;
            scoreview.playerScoreText.text = scoreModel.score.ToString();
        }
    }
    
    private ScoreView GetScoreFromStack()
    {
        ScoreView scoreview = scoresViewStack.Pop();
        if (scoreview == null)
        {
            scoreview = GameObject.Instantiate(scoreViewPrefab, transform);
        }
        return scoreview;
    }

    public void AddScore(string playerName, int score)
    {
        PlayerScore newScore = new PlayerScore
        {
            playerName = playerName,
            score = score
        };
        SaveLocalScore(newScore);
        localScores.Add(newScore);
    }

    public List<PlayerScore> GetHighScores()
    {
        LoadLocalScores();
        TrimLocalScores(); // Store only the top 'maxScoresToShow' 
        return localScores;
    }

    private void TrimLocalScores()
    {
        localScores.Sort((a, b) => b.score.CompareTo(a.score));
        if (localScores.Count > maxScoresToShow)
        {
            localScores.RemoveRange(maxScoresToShow, localScores.Count - maxScoresToShow);
        }
        localScores.Reverse();
    }

    private string GetFilePath(string fileName = "")
    {
        return Application.persistentDataPath + "/" + fileName.Trim();
    }

    private void SaveLocalScore(PlayerScore playerScore)
    {
            string json = JsonUtility.ToJson(playerScore);
            string uniqueString = Guid.NewGuid().ToString();
            string path = GetFilePath(playerScore.playerName + uniqueString + EXT);
            FileStream fileStream = new FileStream(path, FileMode.Create);
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.Write(json);
            }
    }
    
    private string ReadFromFIle(string fileName)
    {
        
        if (System.IO.File.Exists(fileName))
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
        {
            Debug.LogWarning("File not found");
            return "FailedToReadFromFile";
        }


    }
    
    // Method to load the local scores from PlayerPrefs
    private void LoadLocalScores()
    {
        string path = GetFilePath();

        string[] jsonFilePaths = Directory.GetFiles(path, "*.json");

        foreach (string filePath in jsonFilePaths)
        {
            string json = ReadFromFIle(filePath);
            PlayerScore playerScore = new PlayerScore();
            JsonUtility.FromJsonOverwrite(json, playerScore);
            localScores.Add(playerScore);
        }
    }
}