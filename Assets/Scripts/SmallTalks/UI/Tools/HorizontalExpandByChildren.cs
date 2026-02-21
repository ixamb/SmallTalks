using System.Linq;
using UnityEngine;

namespace SmallTalks.UI.Tools
{
    public sealed class HorizontalExpandWidthByChildren : MonoBehaviour
    {
        [SerializeField] private RectTransform referenceRect;
        private RectTransform _rectTransform;

        private void OnEnable()
        {
            _rectTransform = GetComponent<RectTransform>();
            UpdateWidth();
        }

        private void OnTransformChildrenChanged() => UpdateWidth();

        private void OnRectTransformDimensionsChange() => UpdateWidth();

        private int GetValidChildCount()
        {
            return transform.Cast<Transform>().Count(child => child.GetComponent<HorizontalViewNavElement>());
        }

        public void UpdateWidth()
        {
            if (!_rectTransform || !referenceRect) return;

            var validCount = GetValidChildCount();
            var pageWidth = referenceRect.rect.width;
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pageWidth * validCount);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
            UpdateWidth();
        }
#endif
    }
}