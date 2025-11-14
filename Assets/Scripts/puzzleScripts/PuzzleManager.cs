using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("UI 영역")]
    public RectTransform puzzleArea;  
    public RectTransform spawnArea;    

    [Header("프리팹 / 스테이지")]
    public GameObject piecePrefab;    
    public List<StageData> stages;    

    [Header("초기 설정")]
    public int startStageIndex = 0;
    public int maxSpawnAttempts = 30;

    private readonly List<PuzzlePiece> _runtimePieces = new List<PuzzlePiece>();
    private StageData _curStage;
    private int _snappedCount = 0;

    void Start()
    {
        LoadStage(startStageIndex);
    }

    public void LoadStage(int index)
    {
        ClearRuntime();
        if (index < 0 || index >= stages.Count) return;
        _curStage = stages[index];
        
        int targetCount = _curStage.targetPositions != null ? _curStage.targetPositions.Length : 0;
        if (targetCount == 0) return;

        RectTransform prefabRect = piecePrefab.GetComponent<RectTransform>();
        Vector2 prefabSize = prefabRect != null ? prefabRect.sizeDelta : new Vector2(100, 100);
        float prefabDiag = Mathf.Sqrt(prefabSize.x * prefabSize.x + prefabSize.y * prefabSize.y);
        float minGap = prefabDiag * 0.5f * _curStage.minGapMultiplier;

        List<Vector2> placedPositions = new List<Vector2>();

        for (int i = 0; i < targetCount; i++)
        {
            GameObject go = Instantiate(piecePrefab, puzzleArea);
            PuzzlePiece p = go.GetComponent<PuzzlePiece>();

            Vector2 chosenAnchoredPos;

            if (_curStage.useOverlapAvoidance)
            {
                chosenAnchoredPos = FindNonOverlappingAnchoredPosition(minGap, placedPositions, maxSpawnAttempts);
                if (chosenAnchoredPos == Vector2.positiveInfinity)
                {
                    chosenAnchoredPos = WorldToAnchoredPosition(RandomPointInRectTransformWorld(spawnArea), puzzleArea);
                }
            }
            else
            {
                chosenAnchoredPos = WorldToAnchoredPosition(RandomPointInRectTransformWorld(spawnArea), puzzleArea);
            }

            if (_curStage.targetPositions != null)
            {
                Vector2 target = _curStage.targetPositions[i];

                p.Init(puzzleArea, chosenAnchoredPos, target, _curStage.snapDistance, this);
            }

            _runtimePieces.Add(p);
            placedPositions.Add(chosenAnchoredPos);
        }

        _snappedCount = 0;
    }

    void ClearRuntime()
    {
        foreach (var p in _runtimePieces)
            if (p != null) Destroy(p.gameObject);
        _runtimePieces.Clear();
        _snappedCount = 0;
    }

    // PuzzlePiece가 스냅되면 호출 (이벤트 방식)
    public void NotifyPieceSnapped(PuzzlePiece piece)
    {
        _snappedCount++;
        // 빠른 체크: 만약 snappedCount == 전체 개수면 클리어
        if (_snappedCount >= _runtimePieces.Count)
        {
            OnClear();
        }
    }

    void OnClear()
    {
        Debug.Log("Stage Clear: " + (_curStage != null ? _curStage.stageName : "unknown"));
        // 여기에 연출/사운드/다음스테이지 로직 연결
    }

    Vector3 RandomPointInRectTransformWorld(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners); // 0=bl,1=tl,2=tr,3=br
        Vector3 bl = corners[0];
        Vector3 tr = corners[2];

        float x = Random.Range(bl.x, tr.x);
        float y = Random.Range(bl.y, tr.y);
        float z = (bl.z + tr.z) * 0.5f;
        return new Vector3(x, y, z);
    }

    Vector2 WorldToAnchoredPosition(Vector3 worldPos, RectTransform targetArea)
    {
        Camera cam = targetArea.GetComponentInParent<Canvas>()?.worldCamera;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            targetArea,
            RectTransformUtility.WorldToScreenPoint(cam, worldPos),
            cam,
            out localPoint
        );
        return localPoint;
    }

    Vector2 FindNonOverlappingAnchoredPosition(float minGap, List<Vector2> placedPositions, int maxAttempts)
    {
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Vector3 randomWorldPos = RandomPointInRectTransformWorld(spawnArea);
            Vector2 candidate = WorldToAnchoredPosition(randomWorldPos, puzzleArea);

            bool ok = true;
            foreach (var pos in placedPositions)
            {
                if ((pos - candidate).sqrMagnitude <= minGap * minGap)
                {
                    ok = false;
                    break;
                }
            }

            if (ok) return candidate;
        }

        return Vector2.positiveInfinity;
    }
}
