using System;

namespace CardSorting
{
    [Serializable]
    public struct CardSuitData
    {
        public CardSuit cardSuit;
        public CardRankData[] cardRanks;
    }
    
    public enum CardSuit
    {
        None = 0,
        Spades = 1,
        Diamonds = 2,
        Hearts = 3,
        Clubs = 4
    }
}
