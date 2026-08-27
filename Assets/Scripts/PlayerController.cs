using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField]
    private Vehicle ControlledVehicle;

    [SerializeField]
    private float ForwardAcceleration = 0.5f;

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
        PlayerControlInput[1] = Mathf.Clamp(ForwardAcceleration, -1f, 1f);
        if (GravitySensor.current != null && GravitySensor.current.enabled)
        {
            Vector3 gravity = GravitySensor.current.gravity.ReadValue();
            Vector3.Normalize(gravity);
            float roll = Mathf.Atan2(gravity.x, -gravity.y) * Mathf.Rad2Deg;
            PlayerControlInput[0] = Mathf.Clamp(roll / 90, -1f, 1f);
        }

        PlayerControlInput[0] += ManualControlAction.ReadValue<Vector2>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        UpdateControlValue();

        ControlledVehicle.SetControlVector(PlayerControlInput);
    }
}
