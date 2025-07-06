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
        
        
        [Button]
        public void SetPositions()
        {
            var count = _cardSockets.Count;
            for (int i = 0; i < count; i++)
            {
                float t = Mathf.Abs(i - (count - 1) / 2f) / (((count - 1) / 2f));
                var posY = Mathf.Lerp(_height, 0, t * t);
                _cardSockets[i].CardView.Container.anchoredPosition = new Vector2(0f, posY);
                
                var angleZ = Mathf.Lerp(_rotation, -_rotation, (float)i / (count - 1));
                _cardSockets[i].CardView.Container.localEulerAngles = new Vector3(0, 0, angleZ);
                
            }
        }        
        
        public void SetPosition(int index)
        {
            var count = _cardSockets.Count;
            float t = Mathf.Abs(index - (count - 1) / 2f) / (((count - 1) / 2f));
            var posY = Mathf.Lerp(_height, 0, t * t);
            _cardSockets[index].CardView.Container.anchoredPosition = new Vector2(0f, posY);
                
            var angleZ = Mathf.Lerp(_rotation, -_rotation, (float)index / (count - 1));
            _cardSockets[index].CardView.Container.localEulerAngles = new Vector3(0, 0, angleZ);
        }        
        
        public void SetPositionWithTween(int index)
        {
            var count = _cardSockets.Count;
            float t = Mathf.Abs(index - (count - 1) / 2f) / (((count - 1) / 2f));
            var posY = Mathf.Lerp(_height, 0, t * t);
            _cardSockets[index].CardView.Container.DOAnchorPos(new Vector2(0f, posY), 0.35f);
                
            var angleZ = Mathf.Lerp(_rotation, -_rotation, (float)index / (count - 1));
            _cardSockets[index].CardView.Container.DOLocalRotate(new Vector3(0, 0, angleZ), 0.35f);
        }

        public void ChangeCardViewIndex(CardView cardView, int index)
        {
            _cardSockets[index].SetCardView(cardView);
        }
    }
}