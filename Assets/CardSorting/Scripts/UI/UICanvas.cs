using UnityEngine;
using Zenject;

namespace CardSorting
{
    public class UICanvas : MonoBehaviour
    {
        #region Injection

        private GameplayCanvas _gameplayCanvas;
        
        [Inject]
        private void Construct(GameplayCanvas gameplayCanvas)
        {
            _gameplayCanvas = gameplayCanvas;
        }

        #endregion

        public void DealNewCards()
        {
            _gameplayCanvas.DealNewCards();
        }        
        
        public void SameRankSorting()
        {
            _gameplayCanvas.SameRankSorting();
        }

        public void SortCardSuits()
        {
            _gameplayCanvas.SortCardSuits();
        }

        public void SmartSorting()
        {
            _gameplayCanvas.SmartSorting();
        }

        public void ChangeTheme()
        {
            _gameplayCanvas.ChangeTheme();
        }
    }
}
