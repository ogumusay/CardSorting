using System;
using UnityEngine;

namespace CardSorting
{
    [Serializable]
    public struct CardRankData
    {
        public CardRank cardRank;
        public ushort cardValue;
        public Sprite cardImage;
    }

    public enum CardRank
    {
        None = 0,
        Ace = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13
    }
}