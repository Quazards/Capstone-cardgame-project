using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardShopManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> cardContainers;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private CardPile cardPile;

private void Awake()
{
    InstantiateCardInContainer(cardPrefab.GetComponent<Card>(), 2);
}

private void Start()
{
    DisplayAvailableCards(cardPile);
}

public void DisplayAvailableCards(CardPile cardPile)
{
    foreach (GameObject container in cardContainers)
    {
       foreach (Transform child in container.transform)
        {
            Destroy(child.gameObject);
        }
    }

    for (int i = 0; i < 8; i++)
{
    int randomIndex = Random.Range(0, cardPile.cardsInPile.Count);

    var cardData = cardPile.cardsInPile[randomIndex];

    int containerIndex = i % cardContainers.Count;
    GameObject cardObject = Instantiate(cardPrefab, cardContainers[containerIndex].transform);
    Card cardComponent = cardObject.GetComponent<Card>();
    cardComponent.SetUp(cardData);

    var movementScript = cardObject.GetComponent<CardMovementAttemp>();
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }
    // Button buyButton = cardObject.GetComponentInChildren<Button>();
    // buyButton.onClick.AddListener(() => PurchaseCard(cardData));
}
}

void InstantiateCardInContainer(Card cardPrefab, int containerIndex)
{
    if (containerIndex < 0 || containerIndex >= cardContainers.Count)
    {
        Debug.LogError("Invalid container index!");
        return;
    }

    GameObject container = cardContainers[containerIndex];
    Card newCard = Instantiate(cardPrefab, container.transform);
}
}