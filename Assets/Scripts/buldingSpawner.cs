using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class buldingSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private SplineContainer targetSpline;
    [SerializeField] private GameObject[] BuildingPrefab;
    [SerializeField] private float lateralDistance;
    [SerializeField] private float space;

    void Start()
    {
        spawnBuilding();
    }

    // Update is called once per frame
    void spawnBuilding()
    {
        if (targetSpline == null || BuildingPrefab == null)
        {
            Debug.LogWarning("Target Spline or Building Prefab is not assigned.");
            return;
        }
         
        Spline spline = targetSpline.Spline;
        float trackLength = spline.GetLength();

        for (float Dist = 0; Dist < trackLength; Dist += space)
        {
            float t = Dist / trackLength;
            float3 position = spline.EvaluatePosition(t);
            float3 tangent = spline.EvaluateTangent(t);
            Vector3 forward = new Vector3(tangent.x, tangent.y, tangent.z).normalized;
            Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
            Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up) * Quaternion.Euler(180, 90, 90);
            Vector3 leftSpawnPos = new Vector3(position.x, position.y, position.z) - right * lateralDistance;
            Instantiate(BuildingPrefab[1], leftSpawnPos, rotation, transform);
        }

    }
}
