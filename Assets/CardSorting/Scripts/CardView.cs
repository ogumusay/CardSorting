using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardSorting
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private Image _cardImage;

        public void Init(Sprite icon)
        {
            _cardImage.sprite = icon;
        }
    }
}
