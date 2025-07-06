using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CardSorting
{
    public class TransitionView : MonoBehaviour
    {
        [SerializeField] private Image _blackoutImage;

        public void StartTransition(Action onComplete)
        {
            _blackoutImage.gameObject.SetActive(true);
            _blackoutImage.DOFade(1, .75f).OnComplete(new TweenCallback(onComplete));
        }

        public void EndTransition(Action onComplete)
        {
            _blackoutImage.DOFade(0, .75f).OnComplete((() =>
            {
                onComplete?.Invoke();
                _blackoutImage.gameObject.SetActive(false);
            }));
        }
    }
}
