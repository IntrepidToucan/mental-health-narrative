using UnityEngine;
using UnityEngine.UIElements;

namespace Utilities
{
    public class SceneFader : MonoBehaviour
    {
        private UIDocument _uiDoc;
        private VisualElement _rootElement;
        
        private const string HiddenClass = "hidden";
        
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

        public void FadeIn()
        {
            _rootElement.AddToClassList(HiddenClass);
        }
        
        public void FadeOut()
        {
            _rootElement.RemoveFromClassList(HiddenClass);
        }
    }
}
