using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CardShowcase : MonoBehaviour
{
    public CardManager cardManager;
    public GameObject cardPrefab;
    public GameObject cardArea;

    public GameObject cantRemoveCardWarning;
    public GameObject confirmRemoveCardWindow;
    public CardUpdater deletingCard;

    private List<CardUpdater> cards;
    private CardUpdater selectedCard;
    private bool isDeletingCard;

    void Start()
    {
        cards = new List<CardUpdater>();
        selectedCard = null;

        LoadAndDisplayAllCards();

        cantRemoveCardWarning.SetActive(false);
        confirmRemoveCardWindow.SetActive(false);
        isDeletingCard = false;
    }

    void Update()
    {
        if (!isDeletingCard && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            CardUpdater newSelectedCard = null;
            foreach (CardUpdater card in cards)
            {
                card.isSelected = false;

                if (hit.collider != null)
                {
                    if (hit.transform == card.transform)
                    {
                        newSelectedCard = hit.transform.GetComponent<CardUpdater>();
                        newSelectedCard.isSelected = true;
                        selectedCard = newSelectedCard;
                        //Debug.Log(selectedCard.transform.name);
                    }
                }

                card.UpdateSelected();
            }

            if (newSelectedCard == null && selectedCard != null)
            {
                selectedCard.isSelected = true;
                selectedCard.UpdateSelected();
            }

            cantRemoveCardWarning.SetActive(false);
        }
    }

    public void RemoveCardButton()
    {
        if (selectedCard == null)
        {
            cantRemoveCardWarning.SetActive(true);
            return;
        }

        deletingCard.card = selectedCard.card;
        deletingCard.UpdateCardValues();

        isDeletingCard = true;
        confirmRemoveCardWindow.SetActive(true);
    }

    public void CancelRemoveCardButton()
    {
        isDeletingCard = false;
        confirmRemoveCardWindow.SetActive(false);
    }

    public void ConfirmRemoveCardButton()
    {
        foreach(Card card in cardManager.cardList)
        {
            if (card.cardName == deletingCard.card.cardName)
            {
                cardManager.cardList.Remove(card);
                break;
            }
        }

        cardManager.SaveCards();
        LoadAndDisplayAllCards();

        isDeletingCard = false;
        confirmRemoveCardWindow.SetActive(false);
    }

    public void LoadForgeScreen()
    {
        SceneManager.LoadScene("CardEditor");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void LoadAndDisplayAllCards()
    {
        cardManager.LoadCards();
        cardManager.SortCardsByCost();

        foreach (Transform child in cardArea.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        cards.Clear();
        foreach (Card card in cardManager.cardList)
        {
            GameObject cardInGame = Instantiate(cardPrefab, cardArea.transform);
            CardUpdater newCard = cardInGame.GetComponent<CardUpdater>();
            newCard.card = card;
            newCard.UpdateCardValues();

            cards.Add(newCard);
        }
    }
}
