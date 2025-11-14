using System;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSet : MonoBehaviour
{
    private readonly Dictionary<GameObject,int> _puzzlePrefabs= new();
    [Header("퍼즐 오프셋")] [SerializeField] private float offset;
    [Header("남은 퍼즐 개수")][SerializeField] private int puzzleCount;
    
    void Start()
    {
        InitializePuzzle();
    }

    public void InitializePuzzle()
    {
        _puzzlePrefabs.Clear();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            var idx = gameObject.transform.GetChild(i).GetComponent<PuzzleSlot>()?.GetIndex();
            var pzl = gameObject.transform.GetChild(i).gameObject;
            if (idx != null&&!_puzzlePrefabs.ContainsKey(pzl)) _puzzlePrefabs.Add(pzl,(int)idx);
        }
        puzzleCount = gameObject.transform.childCount;
    }
    
    public GameObject GetNearestPuzzle(GameObject puzzle)
    {
        Debug.Log(puzzle);
        var minDistance = float.MaxValue;
        GameObject nearestPuzzle = null;
        foreach (KeyValuePair<GameObject, int> puzzlePrefab in _puzzlePrefabs)
        {
            var dist = Vector3.Distance(puzzlePrefab.Key.transform.position, puzzle.transform.position);
            if (dist<offset && dist< minDistance)
            {
                minDistance = dist;
                nearestPuzzle = puzzlePrefab.Key;
            }
        }
        return nearestPuzzle;
    }

    public void CheckCorrectPuzzle(GameObject puzzleSlot,int currentPuzzleIndex)
    {
        if (_puzzlePrefabs.ContainsKey(puzzleSlot) && _puzzlePrefabs[puzzleSlot] == currentPuzzleIndex)
        {
            puzzleCount--;
            if (puzzleCount <= 0)
            {
                Debug.Log("승리");
            }
        }
        else
        {
            Debug.Log("패배");
        }
    }
}
