using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("설정")]
    public Transform correctSlot;
    public float snapDistance = 50f;
    
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 startPosition;
    private bool isPlaced = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        
        startPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isPlaced)
        {
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
            transform.SetAsLastSibling();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isPlaced)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (correctSlot != null && !isPlaced)
        {
            RectTransform slotRect = correctSlot.GetComponent<RectTransform>();
            float distance = Vector2.Distance(
                rectTransform.anchoredPosition,
                slotRect.anchoredPosition
            );

            if (distance < snapDistance)
            {
                rectTransform.anchoredPosition = slotRect.anchoredPosition;
                isPlaced = true;
                canvasGroup.blocksRaycasts = false;
                
                Debug.Log($"{gameObject.name} 정답!");
            }
        }
    }

    public void ResetPiece()
    {
        rectTransform.anchoredPosition = startPosition;
        isPlaced = false;
        canvasGroup.blocksRaycasts = true;
    }

    public void Init(RectTransform puzzleArea, Vector2 chosenAnchoredPos, Vector2 target, float curStageSnapDistance, PuzzleManager puzzleManager)
    {
        throw new System.NotImplementedException();
    }
}