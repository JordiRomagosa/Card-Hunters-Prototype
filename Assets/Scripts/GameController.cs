using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public const uint DRAW_ENERGY_COST = 2;
    public const uint DISCARD_ENERGY_BONUS = 1;
    public const uint INITIAL_CARD_DRAW = 5;

    public Deck playerDeck;
    public Hand playerHand;

    public GameObject canvas;
    public GameObject cardPlayZone;
    public GameObject cardDiscardZone;
    public GameObject cardHandZone;
    public GameObject cardPrefab;

    public StatusController playerStatus;
    public Character player;
    public StatusController enemyStatus;
    public Character enemy;
    public EnemyAI enemyAI;

    public Button endTurnButton;

    private List<Card> cardsPlayed;
    public Text discardCountDisplay;
    private List<Card> discardedCards;

    void Start()
    {
        cardsPlayed = new List<Card>();
        discardedCards = new List<Card>();

        enemyAI.enemy = enemy;

        StartFirstTurn();
    }

    public void ButtonDrawCard()
    {
        if (player.energyCurrent < DRAW_ENERGY_COST)
        {
            return;
        }
        player.energyCurrent -= DRAW_ENERGY_COST;
        DrawCard();
        UpdateCharactersStatus(true, false);
    }

    public void DrawCard()
    {
        if (playerHand.MaximumHandIsReached())
        {
            //Debug.Log("Max hand reached");
        }

        if (playerDeck.DeckIsEmpty())
        {
            playerDeck.ShuffleDiscardsExceptPlayed(playerHand.cardsInHand, cardsPlayed);
            discardedCards.Clear();
            discardCountDisplay.text = discardedCards.Count.ToString();

            
        }

        Card drawnCard = playerDeck.DrawNextCard();
        playerHand.AddCardToHand(drawnCard);
        playerHand.ShowHandCards();
    }

    public Vector2 GetCanvasMousePosition()
    {
        Vector2 localPoint;
        RectTransform canvasRect = (RectTransform)canvas.transform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, canvas.GetComponent<Canvas>().worldCamera, out localPoint);

        return localPoint / 57.0f;
    }

    public bool DropCard(uint cardID)
    {
        Vector2 mousePos = GetCanvasMousePosition();
        Card playingCard = playerDeck.GetCardWithID(cardID);

        RectTransform rt;
        Vector3[] v = new Vector3[4];

        //If card is played, check return hand
        if (cardsPlayed.Contains(playingCard))
        {
            rt = (RectTransform)cardHandZone.transform;
            rt.GetWorldCorners(v);

            if (mousePos.x > v[0].x && mousePos.x < v[2].x && mousePos.y > v[0].y && mousePos.y < v[2].y)
            {
                cardsPlayed.Remove(playingCard);
                playerHand.AddCardToHand(playingCard);
                player.RemoveCardToUse(playingCard);

                ShowPlayedCards();
                playerHand.ShowHandCards();
                UpdateCharactersStatus(true, false);
                return true;
            }
        }

        //Drop on play zone
        rt = (RectTransform)cardPlayZone.transform;
        rt.GetWorldCorners(v);

        if (mousePos.x > v[0].x && mousePos.x < v[2].x && mousePos.y > v[0].y && mousePos.y < v[2].y)
        {
            if (!player.CanUseCard(playingCard))
            {
                return false;
            }

            Card removedCard = playerHand.RemoveCardFromHand(cardID);
            cardsPlayed.Add(removedCard);
            player.PrepareCardToUse(removedCard);
            UpdateCharactersStatus(true, false);

            playerHand.ShowHandCards();
            ShowPlayedCards();
            return true;
        }

        //Drop on discard zone
        rt = (RectTransform)cardDiscardZone.transform;
        rt.GetWorldCorners(v);

        if (mousePos.x > v[0].x && mousePos.x < v[2].x && mousePos.y > v[0].y && mousePos.y < v[2].y)
        {
            Card removedCard = playerHand.RemoveCardFromHand(cardID);
            discardedCards.Add(removedCard);

            playerHand.ShowHandCards();
            discardCountDisplay.text = discardedCards.Count.ToString();

            player.energyCurrent += DISCARD_ENERGY_BONUS;
            UpdateCharactersStatus(true, false);
            return true;
        }

        return false;
    }

    public void StartFirstTurn()
    {
        for (int i = 0; i < INITIAL_CARD_DRAW; i++)
        {
            DrawCard();
        }

        enemyAI.StartFirstTurn(player);

        UpdateScreenForNewTurn();
    }

    public void EndTurnAndStartNext()
    {
        PrepareNewTurn();
        Invoke("ApplyCardEffectsAndStartNewTurn", 3.0f);
    }

    private void PrepareNewTurn()
    {
        endTurnButton.interactable = false;
        enemyAI.ShowCardEffects();
        UpdateCharactersStatus(true, true, true);
    }

    private void ApplyCardEffectsAndStartNewTurn()
    {
        ApplyCardEffects();
        StartNewTurn();
    }

    private void ApplyCardEffects()
    {
        int maxCardsCount = cardsPlayed.Count;
        if (enemyAI.cardsPlayed.Count > maxCardsCount)
        {
            maxCardsCount = enemyAI.cardsPlayed.Count;
        }

        Card card;
        for (int i = 0; i < maxCardsCount; i++)
        {
            if (i < cardsPlayed.Count)
            {
                card = cardsPlayed[i];
                player.RealizeCardActionsReturnDamage(enemy, card);
                discardedCards.Add(card);
            }

            if (i < enemyAI.cardsPlayed.Count)
            {
                card = enemyAI.cardsPlayed[i];
                enemy.RealizeCardActionsReturnDamage(player, card);
                enemyAI.discardedCards.Add(card);
            }
        }

        cardsPlayed = new List<Card>();
        enemyAI.cardsPlayed = new List<Card>();
    }

    private void StartNewTurn()
    {
        for (int i = 0; i < player.cardDraw; i++)
        {
            DrawCard();
        }

        player.StartNextTurn();
        enemy.StartNextTurn();
        enemyAI.StartNewTurn(player);

        UpdateScreenForNewTurn();
        endTurnButton.interactable = true;
    }

    private void UpdateScreenForNewTurn()
    {
        UpdateCharactersStatus(true, true);
        ShowPlayedCards();
        playerHand.ShowHandCards();
        discardCountDisplay.text = discardedCards.Count.ToString();
    }

    private void ShowPlayedCards()
    {
        foreach (Transform child in cardPlayZone.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Card card in cardsPlayed)
        {
            GameObject cardPlayed = Instantiate(cardPrefab, cardPlayZone.transform);
            CardUpdater newCard = cardPlayed.GetComponent<CardUpdater>();
            newCard.card = card;
            newCard.UpdateCardValues();

            cardPlayed.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void UpdateCharactersStatus(bool updatePlayer, bool updateEnemy, bool showEnemyShield = false)
    {
        if (updatePlayer)
        {
            playerStatus.UpdateStatus(player);
        }
        if (updateEnemy)
        {
            enemyStatus.UpdateStatus(enemy, showEnemyShield);
        }
    }
}
