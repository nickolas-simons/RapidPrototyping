using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using UnityEngine.Assertions;

public class Track : MonoBehaviour
{
    [SerializeField]
    private SplineContainer RoadSpline;

    [SerializeField]
    private SplineExtrude RoadMeshGenerator;

    [SerializeField]
    private float CollisionWidth = 1.5f;

    [SerializeField]
    private float MeshWidth = 2f;

    void InitCheck()
    {
    }

    private void OnValidate()
    {
        InitCheck();

        RoadMeshGenerator.Radius = MeshWidth;
    }

    private void Init()
    {
        RoadMeshGenerator.Radius = MeshWidth;
    }

    // returns normalized position on curve (t), and closest world position along track spine
    public (float,Vector3) GetPositionOnTrack(Vector3 pos)
    {
        float3 outPos;
        float outT;

        SplineUtility.GetNearestPoint(Spline(), pos, out outPos, out outT);

        Debug.Log(outPos.ToString());

        return (outT,new Vector3(outPos.x,outPos.y,outPos.z));
    }

    public float TrackLength()
    {
        return RoadSpline.Spline.GetLength();
    }

    private Spline Spline()
    {
        return RoadSpline.Spline;
    }

    public float GetWidth()
    {
        return CollisionWidth;
    }

    // returns the forward direction of the track, at a specific normalized position (t)
    public Vector3 GetForwardOnTrack(float t)
    {
        float3 tan = Spline().EvaluateTangent(t);
        return new Vector3(tan.x, tan.y, tan.z);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitCheck();
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
