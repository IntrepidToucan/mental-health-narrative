using UnityEngine;
using UnityEngine.UIElements;

namespace UI.HUD
{
    public class Hud : MonoBehaviour
    {
        private UIDocument _uiDoc;
        private Button _logBookButton;
        private Button _pauseMenuButton;

        private void Awake()
        {
            _uiDoc = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            _logBookButton = _uiDoc.rootVisualElement.Q<Button>("log-book-button");
            _logBookButton.RegisterCallback<ClickEvent>(OpenLogBook);
            _logBookButton.focusable = false;
            _logBookButton.tabIndex = -1;
            
            _pauseMenuButton = _uiDoc.rootVisualElement.Q<Button>("pause-menu-button");
            _pauseMenuButton.RegisterCallback<ClickEvent>(OpenPauseMenu);
            _pauseMenuButton.focusable = false;
            _pauseMenuButton.tabIndex = -1;
        }

        private void OpenLogBook(ClickEvent evt)
        {
            Debug.Log("open log book");
        }
        
        private void OpenPauseMenu(ClickEvent evt)
        {
            Debug.Log("open pause menu");
        }
    }
}
