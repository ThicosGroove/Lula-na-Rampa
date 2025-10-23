using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class PlayerAnimation : MonoBehaviour
{
    PlayerMovement playerMovement;
    [SerializeField] Animator anim;

    int currentLevel;
    bool hasWin = false;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        UtilityEvents.GamePause += IdleAnimation;
        UtilityEvents.GameResume += StartMovingAnimation;

        GameplayEvents.StartNewLevel += StartMovingAnimation;
        GameplayEvents.GameOver += GameOverAnimation;

        ScoreEvents.ChangeLevel += UpdateCurrentLevel;

        GameplayEvents.ReachPalace += WinPreparation;
        GameplayEvents.DropFaixa += WinAnimation;
    }

    private void OnDisable()
    {
        UtilityEvents.GamePause -= IdleAnimation;
        UtilityEvents.GameResume -= StartMovingAnimation;

        GameplayEvents.StartNewLevel -= StartMovingAnimation;
        GameplayEvents.GameOver -= GameOverAnimation;

        ScoreEvents.ChangeLevel -= UpdateCurrentLevel;

        GameplayEvents.ReachPalace -= WinPreparation;
        GameplayEvents.DropFaixa -= WinAnimation;
    }
    void Update()
    {
        UpdateAnimations();
        if (!GamePlayManager.Instance.isNormalMode)
        {
            UpdateAnimationSpeedPerLevel();
        }

        if (hasWin)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 180, 0)), 0.01f);
        }
    }

    void UpdateCurrentLevel(int _)
    {
        currentLevel++;
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
        Debug.Log("Win Preparation");
        anim.SetBool(Const.RUN_ANIMATION, false);

        GameplayEvents.OnDropFaixa();
    }

    private void WinAnimation()
    {
        Debug.Log("Set Anim Win True");
        hasWin = true;
        
        anim.SetBool(Const.WIN_ANIMATION, true);
    }

    void UpdateAnimations()
    {
        if (playerMovement.isGrounded)
        {
            anim.SetBool(Const.JUMP_ANIMATION, false);
        }
        else
        {
            anim.SetBool(Const.JUMP_ANIMATION, true);
        }


        if (playerMovement.isRolling)
        {
            anim.SetBool(Const.ROLL_ANIMATION, true);
        }
        else
        {
            anim.SetBool(Const.ROLL_ANIMATION, false);
        }
    }

    void UpdateAnimationSpeedPerLevel()
    {
        switch (currentLevel)
        {

            case 1:
                anim.SetFloat(Const.JUMP_SPEED_ANIMATION, 1);
                anim.SetFloat(Const.ROLL_SPEED_ANIMATION, 1f);
                break;
            case 2:
                anim.SetFloat(Const.JUMP_SPEED_ANIMATION, 1);
                anim.SetFloat(Const.ROLL_SPEED_ANIMATION, 1f);
                break;
            case 3:
                anim.SetFloat(Const.JUMP_SPEED_ANIMATION, 1);
                anim.SetFloat(Const.ROLL_SPEED_ANIMATION, 1.2f);
                break;
            case 4:
                anim.SetFloat(Const.JUMP_SPEED_ANIMATION, 1);
                anim.SetFloat(Const.ROLL_SPEED_ANIMATION, 1.2f);
                break;
            case 5:
                anim.SetFloat(Const.JUMP_SPEED_ANIMATION, 1);
                anim.SetFloat(Const.ROLL_SPEED_ANIMATION, 1.3f);
                break;
            case 6:
                anim.SetFloat(Const.JUMP_SPEED_ANIMATION, 1.2f);
                anim.SetFloat(Const.ROLL_SPEED_ANIMATION, 1.4f);
                break;
            case 7:
                anim.SetFloat(Const.JUMP_SPEED_ANIMATION, 1.3f);
                anim.SetFloat(Const.ROLL_SPEED_ANIMATION, 1.4f);
                break;
            case 8:
                anim.SetFloat(Const.JUMP_SPEED_ANIMATION, 1.3f);
                anim.SetFloat(Const.ROLL_SPEED_ANIMATION, 1.5f);
                break;
            case 9:
                anim.SetFloat(Const.JUMP_SPEED_ANIMATION, 1.3f);
                anim.SetFloat(Const.ROLL_SPEED_ANIMATION, 1.5f);
                break;
            case 10:
                anim.SetFloat(Const.JUMP_SPEED_ANIMATION, 1.3f);
                anim.SetFloat(Const.ROLL_SPEED_ANIMATION, 1.7f);
                break;
            case 11:
                anim.SetFloat(Const.JUMP_SPEED_ANIMATION, 1.5f);
                anim.SetFloat(Const.ROLL_SPEED_ANIMATION, 1.7f);
                break;
            case 12:
                anim.SetFloat(Const.JUMP_SPEED_ANIMATION, 1.5f);
                anim.SetFloat(Const.ROLL_SPEED_ANIMATION, 2.2f);
                break;
            case 13:
                anim.SetFloat(Const.JUMP_SPEED_ANIMATION, 1.8f);
                anim.SetFloat(Const.ROLL_SPEED_ANIMATION, 2.3f);
                break;
            default:
                break;
        }
    }
}
