using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticatorApp : MonoBehaviour
{
    [Header("UI Elements")]
    public Text codeText; // Display current code
    public Slider timerSlider; // Optional: visual countdown

    [Header("Settings")]
    public float codeDuration = 30f; // Duration of each code in seconds
    private float timer;

    private string currentCode;

    void Start()
    {
        GenerateNewCode();
        timer = codeDuration;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        // Update slider (optional)
        if (timerSlider != null)
        {
            timerSlider.value = timer / codeDuration;
        }

        // Timer expired
        if (timer <= 0)
        {
            GenerateNewCode();
            timer = codeDuration;
        }
    }

    void GenerateNewCode()
    {
        // Generate a 6-digit random code
        currentCode = Random.Range(100000, 999999).ToString();
        codeText.text = currentCode;

        Debug.Log("New Authenticator Code: " + currentCode);
    }

    public string GetCurrentCode()
    {
        return currentCode;
    }
    
    public void ResetTimer()
    {
        timer = 0;
    }
}