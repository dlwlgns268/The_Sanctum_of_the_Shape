using UnityEngine;

[CreateAssetMenu(menuName = "Puzzle/StageData")]
public class StageData : ScriptableObject
{
    public string stageName = "New Stage";
    // target positions: puzzleArea 로컬 anchoredPosition 기준
    public Vector2[] targetPositions;
    public float snapDistance = 40f;
    public bool useOverlapAvoidance = true;
    public float minGapMultiplier = 0.85f; 
}