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
        public RectTransform Container => _container;

        public void Init(Sprite icon)
        {
            _cardImage.sprite = icon;
        }
    }
}
