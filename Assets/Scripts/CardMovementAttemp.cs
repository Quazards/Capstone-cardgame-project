using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovementAttemp : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private bool isDragged;
    private Canvas cardCanvas;
    private RectTransform rectTransform;
    private Card card;
    private GameObject Hand;

    private readonly string CANVAS_TAG = "CardCanvas";

    private void Start()
    {
        cardCanvas = GameObject.FindGameObjectWithTag(CANVAS_TAG).GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        card = GetComponent<Card>();

        Hand = GameObject.FindGameObjectWithTag("Hand");
        card.transform.SetParent(Hand.transform);
        card.transform.localScale = Vector3.one;
        card.transform.position = new Vector3(transform.position.x, transform.position.y);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += (eventData.delta / cardCanvas.scaleFactor);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragged = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragged= false;
    }


}
