using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CardSorting
{
    public class MenuCanvas : MonoBehaviour
    {
        #region Injection

        private SceneController _sceneController;
        
        [Inject]
        private void Construct(SceneController sceneController)
        {
            _sceneController = sceneController;
        }

        #endregion

        public void LoadGameScene()
        {
            _sceneController.LoadGameScene();
        }
    }
}
