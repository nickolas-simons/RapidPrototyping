using UnityEngine;
using UnityEngine.Splines;

public class SplinePedestrianSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject pedestrianPrefab;

    [SerializeField]
    private SplineContainer trackSpline;

    [SerializeField]
    private int totalPrefabsToSpawn = 10;

    [SerializeField]
    private float minLateralOffset = -3f;

    [SerializeField]
    private float maxLateralOffset = 3f;

    void Start()
    {
        SpawnPedestriansOnSpline();
    }

    void SpawnPedestriansOnSpline()
    {
        if (pedestrianPrefab == null || trackSpline == null)
        {
            Debug.LogWarning("Pedestrian prefab or Track Spline is missing!");
            return;
        }

        for (int i = 0; i < totalPrefabsToSpawn; i++)
        {
            float t = Random.Range(0f, 1f);
            trackSpline.Evaluate(t, out var position, out var tangent, out var up);
            Vector3 pos = position;
            Vector3 tan = tangent;
            Vector3 trackUp = up;
            Quaternion rotation = Quaternion.LookRotation(tan, trackUp);
            float randomOffset = Random.Range(minLateralOffset, maxLateralOffset);
            Vector3 rightDir = Vector3.Cross(trackUp, tan).normalized;
            Vector3 finalPosition = pos + (rightDir * randomOffset);

            Instantiate(pedestrianPrefab, finalPosition, rotation, transform);
        }
    }
}