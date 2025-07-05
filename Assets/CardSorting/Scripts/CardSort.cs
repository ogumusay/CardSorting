using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardSorting
{
    public class CardSort : MonoBehaviour
    {
        [SerializeField] private CardSettings _cardSettings;
        
        /*
        private Card[] _cards = new[]
        {
            new Card(CardSuit.Hearts, CardRank.Ace, 1),
            new Card(CardSuit.Spades, CardRank.Two, 2),
            new Card(CardSuit.Diamonds, CardRank.Five, 5),
            new Card(CardSuit.Hearts, CardRank.Four, 4),
            new Card(CardSuit.Spades, CardRank.Ace, 1),
            new Card(CardSuit.Diamonds, CardRank.Three, 3),
            new Card(CardSuit.Clubs, CardRank.Four, 4),
            new Card(CardSuit.Spades, CardRank.Four, 4),
            new Card(CardSuit.Diamonds, CardRank.Ace, 1),
            new Card(CardSuit.Spades, CardRank.Three, 3),
            new Card(CardSuit.Diamonds, CardRank.Four, 4),
        };
        */



        private Card[] _cards = new[]
        {
            new Card(CardSuit.Spades, CardRank.Queen, 10),
            new Card(CardSuit.Spades, CardRank.King, 3),
            new Card(CardSuit.Spades, CardRank.Jack, 2),
            new Card(CardSuit.Spades, CardRank.Two, 4),
            new Card(CardSuit.Spades, CardRank.Ace, 1),
        };

        private void Sort()
        {
            
        }

        [Button]
        private void SortCardSuits()
        {
            var dictionary = new Dictionary<CardSuit, List<Card>>(GlobalConst.NUMBER_OF_CARD_SUITS);
            dictionary.Add(CardSuit.Spades, new List<Card>());
            dictionary.Add(CardSuit.Diamonds, new List<Card>());
            dictionary.Add(CardSuit.Clubs, new List<Card>());
            dictionary.Add(CardSuit.Hearts, new List<Card>());
            var consecutiveList = new List<Card>();
            var nonConsecutiveList = new List<Card>();

            var cards = _cards;
            
            foreach (var card in cards)
            {
                Debug.Log(card.CardSuit + " " + card.CardRank);
            }
            
            foreach (var card in cards)
            {
                dictionary[card.CardSuit].Add(card);
            }

            foreach (var card in dictionary)
            {
                InsertionSort(card.Value);
                ConsecutiveSort(card.Value, consecutiveList, nonConsecutiveList);
            }
            
            Debug.LogError(">>>>>");
            Debug.LogError(">>>>>");
            Debug.LogError("Consecutive List");
            
            foreach (var card in consecutiveList)
            {
                Debug.Log(card.CardSuit + " " + card.CardRank);
            }
            
            Debug.LogError("Non Consecutive List");
            foreach (var card in nonConsecutiveList)
            {
                Debug.Log(card.CardSuit + " " + card.CardRank);
            }
        }
        
        private void InsertionSort(List<Card> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    if (list[j - 1].CardRank > list[j].CardRank)
                    {
                        (list[j - 1], list[j]) = (list[j], list[j - 1]);
                    }
                }
            }
        }           
        
        private void ConsecutiveSort(List<Card> list, List<Card> consecutiveList, List<Card> nonConsecutiveList)
        {
            int minNumberOfSortedCards = GlobalConst.MIN_NUMBER_OF_SORTED_CARDS;
            if (list.Count < minNumberOfSortedCards)
            {
                nonConsecutiveList.AddRange(list);
                return;
            }

            int consecutiveCounter = 1;
            int index = 0;
                
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i].CardRank + 1 == list[i + 1].CardRank)
                {
                    if (consecutiveCounter == 1) index = i;
                    consecutiveCounter++;
                }
                else
                { 
                    if (consecutiveCounter >= minNumberOfSortedCards)
                    {
                        int upperBound = index + consecutiveCounter;
                        for (int j = index; j < upperBound; j++)
                        {
                            consecutiveList.Add(list[j]);
                        }
                    }
                    else
                    {
                        int upperBound = index + consecutiveCounter;
                        for (int j = index; j < upperBound; j++)
                        {
                            nonConsecutiveList.Add(list[j]);
                        }
                    }
                    
                    index = i + 1;
                    consecutiveCounter = 1;
                }
            }
            
            if (consecutiveCounter >= minNumberOfSortedCards)
            {
                int upperBound = index + consecutiveCounter;
                for (int j = index; j < upperBound; j++)
                {
                    consecutiveList.Add(list[j]);
                }
            }
            else
            {
                int upperBound = index + consecutiveCounter;
                for (int j = index; j < upperBound; j++)
                {
                    nonConsecutiveList.Add(list[j]);
                }
            }
        }       
    }
}
