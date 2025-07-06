using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardSorting
{
    public class CardView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image _cardImage;
        [SerializeField] private RectTransform _rectTransform;
        public Card Card { get; private set; }
     
        public delegate void CardViewDragHandler(CardView cardView);
        public static CardViewDragHandler onDrag;
        public static CardViewDragHandler onDrop;
        
        public RectTransform RectTransform => _rectTransform;

        public void SetImage(Sprite icon)
        {
            _cardImage.sprite = icon;
        }

        public void Init(Card card)
        {
            Card = card;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.localEulerAngles = Vector3.zero;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
            onDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            onDrop?.Invoke(this);
        }
    }
}
