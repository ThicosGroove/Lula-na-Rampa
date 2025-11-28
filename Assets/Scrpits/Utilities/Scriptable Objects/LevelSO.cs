using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "ScriptableObjects/LevelSO", order = 1)]
public class LevelSO : ScriptableObject
{
    [Header("Level Info")]
    public int level;

    [Header("Player")]
    public float player_Slide_Speed;
    public float player_Jump_Height;
    public float player_Gravity;
    public float player_Roll_Speed;

    [Header("Obstacles")]
    public float obstacle_Speed;
    public float obstacle_Spawn_Distance;

    [Header("Player Animation Multipliers")]
    [Tooltip("1 = Velocidade Normal. 1.5 = 50% mais rápido.")]
    public float anim_Jump_Speed_Multi = 1.0f;
    public float anim_Roll_Speed_Multi = 1.0f;
    public float anim_Run_Speed_Multi = 1.0f;
}
