using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TimerText;

    [SerializeField]
    private TextMeshProUGUI Score;

    [SerializeField]
    private GameManager gm;

    [SerializeField]
    private Vehicle PlayerVehicle;

    [SerializeField]
    private GameObject SteeringWheel;

    [SerializeField]
    private float SteeringAngleBounds = 90f;

    [SerializeField]
    private float WheelInterpRate = 0.9f;

    private Quaternion base_wheel_rot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base_wheel_rot = SteeringWheel.transform.localRotation;
    }


    // Update is called once per frame
    void Update()
    {
        float desired_y_rot = PlayerVehicle.GetNormalizedAngularSpeed() * SteeringAngleBounds;
        Quaternion target_rot = base_wheel_rot * Quaternion.Euler(0,desired_y_rot, 0);
        
        float curr_z_rot = SteeringWheel.transform.localRotation.eulerAngles.z;
        SteeringWheel.transform.localRotation = Quaternion.Slerp(target_rot, SteeringWheel.transform.localRotation, WheelInterpRate);


        Score.text = gm.GetScore().ToString();

        int seconds = Mathf.RoundToInt(gm.GetRemainingTime());
        int minutes = seconds / 60;
        seconds %= 60;
        string formatted_text = $"{minutes:00}:{seconds:00}";

        TimerText.text = formatted_text;
    }
}
