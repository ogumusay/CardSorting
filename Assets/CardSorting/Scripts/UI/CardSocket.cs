using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardSorting
{
    public class CardSocket : MonoBehaviour
    {
        public CardView CardView { get; private set; }

        public void SetCardView(CardView cardView)
        {
            CardView = cardView;
            cardView.transform.SetParent(transform);
        }
        
    }
}
