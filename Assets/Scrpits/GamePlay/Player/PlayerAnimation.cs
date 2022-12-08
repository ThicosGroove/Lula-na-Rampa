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
            anim.SetBool("IsJump", true);
        }
        else
        {
            anim.SetBool("IsJump", false);
        }


        if (playerController.isRolling)
        {
            anim.SetBool("IsRoll", true);
        }
        else
        {
            anim.SetBool("IsRoll", false);
        }
    }
}
