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
            Debug.LogWarning("Pulou");

            anim.SetBool("IsJump", true);
            StartCoroutine(JumpAnimationDelay());
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

    IEnumerator JumpAnimationDelay()
    {
        yield return new WaitForSeconds(1f);

        playerController.isJump = false;
        anim.SetBool("IsJump", false);
    }
}
