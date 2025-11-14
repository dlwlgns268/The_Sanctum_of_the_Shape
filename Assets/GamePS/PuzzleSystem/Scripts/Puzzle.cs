using System;
using UnityEngine;
using UnityEngine.EventSystems;


using UnityEngine;
using UnityEngine.EventSystems;

public class Puzzle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Func<GameObject, GameObject> OnSetPuzzle;
    public Action<GameObject,int> OnSetPuzzleCheck;
    [SerializeField] private int puzzleIndex;
    [SerializeField] private bool isDragging = false;
    [SerializeField] private bool isPlaced = false;
    [SerializeField] private Vector2 parentPosition;
    private Vector3 _offset;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        Vector3 mouseWorld = GetMouseWorldPosition();
        _offset = transform.position - mouseWorld;
        transform.localScale = Vector3.one * 1.1f;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        transform.localScale = Vector3.one;
        var obj = OnSetPuzzle?.Invoke(gameObject);
        if (obj != null)
        {
            transform.position = obj.transform.position;
            OnSetPuzzleCheck?.Invoke(obj,puzzleIndex);
            isPlaced = true;
        }
        else
        {
            gameObject.transform.position = parentPosition;
        }
    }

    private void Start()
    {
        parentPosition = transform.position;
    }
    private void Update()
    {
        if (isDragging&&!isPlaced)
        {
            Vector3 mouseWorld = GetMouseWorldPosition();
            transform.position = mouseWorld + _offset;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
        return Camera.main.ScreenToWorldPoint(screenPos);
    }
}