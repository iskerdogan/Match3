using System;
using DG.Tweening;
using UnityEngine;

namespace Game.View
{
    public class TileMismatchAnimation : MonoBehaviour
    {
        [SerializeField] private float duration;

        public float Duration => duration;

        private Sequence _sequence;
        public void Execute(Transform transformTile,Action onComplete = null)
        {
            transformTile.eulerAngles = Vector3.zero;
            var appendDuration = duration / 4;
            _sequence = DOTween.Sequence();
            _sequence.Append(transformTile.DORotate(transformTile.eulerAngles + Vector3.forward * 30, appendDuration));
            _sequence.Append(transformTile.DORotate(Vector3.zero, appendDuration));
            _sequence.Append(transformTile.DORotate(transformTile.eulerAngles + Vector3.forward * -30, appendDuration));
            _sequence.Append(transformTile.DORotate(Vector3.zero, appendDuration));
            _sequence.SetEase(Ease.Linear);
        }
    }
}
