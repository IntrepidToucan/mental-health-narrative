using UnityEngine;
using UnityEngine.UIElements;

namespace Utilities
{
    public class SceneFader : MonoBehaviour
    {
        private UIDocument _uiDoc;
        private VisualElement _rootElement;
        
        private const string HiddenClass = "hidden";
        
        public bool IsFadedIn(float opacityThreshold = 0f)
        {
            return _rootElement.ClassListContains(HiddenClass) &&
                   _rootElement.resolvedStyle.opacity <= opacityThreshold + Mathf.Epsilon;
        }

        public bool IsFadedOut(float opacityThreshold = 1f)
        {
            return !_rootElement.ClassListContains(HiddenClass) &&
                   _rootElement.resolvedStyle.opacity >= opacityThreshold + Mathf.Epsilon;
        }
        
        public void FadeIn() => _rootElement.AddToClassList(HiddenClass);
        public void FadeOut() => _rootElement.RemoveFromClassList(HiddenClass);
        
        private void Awake()
        {
            _uiDoc = GetComponent<UIDocument>();
            _uiDoc.sortingOrder = 100f;
        }

        private void OnEnable()
        {
            _rootElement = _uiDoc.rootVisualElement.Q("root");
            // This UI element is in front of everything else,
            // so prevent it from intercepting mouse events.
            _rootElement.pickingMode = PickingMode.Ignore;
        }
    }
}
