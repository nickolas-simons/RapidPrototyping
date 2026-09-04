using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
[ExecuteAlways]
public class FoliageSpawner : MonoBehaviour
{
    [SerializeField] private SplineContainer targetSpline;
    [SerializeField] private GameObject foliagePrefab; 
    [SerializeField] private float verticalOffset = 0f;
    [SerializeField] private float lateralDistance;
    [SerializeField] private float spacing = 3f;
    [SerializeField] private float YRotation = 0f;

    void Start()
    {
        SpawnFoliage();
    }

    void SpawnFoliage()
    {
        if (targetSpline == null || foliagePrefab == null)
        {
            Debug.LogWarning("Target Spline or Foliage Prefab is not assigned.");
            return;
        }

        Spline spline = targetSpline.Spline;
        float trackLength = spline.GetLength();
        float currentDist = 0f;

        while (currentDist < trackLength)
        {
            float t = currentDist / trackLength;
            float3 position = spline.EvaluatePosition(t);
            float3 tangent = spline.EvaluateTangent(t);
            Vector3 forward = new Vector3(tangent.x, tangent.y, tangent.z).normalized;
            Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
            Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up) * Quaternion.Euler(180, 90, 180);
            rotation *= Quaternion.Euler(0f, YRotation, 0f);
            Vector3 leftSpawnPos = new Vector3(position.x, position.y, position.z) - right * lateralDistance;
            leftSpawnPos += Vector3.up * verticalOffset;
            Instantiate(foliagePrefab, leftSpawnPos, rotation, transform);
            currentDist += spacing;
        }
    }
}