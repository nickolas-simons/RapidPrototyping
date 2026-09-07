using UnityEngine;

public class IntroImpactEvent : MonoBehaviour
{
    [SerializeField] private Animator flashAnimator;

    public void ImpactFlash()
    {
        flashAnimator.SetTrigger("FlashTrigger");
    }
}