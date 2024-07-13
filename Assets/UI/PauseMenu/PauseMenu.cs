using System.Collections;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.PauseMenu
{
    public class PauseMenu : MonoBehaviour
    {
        [Header("UXML")]
        [SerializeField] private VisualTreeAsset logBookMenuUxml;
        [SerializeField] private VisualTreeAsset settingsMenuUxml;
        
        private const string SelectedClass = "selected";
        private const string VisibleClass = "visible";
        
        private UIDocument _uiDoc;
        private UiManager.PauseMenuTab _activeTabId = UiManager.PauseMenuTab.Settings;

        private VisualElement _rootContainer;
        private VisualElement _tabsContainer;
        private Button _logBookTab;
        private Button _settingsTab;
        private VisualElement _tabContentContainer;

        public void SetActiveTab(UiManager.PauseMenuTab tabId)
        {
            if (_activeTabId == tabId && _tabContentContainer.Children().Any()) return;

            _tabsContainer.Query<Button>(className: "tab").ForEach(tab =>
                tab.RemoveFromClassList(SelectedClass));
            _tabContentContainer.Clear();
            
            _activeTabId = tabId;
            Button button = null;
            
            switch (_activeTabId)
            {
                case UiManager.PauseMenuTab.LogBook:
                    button = _logBookTab;
                    _tabContentContainer.Add(logBookMenuUxml.Instantiate());
                    break;
                case UiManager.PauseMenuTab.Settings:
                    button = _settingsTab;
                    _tabContentContainer.Add(settingsMenuUxml.Instantiate());
                    break;
                default:
                    Debug.LogWarning($"Invalid pause menu tab ID: {tabId}");
                    break;
            }

            button?.AddToClassList(SelectedClass);
            button?.Focus();
        }

        private void Awake()
        {
            _uiDoc = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            _uiDoc.rootVisualElement.RegisterCallback<ClickEvent>(HandleOverlayClick);

            _rootContainer = _uiDoc.rootVisualElement.Q("root");
            _tabsContainer = _uiDoc.rootVisualElement.Q("tabs-container");
            _tabContentContainer = _uiDoc.rootVisualElement.Q("tab-content-container");
            
            _logBookTab = _uiDoc.rootVisualElement.Q<Button>("log-book-tab");
            _logBookTab.RegisterCallback<ClickEvent>(SelectLogBookTab);
            _logBookTab.RegisterCallback<KeyDownEvent>(SelectLogBookTab);
            
            _settingsTab = _uiDoc.rootVisualElement.Q<Button>("settings-tab");
            _settingsTab.RegisterCallback<ClickEvent>(SelectSettingsTab);
            _settingsTab.RegisterCallback<KeyDownEvent>(SelectSettingsTab);

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
        
        private void SelectLogBookTab(ClickEvent evt) => SetActiveTab(UiManager.PauseMenuTab.LogBook);
        private void SelectSettingsTab(ClickEvent evt) => SetActiveTab(UiManager.PauseMenuTab.Settings);

        private void SelectLogBookTab(KeyDownEvent evt)
        {
            if (UiManager.IsSubmitKeyDown(evt)) SetActiveTab(UiManager.PauseMenuTab.LogBook);
        }
        
        private void SelectSettingsTab(KeyDownEvent evt)
        {
            if (UiManager.IsSubmitKeyDown(evt)) SetActiveTab(UiManager.PauseMenuTab.Settings);
        }
    }
}
