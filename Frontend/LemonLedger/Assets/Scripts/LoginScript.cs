using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LoginScript : MonoBehaviour
{
    // these will be hooked up in Awake()
    private TMP_InputField emailField;
    private TMP_InputField passwordField;
    
    [Tooltip("Name of your Main Menu scene (must be in Build Settings)")]
    public string mainMenuSceneName = "GameStart";

    [System.Serializable]
    private class LoginRequest
    {
        public string email;
        public string password;
    }

    [System.Serializable]
    private class LoginResponse
    {
        public string token;
        public string refreshToken;
        public string error;
    }

    private void Awake()
    {
        // find the two input‐field GameObjects by name in your hierarchy
        emailField    = GameObject.Find("Canvas/email_field")   ?.GetComponent<TMP_InputField>();
        passwordField = GameObject.Find("Canvas/password_field")?.GetComponent<TMP_InputField>();

        if (emailField == null || passwordField == null)
        {
            Debug.LogError("Could not find email_field or password_field under Canvas!");
        }
    }

    /// <summary>
    /// Hook this to your Button's OnClick()
    /// </summary>
    public void OnLoginButtonClicked()
    {
        if (emailField == null || passwordField == null) return;

        string email    = emailField.text.Trim();
        string password = passwordField.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("Email or password is empty.");
            return;
        }

        StartCoroutine(LoginCoroutine(email, password));
    }

    private IEnumerator LoginCoroutine(string email, string password)
    {
        // build the JSON payload
        var payload = new LoginRequest { email = email, password = password };
        string json = JsonUtility.ToJson(payload);

        using var www = new UnityWebRequest("http://68.183.67.239:5000/api/Auth/login", "POST")
        {
            uploadHandler   = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json)),
            downloadHandler = new DownloadHandlerBuffer()
        };
        www.SetRequestHeader("Content-Type", "application/json");

        // if you're on a self‐signed cert locally, uncomment:
        www.certificateHandler = new BypassCertificate();

        // send and wait
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError ||
            www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Login network error: {www.error}");
            yield break;
        }

        var response = JsonUtility.FromJson<LoginResponse>(www.downloadHandler.text);
        if (!string.IsNullOrEmpty(response.error))
        {
            Debug.LogError($"Login failed: {response.error}");
            // TODO: show UI message
        }
        else
        {
            Debug.Log("Login successful, loading Main Menu...");
            SceneManager.LoadScene("MainMenu");
        }
    }

    // ONLY for testing with self‐signed certs
    private class BypassCertificate : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData) => true;
    }
}
