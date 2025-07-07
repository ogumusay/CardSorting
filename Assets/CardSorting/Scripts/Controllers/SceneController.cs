using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CardSorting
{
    public class SceneController
    {
        #region Injection

        private TransitionView _transitionView;
        
        [Inject]
        private void Construct(TransitionView transitionView)
        {
            _transitionView = transitionView;
        }
        
        #endregion
        
        public void LoadGameScene()
        {
            _transitionView.StartTransition(StartLoadingGameSceneAsync);
        }

        private void StartLoadingGameSceneAsync()
        {
            LoadGameSceneAsync().Forget();
        }

        private async UniTaskVoid LoadGameSceneAsync()
        {
            var asyncLoad = SceneManager.LoadSceneAsync(GlobalConst.GAME_SCENE_INDEX);
            
            while (!asyncLoad.isDone)
            {
                await UniTask.Yield();
            }
            
            _transitionView.EndTransition(null);
        }
    }
}
