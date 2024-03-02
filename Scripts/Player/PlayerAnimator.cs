using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private string isMovingName;
    [SerializeField] private string angryAnimationName;

    private void Start()
    {
        // EndCutScene.Instance.OnPlaybackStarted += PlayAngryAnimation;
    }

    private void Update()
    {
        animator.SetBool(isMovingName, playerController.IsMoving);
    }

    private void PlayAngryAnimation()
    {
        animator.Play(angryAnimationName);
    }
}
