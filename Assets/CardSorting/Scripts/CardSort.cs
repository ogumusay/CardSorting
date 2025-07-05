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
            new Card(CardSuit.Clubs, CardRank.Seven, 7),
            new Card(CardSuit.Clubs, CardRank.Three, 3),
            new Card(CardSuit.Clubs, CardRank.King, 10),
            new Card(CardSuit.Hearts, CardRank.Ten, 10),
            new Card(CardSuit.Diamonds, CardRank.Ten, 10),
            new Card(CardSuit.Spades, CardRank.Nine, 9),
            new Card(CardSuit.Diamonds, CardRank.Four, 4),
            new Card(CardSuit.Diamonds, CardRank.Two, 2),
            new Card(CardSuit.Clubs, CardRank.Ace, 1),
            new Card(CardSuit.Clubs, CardRank.Two, 2),
            new Card(CardSuit.Hearts, CardRank.Two, 2),
        };

        #region 7-7-7 Sorting

        [Button]
        private void RandomCards()
        {
            _cards = _cardSettings.GetRandomCards();
        }
        
        [Button]
        public void SameRankSorting()
        {
            var cardRankDictionary = new Dictionary<CardRank, List<Card>>();
            var sameRankList = new List<Card>();
            var nonSameRankList = new List<Card>();
            var sortedList = new List<Card>();
            
            foreach (var card in _cards)
            {
                if (cardRankDictionary.ContainsKey(card.CardRank))
                {
                    cardRankDictionary[card.CardRank].Add(card);
                }
                else
                {
                    cardRankDictionary.Add(card.CardRank, new List<Card>(){card});
                }
            }

            foreach (var card in cardRankDictionary)
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
            
            sortedList.AddRange(sameRankList);
            sortedList.AddRange(nonSameRankList);
            
            for (int i = 0; i < sortedList.Count; i++)
            {
                var card = sortedList[i];
                _cardViews[i].Init(_cardSettings.GetCardImage(card.CardSuit, card.CardRank));
            }
        }

        #endregion
        
        #region 1-2-3 Sorting

        [Button]
        private void SortCardSuits()
        {
            var cardSuitDictionary = new Dictionary<CardSuit, List<Card>>(GlobalConst.NUMBER_OF_CARD_SUITS)
            {
                { CardSuit.Spades, new List<Card>() },
                { CardSuit.Diamonds, new List<Card>() },
                { CardSuit.Hearts, new List<Card>() },
                { CardSuit.Clubs, new List<Card>() }
            };
            
            var consecutiveList = new List<ConsecutiveSortData>();
            var sortedList = new List<Card>();
            
            foreach (var card in _cards)
            {
                cardSuitDictionary[card.CardSuit].Add(card);
            }

            foreach (var card in cardSuitDictionary)
            {
                InsertionSort(card.Value);
                var consecutiveSortData = ConsecutiveSort(card.Value);
                consecutiveList.Add(consecutiveSortData);
            }

            foreach (var consecutiveSortData in consecutiveList)
            {
                sortedList.AddRange(consecutiveSortData.ConsecutiveList);
            }            
            foreach (var consecutiveSortData in consecutiveList)
            {
                sortedList.AddRange(consecutiveSortData.NonConsecutiveList);
            }
            
            for (int i = 0; i < sortedList.Count; i++)
            {
                var card = sortedList[i];
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
        
        private ConsecutiveSortData ConsecutiveSort(List<Card> list)
        {
            List<Card> consecutiveList = new();
            List<Card> nonConsecutiveList = new();
            
            int minNumberOfSortedCards = GlobalConst.MIN_NUMBER_OF_SORTED_CARDS;
            if (list.Count < minNumberOfSortedCards)
            {
                nonConsecutiveList.AddRange(list);
                return new ConsecutiveSortData()
                {
                    ConsecutiveList = consecutiveList,
                    NonConsecutiveList = nonConsecutiveList
                };
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
            
            return new ConsecutiveSortData()
            {
                ConsecutiveList = consecutiveList,
                NonConsecutiveList = nonConsecutiveList
            };
        }    
        
        
        #endregion

        #region Smart Sorting

        [Button]
        private void SmartSorting()
        {
            // CONSECUTIVE SORTING
            var cardSuitDictionary = new Dictionary<CardSuit, List<Card>>(GlobalConst.NUMBER_OF_CARD_SUITS)
            {
                { CardSuit.Spades, new List<Card>() },
                { CardSuit.Diamonds, new List<Card>() },
                { CardSuit.Hearts, new List<Card>() },
                { CardSuit.Clubs, new List<Card>() }
            };
            
            var allCombinations = new List<List<Card>>();
            var mergedCombinations = new List<List<Card>>();
            
            foreach (var card in _cards)
            {
                cardSuitDictionary[card.CardSuit].Add(card);
            }

            foreach (var card in cardSuitDictionary)
            {
                InsertionSort(card.Value);
                var sortData = ConsecutiveSort(card.Value);
                allCombinations.AddRange(FindConsecutiveSubsequences(sortData.ConsecutiveList));
            }

            // SAME RANK SORTING
            var cardRankDictionary = new Dictionary<CardRank, List<Card>>();
            
            foreach (var card in _cards)
            {
                if (cardRankDictionary.ContainsKey(card.CardRank))
                {
                    cardRankDictionary[card.CardRank].Add(card);
                }
                else
                {
                    cardRankDictionary.Add(card.CardRank, new List<Card>(){card});
                }
            }
            
            foreach (var card in cardRankDictionary)
            {
                if (card.Value.Count >= GlobalConst.MIN_NUMBER_OF_SORTED_CARDS)
                {
                    allCombinations.AddRange(FindConsecutiveSubsequences(card.Value));
                }
            }


            if (allCombinations.Count == 1)
            {
                mergedCombinations.Add(allCombinations[0]);
            }
            else
            {
                for (int i = 0; i < allCombinations.Count - 1; i++)
                {
                    List<Card> tempCardList = new List<Card>();
                    tempCardList.AddRange(allCombinations[i]);
                    for (int j = i + 1; j < allCombinations.Count; j++)
                    {
                        if (CanCombine(tempCardList, allCombinations[j]))
                        {
                            tempCardList.AddRange(allCombinations[j]);
                        }
                    }
                
                    mergedCombinations.Add(tempCardList);
                }
            }


            List<Card> bestCombination = new List<Card>();
            int bestValuesSum = 0;
            foreach (var combination in mergedCombinations)
            {
                int valuesSum = 0;
                foreach (var card in combination)
                {
                    valuesSum += card.CardValue;
                }

                if (valuesSum > bestValuesSum)
                {
                    bestCombination = combination;
                    bestValuesSum = valuesSum;
                }
            }

            foreach (var card in _cards)
            {
                if (!bestCombination.Contains(card))
                {
                    bestCombination.Add(card);
                }
            }

            for (int i = 0; i < bestCombination.Count; i++)
            {
                var card = bestCombination[i];
                _cardViews[i].Init(_cardSettings.GetCardImage(card.CardSuit, card.CardRank));
            }
        }

        private bool CanCombine(List<Card> cardList1, List<Card> cardList2)
        {
            foreach (var card in cardList1)
            {
                if (cardList2.Contains(card))
                {
                    return false;
                }
            }

            return true;
        }
        
        private List<List<Card>> FindConsecutiveSubsequences(List<Card> cards)
        {
            List<List<Card>> subsequences = new List<List<Card>>();

            for (int i = 0; i < cards.Count; i++)
            {
                for (int j = GlobalConst.MIN_NUMBER_OF_SORTED_CARDS; j <= cards.Count - i; j++)
                {
                    List<Card> subsequence = cards.GetRange(i, j);
                    subsequences.Add(subsequence);
                }
            }

            return subsequences;
        }
        
        #endregion
    }

    public class ConsecutiveSortData
    {
        public List<Card> ConsecutiveList;
        public List<Card> NonConsecutiveList;
    }
}