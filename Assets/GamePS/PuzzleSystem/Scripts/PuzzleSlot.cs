using UnityEngine;
using UnityEngine.EventSystems;


using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleSlot : MonoBehaviour
{
    [SerializeField] private int puzzleIndex;

    public int GetIndex()
    {
        return puzzleIndex;
    }
}

