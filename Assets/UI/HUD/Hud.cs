using Characters.Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.HUD
{
    public class Hud : MonoBehaviour
    {
        private Player _player;
        private UIDocument _uiDoc;

        private VisualElement _rootContainer;
        
        private Button _logBookButton;
        private Button _pauseMenuButton;

        public void SetParams(Player player)
        {
            _player = player;
        }

        private void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
            
            _uiDoc = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            _rootContainer = _uiDoc.rootVisualElement.Q("hud");
            
            _logBookButton = _rootContainer.Q<Button>("log-book-button");
            _logBookButton.RegisterCallback<ClickEvent>(OpenLogBook);
            _logBookButton.focusable = false;
            _logBookButton.tabIndex = -1;
            
            _pauseMenuButton = _rootContainer.Q<Button>("pause-menu-button");
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
