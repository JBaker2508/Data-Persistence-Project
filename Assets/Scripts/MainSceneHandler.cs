using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneHandler : MonoBehaviour
{
    public void ReturnToMenu()
    { SceneManager.LoadScene(0); }
}