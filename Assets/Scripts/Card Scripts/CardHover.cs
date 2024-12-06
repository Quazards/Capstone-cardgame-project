using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 initialScale;
    [HideInInspector] public bool isPointerOverCard = false;
    [HideInInspector] public bool isHovering;
    public bool isNotCardDisplay = true;

    private CardRotation cardRotation;
    
    private void Start()
    {
        initialScale = Vector3.one;
        cardRotation = GetComponent<CardRotation>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isNotCardDisplay)
        {
            if (!cardRotation.isDragging)
            {
                isPointerOverCard = true;
                isHovering = true;
                transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOverCard = false;
        isHovering = false;
        transform.localScale = initialScale;
    }
}
