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
        TiltAction = InputSystem.actions.FindAction("Tilt");
        ManualControlAction = InputSystem.actions.FindAction("ManualControl");
    }

    private void UpdateControlValue(){
        PlayerControlInput = ManualControlAction.ReadValue<Vector2>();
        Vector3 Gravity = TiltAction.ReadValue<Vector3>();

        if(Gravity != Vector3.zero)
        {
            float roll = Vector3.SignedAngle(Gravity, Vector3.down, Vector3.back);
            float pitch = Vector3.SignedAngle(Gravity, Vector3.down, Vector3.right);
            PlayerControlInput[0] = Mathf.Clamp01(PlayerControlInput[0] + roll);
            PlayerControlInput[1] = Mathf.Clamp01(PlayerControlInput[1] + pitch);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateControlValue();

        ControlledVehicle.SetControlVector(PlayerControlInput);
    }
}
