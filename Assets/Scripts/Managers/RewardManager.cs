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

    [SerializeField] private List<Transform> rewardParents; 

    [Range(0, 100)] public int commonChance = 70;
    [Range(0, 100)] public int uncommonChance = 28;
    [Range(0, 100)] public int rareChance = 2;

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

        skipButton.gameObject.SetActive(true);
        skipButton.onClick.RemoveAllListeners();
        skipButton.onClick.AddListener(OnSkipButtonPressed);

        GenerateCardRewards();
    }

    private void GenerateCardRewards()
    {
        List<ScriptableCard> selectedCards = GetRandomCardsByRarity();
        ShowCardRewards(selectedCards);
    }

    private List<ScriptableCard> GetRandomCardsByRarity()
    {

        List<ScriptableCard> selectedCards = new List<ScriptableCard>();
        int loopCount = 0;

        while (selectedCards.Count < cardRewardAmount)
        {
            int rarityRoll = Random.Range(0, 100);

            CardRarity selectedRarity;

            if (rarityRoll < rareChance)
            {
                selectedRarity = CardRarity.Rare;
            }
            else if (rarityRoll < rareChance + uncommonChance)
            {
                selectedRarity = CardRarity.Uncommon;
            }
            else
                selectedRarity = CardRarity.Common;

            List<ScriptableCard> filteredCards = allCards.cardsInPile.FindAll(card => card.card_Rarity == selectedRarity);

            if (filteredCards.Count == 0)
                continue;

            int randomIndex = Random.Range(0, filteredCards.Count);
            ScriptableCard selectedCard = filteredCards[randomIndex];

            if (!selectedCards.Contains(selectedCard))
            {
                selectedCards.Add(selectedCard);
            }

            loopCount++;
            if (loopCount > 10)
            {
                Debug.Log("Loop exceeded 10 times");
                break;
            }
        }

        foreach (var card in selectedCards)
        {
            if (card == null)
            {
                Debug.LogError("Null card found in selected cards");
            }
        }


        return selectedCards;
    }

    private void ShowCardRewards(List<ScriptableCard> rewardCards)
    {
        ClearExistingRewards();


        for (int i = 0; i < rewardCards.Count; i++)
        {
            Transform parent = rewardParents[i % rewardParents.Count];
            GameObject cardDisplayButton = Instantiate(buttonPrefab, parent);

            CardUI cardUI = cardDisplayButton.GetComponent<CardUI>();
            cardUI.SetCardData(rewardCards[i]);

            Button button = cardDisplayButton.GetComponent<Button>();
            int cardIndex = i;
            button.onClick.AddListener(() => OnCardSelected(rewardCards[cardIndex]));
        }
    }

    private void OnCardSelected(ScriptableCard card)
    {

        PlayerDeckManager.Instance.AddCardToDeck(card);
        ProceedToNextEncounter();
    }

    private void OnSkipButtonPressed()
    {
        Debug.Log("Skipped rewards.");
        ProceedToNextEncounter();
    }

    private void ProceedToNextEncounter()
    {
        // rewardCanvas.enabled = false;

        // ClearExistingRewards();
        DataPersistenceManager.instance.SaveGame();
        SceneController.Instance.LoadSceneByName("Post-Game Screen");
        // EncounterManager.Instance.StartCombatEncounter();
    }

    private void ClearExistingRewards()
    {
        foreach (Transform parent in rewardParents)
        {
            foreach (Transform child in parent)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
