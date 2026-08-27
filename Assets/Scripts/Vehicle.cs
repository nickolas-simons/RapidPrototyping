using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Vehicle : MonoBehaviour
{
    public UnityEvent OnCrash = new UnityEvent();
    private float forward_speed = 0f;

    private float angular_speed = 0f;

    private float track_progress = 0f;

    private float normalized_speed = 0f;

    private float normalized_angular_speed = 0f;

    private bool crashing = false;

    // vector representing the controlling intent of the vehicle, in bounds 0-1,
    // x axis represents turning intent
    // y axis represents braking and forward acceleraion
    private Vector2 control_vector = Vector2.zero;

    [SerializeField]
    private Track TrackObject = null;

    [Tooltip("max rate at which the car is rotated, proporional to control vector (deg/s)")]
    [SerializeField]
    private float MaxAngularSpeed = 20f;

    [Tooltip("max rate at which the car's forward speed is increased, proportional to control vector (m/s^2) ")]
    [SerializeField]
    private float MaxForwardAcceleration = 0.2f;

    [Tooltip("max forward speed of the var (m/s) ")]
    [SerializeField]
    private float MaxForwardSpeed = 1f;

    [Tooltip("min forward speed of the var (m/s) ")]
    [SerializeField]
    private float MinForwardSpeed = 0.1f;

    [Tooltip("speed decrease value after a crash (m/s) ")]
    [SerializeField]
    private float CrashSpeedDecrease = 10f;

    [Tooltip("speed decrease value after a crash (m/s) ")]
    [SerializeField]
    private float CrashLerpTime = 1f;

    [Tooltip("speed decrease value after a crash (m/s) ")]
    [SerializeField]
    private float CrashInwardPushPercentage = 0.1f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnCrash.AddListener(CrashHandler);
        InitCheck();
    }

    private void InitCheck()
    {
    }

    private void UpdatePosition()
    {
        // rotate the forward vector about the vertical axis, angular speed degrees
        Vector3 new_forward = Quaternion.AngleAxis(angular_speed*Time.deltaTime, Vector3.up) * transform.forward;
        Vector3 new_pos = transform.position + forward_speed*Time.deltaTime * new_forward;

        Debug.DrawRay(transform.position, transform.forward, Color.red);
        Debug.DrawRay(transform.position, new_forward, Color.green);


        (float t, Vector3 projected_pos) = TrackObject.GetPositionOnTrack(new_pos);
        track_progress = t;

        transform.position = new_pos;
        transform.rotation = Quaternion.LookRotation(new_forward, Vector3.up);

        float distance = Vector3.Distance(new_pos, projected_pos);
        Debug.Log("distance " + distance.ToString());
        if (!crashing && distance > TrackObject.GetWidth())
        {
            crashing = true;
            Debug.Log("CRASH");
            OnCrash.Invoke();
        }
    }

    // update vehicle velocities based on control vector
    private void UpdateVelocity()
    {
        float angular_intent = control_vector[0];
        float forward_intent = control_vector[1];

        float forward_delta = MaxForwardAcceleration * forward_intent * Time.deltaTime;
        forward_speed = Mathf.Clamp(forward_speed + forward_delta, MinForwardSpeed, MaxForwardSpeed);
        normalized_speed = forward_speed / MaxForwardSpeed;

        angular_speed = angular_intent*MaxAngularSpeed;
        angular_speed = Mathf.Clamp(angular_speed, -MaxAngularSpeed, MaxAngularSpeed);
        normalized_angular_speed = angular_speed / MaxAngularSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVelocity();
        UpdatePosition();
    }

    public void SetControlVector(Vector2 in_control)
    {
        control_vector = in_control;
    }

    private void CrashHandler()
    {
        Debug.Log("HANDLE CRASH");
        forward_speed = Mathf.Clamp(forward_speed-CrashSpeedDecrease,MinForwardSpeed, MaxForwardSpeed);
        StartCoroutine(Bounce());
    }

    private IEnumerator Bounce()
    {
        (float t, Vector3 p) = TrackObject.GetPositionOnTrack(transform.position);
        Vector3 TrackForward = TrackObject.GetForwardOnTrack(t);

        Quaternion init_rot = transform.rotation;

        Vector3 ReflectionVector = Vector3.Reflect(transform.forward, TrackForward.normalized)*-1;
        Quaternion target_rot = Quaternion.LookRotation(ReflectionVector, Vector3.up);
        float start = Time.time;

        transform.position = CrashInwardPushPercentage * p + (1f - CrashInwardPushPercentage) * transform.position;

        while (Time.time - start < CrashLerpTime)
        {
            t = (Time.time-start)/CrashLerpTime;
            transform.rotation = Quaternion.SlerpUnclamped(init_rot, target_rot, t);
            yield return null;
        }
        transform.rotation = target_rot;

        transform.position = CrashInwardPushPercentage * p + (1f - CrashInwardPushPercentage) * transform.position;
        crashing = false;
        yield return null;
    }

    public float GetTrackProgress()
    {
        return track_progress;
    }

    public float GetNormalizedSpeed()
    {
        return normalized_speed;
    }

    public float GetNormalizedAngularSpeed()
    {
        return normalized_angular_speed;
    }

    public void ResetSpeeds()
    {
        forward_speed = 0;
        angular_speed = 0;
    }
}
