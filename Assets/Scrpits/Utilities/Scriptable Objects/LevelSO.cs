using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "ScriptableObjects/LevelSO", order = 1)]
public class LevelSO : ScriptableObject
{
    public int level;
    public float minDistChangeSpeed;

    public float obstacle_Initial_Speed;
    public float obstacle_Speed;
    public float obstacle_Spawn_Delay;
    public float collectable_Spawn_Delay;
    public float player_Slide_Speed;
    public float player_Jump_Speed;
    public float player_Roll_Speed;

    [Header("Caminhão Patriota")]
    public float speedMulti;
}
