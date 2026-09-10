using System.Collections.Generic;
using UnityEngine;

public class BloodFXPool : MonoBehaviour
{
    public static BloodFXPool Instance;

    [SerializeField]
    private GameObject BloodFXPrefab;

    [SerializeField]
    private int PoolSize = 10;

    private List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < PoolSize; i++)
        {
            GameObject fx = Instantiate(BloodFXPrefab);
            fx.SetActive(false);
            pool.Add(fx);
        }
    }

    public void PlayBlood(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                GameObject fx = pool[i];

                fx.transform.position = position;
                fx.transform.rotation = rotation;
                fx.SetActive(true);

                ParticleSystem ps = fx.GetComponent<ParticleSystem>();

                if (ps != null)
                {
                    ps.Clear();
                    ps.Play();
                }

                return;
            }
        }
    }
}