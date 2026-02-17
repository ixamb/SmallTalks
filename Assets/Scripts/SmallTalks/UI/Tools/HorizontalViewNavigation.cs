using System.Linq;
using UnityEngine;

namespace SmallTalks.UI.Tools
{
    public sealed class HorizontalViewNavigation : MonoBehaviour
    {
        [SerializeField] private float navigationSpeed = 10f;

        private int _elements;
    
        private bool _navigate;
        private float _xNavigationGoal;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _elements = transform.Cast<Transform>().Count(child => child.GetComponent<HorizontalViewNavElement>() != null);
        }

        private void Update()
        {
            if (!_navigate)
                return;

            var currentX = _rectTransform.anchoredPosition.x;
            var newX = Mathf.Lerp(currentX, -_xNavigationGoal, navigationSpeed * Time.deltaTime);
            _rectTransform.anchoredPosition = new Vector2(newX, _rectTransform.anchoredPosition.y);
            if (!(Mathf.Abs(newX - -_xNavigationGoal) < 0.5f))
                return;
            
            _rectTransform.anchoredPosition = new Vector2(-_xNavigationGoal, _rectTransform.anchoredPosition.y);
            _navigate = false;
        }

        public void NavigateTo(HorizontalViewNavElement horizontalViewNavElement)
        {
            var width = _rectTransform.rect.width;
            _xNavigationGoal = (width / _elements) * horizontalViewNavElement.Index;
            _navigate = true;
        }
    }
}