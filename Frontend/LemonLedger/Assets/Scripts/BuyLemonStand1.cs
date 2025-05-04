using UnityEngine;
using UnityEngine.SceneManagement;

public class BuyLemonStand1 : MonoBehaviour
{
   public void OnTakeLoanButtonClicked()
    {
        SceneManager.LoadScene("PurchaseLemonade2");
    }
}
