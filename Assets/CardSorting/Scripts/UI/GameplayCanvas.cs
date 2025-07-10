using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CardSorting
{
    public class GameplayCanvas : MonoBehaviour
    {
        [SerializeField] private CardView[] _cardViews;
        [SerializeField] private Transform[] _cardSockets;
        [SerializeField] private CardLayoutView _cardLayoutView;
        [SerializeField] private Transform _cardDealPoint;
        [SerializeField] private Image _cardDealImage;
        private int _currentBackgroundThemeIndex;
        private bool _isTweening;
        
        #region Injection

        private BoardController _boardController;
        private CardSettings _cardSettings;
        
        [Inject]
        private void Construct(BoardController boardController,
            CardSettings cardSettings)
        {
            _boardController = boardController;
            _cardSettings = cardSettings;
        }

        #endregion

        public void Initialize()
        {
            DealNewCards();
        }

        public void Dispose()
        {
            
        }
        
        private async UniTaskVoid PlayDealingCardsAnimation()
        {
            _isTweening = true;
            
            await UniTask.Yield();
            await UniTask.Yield();
            InitCards();
            foreach (var cardView in _cardViews)
            {
                cardView.transform.position = _cardDealPoint.position;
                cardView.ShowBackground();
            }
            
            for (int i = 0; i < _boardController.SortedCardList.Count; i++)
            {
                await UniTask.Delay(100);
                var card = _boardController.SortedCardList[i];
                var cardView = GetCardView(card);
                _cardLayoutView.SetCardViewIndex(cardView, i);
                _cardLayoutView.SetPositionWithTween(i);
                cardView.PlayFlipAnimation();
            }

            await UniTask.Delay(500);
            _isTweening = false;
        }
        
        public void DealNewCards()
        {
            if (_isTweening) return;
            
            _boardController.GetNewCards();
            PlayDealingCardsAnimation().Forget();
        }        
        
        public void DealHandExample()
        {
            if (_isTweening) return;
            
            _boardController.GetHandExample();
            PlayDealingCardsAnimation().Forget();
        }
        
        private void InitCards()
        {
            for (int i = 0; i < _boardController.SortedCardList.Count; i++)
            {
                var card = _boardController.SortedCardList[i];
                _cardViews[i].Init(card);
                _cardViews[i].SetImage(_cardSettings.GetCardImage(card.CardSuit, card.CardRank));
                _cardLayoutView.SetCardViewIndex(_cardViews[i], i);
            }
        }

        public void RankGrouping()
        {
            if (_isTweening) return;
            
            _boardController.RankGrouping();
            UpdateCardPositions().Forget();
        }

        public void ConsecutiveSorting()
        {
            if (_isTweening) return;
            
            _boardController.ConsecutiveSorting();
            UpdateCardPositions().Forget();
        }

        public void SmartSorting()
        {
            if (_isTweening) return;
            
            _boardController.SmartSorting();
            UpdateCardPositions().Forget();
        }

        public void InsertCard(int from, int to)
        {
            _boardController.InsertCard(from, to);
            UpdateCardPositions().Forget();
        }

        private async UniTaskVoid UpdateCardPositions()
        {
            _isTweening = true;
            for (int i = 0; i < _boardController.SortedCardList.Count; i++)
            {
                var card = _boardController.SortedCardList[i];
                _cardLayoutView.SetCardViewIndex(GetCardView(card), i);
                _cardLayoutView.SetPositionWithTween(i);
            }

            await UniTask.Delay(350);
            _isTweening = false;
        }
        
        public void ChangeTheme()
        {
            if (_currentBackgroundThemeIndex < _cardSettings.cardBackgroundThemes.Length - 1)
            {
                _currentBackgroundThemeIndex++;
            }
            else
            {
                _currentBackgroundThemeIndex = 0;
            }

            var sprite = _cardSettings.cardBackgroundThemes[_currentBackgroundThemeIndex];
            foreach (var cardView in _cardViews)
            {
                cardView.SetBackgroundImage(sprite);
            }

            _cardDealImage.sprite = sprite;
        }
        
        private CardView GetCardView(Card card)
        {
            foreach (var cardView in _cardViews)
            {
                if (cardView.Card.Equals(card))
                {
                    return cardView;
                }
            }

            return _cardViews[0];
        }
    }
}
