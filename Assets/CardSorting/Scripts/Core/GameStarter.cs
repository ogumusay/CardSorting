using System;
using UnityEngine;
using Zenject;

namespace CardSorting
{
    public class GameStarter : IInitializable, IDisposable
    {
        #region Injection
        
        private BoardController _boardController;
        private GameplayCanvas _gameplayCanvas;
        
        public GameStarter(BoardController boardController,
            GameplayCanvas gameplayCanvas)
        {
            _boardController = boardController;
            _gameplayCanvas = gameplayCanvas;
        }
        
        #endregion
        
        public void Initialize()
        {
            _boardController.Initialize();
            _gameplayCanvas.Initialize();
        }

        public void Dispose()
        {
            _boardController.Dispose();
            _gameplayCanvas.Dispose();
        }
    }
}
