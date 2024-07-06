using Characters.NPCs;
using UI.Dialogue;
using UI.HUD;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class UiManager : Singleton<UiManager>
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject dialogueOverlayPrefab;
        [SerializeField] private GameObject hudPrefab;
        [SerializeField] private GameObject pauseMenuPrefab;

        public DialogueOverlay DialogueOverlay { get; private set; }

        private Hud _hud;
        
        public void ShowDialogueOverlay(Npc npc)
        {
            HideHud();
            TryDestroyDialogueOverlay();

            DialogueOverlay = Instantiate(dialogueOverlayPrefab, gameObject.transform).GetComponent<DialogueOverlay>();
            DialogueOverlay.SetParams(npc);
        }

        public void HideDialogueOverlay()
        {
            TryDestroyDialogueOverlay();
            ShowHud();
        }

        protected override void Awake()
        {
            PersistAcrossScenes = true;
            
            base.Awake();
            
            _hud = Instantiate(hudPrefab, gameObject.transform).GetComponent<Hud>();
        }

        private void HideHud() => _hud.gameObject.SetActive(false);
        private void ShowHud() => _hud.gameObject.SetActive(true);

        private void TryDestroyDialogueOverlay()
        {
            if (DialogueOverlay == null) return;
            
            Destroy(DialogueOverlay.gameObject);
            DialogueOverlay = null;
        }
    }
}
