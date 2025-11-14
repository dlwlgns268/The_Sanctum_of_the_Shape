using UnityEngine;

public class PuzzlePuzzleSetConnecter : MonoBehaviour
{
    [SerializeField] GameObject puzzles;
    [SerializeField] PuzzleSet puzzleSet;

    private void Start()
    {
        foreach (Transform child in puzzles.transform)
        {
            Puzzle puzzle = child.GetComponent<Puzzle>();
            if (puzzle != null)
            {
                puzzle.OnSetPuzzle += puzzleSet.GetNearestPuzzle;
                puzzle.OnSetPuzzleCheck += puzzleSet.CheckCorrectPuzzle;
            }
        }
    }
}