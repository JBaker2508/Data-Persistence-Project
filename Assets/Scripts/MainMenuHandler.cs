using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Drawing;
using UnityEngine.Assertions.Must;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuHandler : MonoBehaviour
{
    public TMP_InputField InputText;
    public string currentInputText;
    public Text HighScoreText;
    public Button StartButton;

    private void Start()
    {
        StartButton.interactable = false;

        if (DataManager.Instance.CurrentName != string.Empty)
        { 
            InputText.text = DataManager.Instance.CurrentName;
            StartButton.interactable = true;
        }

        UpdateHighScoreText();
    }

    public void StartGame()
    { SceneManager.LoadScene(1); }
    public void Exit()
    {
        DataManager.Instance.SaveCurrentName();
        DataManager.Instance.SaveHighScore();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

    public void SetName()
    { 
        DataManager.Instance.CurrentName = InputText.textComponent.text;

        StartButton.interactable = true;
    }
    public void ResetHighScore()
    { 
        DataManager.Instance.ResetHighScore();

        UpdateHighScoreText();
    }
    private void UpdateHighScoreText()
    { HighScoreText.text = DataManager.Instance.GetHighScoreData(); }
}