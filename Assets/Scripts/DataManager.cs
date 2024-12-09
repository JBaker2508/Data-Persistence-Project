using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public string CurrentName;
    public string HighScoreName;
    public int CurrentScore;
    public int CurrentRound = 1;
    public int HighScore;
    public bool hasHighScore;
    internal string highScoreStr = "High Score: ";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadCurrentName();
        LoadHighScore();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public string GetHighScoreData()
    {
        string highScore = highScoreStr;

        if (hasHighScore)
        { highScore = highScore + HighScoreName + " : " + HighScore; }
        else
        { highScore += "0"; }

        return highScore;
    }

    #region Serialization of Data
    [System.Serializable]
    class SaveData
    {
        public string CurrentName;
        public string HighScoreName;
        public int HighScore;
    }

    public void SaveCurrentName()
    {
        SaveData data = new SaveData();
        data.CurrentName = CurrentName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/currentname.json", json);
    }
    public void LoadCurrentName()
    {
        string path = Application.persistentDataPath + "/currentname.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            CurrentName = data.CurrentName;
        }
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.HighScoreName = HighScoreName;
        data.HighScore = HighScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/highscore.json", json);
    }
    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/highscore.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            HighScoreName = data.HighScoreName;
            HighScore = data.HighScore;
            hasHighScore = true;
        }
        else
        { hasHighScore = false; }
    }

    public void ResetHighScore()
    {
        string path = Application.persistentDataPath + "/highscore.json";
        if (File.Exists(path))
        {
            File.Delete(path);
            HighScoreName = "";
            HighScore = 0;
            hasHighScore = false;
        }
    }
    #endregion Serialization of Data
}