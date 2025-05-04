using UnityEngine;
using UnityEngine.SceneManagement;

public class BuyLemonStand : MonoBehaviour
{
   public void OnTakeLoanButtonClicked()
    {
        SceneManager.LoadScene("Home");
    }
}
