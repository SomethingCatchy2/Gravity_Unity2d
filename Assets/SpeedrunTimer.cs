using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class SpeedrunTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float elapsedTime = 0f;
    private bool isRunning = false; // was true
    private bool hasFinished = false;

    private PlayerController playerController;
    private ParticleSystem finishEffect;

    // Flash settings
    private Color textColorWhite = Color.white;
    private Color textColorBlack = Color.black;
    private float flashSpeed = 0.5f; // seconds per flash cycle

    void Awake()
    {
        timerText = GetComponentInChildren<TextMeshProUGUI>();
        if (timerText == null)
        {
            Debug.LogError("TextMeshProUGUI component is missing on SpeedrunTimer!");
            enabled = false;
            return;
        }
    }

    void Start()
    {
        // Find the player controller to detect when the game is finished
        playerController = UnityEngine.Object.FindAnyObjectByType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogWarning("PlayerController not found. Timer will not stop automatically.");
        }
        else
        {
            finishEffect = playerController.FireWork1;
        }

        // Reset timer
        elapsedTime = 0f;
        isRunning = false; // wait for player input
        UpdateTimerDisplay();
    }

    void Update()
    {
        // wait for player movement to start timer
        if (!isRunning && !hasFinished)
        {
            if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
            {
                isRunning = true;
            }
            else
            {
                return;
            }
        }

        // once running, proceed as before
        if (isRunning && !hasFinished)
        {
            // Check if fireworks are playing (indicating finish)
            if (finishEffect != null && finishEffect.isPlaying)
            {
                FinishTimer();
            }
            else
            {
                elapsedTime += Time.deltaTime;
                UpdateTimerDisplay();
            }
        }
    }

    void UpdateTimerDisplay()
    {
        // Calculate hours, minutes, seconds, and milliseconds
        TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
        int hours = timeSpan.Hours;
        int minutes = timeSpan.Minutes;
        int seconds = timeSpan.Seconds;
        int milliseconds = timeSpan.Milliseconds;

        // Format timer display: HH:MM:SS.mmm
        timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", 
            hours, minutes, seconds, milliseconds);
    }

    void FinishTimer()
    {
        isRunning = false;
        hasFinished = true;
        Debug.Log("Speedrun finished! Final time: " + timerText.text);
        
        // Start flashing effect
        StartCoroutine(FlashTimerText());
    }

    IEnumerator FlashTimerText()
    {
        bool isWhite = true;
        
        while (true)
        {
            timerText.color = isWhite ? textColorWhite : textColorBlack;
            isWhite = !isWhite;
            yield return new WaitForSeconds(flashSpeed);
        }
    }
}
