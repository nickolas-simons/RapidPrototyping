using System;
using System.Collections;
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
    private float RoadWidthBuffer;

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
    private Vector2 SpeedRange;

    private Vector3 track_right;

    private Vector3 track_edge;

    private float track_width;

    private float distance_from_edge = 0;

    private float speed;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("HIT!!!");
            OnHit.Invoke();
        }
    }

    void Start()
    {
        OnHit.AddListener(Die);

        if (TrackObj != null)
        {
            
            track_width = TrackObj.GetWidth()+RoadWidthBuffer;
            (float t, Vector3 p) = TrackObj.GetPositionOnTrack(transform.position);
            track_right = Vector3.Cross(TrackObj.GetForwardOnTrack(t), Vector3.up).normalized;
            track_edge = p + track_right * (MovementDirection == Direction.left ? -1 : 1) * track_width;
        }

        speed = UnityEngine.Random.Range(SpeedRange[0], SpeedRange[1]);
    }

    // Update is called once per frame
    void Update()
    {
        if (TrackObj != null)
        {
            float distance_moved = Time.deltaTime * speed;
            distance_from_edge = Mathf.Repeat(distance_from_edge + distance_moved,track_width * 2);
            transform.position = track_right * (MovementDirection == Direction.left ? 1 : -1) * distance_from_edge + track_edge;
        }
    }

    void Die()
    {
        if (Hit)
        {
            Animator.Play(name = Hit.name);
            
        }
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        while (Animator.IsPlaying(Hit.name))
        {
            yield return null;
        }
        Destroy(gameObject);
        yield return null;
    }
}
