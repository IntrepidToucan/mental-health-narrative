using System.Collections;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

namespace UI.HUD
{
    public class Hud : MonoBehaviour
    {
        [Header("UXML")]
        [SerializeField] private VisualTreeAsset hudNotificationUxml;
        
        private const string VisibleClass = "visible";
        
        private UIDocument _uiDoc;

        private Button _logBookButton;
        private Button _pauseMenuButton;
        private VisualElement _notificationsContainer;

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

            _notificationsContainer = _uiDoc.rootVisualElement.Q("notifications-container");
            
            EventManager.OnNotificationTrigger += HandleNotificationTrigger;
            EventManager.OnTutorialTrigger += HandleTutorialTrigger;
        }
        
        private void OnDisable()
        {
            EventManager.OnNotificationTrigger -= HandleNotificationTrigger;
            EventManager.OnTutorialTrigger -= HandleTutorialTrigger;
        }
        
        private static void OpenLogBook(ClickEvent evt) => UiManager.Instance.OpenPauseMenu(PauseMenuTab.LogBook);
        private static void OpenPauseMenu(ClickEvent evt) => UiManager.Instance.OpenPauseMenu();
        
        private void HandleNotificationTrigger(NotificationData notificationData)
        {
            var notificationUi = hudNotificationUxml.Instantiate();
            notificationUi.Q<Label>("hud-notification-text").text = notificationData.text;

            _notificationsContainer.Add(notificationUi);

            StartCoroutine(HandleNotificationLifeCycle(notificationUi, notificationData.ttl));
        }

        private void HandleTutorialTrigger(TutorialData tutorialData)
        {
            Debug.Log(tutorialData);
        }

        private IEnumerator HandleNotificationLifeCycle(TemplateContainer notificationUi, float notificationTtl)
        {
            yield return new WaitForSeconds(0.1f);

            var rootElement = notificationUi.Q("hud-notification");
            rootElement.AddToClassList(VisibleClass);
            
            yield return new WaitForSeconds(notificationTtl);

            rootElement.RemoveFromClassList(VisibleClass);
            
            yield return new WaitForSeconds(rootElement.resolvedStyle.transitionDuration.ElementAt(0).value);
            
            _notificationsContainer.Remove(notificationUi);
        }
    }
}
