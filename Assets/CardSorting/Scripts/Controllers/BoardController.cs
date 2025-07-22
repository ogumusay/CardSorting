using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CardSorting
{
    public class BoardController
    {
        #region Injection

        private CardSettings _cardSettings;

        [Inject]
        private void Construct(CardSettings cardSettings)
        {
            _cardSettings = cardSettings;
        }
        
        #endregion
        
        private List<Card> _cardList = new();
        public List<Card> CardList => _cardList;

        private List<Card> _sortedCardList = new();
        public List<Card> SortedCardList => _sortedCardList;
        
        private SortingResult _cachedConsecutiveSortedCardList;
        private SortingResult _cachedRankGroupedSortedCardList;
        private SortingResult _cachedSmartSortedCardList;

        public void Initialize()
        {
            
        }

        public void Dispose()
        {
            
        }
        
        public void GetNewCards()
        {
            _cardList = _cardSettings.GetRandomCards();
            ResetSortedList();
            ClearCache();
        }        
        
        public void GetHandExample()
        {
            _cardList = new List<Card>
            {
                _cardSettings.GetCard(CardSuit.Hearts, CardRank.Ace),
                _cardSettings.GetCard(CardSuit.Spades, CardRank.Two),
                _cardSettings.GetCard(CardSuit.Diamonds, CardRank.Five),
                _cardSettings.GetCard(CardSuit.Hearts, CardRank.Four),
                _cardSettings.GetCard(CardSuit.Spades, CardRank.Ace),
                _cardSettings.GetCard(CardSuit.Diamonds, CardRank.Three),
                _cardSettings.GetCard(CardSuit.Clubs, CardRank.Four),
                _cardSettings.GetCard(CardSuit.Spades, CardRank.Four),
                _cardSettings.GetCard(CardSuit.Diamonds, CardRank.Ace),
                _cardSettings.GetCard(CardSuit.Spades, CardRank.Three),
                _cardSettings.GetCard(CardSuit.Diamonds, CardRank.Four)
            };
            ResetSortedList();
            ClearCache();
        }

        private void ResetSortedList()
        {
            _sortedCardList.Clear();
            _sortedCardList.AddRange(_cardList);
        }

        public void InsertCard(int from, int to)
        {
            var tempCard = _sortedCardList[from];
            _sortedCardList.RemoveAt(from);
            _sortedCardList.Insert(to, tempCard);
        }

        private List<Card> GetSortedList(SortingResult sortingResult)
        {
            var sortedList = new List<Card>();
            foreach (var sequence in sortingResult.GroupedCards)
            {
                sortedList.AddRange(sequence.Cards);
            }
            foreach (var card in sortingResult.UngroupedCards)
            {
                sortedList.Add(card);
            }

            return sortedList;
        }

        private void ClearCache()
        {
            _cachedConsecutiveSortedCardList = null;
            _cachedRankGroupedSortedCardList = null;
            _cachedSmartSortedCardList = null;
        }
        
        #region 7-7-7 Sorting
        
        public SortingResult RankGrouping()
        {
            if (_cachedRankGroupedSortedCardList != null)
            {
                _sortedCardList = GetSortedList(_cachedRankGroupedSortedCardList);
                return _cachedRankGroupedSortedCardList;
            }

            ResetSortedList();
            SortingResult sortingResult = new SortingResult()
            {
                GroupedCards = new List<Sequence>(),
                UngroupedCards = new List<Card>()
            };
            
            var cardRankDictionary = new Dictionary<CardRank, List<Card>>();
            var sameRankLists = new List<List<Card>>();

            InsertionSortByRank(_sortedCardList);
            
            // Getting same rank cards
            foreach (var card in _sortedCardList)
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
            
            foreach (var cardList in sameRankLists)
            {
                if (cardList.Count >= GlobalConst.MIN_NUMBER_OF_SORTED_CARDS)
                {
                    sortingResult.GroupedCards.Add(new Sequence(){Cards = cardList});
                }
                else
                {
                    sortingResult.UngroupedCards.AddRange(cardList);
                }
            }

            _cachedRankGroupedSortedCardList = sortingResult;
            _sortedCardList = GetSortedList(sortingResult);
            return sortingResult;
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
        
        #endregion
        
        #region 1-2-3 Sorting
        
        public SortingResult ConsecutiveSorting()
        {
            if (_cachedConsecutiveSortedCardList != null)
            {
                _sortedCardList = GetSortedList(_cachedConsecutiveSortedCardList);
                return _cachedConsecutiveSortedCardList;
            }
            
            ResetSortedList();
            SortingResult sortingResult = new SortingResult()
            {
                GroupedCards = new List<Sequence>(),
                UngroupedCards = new List<Card>()
            };

            var cardSuitDictionary = new Dictionary<CardSuit, List<Card>>(GlobalConst.NUMBER_OF_CARD_SUITS)
            {
                { CardSuit.Spades, new List<Card>() },
                { CardSuit.Diamonds, new List<Card>() },
                { CardSuit.Hearts, new List<Card>() },
                { CardSuit.Clubs, new List<Card>() }
            };
            var consecutiveSortResults = new List<ConsecutiveSortResult>();
            
            // Insert cards by cards suit
            foreach (var card in _sortedCardList)
            {
                cardSuitDictionary[card.CardSuit].Add(card);
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
                if (consecutiveSortData.ConsecutiveList.Count > 0 )
                {
                    sortingResult.GroupedCards.Add(new Sequence(){Cards = consecutiveSortData.ConsecutiveList});
                }
            }            
            foreach (var consecutiveSortData in consecutiveSortResults)
            {
                sortingResult.UngroupedCards.AddRange(consecutiveSortData.NonConsecutiveList);
            }

            _cachedConsecutiveSortedCardList = sortingResult;
            _sortedCardList = GetSortedList(sortingResult);
            return sortingResult;
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
        
        public SortingResult SmartSorting()
        {
            if (_cachedSmartSortedCardList != null)
            {
                _sortedCardList = GetSortedList(_cachedSmartSortedCardList);
                return _cachedSmartSortedCardList;
            }
            
            ResetSortedList();
            SortingResult sortingResult = new SortingResult()
            {
                GroupedCards = new List<Sequence>(),
                UngroupedCards = new List<Card>()
            };
            
            var allCombinations = new List<Sequence>();
            var mergedCombinations = new List<List<Sequence>>();
            
            // Get possible combinations from Consecutive Sorting
            var cardSuitDictionary = new Dictionary<CardSuit, List<Card>>(GlobalConst.NUMBER_OF_CARD_SUITS)
            {
                { CardSuit.Spades, new List<Card>() },
                { CardSuit.Diamonds, new List<Card>() },
                { CardSuit.Hearts, new List<Card>() },
                { CardSuit.Clubs, new List<Card>() }
            };
            
            foreach (var card in _sortedCardList)
            {
                cardSuitDictionary[card.CardSuit].Add(card);
            }

            foreach (var card in cardSuitDictionary)
            {
                InsertionSortByRank(card.Value);
                var sortData = ConsecutiveSort(card.Value);
                allCombinations.AddRange(FindConsecutiveSequences(sortData.ConsecutiveList));
            }

            // Get possible combinations from Same Rank Sorting
            var cardRankDictionary = new Dictionary<CardRank, List<Card>>();
            
            foreach (var card in _sortedCardList)
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
                    allCombinations.AddRange(FindConsecutiveSequences(card.Value));
                }
            }
            
            // Combine all possible combinations
            if (allCombinations.Count == 1)
            {
                mergedCombinations.Add(allCombinations);
            }
            else
            {
                for (int i = 0; i < allCombinations.Count - 1; i++)
                {
                    var tempCardList = new List<Sequence>();
                    tempCardList.Add(allCombinations[i]);
                    for (int j = i + 1; j < allCombinations.Count; j++)
                    {
                        if (CanCombine(tempCardList, allCombinations[j].Cards))
                        {
                            tempCardList.Add(allCombinations[j]);
                        }
                    }
                
                    mergedCombinations.Add(tempCardList);
                }
            }
            
            // Find the best combination
            var bestCombination = new List<Sequence>();
            int bestValuesSum = 0;
            foreach (var combination in mergedCombinations)
            {
                int valuesSum = 0;
                foreach (var sequence in combination)
                {
                    foreach (var card in sequence.Cards)
                    {
                        valuesSum += card.CardValue;
                    }
                }
                
                if (valuesSum > bestValuesSum)
                {
                    bestCombination = combination;
                    bestValuesSum = valuesSum;
                }
                else if (valuesSum == bestValuesSum && combination.Count < bestCombination.Count)
                {
                    bestCombination = combination;
                    bestValuesSum = valuesSum;
                }
            }

            var groupedCards = new List<Card>();
            foreach (var sequence in bestCombination)
            {
                sortingResult.GroupedCards.Add(sequence);
                groupedCards.AddRange(sequence.Cards);
            }
            
            foreach (var card in _cardList)
            {
                if (!groupedCards.Contains(card))
                {
                    sortingResult.UngroupedCards.Add(card);
                }
            }

            _cachedSmartSortedCardList = sortingResult;
            _sortedCardList = GetSortedList(sortingResult);
            return sortingResult;
        }

        private bool CanCombine(List<Sequence> cardList1, List<Card> cardList2)
        {
            foreach (var sequence in cardList1)
            {
                foreach (var card in sequence.Cards)
                {
                    if (cardList2.Contains(card))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        
        private List<Sequence> FindConsecutiveSequences(List<Card> cards)
        {
            var subsequences = new List<Sequence>();

            for (int i = 0; i < cards.Count; i++)
            {
                for (int j = GlobalConst.MIN_NUMBER_OF_SORTED_CARDS; j <= cards.Count - i; j++)
                {
                    subsequences.Add(new Sequence(){Cards = cards.GetRange(i, j)});
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
    
    public class SortingResult
    {
        public List<Sequence> GroupedCards;
        public List<Card> UngroupedCards;
    }

    [Serializable]
    public class Sequence
    {
        public List<Card> Cards;
    }
}