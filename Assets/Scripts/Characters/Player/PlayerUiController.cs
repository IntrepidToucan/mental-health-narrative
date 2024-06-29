using UI.Dialogue;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerUiController : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject dialogueOverlayPrefab;
        [SerializeField] private GameObject hudPrefab;

        public DialogueOverlay DialogueOverlay { get; private set; }

        public void CreateDialogueOverlay(Player player)
        {
            if (DialogueOverlay is not null) DestroyDialogueOverlay();
            
            DialogueOverlay = Instantiate(dialogueOverlayPrefab).GetComponent<DialogueOverlay>();
            DialogueOverlay.Init(player);
        }

        public void DestroyDialogueOverlay()
        {
            if (DialogueOverlay is null) return;
            
            Destroy(DialogueOverlay.gameObject);
            DialogueOverlay = null;
        }
    }
}
