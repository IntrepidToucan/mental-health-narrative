using Ink.Runtime;
using UI.Dialogue;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerDialogueController : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject dialogueBoxPrefab;
        [SerializeField] private GameObject dialogueChoicesOverlayPrefab;

        private GameObject _dialogueChoicesOverlay;
        private Story _inkStory;

        public void StartDialogue(Story inkStory)
        {
            _inkStory = inkStory;
            _dialogueChoicesOverlay ??= Instantiate(dialogueChoicesOverlayPrefab);
            
            DisplayDialogueStep();
        }

        private void DisplayDialogueStep()
        {
            Debug.Log(_inkStory.ContinueMaximally());
            
            var overlay = _dialogueChoicesOverlay.GetComponent<DialogueChoicesOverlay>();
            overlay.SetDialogueChoices(_inkStory.currentChoices);
        }

        private void EndDialogue()
        {
            if (_dialogueChoicesOverlay is not null)
            {
                Destroy(_dialogueChoicesOverlay);
                _dialogueChoicesOverlay = null;
            }
        }
    }
}
