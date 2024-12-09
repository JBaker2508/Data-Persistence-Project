using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public Rigidbody Paddle;

    public Text HighScoreText;
    public Text ScoreText;
    public Text RoundText;
    public GameObject GameOverText;
    public GameObject RoundOverText;
    private int totalBlocks = 0;

    private bool m_Started = false;
    private bool m_GameOver = false;
    private bool m_RoundComplete = false;
    private int m_Points;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateHighScoreText();
        ScoreText.text = $"Score: {DataManager.Instance.CurrentScore}";
        RoundText.text = "Round " + DataManager.Instance.CurrentRound;

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.5f / step);
        
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                int randVal = GetRandomPointValue();

                if (randVal < 10)
                {
                    Vector3 position = new Vector3(-1.75f + step * x, 2.0f + i * 0.3f, 0);
                    var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                    brick.PointValue = randVal;
                    brick.onDestroyed.AddListener(AddPoint);
                    totalBlocks++;
                }
            }
        }
    }

    private void Update()
    {
        if (totalBlocks == 0 && !m_RoundComplete)
        { RoundComplete(); }

        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_RoundComplete)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DataManager.Instance.CurrentRound++;
                RoundOverText.SetActive(false);

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
        }
    }

    private int GetRandomPointValue()
    { return Random.Range(1, 11); }

    void AddPoint(int point)
    {
        totalBlocks--;
        m_Points += point;
        UpdateCurrentScore();
        ScoreText.text = $"Score: {m_Points}";
    }

    public void SetHighScore()
    {
        if (m_Points > DataManager.Instance.HighScore)
        {
            //Set it on the instance
            DataManager.Instance.HighScoreName = DataManager.Instance.CurrentName;
            DataManager.Instance.HighScore = m_Points;

            //Set the new score locally. highScoreStr
            HighScoreText.text = DataManager.Instance.highScoreStr + DataManager.Instance.HighScoreName + " : " + DataManager.Instance.HighScore;

            //Save the new high score
            DataManager.Instance.SaveHighScore();

            //Indicate that a high score has been achieved
            DataManager.Instance.hasHighScore = true;
        }
    }
    private void UpdateCurrentScore()
    { DataManager.Instance.CurrentScore = m_Points; }
    private void UpdateHighScoreText()
    { HighScoreText.text = DataManager.Instance.GetHighScoreData(); }

    public void RoundComplete()
    {
        m_RoundComplete = true;
        Ball.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
        Paddle.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;

        SetHighScore();
        RoundOverText.SetActive(true);
    }
    public void GameOver()
    {
        m_GameOver = true;
        DataManager.Instance.CurrentScore = 0;
        SetHighScore();
        GameOverText.SetActive(true);
    }
}