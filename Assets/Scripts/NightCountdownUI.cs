using TMPro;
using UnityEngine;

public class NightCountdownUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text timerText;

    [Header("Settings")]
    [SerializeField] private int dayNumber = 3;
    [SerializeField] private float countdownTime = 97f;

    private float currentTime;

    private void Start()
    {
        currentTime = countdownTime;
        dayText.text = "Day " + dayNumber;
        UpdateTimerText();
    }

    private void Update()
    {
        if (currentTime <= 0f)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime < 0f)
            currentTime = 0f;

        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timerText.text = $"Nightfall in {minutes}:{seconds:00}";
    }
}
