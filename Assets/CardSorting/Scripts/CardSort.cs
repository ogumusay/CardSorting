using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace CardSorting
{
    public class CardSort : MonoBehaviour
    {
        [SerializeField] private CardSettings _cardSettings;
        [SerializeField] private CardView[] _cardViews;
        [SerializeField] private Transform[] _cardSockets;
        [SerializeField] private CardLayoutView _cardLayoutView;
        
        private List<Card> _cardList = new()
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
        private List<Card> _cards = new()
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
        */
        
        [Button]
        private void RandomCards()
        {
            _cardList = _cardSettings.GetRandomCards();
            InitCards();
        }

        private void InitCards()
        {
            for (int i = 0; i < _cardList.Count; i++)
            {
                var card = _cardList[i];
                _cardViews[i].Init(card);
                _cardViews[i].SetImage(_cardSettings.GetCardImage(card.CardSuit, card.CardRank));
            }
        }

        private void UpdateCardPositions()
        {
            for (int i = 0; i < _cardList.Count; i++)
            {
                var card = _cardList[i];
                _cardLayoutView.ChangeCardViewIndex(GetCardView(card), i);
                _cardLayoutView.SetPositionWithTween(i);
            }

        }

        private CardView GetCardView(Card card)
        {
            foreach (var cardView in _cardViews)
            {
                if (cardView.Card.Equals(card))
                {
                    return cardView;
                }
            }

            return _cardViews[0];
        }
        
        #region 7-7-7 Sorting
        
        [Button]
        public void SameRankSorting()
        {
            var cardRankDictionary = new Dictionary<CardRank, List<Card>>();
            var sortedList = new List<Card>();
            var sameRankLists = new List<List<Card>>();

            InsertionSortByRank(_cardList);
            
            // Getting same rank cards
            foreach (var card in _cardList)
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
                sameRankLists.Add(card.Value);
            }
            
            InsertionSortBySameRankQuantity(sameRankLists);
            
            foreach (var card in sameRankLists)
            {
                sortedList.AddRange(card);
            }

            _cardList = sortedList;
            UpdateCardPositions();
        }

        #endregion
        
        #region 1-2-3 Sorting

        [Button]
        private void SortCardSuits()
        {
            var cardSuitDictionary = new Dictionary<CardSuit, List<Card>>(GlobalConst.NUMBER_OF_CARD_SUITS) ;
            var consecutiveSortResults = new List<ConsecutiveSortResult>();
            var sortedList = new List<Card>();
            
            // Insert cards by cards suit
            foreach (var card in _cardList)
            {
                if (cardSuitDictionary.ContainsKey(card.CardSuit))
                {
                    cardSuitDictionary[card.CardSuit].Add(card);
                }
                else
                {
                    cardSuitDictionary.Add(card.CardSuit, new List<Card>(){card});
                }
            }

            // Sort cards by rank and get consecutive sequences
            foreach (var card in cardSuitDictionary)
            {
                InsertionSortByRank(card.Value);
                var consecutiveSortData = ConsecutiveSort(card.Value);
                consecutiveSortResults.Add(consecutiveSortData);
            }
            
            foreach (var consecutiveSortData in consecutiveSortResults)
            {
                sortedList.AddRange(consecutiveSortData.ConsecutiveList);
            }            
            foreach (var consecutiveSortData in consecutiveSortResults)
            {
                sortedList.AddRange(consecutiveSortData.NonConsecutiveList);
            }

            _cardList = sortedList;
            UpdateCardPositions();
        }
        
        private void InsertionSortByRank(List<Card> list)
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
        
        private void InsertionSortBySameRankQuantity(List<List<Card>> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    if (list[j - 1].Count < list[j].Count)
                    {
                        (list[j - 1], list[j]) = (list[j], list[j - 1]);
                    }
                }
            }
        }           
        
        private ConsecutiveSortResult ConsecutiveSort(List<Card> list)
        {
            List<Card> consecutiveList = new();
            List<Card> nonConsecutiveList = new();
            
            int minNumberOfSortedCards = GlobalConst.MIN_NUMBER_OF_SORTED_CARDS;
            if (list.Count < minNumberOfSortedCards)
            {
                nonConsecutiveList.AddRange(list);
                return new ConsecutiveSortResult()
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
            
            return new ConsecutiveSortResult()
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
            var allCombinations = new List<List<Card>>();
            var mergedCombinations = new List<List<Card>>();
            
            // Get possible combinations from Consecutive Sorting
            var cardSuitDictionary = new Dictionary<CardSuit, List<Card>>(GlobalConst.NUMBER_OF_CARD_SUITS);
            
            foreach (var card in _cardList)
            {
                if (cardSuitDictionary.ContainsKey(card.CardSuit))
                {
                    cardSuitDictionary[card.CardSuit].Add(card);
                }
                else
                {
                    cardSuitDictionary.Add(card.CardSuit, new List<Card>(){card});
                }
            }

            foreach (var card in cardSuitDictionary)
            {
                InsertionSortByRank(card.Value);
                var sortData = ConsecutiveSort(card.Value);
                allCombinations.AddRange(FindConsecutiveSubsequences(sortData.ConsecutiveList));
            }

            // Get possible combinations from Same Rank Sorting
            var cardRankDictionary = new Dictionary<CardRank, List<Card>>();
            
            foreach (var card in _cardList)
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
            
            // Combine all possible combinations
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
            
            // Find the best combination
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

            foreach (var card in _cardList)
            {
                if (!bestCombination.Contains(card))
                {
                    bestCombination.Add(card);
                }
            }

            _cardList = bestCombination;
            UpdateCardPositions();
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

    public class ConsecutiveSortResult
    {
        public List<Card> ConsecutiveList;
        public List<Card> NonConsecutiveList;
    }
}