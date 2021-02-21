using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public const string PLAYER_DEFAULT_SAVE_FILE = "Assets/deck.json";

    public string saveFile = PLAYER_DEFAULT_SAVE_FILE;

    public List<Card> cardList;

    public void LoadCards()
    {
        cardList = new List<Card>();

        var sr = File.OpenText(saveFile);
        var line = sr.ReadLine();
        while (line != null)
        {
            cardList.Add(JsonUtility.FromJson<Card>(line));
            line = sr.ReadLine();
        }
        sr.Close();
    }

    public List<Card> LoadDeck(string saveFile)
    {
        this.saveFile = saveFile;
        LoadCards();
        return cardList;
    }

    public void SaveCards()
    {
        if (File.Exists(saveFile))
        {
            File.Delete(saveFile);
        }

        var sw = File.CreateText(saveFile);
        string json;

        for (var i = 0; i < cardList.Count; i++)
        {
            json = JsonUtility.ToJson(cardList[i]);
            sw.WriteLine(json);
        }
        sw.Close();
    }

    public void AddCardToSaveFile(Card newCard)
    {
        StreamWriter sw = null;
        if (!File.Exists(saveFile))
        {
            sw = File.CreateText(saveFile);
        }
        if (sw == null)
        {
            sw = File.AppendText(saveFile);
        }

        string json = JsonUtility.ToJson(newCard);
        sw.WriteLine(json);
        sw.Close();
    }

    public void SortCardsByCost()
    {
        List<Card> sortedCards = new List<Card>();
        uint currentEnergyCost = 0;

        while (sortedCards.Count < cardList.Count)
        {
            foreach (Card card in cardList)
            {
                if (card.energyCost == currentEnergyCost)
                {
                    sortedCards.Add(card);
                }
            }
            currentEnergyCost += 1;
        }

        cardList = sortedCards;
    }
}
