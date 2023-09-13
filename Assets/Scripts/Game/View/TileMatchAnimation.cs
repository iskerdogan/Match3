using System;
using DG.Tweening;
using UnityEngine;

namespace Game.View
{
    public interface ITileMatchAnimation
    {
        public float Duration { get; }
    }
    public class TileMatchAnimation : MonoBehaviour , ITileMatchAnimation
    {
        [SerializeField] private float duration;

        public float Duration => duration;

        public void Execute(Transform transformTile,Action onComplete = null)
        {
            transformTile.DOScale(0, duration).SetEase(Ease.InBack).OnComplete(()=> onComplete?.Invoke());
        }
    }
}