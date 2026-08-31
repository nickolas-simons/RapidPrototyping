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
    private Animator Anim;

    [SerializeField]
    private float HitDuration;

    [SerializeField]
    private Direction MovementDirection;

    [SerializeField]
    private Vector2 SpeedRange;

    [SerializeField]
    private float SlideRate;

    private Vector3 track_right;

    private Vector3 track_edge;

    private float track_width;

    private float distance_from_edge = 0;

    private float speed;

    private bool bIsHit = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bIsHit = true;
            transform.parent = other.gameObject.transform; 
            Debug.Log("HIT!!!");
            OnHit.Invoke();
        }
    }

    void Start()
    {
        OnHit.AddListener(Die);
        if (MovementDirection == Direction.left)
            transform.localScale = new Vector3(-1, 1, 1);

        if (TrackObj != null)
        {
            
            track_width = TrackObj.GetWidth()+RoadWidthBuffer;
            (float t, Vector3 p) = TrackObj.GetPositionOnTrack(transform.position);
            track_right = Vector3.Cross(TrackObj.GetForwardOnTrack(t), Vector3.up).normalized;
            track_edge = p + track_right * (MovementDirection == Direction.left ? -1 : 1) * track_width;
            distance_from_edge = Vector3.Distance(transform.position, track_edge);
        }

        speed = UnityEngine.Random.Range(SpeedRange[0], SpeedRange[1]);
    }

    // Update is called once per frame
    void Update()
    {
        if (TrackObj != null && !bIsHit)
        {
            float distance_moved = Time.deltaTime * speed;
            distance_from_edge = Mathf.Repeat(distance_from_edge + distance_moved,track_width * 2);
            transform.position = track_right * (MovementDirection == Direction.left ? 1 : -1) * distance_from_edge + track_edge;
        }
    }

    void Die()
    {
        Anim.SetBool("bIsHit", true);
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        float time_start = Time.time;
        while (Time.time - time_start <  HitDuration)
        {
            transform.localPosition += new Vector3(0,SlideRate * Time.deltaTime,0);
            yield return null;
        }
        Destroy(gameObject);
        yield return null;
    }
}
