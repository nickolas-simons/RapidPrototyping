using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    [SerializeField] private Animator cockpitAnimator;
    [SerializeField] private Animator introUIAnimator;

    [SerializeField] private GameObject introUI;
    [SerializeField] private GameObject startMenu;

    [SerializeField] private Button IntroButton;

    [SerializeField] private float introDuration = 1.0f;

    private bool triggered = false;

    void Start()
    {
        introUI.SetActive(true);
        startMenu.SetActive(false);
        IntroButton.onClick.AddListener(StartIntro);
    }
    private void StartIntro()
    {
        triggered = true;

        cockpitAnimator.SetTrigger("IntroTrigger");
        introUIAnimator.SetTrigger("IntroTrigger");

        StartCoroutine(FinishIntro());
    }

    private IEnumerator FinishIntro()
    {
        yield return new WaitForSecondsRealtime(introDuration);

        introUI.SetActive(false);
        startMenu.SetActive(true);

        // 等当前这次点击完全松开
        if (Mouse.current != null)
        {
            while (Mouse.current.leftButton.isPressed)
            {
                yield return null;
            }
        }
    }
}