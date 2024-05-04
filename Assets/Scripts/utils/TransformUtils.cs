using DG.Tweening;
using NTC.MonoCache;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class TransformUtils : MonoCache {
        public void TransformItem(List<Item> items, Transform item, TweenCallback OnComplete)
        {
            var scale = item.localScale;
            item
                .DOLocalMove(new Vector3(0, items.Count * scale.y, 0), 0.2f)
                .OnComplete(OnComplete)
                .SetLink(item.gameObject);
        }
    }
}
