using UnityEngine;
using UnityEngine.Events;

enum Direction
{
    left = 0, right = 1
}
public class Creature : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public UnityEvent OnHit = new UnityEvent();

    [SerializeField]
    private Track TrackObj;

    [SerializeField]
    private AnimationClip Idle;

    [SerializeField]
    private AnimationClip Walk;

    [SerializeField]
    private AnimationClip Hit;

    [SerializeField]
    private Animation Animator;

    [SerializeField]
    private Direction MovementDirection;

    [SerializeField]
    private Vector2 SpeedOffsetRange;

    private Vector3 track_pos;

    private Vector3 track_forward;

    private float track_width;

    void Start()
    {
        OnHit.AddListener(Die);

        if (TrackObj != null)
        {
            (float t,Vector3 p) = TrackObj.GetPositionOnTrack(transform.position);
            track_pos = p;
            track_forward = TrackObj.GetForwardOnTrack(t);
            track_width = TrackObj.GetWidth();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Die()
    {
        Animator.Play(name = Hit.name);
        Destroy(this);
    }
}
