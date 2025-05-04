using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour
{
    private void Awake()
    {

    }

    private void Start()
    {

    }

    public void OnNewGameButtonClicked()
    {
        SceneManager.LoadScene("GameStart");
    }
}
