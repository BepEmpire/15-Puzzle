using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    public Difficulty difficulty;
    public float timeLimit;
    public int reward;
}