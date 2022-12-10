using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();
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
