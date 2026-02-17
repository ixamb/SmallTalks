using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SmallTalks.UI.Tools
{
    [ExecuteAlways]
    public class AdaptiveTextWidth : MonoBehaviour
    {
        [SerializeField] private float maxWidth = 283f;
        private TMP_Text _tmpText;
        private string _lastText = "";
        private float _lastWidth = -1f;

        private void OnEnable()
        {
            _tmpText = GetComponent<TMP_Text>();
        }

        private void LateUpdate()
        {
            if (_tmpText is null || _tmpText.text == _lastText)
                return;
        
            _lastText = _tmpText.text;
            AdjustWidth();
        }

        void AdjustWidth()
        {
            _tmpText.textWrappingMode = TextWrappingModes.Normal;
            _tmpText.ForceMeshUpdate();
        
            var preferredWidth = _tmpText.preferredWidth;
            var newWidth = Mathf.Min(preferredWidth, maxWidth);

            if (!(Mathf.Abs(newWidth - _lastWidth) > 0.1f))
                return;
            _lastWidth = newWidth;
            var rectTransform = _tmpText.rectTransform;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
            _tmpText.ForceMeshUpdate();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform.parent as RectTransform);
        }
    }
}