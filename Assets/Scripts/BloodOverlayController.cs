using UnityEngine;

public class BloodOverlayController : MonoBehaviour
{
    public static BloodOverlayController Instance;

    [SerializeField]
    private GameObject bloodStage1;

    [SerializeField]
    private GameObject bloodStage2;

    [SerializeField]
    private GameObject bloodStage3;

    public int stage1HitCount = 1;
    public int stage2HitCount = 3;
    public int stage3HitCount = 6;

    private int hitCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ResetBlood();
    }

    public void AddHit()
    {
        hitCount++;

        Debug.Log("Blood Hit Count: " + hitCount);

        UpdateBloodStage();
    }

    public void ResetBlood()
    {
        hitCount = 0;
        bloodStage1.SetActive(false);
        bloodStage2.SetActive(false);
        bloodStage3.SetActive(false);
    }

    private void UpdateBloodStage()
    {
            bloodStage1.SetActive(false);
            bloodStage2.SetActive(false);
            bloodStage3.SetActive(false);

        if (hitCount >= stage3HitCount)
        {
                bloodStage3.SetActive(true);
        }
        else if (hitCount >= stage2HitCount)
        {
                bloodStage2.SetActive(true);
        }
        else if (hitCount >= stage1HitCount)
        {
                bloodStage1.SetActive(true);
        }
    }
}