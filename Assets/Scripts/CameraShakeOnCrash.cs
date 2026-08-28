using System.Collections;
using UnityEngine;

public class CameraShakeOnCrash : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private Vector3 strength = new Vector3(0.08f, 0.05f, 0f);

    [Tooltip("How quickly the shake changes direction.")]
    [SerializeField] private float frequency = 25f;

    [Tooltip("Controls how the shake fades out over time.")]
    [SerializeField] private AnimationCurve shakeCurve =
        AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

    private Vector3 originalLocalPosition;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        originalLocalPosition = transform.localPosition;
    }

    public void Shake()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            transform.localPosition = originalLocalPosition;
        }

        shakeCoroutine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float normalizedTime = Mathf.Clamp01(timer / duration);
            float intensity = shakeCurve.Evaluate(normalizedTime);

            float x = (Mathf.PerlinNoise(Time.time * frequency, 0f) * 2f - 1f) * strength.x * intensity;
            float y = (Mathf.PerlinNoise(0f, Time.time * frequency) * 2f - 1f) * strength.y * intensity;
            float z = (Mathf.PerlinNoise(Time.time * frequency, Time.time * frequency) * 2f - 1f) * strength.z * intensity;

            transform.localPosition =
                originalLocalPosition + new Vector3(x, y, z);
            yield return null;
        }

        transform.localPosition = originalLocalPosition;
        shakeCoroutine = null;

    }

    private void OnDisable()
    {
        if(shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);
        transform.localPosition = originalLocalPosition;
    }
}