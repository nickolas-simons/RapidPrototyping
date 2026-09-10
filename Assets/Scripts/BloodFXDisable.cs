using System.Collections;
using UnityEngine;

public class BloodFXDisable : MonoBehaviour
{
    [SerializeField]
    private float DisableDelay = 1f;

    private void OnEnable()
    {
        StartCoroutine(DisableAfterTime());
    }

    private IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(DisableDelay);

        gameObject.SetActive(false);
    }
}