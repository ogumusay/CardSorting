using System.Collections.Generic;
using UnityEngine;

namespace CardSorting
{
    [CreateAssetMenu(menuName = "Card Sorting/Card Settings")]
    public class CardSettings : ScriptableObject
    {
        public CardSuitData[] cardSuits;
        public Sprite[] cardBackgroundThemes;

        public List<Card> GetRandomCards()
        {
            var list = new List<Card>();
            var rndList = new List<Card>();
            foreach (var cardSuitData in cardSuits)
            {
                foreach (var cardRankData in cardSuitData.cardRanks)
                {
                    list.Add(new Card(cardSuitData.cardSuit, cardRankData.cardRank, cardRankData.cardValue));
                }
            }

            for (int i = 0; i < 11; i++)
            {
                var rnd = Random.Range(0, list.Count);
                rndList.Add(list[rnd]);
                list.RemoveAt(rnd);
            }

            return rndList;
        }

        public Sprite GetCardImage(CardSuit cardSuit, CardRank cardRank)
        {
            foreach (var cardSuitData in cardSuits)
            {
                if (cardSuitData.cardSuit == cardSuit)
                {
                    foreach (var cardRankData in cardSuitData.cardRanks)
                    {
                        if (cardRankData.cardRank == cardRank)
                        {
                            return cardRankData.cardImage;
                        }
                    }
                }
            }

            return null;
        }

        public Card GetCard(CardSuit cardSuit, CardRank cardRank)
        {
            foreach (var cardSuitData in cardSuits)
            {
                if (cardSuitData.cardSuit != cardSuit) continue;
                
                foreach (var cardRankData in cardSuitData.cardRanks)
                {
                    if (cardRank != cardRankData.cardRank) continue;

                    return new Card(cardSuit, cardRank, cardRankData.cardValue);
                }
            }

            return new Card(CardSuit.Spades, CardRank.Ace, 1);
        }
    }
}
