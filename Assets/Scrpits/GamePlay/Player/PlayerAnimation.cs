using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [SerializeField] private Animator anim;

    private bool hasWin = false;

    void Start()
    {
        // Garante que pegamos o Animator (seja no objeto ou filhos)
        if (anim == null) anim = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        // Inicializa com os dados do nível atual assim que o jogo abre
        if (LevelManager.Instance != null)
        {
            UpdateAnimSpeedsFromData(LevelManager.Instance.GetCurrentLevelIndex());
        }
    }

    private void OnEnable()
    {
        UtilityEvents.GamePause += IdleAnimation;
        UtilityEvents.GameResume += StartMovingAnimation;
        GameplayEvents.StartNewLevel += StartMovingAnimation;
        GameplayEvents.GameOver += GameOverAnimation;

        // Evento de mudança de nível
        ScoreEvents.ChangeLevel += OnLevelChanged;

        GameplayEvents.ReachPalace += WinPreparation;
        GameplayEvents.DropFaixa += WinAnimation;
    }

    private void OnDisable()
    {
        UtilityEvents.GamePause -= IdleAnimation;
        UtilityEvents.GameResume -= StartMovingAnimation;
        GameplayEvents.StartNewLevel -= StartMovingAnimation;
        GameplayEvents.GameOver -= GameOverAnimation;

        ScoreEvents.ChangeLevel -= OnLevelChanged;

        GameplayEvents.ReachPalace -= WinPreparation;
        GameplayEvents.DropFaixa -= WinAnimation;
    }

    void Update()
    {
        // Apenas lógica de estados (Booleanos) fica no Update
        UpdateAnimationStates();

        if (hasWin)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 180, 0)), 5f * Time.deltaTime);
        }
    }

    // --- NOVA LÓGICA: Sincronização via Scriptable Object ---

    void OnLevelChanged(int levelIndex)
    {
        UpdateAnimSpeedsFromData(levelIndex);
    }

    void UpdateAnimSpeedsFromData(int levelIndex)
    {
        if (LevelManager.Instance == null) return;

        // Pega o SO do nível atual através do Manager
        LevelSO data = LevelManager.Instance.GetLevelData(levelIndex);

        if (data != null && anim != null)
        {
            // Aplica os valores que você configurou no arquivo do nível
            anim.SetFloat(Const.JUMP_SPEED_ANIMATION, data.anim_Jump_Speed_Multi);
            anim.SetFloat(Const.ROLL_SPEED_ANIMATION, data.anim_Roll_Speed_Multi);
            anim.SetFloat(Const.RUN_SPEED_ANIMATION, data.anim_Run_Speed_Multi);
        }
    }
    

    void UpdateAnimationStates()
    {
        // Lógica simplificada
        anim.SetBool(Const.JUMP_ANIMATION, !playerMovement.isGrounded);
        anim.SetBool(Const.ROLL_ANIMATION, playerMovement.isRolling);
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
        GameplayEvents.OnDropFaixa();
    }

    private void WinAnimation()
    {
        hasWin = true;
        anim.SetBool(Const.WIN_ANIMATION, true);
    }
}