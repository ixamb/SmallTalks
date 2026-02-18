using JetBrains.Annotations;

namespace SmallTalks.UI.Tools
{
    using UnityEngine;

    [ExecuteAlways]
    public class SafeAreaCanvas : MonoBehaviour
    {
        [UsedImplicitly] [SerializeField] private bool applyTop = true;
        [UsedImplicitly] [SerializeField] private bool applyBottom = true;
        [UsedImplicitly] [SerializeField] private bool applyLeft = true;
        [UsedImplicitly] [SerializeField] private bool applyRight = true;

        private RectTransform _rectTransform;
        private Rect _lastSafeArea;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
            ApplySafeArea();
        }
#endif
        
        private void OnEnable()
        {
            _rectTransform = GetComponent<RectTransform>();
            ApplySafeArea();
        }

        private void LateUpdate()
        {
            if (Screen.safeArea != _lastSafeArea)
                ApplySafeArea();
        }

        void ApplySafeArea()
        {
            _rectTransform.anchorMin = Vector2.zero;
            _rectTransform.anchorMax = Vector2.one;
            _rectTransform.offsetMin = Vector2.zero;
            _rectTransform.offsetMax = Vector2.zero;

#if !UNITY_EDITOR
            Rect safeArea = Screen.safeArea;
            _lastSafeArea = safeArea;

            var canvas = GetComponentInParent<Canvas>();
            var scale = 1f / canvas.scaleFactor;

            float screenWidth  = Screen.width;
            float screenHeight = Screen.height;

            var left = safeArea.xMin;
            var bottom = safeArea.yMin;
            var right = screenWidth - safeArea.xMax;
            var top = screenHeight - safeArea.yMax;

            _rectTransform.offsetMin = new Vector2(
                applyLeft   ? left   * scale : 0,
                applyBottom ? bottom * scale : 0
            );
            _rectTransform.offsetMax = new Vector2(
                applyRight ? -right * scale : 0,
                applyTop   ? -top   * scale : 0
            );
#endif
        }
    }
}