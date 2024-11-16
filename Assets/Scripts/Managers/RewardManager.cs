using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;

    [SerializeField] private Canvas rewardCanvas;

    [SerializeField] private CardPile allCards;

    [SerializeField] private Button skipButton;

    [SerializeField] private GameObject buttonPrefab;

    [SerializeField] private Transform cardContainer;

    private List<ScriptableCard> rewardCards = new List<ScriptableCard>();

    public int cardRewardAmount = 3;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        rewardCanvas.enabled = false;
    }

    public void ShowRewardScreen()
    {
        rewardCanvas.enabled = true;

        foreach(Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        rewardCards = GetRandomCards(allCards, cardRewardAmount);

        foreach (ScriptableCard card in rewardCards)
        {
            GameObject cardButton = CreateCardButton(card);
            cardButton.transform.SetParent(cardContainer, false);
        }

        skipButton.gameObject.SetActive(true);

    }

    private GameObject CreateCardButton(ScriptableCard card)
    {
        GameObject buttonInstance = Instantiate(buttonPrefab, cardContainer);
        Button buttonComponent = buttonInstance.GetComponent<Button>();
        TextMeshProUGUI buttonText = buttonInstance.GetComponentInChildren<TextMeshProUGUI>();

        buttonText.text = card.card_Name;

        buttonComponent.onClick.AddListener(() => OnCardSelected(card));

        return buttonInstance;
    }

    private List<ScriptableCard> GetRandomCards(CardPile allCards, int amount)
    {
        List<ScriptableCard> randomCards = new List<ScriptableCard>();

        for(int i = 0; i < amount; i++)
        {
            int randomIndex = Random.Range(0, allCards.cardsInPile.Count);
            randomCards.Add(allCards.cardsInPile[randomIndex]);
        }

        return randomCards;
    }

    public void OnCardSelected(ScriptableCard card )
    {
        PlayerDeckManager.Instance.AddCardToDeck(card);
        ProceedToNextEncounter();
    }

    public void OnSkipButtonPressed()
    {
        ProceedToNextEncounter();
    }

    //starts another encounter, can be change to fit needs
    private void ProceedToNextEncounter()
    {
        rewardCanvas.enabled = false;
 
        EncounterManager.Instance.StartCombatEncounter();
    }
}
