using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardSorting
{
    public class CardLayoutView : MonoBehaviour
    {
        [SerializeField] private List<CardSocket> _cardSockets;
        [SerializeField] private float _rotation;
        [SerializeField] private float _height;
        [SerializeField] private CardSort _cardSort;
        
        private int _lastDropIndex;
        
        private void Awake()
        {
            CardView.onDrag += OnDrag;
            CardView.onDrop += OnDrop;
        }        
        
        private void OnDestroy()
        {
            CardView.onDrag -= OnDrag;
            CardView.onDrop -= OnDrop;
        }

        private void OnDrag(CardView cardView)
        {
            for (int i = 0; i < _cardSockets.Count; i++)
            {
                var cardSocket = _cardSockets[i];
                if (cardSocket.transform.position.x - cardView.transform.position.x < 75)
                {
                    cardView.transform.SetParent(cardSocket.transform);
                    _lastDropIndex = i;
                }
            }
        }
        
        private void OnDrop(CardView cardView)
        {
            _cardSort.InsertCard(GetCardIndex(cardView.Card), _lastDropIndex);
        }

        [Button]
        public void SetPositions()
        {
            var count = _cardSockets.Count;
            for (int i = 0; i < count; i++)
            {
                float t = Mathf.Abs(i - (count - 1) / 2f) / (((count - 1) / 2f));
                var posY = Mathf.Lerp(_height, 0, t * t);
                _cardSockets[i].CardView.RectTransform.anchoredPosition = new Vector2(0f, posY);
                
                var angleZ = Mathf.Lerp(_rotation, -_rotation, (float)i / (count - 1));
                _cardSockets[i].CardView.RectTransform.localEulerAngles = new Vector3(0, 0, angleZ);
            }
        }        
        
        public void SetPositionWithTween(int index)
        {
            var count = _cardSockets.Count;
            float t = Mathf.Abs(index - (count - 1) / 2f) / (((count - 1) / 2f));
            var posY = Mathf.Lerp(_height, 0, t * t);
            _cardSockets[index].CardView.RectTransform.DOAnchorPos(new Vector2(0f, posY), 0.35f);
                
            var angleZ = Mathf.Lerp(_rotation, -_rotation, (float)index / (count - 1));
            _cardSockets[index].CardView.RectTransform.DOLocalRotate(new Vector3(0, 0, angleZ), 0.35f);
        }

        public void SetCardViewIndex(CardView cardView, int index)
        {
            _cardSockets[index].SetCardView(cardView);
        }

        public int GetCardIndex(Card card)
        {
            for (int i = 0; i < _cardSockets.Count; i++)
            {
                if (_cardSockets[i].CardView.Card.Equals(card))
                {
                    return i;
                }
            }

            return 0;
        }
    }
}