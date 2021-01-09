using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggingInventory : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    [SerializeField] private RectTransform inventoryWindow;
    private Vector2 lastMousePosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        lastMousePosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentMousePosition = eventData.position;
        Vector2 diff = currentMousePosition - lastMousePosition;
        RectTransform rect = GetComponent<RectTransform>();

        Vector3 newPosition = inventoryWindow.position + new Vector3(diff.x, diff.y, inventoryWindow.transform.position.z);
        Vector3 oldPos = inventoryWindow.position;
        inventoryWindow.position = newPosition;
        if (!IsRectTransformInsideSreen(rect))
        {
            inventoryWindow.position = oldPos;
        }
        lastMousePosition = currentMousePosition;
    }

    private bool IsRectTransformInsideSreen(RectTransform topHandle)
    {
        bool isInside = false;
        Vector3[] corners = new Vector3[4];
        topHandle.GetWorldCorners(corners);
        int visibleCorners = 0;
        Rect rect = new Rect(0, 0, Screen.width, Screen.height);
        foreach (Vector3 corner in corners)
        {
            if (rect.Contains(corner))
            {
                visibleCorners++;
            }
        }
        if (visibleCorners == 4)
        {
            isInside = true;
        }
        return isInside;
    }
}