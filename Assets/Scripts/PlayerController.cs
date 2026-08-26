using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private Vehicle ControlledVehicle;

    private InputAction TiltAction;

    private Vector2 PlayerControlInput;

    private InputAction ManualControlAction;
    void Start()
    {
        if (GravitySensor.current != null)
        {
            InputSystem.EnableDevice(GravitySensor.current);
        }
        ManualControlAction = InputSystem.actions.FindAction("ManualControl");
    }

    private void UpdateControlValue(){

        PlayerControlInput = Vector2.zero;
        if (GravitySensor.current != null && GravitySensor.current.enabled)
        {
            Vector3 gravity = GravitySensor.current.gravity.ReadValue();
            float pitch = Mathf.Atan2(gravity.x, gravity.y) * Mathf.Rad2Deg;
            float roll = Mathf.Atan2(-gravity.z, Mathf.Sqrt(gravity.x * gravity.x + gravity.y * gravity.y)) * Mathf.Rad2Deg;
            PlayerControlInput[0] = Mathf.Clamp(pitch / 180, -1f, 1f);
            PlayerControlInput[1] = Mathf.Clamp(roll / 180, -1f, 1f);
        }

        PlayerControlInput += ManualControlAction.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateControlValue();

        ControlledVehicle.SetControlVector(PlayerControlInput);
    }
}
