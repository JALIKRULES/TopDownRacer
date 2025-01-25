using UnityEngine;

public class CarUIHandler : MonoBehaviour
{
    Animator animator = null;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {

    }

    public void StartCarEnteranceAnimation(bool isAppearingOnRightSide)
    {
        if (isAppearingOnRightSide)
            animator.Play("CarUIAppearFromRight");
        else animator.Play("CarUIAppearFromLeft");

    }

    public void StartCarExitAnimation(bool isExitingOnRightSide)
    {
        if (isExitingOnRightSide)
            animator.Play("CarUIDisappearToRight");
        else animator.Play("CarUIDisappearToLeft");
    }

    //Eventa
    public void OnCarExitAnimationCompleated()
    {
        Destroy(gameObject);
    }
}
