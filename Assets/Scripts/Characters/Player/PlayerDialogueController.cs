using System.Collections;
using Characters.NPCs;
using Ink.Runtime;
using UI.Dialogue;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerDialogueController : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private float opacityThreshold_DialogueImage = 0.2f;
        [SerializeField] private float opacityThreshold_DialogueTextContainer = 0.8f;
        [SerializeField] private float opacityThreshold_DialogueText = 1f;
        [SerializeField] private float visibilityDelay_DialogueImage = 0.1f;
        [SerializeField] private float visibilityDelay_DialogueChoice = 0.2f;

        [Header("Debug")]
        [SerializeField] private bool debugInk = false;
        
        [Header("UI")]
        [SerializeField] private GameObject dialogueOverlayPrefab;

        private PlayerInput _playerInput;
        private DialogueOverlay _dialogueOverlay;
        
        private Story _inkStory;
        private Npc _npc;

        public void StartDialogue(Npc npc, Story inkStory)
        {
            _playerInput.SwitchCurrentActionMap("UI");
            
            _inkStory = inkStory;
            _npc = npc;

            _dialogueOverlay = Instantiate(dialogueOverlayPrefab).GetComponent<DialogueOverlay>();
            _dialogueOverlay.Init(this);

            StartCoroutine(ShowDialogueBox());
        }

        public void ContinueDialogue()
        {
            // In case the user has managed to access the hidden "Continue Dialogue" button
            // even if there are dialogue choices available.
            if (_inkStory.currentChoices.Count > 0) return;
            
            StartCoroutine(TransitionToNextDialogue());
        }

        public void SelectDialogueChoice(Choice choice)
        {
            _inkStory.ChooseChoiceIndex(choice.index);

            StartCoroutine(TransitionToNextDialogue());
        }

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        private IEnumerator ShowDialogueBox()
        {
            // We need to delay before showing the dialogue image.
            // If we add the visibility class immediately, the animation transition doesn't work.
            yield return new WaitForSeconds(visibilityDelay_DialogueImage);
            
            _dialogueOverlay.ShowDialogueImage();
            
            yield return new WaitUntil(() => _dialogueOverlay.IsDialogueImageVisible(opacityThreshold_DialogueImage));
            
            _dialogueOverlay.ShowDialogueTextContainer();
            
            yield return new WaitUntil(() => _dialogueOverlay.IsDialogueTextContainerVisible(opacityThreshold_DialogueTextContainer));
            
            StartCoroutine(ShowNextDialogue());
        }

        private IEnumerator ShowNextDialogue()
        {
            if (debugInk)
            {
                Debug.Log($"Ink canContinue? {_inkStory.canContinue}, {_inkStory.currentChoices.Count} choices, currentText: {_inkStory.currentText}");
            }

            if (!_inkStory.canContinue)
            {
                yield return new WaitUntil(() => _dialogueOverlay.IsDialogueTextContainerVisible());
                
                StartCoroutine(EndDialogue());
                
                yield break;
            }
            
            _dialogueOverlay.SetDialogue(_inkStory.Continue());
            _dialogueOverlay.ShowDialogueText();
            
            // Instantiate the dialogue choice UI elements here so that we `yield` afterward.
            // If we don't include the delay that comes with the `yield`,
            // the transition animation for the first dialogue choice box won't work.
            _dialogueOverlay.SetDialogueChoices(_inkStory.currentChoices);
            
            yield return new WaitUntil(() => _dialogueOverlay.IsDialogueTextVisible(opacityThreshold_DialogueText));
            
            StartCoroutine(_dialogueOverlay.ShowDialogueChoicesWithDelay(visibilityDelay_DialogueChoice));
        }

        private IEnumerator TransitionToNextDialogue()
        {
            _dialogueOverlay.HideDialogueText();
            
            StartCoroutine(_dialogueOverlay.HideDialogueChoicesWithDelay(visibilityDelay_DialogueChoice));
            
            yield return new WaitUntil(() => _dialogueOverlay.IsDialogueTextHidden());
            yield return new WaitUntil(() => _dialogueOverlay.AreDialogueChoicesHidden());

            StartCoroutine(ShowNextDialogue());
        }

        private IEnumerator EndDialogue()
        {
            _dialogueOverlay.HideDialogueTextContainer();
            _dialogueOverlay.HideDialogueImage();
            
            yield return new WaitUntil(() => _dialogueOverlay.IsDialogueTextContainerHidden());
            yield return new WaitUntil(() => _dialogueOverlay.IsDialogueImageHidden());
            
            // TODO(P0): Add guard checks (bug: press space bar many times in quick succession).
            Destroy(_dialogueOverlay.gameObject);
            _dialogueOverlay = null;
            
            _inkStory = null;
            _npc = null;
            
            _playerInput.SwitchCurrentActionMap("Player");
        }
    }
}
