using Characters.NPCs;
using UI.Dialogue;
using UI.HUD;
using UnityEngine;

namespace Characters.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerUiController : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject dialogueOverlayPrefab;
        [SerializeField] private GameObject hudPrefab;
        [SerializeField] private GameObject pauseMenuPrefab;

        public DialogueOverlay DialogueOverlay { get; private set; }

        private Player _player;
        private Hud _hud;
        
        public void CreateDialogueOverlay(Npc npc)
        {
            HideHud();
            
            if (DialogueOverlay is not null) DestroyDialogueOverlay();

            DialogueOverlay = Instantiate(dialogueOverlayPrefab).GetComponent<DialogueOverlay>();
            DialogueOverlay.SetParams(_player, npc);
        }

        public void DestroyDialogueOverlay()
        {
            if (DialogueOverlay is null) return;
            
            Destroy(DialogueOverlay.gameObject);
            DialogueOverlay = null;

            ShowHud();
        }

        private void Awake()
        {
            _player = GetComponent<Player>();
            
            _hud = Instantiate(hudPrefab).GetComponent<Hud>();
            _hud.SetParams(_player);
        }

        private void HideHud() => _hud.gameObject.SetActive(false);
        private void ShowHud() => _hud.gameObject.SetActive(true);
    }
}
