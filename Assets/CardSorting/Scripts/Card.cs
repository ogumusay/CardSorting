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

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(Card)) return false;

            Card card = (Card)obj;
            return CardSuit == card.CardSuit && CardRank == card.CardRank;
        }
    }
}
