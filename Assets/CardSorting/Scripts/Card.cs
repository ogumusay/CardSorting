using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardSorting
{
    public struct Card
    {
        public CardSuit CardSuit;
        public CardRank CardRank;
        public ushort CardValue;

        public Card(CardSuit cardSuit, CardRank cardRank, ushort cardValue)
        {
            CardSuit = cardSuit;
            CardRank = cardRank;
            CardValue = cardValue;
        }
    }
}
