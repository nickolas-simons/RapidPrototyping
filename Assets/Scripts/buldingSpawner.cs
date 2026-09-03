using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

public class buldingSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct BuildingInfo
    {
        public GameObject prefab;
        public float width;
        public float verticalOffset;
    }
    [SerializeField] private SplineContainer targetSpline;
    [SerializeField] private BuildingInfo[] buildings; 
    [SerializeField] private float lateralDistance;
    [SerializeField] private float spacing = 0f;
    void Start()
    {
        spawnBuilding();
    }

    void spawnBuilding()
    {
        if (targetSpline == null || buildings == null || buildings.Length == 0)
        {
            Debug.LogWarning("Target Spline or Buildings array is not assigned.");
            return;
        }
         
        Spline spline = targetSpline.Spline;
        float trackLength = spline.GetLength();
        float currentDist = 0f;
        int lastIndex = -1;
        while (currentDist < trackLength)
        {
            int randomIndex;
            if (lastIndex == -1 || buildings.Length <= 1)
            {
                randomIndex = Random.Range(0, buildings.Length);
            }
            else
            {
                int offset = Random.Range(1, buildings.Length);
                randomIndex = (lastIndex + offset) % buildings.Length;
            }
            lastIndex = randomIndex;
            BuildingInfo selectedBuilding = buildings[randomIndex];
            float centerDist = currentDist + (selectedBuilding.width * 0.5f);
            float t = centerDist / trackLength;
            float3 position = spline.EvaluatePosition(t);
            float3 tangent = spline.EvaluateTangent(t);
            Vector3 forward = new Vector3(tangent.x, tangent.y, tangent.z).normalized;
            Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
            Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up) * Quaternion.Euler(180, 90, 180);
            Vector3 leftSpawnPos = new Vector3(position.x, position.y, position.z) - right * lateralDistance;
            leftSpawnPos += Vector3.up * selectedBuilding.verticalOffset;
            if (selectedBuilding.prefab != null)
            {
                Instantiate(selectedBuilding.prefab, leftSpawnPos, rotation, transform);
            }

            currentDist += selectedBuilding.width + spacing;
        }
    }
}