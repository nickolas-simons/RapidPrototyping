using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private Slider Speedometer;

    [SerializeField]
    private Slider ProgressBar;

    [SerializeField]
    private TextMeshProUGUI TimerText;

    [SerializeField]
    private Vehicle PlayerVehicle;

    [SerializeField]
    private Image SteeringWheel;

    [SerializeField]
    private float SteeringAngleBounds = 90f;

    private float timer_start = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void ResetTimer()
    {
        timer_start = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Speedometer.value = PlayerVehicle.GetNormalizedSpeed();
        ProgressBar.value = PlayerVehicle.GetTrackProgress();
        SteeringWheel.transform.localRotation = Quaternion.Euler(0,0,PlayerVehicle.GetNormalizedAngularSpeed() * -1f* SteeringAngleBounds);

        int seconds = Mathf.RoundToInt(Time.time - timer_start);
        int minutes = seconds / 60;
        seconds %= 60;
        string formatted_text = $"{minutes:00}:{seconds:00}";

        TimerText.text = formatted_text;
    }
}
