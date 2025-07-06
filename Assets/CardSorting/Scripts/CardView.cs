using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardSorting
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private Image _cardImage;
        [SerializeField] private RectTransform _container;
        public Card Card { get; private set; }
        
        public RectTransform Container => _container;

        public void SetImage(Sprite icon)
        {
            _cardImage.sprite = icon;
        }

        public void Init(Card card)
        {
            Card = card;
        }
    }
}
