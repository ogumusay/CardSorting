using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CardSorting
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private TransitionView _transitionView;
        
        
        public void LoadGameScene()
        {
            _transitionView.StartTransition(StartLoadingGameSceneAsync);
        }

        private void StartLoadingGameSceneAsync()
        {
            StartCoroutine(LoadGameSceneAsync());
        }

        private IEnumerator LoadGameSceneAsync()
        {
            var asyncLoad = SceneManager.LoadSceneAsync(GlobalConst.GAME_SCENE_INDEX);
            
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            
            _transitionView.EndTransition(null);
        }
    }
}
