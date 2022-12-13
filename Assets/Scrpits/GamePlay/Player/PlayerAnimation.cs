using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        GameplayEvents.StartNewLevel += StartMovingAnimation;
    }

    private void OnDisable()
    {
        GameplayEvents.StartNewLevel -= StartMovingAnimation;        
    }

    private void StartMovingAnimation()
    {
        anim.SetBool(Const.RUN_ANIMATION, true);
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
