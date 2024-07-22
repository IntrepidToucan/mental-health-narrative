using System.Collections;
using System.Linq;
using Managers;
using UI.PauseMenu.LogBook;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

namespace UI.PauseMenu
{
    [RequireComponent(typeof(LogBookMenu))]
    public class PauseMenu : MonoBehaviour
    {
        [Header("UXML")]
        [SerializeField] private VisualTreeAsset settingsMenuUxml;
        
        private const string SelectedClass = "selected";
        private const string VisibleClass = "visible";

        private LogBookMenu _logBookMenu;
        private UIDocument _uiDoc;
        private PauseMenuTab _activeTabId = PauseMenuTab.Settings;

        private VisualElement _rootContainer;
        private VisualElement _tabsContainer;
        private Button _settingsTab;
        private VisualElement _tabContentContainer;
        private Button _backButton;

        public Button LogBookTab { get; private set; }

        public void SetActiveTab(PauseMenuTab tabId)
        {
            if (_activeTabId == tabId && _tabContentContainer.Children().Any()) return;

            HideBackButton();
            _tabsContainer.Query<Button>(className: "tab")
                .ForEach(tab => tab.RemoveFromClassList(SelectedClass));
            _tabContentContainer.Clear();
            
            _activeTabId = tabId;
            Button newActiveTab = null;
            
            switch (_activeTabId)
            {
                case PauseMenuTab.LogBook:
                    newActiveTab = LogBookTab;
                    _logBookMenu.AddToContainer(_tabContentContainer);
                    break;
                case PauseMenuTab.Settings:
                    newActiveTab = _settingsTab;
                    _tabContentContainer.Add(settingsMenuUxml.Instantiate());
                    break;
                default:
                    Debug.LogError($"Invalid pause menu tab ID: {_activeTabId}");
                    break;
            }

            newActiveTab?.AddToClassList(SelectedClass);
            newActiveTab?.Focus();
        }

        public void ShowBackButton()
        {
            _backButton.focusable = true;
            _backButton.style.visibility = Visibility.Visible;
        }

        public void HideBackButton()
        {
            _backButton.focusable = false;
            _backButton.style.visibility = Visibility.Hidden;
        }

        private void Awake()
        {
            _logBookMenu = GetComponent<LogBookMenu>();
            _uiDoc = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            _uiDoc.rootVisualElement.RegisterCallback<ClickEvent>(HandleOverlayClick);

            _rootContainer = _uiDoc.rootVisualElement.Q("pause-menu");
            _tabsContainer = _uiDoc.rootVisualElement.Q("tabs-container");
            _tabContentContainer = _uiDoc.rootVisualElement.Q("tab-content-container");
            
            LogBookTab = _uiDoc.rootVisualElement.Q<Button>("log-book-tab");
            LogBookTab.RegisterCallback<ClickEvent>(SelectLogBookTab);
            LogBookTab.RegisterCallback<KeyDownEvent>(SelectLogBookTab);
            
            _settingsTab = _uiDoc.rootVisualElement.Q<Button>("settings-tab");
            _settingsTab.RegisterCallback<ClickEvent>(SelectSettingsTab);
            _settingsTab.RegisterCallback<KeyDownEvent>(SelectSettingsTab);
            
            _backButton = _uiDoc.rootVisualElement.Q<Button>("back-button");
            _backButton.RegisterCallback<ClickEvent>(HandleBackAction);
            _backButton.RegisterCallback<KeyDownEvent>(HandleBackAction);

            var closeButton = _uiDoc.rootVisualElement.Q<Button>("close-button");
            closeButton.RegisterCallback<ClickEvent>(CloseMenu);
            closeButton.RegisterCallback<KeyDownEvent>(CloseMenu);

            StartCoroutine(AddVisibleClass());
        }

        private IEnumerator AddVisibleClass()
        {
            // Use real-time seconds since the game is paused and the timescale is 0.
            yield return new WaitForSecondsRealtime(0.1f);
            
            _rootContainer.AddToClassList(VisibleClass);
        }

        private static void HandleOverlayClick(ClickEvent evt)
        {
            if (evt.target is VisualElement { name: "root" }) UiManager.Instance.ClosePauseMenu();
        }
        
        private static void CloseMenu(ClickEvent evt) => UiManager.Instance.ClosePauseMenu();
        
        private static void CloseMenu(KeyDownEvent evt)
        {
            if (UiManager.IsSubmitKeyDown(evt)) UiManager.Instance.ClosePauseMenu();
        }
        
        private void HandleBackAction(ClickEvent evt) => HandleBackAction();
        
        private void HandleBackAction(KeyDownEvent evt)
        {
            if (UiManager.IsSubmitKeyDown(evt)) HandleBackAction();
        }

        private void HandleBackAction()
        {
            switch (_activeTabId)
            {
                case PauseMenuTab.LogBook:
                    _logBookMenu.HandleBackAction();
                    break;
                case PauseMenuTab.Settings:
                    break;
                default:
                    Debug.LogError($"Invalid pause menu tab ID: {_activeTabId}");
                    break;
            }
        }
        
        private void SelectLogBookTab(ClickEvent evt) => SetActiveTab(PauseMenuTab.LogBook);
        private void SelectSettingsTab(ClickEvent evt) => SetActiveTab(PauseMenuTab.Settings);

        private void SelectLogBookTab(KeyDownEvent evt)
        {
            if (UiManager.IsSubmitKeyDown(evt)) SetActiveTab(PauseMenuTab.LogBook);
        }
        
        private void SelectSettingsTab(KeyDownEvent evt)
        {
            if (UiManager.IsSubmitKeyDown(evt)) SetActiveTab(PauseMenuTab.Settings);
        }
    }
}
