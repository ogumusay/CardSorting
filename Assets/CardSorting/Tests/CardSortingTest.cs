using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

namespace CardSorting.Test
{
    public class CardSortingTest : ScriptableObject
    {
        public CardSettings cardSettings;
        
        [Test]
        public void ConsecutiveSorting()
        {
            var boardController = new BoardController();

            boardController.CardList.Clear();
            boardController.CardList.AddRange(CreateCardList());

            var result = boardController.ConsecutiveSorting();

            var expectedResult = new SortingResult();
            expectedResult.GroupedCards = new List<Sequence>()
            {
                new Sequence()
                {
                    Cards = new() { 
                        cardSettings.GetCard(CardSuit.Spades, CardRank.Ace),
                        cardSettings.GetCard(CardSuit.Spades, CardRank.Two),
                        cardSettings.GetCard(CardSuit.Spades, CardRank.Three),
                        cardSettings.GetCard(CardSuit.Spades, CardRank.Four)
                    }
                },
                new Sequence()
                {
                    Cards = new() { 
                        cardSettings.GetCard(CardSuit.Diamonds, CardRank.Three), 
                        cardSettings.GetCard(CardSuit.Diamonds, CardRank.Four),
                        cardSettings.GetCard(CardSuit.Diamonds, CardRank.Five)
                    }
                }
            };
            expectedResult.UngroupedCards = new List<Card>()
            {
                cardSettings.GetCard(CardSuit.Diamonds, CardRank.Ace),
                cardSettings.GetCard(CardSuit.Hearts, CardRank.Ace),
                cardSettings.GetCard(CardSuit.Hearts, CardRank.Four),
                cardSettings.GetCard(CardSuit.Clubs, CardRank.Four), 
            };

            Assert.IsTrue(AreEqual(expectedResult, result));
        }

        [Test]
        public void RankGrouping()
        {
            var boardController = new BoardController();

            boardController.CardList.Clear();
            boardController.CardList.AddRange(CreateCardList());

            var result = boardController.RankGrouping();
            
            var expectedResult = new SortingResult();
            expectedResult.GroupedCards = new List<Sequence>()
            {
                new Sequence()
                {
                    Cards = new() { 
                        cardSettings.GetCard(CardSuit.Spades, CardRank.Ace), 
                        cardSettings.GetCard(CardSuit.Diamonds, CardRank.Ace),
                        cardSettings.GetCard(CardSuit.Hearts, CardRank.Ace)
                    },
                },
                new Sequence()
                {
                    Cards = new() { 
                        cardSettings.GetCard(CardSuit.Spades, CardRank.Four), 
                        cardSettings.GetCard(CardSuit.Diamonds, CardRank.Four),
                        cardSettings.GetCard(CardSuit.Hearts, CardRank.Four),
                        cardSettings.GetCard(CardSuit.Clubs, CardRank.Four)
                    }
                }
            };
            expectedResult.UngroupedCards = new List<Card>()
            {
                cardSettings.GetCard(CardSuit.Spades, CardRank.Two), 
                cardSettings.GetCard(CardSuit.Spades, CardRank.Three),
                cardSettings.GetCard(CardSuit.Diamonds, CardRank.Three),
                cardSettings.GetCard(CardSuit.Diamonds, CardRank.Five)
            };
            
            Assert.IsTrue(AreEqual(expectedResult, result));
        }

        [Test]
        public void SmartSorting()
        {
            var boardController = new BoardController();

            boardController.CardList.Clear();
            boardController.CardList.AddRange(CreateCardList());

            var result = boardController.SmartSorting();

            var expectedResult = new SortingResult();
            expectedResult.GroupedCards = new List<Sequence>()
            {
                new Sequence()
                {
                    Cards = new() { 
                        cardSettings.GetCard(CardSuit.Spades, CardRank.Ace), 
                        cardSettings.GetCard(CardSuit.Spades, CardRank.Two),
                        cardSettings.GetCard(CardSuit.Spades, CardRank.Three)
                    }
                },
                new Sequence()
                {
                    Cards = new() { 
                        cardSettings.GetCard(CardSuit.Spades, CardRank.Four), 
                        cardSettings.GetCard(CardSuit.Hearts, CardRank.Four),
                        cardSettings.GetCard(CardSuit.Clubs, CardRank.Four)
                    }
                },
                new Sequence()
                {
                    Cards = new() { 
                        cardSettings.GetCard(CardSuit.Diamonds, CardRank.Three), 
                        cardSettings.GetCard(CardSuit.Diamonds, CardRank.Four),
                        cardSettings.GetCard(CardSuit.Diamonds, CardRank.Five)
                    }
                }
            };
            expectedResult.UngroupedCards = new List<Card>()
            {
                cardSettings.GetCard(CardSuit.Diamonds, CardRank.Ace),
                cardSettings.GetCard(CardSuit.Hearts, CardRank.Ace)
            };

            Assert.IsTrue(AreEqual(expectedResult, result));
        }

        private bool AreEqual(SortingResult expectedResult, SortingResult result)
        {
            var ungroupedList = result.UngroupedCards.OrderBy(card => card.CardRank).ThenBy(card => card.CardSuit);
            bool ungroupedListEquals = ungroupedList.SequenceEqual(expectedResult.UngroupedCards);
            if (!ungroupedListEquals)
            {
                return false;
            }
            
            for (int i = 0; i < result.GroupedCards.Count; i++)
            {
                var sequence = result.GroupedCards[i];
                result.GroupedCards[i].Cards = sequence.Cards.OrderBy(card => card.CardRank).ThenBy(card => card.CardSuit).ToList();
            }
            
            foreach (var expectedCards in expectedResult.GroupedCards)
            {
                bool contains = false;
                foreach (var sequence in result.GroupedCards)
                {
                    if (expectedCards.Cards.SequenceEqual(sequence.Cards))
                    {
                        contains = true;
                        break;
                    }
                }

                if (!contains)
                {
                    return false;
                }
            }

            return true;
        }
        
        private List<Card> CreateCardList()
        {
            var cardList = new List<Card>
            {
                cardSettings.GetCard(CardSuit.Hearts, CardRank.Ace),
                cardSettings.GetCard(CardSuit.Spades, CardRank.Two),
                cardSettings.GetCard(CardSuit.Diamonds, CardRank.Five),
                cardSettings.GetCard(CardSuit.Hearts, CardRank.Four),
                cardSettings.GetCard(CardSuit.Spades, CardRank.Ace),
                cardSettings.GetCard(CardSuit.Diamonds, CardRank.Three),
                cardSettings.GetCard(CardSuit.Clubs, CardRank.Four),
                cardSettings.GetCard(CardSuit.Spades, CardRank.Four),
                cardSettings.GetCard(CardSuit.Diamonds, CardRank.Ace),
                cardSettings.GetCard(CardSuit.Spades, CardRank.Three),
                cardSettings.GetCard(CardSuit.Diamonds, CardRank.Four)
            };

            return cardList;
        }
    }
}