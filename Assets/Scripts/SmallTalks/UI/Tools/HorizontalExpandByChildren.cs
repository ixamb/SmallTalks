using System.Linq;
using UnityEngine;

namespace SmallTalks.UI.Tools
{
    [ExecuteAlways] // TODO: should be optimized on publication
    public class HorizontalExpandWidthByChildren : MonoBehaviour
    {
        [SerializeField] private RectTransform referenceRect;
        private RectTransform _rectTransform;
        private float _lastReferenceWidth = -1f;
        private int _lastValidChildCount = -1;


        private void OnEnable()
        {
            _rectTransform = GetComponent<RectTransform>();
            UpdateWidth();
        }
        

        private void OnTransformChildrenChanged()
        {
            UpdateWidth();
        }

        private void LateUpdate()
        {
            if (!referenceRect) return;

            var currentValidCount = GetValidChildCount();
    
            if (currentValidCount != _lastValidChildCount || referenceRect.rect.width != _lastReferenceWidth)
            {
                _lastValidChildCount = currentValidCount;
                _lastReferenceWidth = referenceRect.rect.width;
                UpdateWidth();
            }
        }
        
        private int GetValidChildCount()
        {
            return transform.Cast<Transform>().Count(child => child.GetComponent<HorizontalViewNavElement>());
        }

        private void UpdateWidth()
        {
            if (!_rectTransform || !referenceRect) return;

            var validCount = GetValidChildCount();
            var pageWidth = referenceRect.rect.width;
            var totalWidth = pageWidth * validCount;

            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, totalWidth);
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