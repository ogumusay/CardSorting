using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardSorting
{
    public class CardSort : MonoBehaviour
    {
        [SerializeField] private CardSettings _cardSettings;
        [SerializeField] private CardView[] _cardViews;
        
        
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
        


        /*
        private Card[] _cards = new[]
        {
            new Card(CardSuit.Spades, CardRank.Queen, 10),
            new Card(CardSuit.Spades, CardRank.King, 3),
            new Card(CardSuit.Spades, CardRank.Jack, 2),
            new Card(CardSuit.Spades, CardRank.Two, 4),
            new Card(CardSuit.Spades, CardRank.Ace, 1),
        };
        */

        private void Sort()
        {
            
        }

        #region 7-7-7 Sorting

        [Button]
        public void SameRankSorting()
        {
            var cards = _cardSettings.GetRandomCards();
            var dictionary = new Dictionary<CardRank, List<Card>>();
            var sameRankList = new List<Card>();
            var nonSameRankList = new List<Card>();
            var list = new List<Card>();
            
            foreach (var card in cards)
            {
                if (dictionary.ContainsKey(card.CardRank))
                {
                    dictionary[card.CardRank].Add(card);
                }
                else
                {
                    dictionary.Add(card.CardRank, new List<Card>(){card});
                }
            }

            foreach (var card in dictionary)
            {
                if (card.Value.Count >= GlobalConst.MIN_NUMBER_OF_SORTED_CARDS)
                {
                    sameRankList.AddRange(card.Value);
                }
                else
                {
                    nonSameRankList.AddRange(card.Value);
                }
            }
            
            list.AddRange(sameRankList);
            list.AddRange(nonSameRankList);
            for (int i = 0; i < list.Count; i++)
            {
                var card = list[i];
                _cardViews[i].Init(_cardSettings.GetCardImage(card.CardSuit, card.CardRank));
            }
        }

        #endregion
        
        #region 1-2-3 Sorting

        [Button]
        private void SortCardSuits()
        {
            var dictionary = new Dictionary<CardSuit, List<Card>>(GlobalConst.NUMBER_OF_CARD_SUITS)
            {
                { CardSuit.Spades, new List<Card>() },
                { CardSuit.Diamonds, new List<Card>() },
                { CardSuit.Hearts, new List<Card>() },
                { CardSuit.Clubs, new List<Card>() }
            };
            var consecutiveList = new List<Card>(); 
            var nonConsecutiveList = new List<Card>();
            var list = new List<Card>();

            var cards = _cardSettings.GetRandomCards();
            
            foreach (var card in cards)
            {
                dictionary[card.CardSuit].Add(card);
            }

            foreach (var card in dictionary)
            {
                InsertionSort(card.Value);
                ConsecutiveSort(card.Value, consecutiveList, nonConsecutiveList);
            }
            
            list.AddRange(consecutiveList);
            list.AddRange(nonConsecutiveList);
            for (int i = 0; i < list.Count; i++)
            {
                var card = list[i];
                _cardViews[i].Init(_cardSettings.GetCardImage(card.CardSuit, card.CardRank));
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
        
        
        #endregion
    }
}
