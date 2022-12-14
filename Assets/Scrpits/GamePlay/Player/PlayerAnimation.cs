using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    Animator anim;

    private bool hasReach = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        UtilityEvents.GamePause += IdleAnimation;
        UtilityEvents.GameResume += StartMovingAnimation;

        GameplayEvents.StartNewLevel += StartMovingAnimation;
        GameplayEvents.GameOver += GameOverAnimation;


        GameplayEvents.ReachPalace += WinPreparation;
        GameplayEvents.DropFaixa += WinAnimation;
    }

    private void OnDisable()
    {
        UtilityEvents.GamePause -= IdleAnimation;
        UtilityEvents.GameResume -= StartMovingAnimation;

        GameplayEvents.GameOver -= GameOverAnimation;
        GameplayEvents.StartNewLevel -= StartMovingAnimation;

        GameplayEvents.ReachPalace -= WinPreparation;
        GameplayEvents.DropFaixa -= WinAnimation;
    }

    private void GameOverAnimation()
    {
        anim.SetBool(Const.RUN_ANIMATION, false);
    }

    private void IdleAnimation()
    {
        anim.SetBool(Const.RUN_ANIMATION, false);

    }

    private void StartMovingAnimation()
    {
        anim.SetBool(Const.RUN_ANIMATION, true);
    }

    private void WinPreparation()
    {
        anim.SetBool(Const.RUN_ANIMATION, false);
    }

    private void WinAnimation()
    {
        anim.SetBool(Const.WIN_ANIMATION, true);
    }

    void Update()
    {
        if (playerController.isJump)
        {
            anim.SetBool(Const.JUMP_ANIMATION, true);
        }
        else
        {
            anim.SetBool(Const.JUMP_ANIMATION, false);
        }


        if (playerController.isRolling)
        {
            anim.SetBool(Const.ROLL_ANIMATION, true);
        }
        else
        {
            anim.SetBool(Const.ROLL_ANIMATION, false);
        }



    }
}
