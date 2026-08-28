using UnityEngine;

public class Settings : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static Settings Instance { get; private set; }

    public float GyroSensitivity = 1f;

    public float ShoutSensitivity = 1f;
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

}
