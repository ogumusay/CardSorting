using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardSorting
{
    public class CardLayoutView : MonoBehaviour
    {
        [SerializeField] private List<CardView> _cardViews;
        [SerializeField] private float _rotation;
        [SerializeField] private float _height;
        
        
        [Button]
        public void SetPositions()
        {
            var count = _cardViews.Count;
            for (int i = 0; i < count; i++)
            {
                float t = Mathf.Abs(i - (count - 1) / 2f) / (((count - 1) / 2f));
                var posY = Mathf.Lerp(_height, 0, t * t);
                _cardViews[i].Container.anchoredPosition = new Vector2(0f, posY);
                
                var angleZ = Mathf.Lerp(_rotation, -_rotation, (float)i / (count - 1));
                _cardViews[i].Container.localEulerAngles = new Vector3(0, 0, angleZ);
                
            }
        }
        
    }
}