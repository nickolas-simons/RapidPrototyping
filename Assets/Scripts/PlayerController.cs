using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    const int WINDOW_SIZE = 1;

    [SerializeField]
    private Vehicle ControlledVehicle;

    [SerializeField]
    private float PassiveAcceleration = -0.1f;

    [SerializeField]
    private float PressedAcceleration = 1f;

    private Vector2 PlayerControlInput;

    private InputAction ManualControlAction;

    private float[] sample_window = new float[WINDOW_SIZE];

    bool MicrophoneInUse = false;
    void Start()
    {
        if (GravitySensor.current != null)
        {
            InputSystem.EnableDevice(GravitySensor.current);
        }
         
        ManualControlAction = InputSystem.actions.FindAction("ManualControl");
        PlayerControlInput[1] = PressedAcceleration;
    }


    public void StartShout()
    {
        PlayerControlInput[1] = PassiveAcceleration;
    }

    public void EndShout()
    {
        PlayerControlInput[1] = PressedAcceleration;
    }

    private void UpdateControlValues(){
        PlayerControlInput[0] = 0f;
        float GyroSensitivity = Settings.Instance.GyroSensitivity;

        if (GravitySensor.current != null && GravitySensor.current.enabled)
        {
            Vector3 gravity = GravitySensor.current.gravity.ReadValue();
            if(gravity != Vector3.zero)
            {
                Vector3.Normalize(gravity);
                float roll = Mathf.Atan2(gravity.x, -gravity.y) * Mathf.Rad2Deg;
                PlayerControlInput[0] = Mathf.Clamp(roll / 90 * GyroSensitivity, -1f, 1f);
            }
        }

        PlayerControlInput[0] += ManualControlAction.ReadValue<Vector2>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        UpdateControlValues();

        ControlledVehicle.SetControlVector(PlayerControlInput);
    }
}
