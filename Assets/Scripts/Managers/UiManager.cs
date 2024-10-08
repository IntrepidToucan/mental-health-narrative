using Characters.NPCs;
using Characters.Player;
using UI.Dialogue;
using UI.HUD;
using UI.PauseMenu;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

namespace Managers
{
    public class UiManager : PersistedSingleton<UiManager>
    {
        [Header("Data/Notifications")]
        [SerializeField] private NotificationData notificationData_ItemAcquired;
        [SerializeField] private NotificationData notificationData_LogBookUpdated;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject dialogueOverlayPrefab;
        [SerializeField] private GameObject hudPrefab;
        [SerializeField] private GameObject pauseMenuPrefab;

        private Hud _hud;
        private PauseMenu _pauseMenu;
        
        public static bool IsSubmitKeyDown(KeyDownEvent evt) => evt.keyCode is KeyCode.Space or KeyCode.Return;
        
        public DialogueOverlay DialogueOverlay { get; private set; }
        
        public NotificationData NotificationData_ItemAcquired => notificationData_ItemAcquired;
        public NotificationData NotificationData_LogBookUpdated => notificationData_LogBookUpdated;

        public void OpenPauseMenu(PauseMenuTab tabId = PauseMenuTab.Settings)
        {
            if (_pauseMenu == null)
            {
                Time.timeScale = 0;
                Player.Instance.PlayerInput.SwitchCurrentActionMap("UI");

                HideHud();

                _pauseMenu = Instantiate(pauseMenuPrefab, gameObject.transform).GetComponent<PauseMenu>();
            }
            
            _pauseMenu.SetActiveTab(tabId);
        }
        public void ClosePauseMenu()
        {
            TryDestroyPauseMenu();
            ShowHud();
            
            Player.Instance.PlayerInput.SwitchCurrentActionMap("Player");
            Time.timeScale = 1;
        }

        public void ShowDialogueOverlay(Npc npc)
        {
            HideHud();
            TryDestroyDialogueOverlay();

            DialogueOverlay = Instantiate(dialogueOverlayPrefab, gameObject.transform).GetComponent<DialogueOverlay>();
            DialogueOverlay.SetNpc(npc);
        }

        public void HideDialogueOverlay()
        {
            TryDestroyDialogueOverlay();
            ShowHud();
        }

        protected override void Awake()
        {
            base.Awake();
            
            _hud = Instantiate(hudPrefab, gameObject.transform).GetComponent<Hud>();
        }

        private void HideHud() => _hud.Hide();
        private void ShowHud() => _hud.Show();
        
        private void TryDestroyPauseMenu()
        {
            if (_pauseMenu == null) return;
            
            Destroy(_pauseMenu.gameObject);
            _pauseMenu = null;
        }

        private void TryDestroyDialogueOverlay()
        {
            if (DialogueOverlay == null) return;
            
            Destroy(DialogueOverlay.gameObject);
            DialogueOverlay = null;
        }
    }
}
