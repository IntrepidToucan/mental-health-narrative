using System.Collections;
using Characters.NPCs;
using Ink.Runtime;
using UnityEngine;

namespace Characters.Player
{
    [RequireComponent(typeof(Player))]
    public class PlayerDialogueController : MonoBehaviour
    {
        [Header("Animation/Advancement")]
        [SerializeField, Range(0f, 1f)] private float advancementOpacity_DialogueChoice = 0.5f;
        [SerializeField, Range(0f, 1f)] private float advancementOpacity_DialogueText = 0.5f;
        
        [Header("Animation/Delay")]
        [SerializeField, Min(0f)] private float visibilityDelay_DialogueChoice = 0.2f;
        [SerializeField, Min(0f)] private float visibilityDelay_DialogueImage = 0.1f;
        
        [Header("Animation/Opacity")]
        [SerializeField, Range(0f, 1f)] private float opacityThreshold_DialogueImage = 0.2f;
        [SerializeField, Range(0f, 1f)] private float opacityThreshold_DialogueText = 1f;
        [SerializeField, Range(0f, 1f)] private float opacityThreshold_DialogueTextContainer = 0.8f;

        [Header("Debug")]
        [SerializeField] private bool debugInk;

        private enum AdvanceableState
        {
            CanNotAdvance,
            CanAdvanceDialogue,
            CanSelectDialogueChoice
        }

        private Player _player;
        private Story _inkStory;
        private Npc _npc;
        private AdvanceableState _advanceableState = AdvanceableState.CanNotAdvance;

        public void StartDialogue(Npc npc, Story inkStory)
        {
            _player.PlayerInput.SwitchCurrentActionMap("UI");
            
            _inkStory = inkStory;
            _npc = npc;
            _advanceableState = AdvanceableState.CanNotAdvance;

            _player.UiController.CreateDialogueOverlay(_player);
            
            StartCoroutine(ShowDialogueBox());
        }

        public void TryAdvanceDialogue()
        {
            if (_advanceableState != AdvanceableState.CanAdvanceDialogue) return;

            _advanceableState = AdvanceableState.CanNotAdvance;
            
            StartCoroutine(TransitionToNextDialogue());
        }

        public void TrySelectDialogueChoice(Choice choice)
        {
            if (_advanceableState != AdvanceableState.CanSelectDialogueChoice) return;

            _advanceableState = AdvanceableState.CanNotAdvance;
            
            _inkStory.ChooseChoiceIndex(choice.index);

            StartCoroutine(TransitionToNextDialogue());
        }

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private IEnumerator ShowDialogueBox()
        {
            // We need to delay before showing the dialogue image.
            // If we add the visibility class immediately, the animation transition won't work.
            yield return new WaitForSeconds(visibilityDelay_DialogueImage);
            
            _player.UiController.DialogueOverlay.ShowDialogueImage();
            
            yield return new WaitUntil(() => _player.UiController.DialogueOverlay.IsDialogueImageVisible(opacityThreshold_DialogueImage));
            
            _player.UiController.DialogueOverlay.ShowDialogueTextContainer();
            
            yield return new WaitUntil(() => _player.UiController.DialogueOverlay.IsDialogueTextContainerVisible(opacityThreshold_DialogueTextContainer));
            
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
                yield return new WaitUntil(() => _player.UiController.DialogueOverlay.IsDialogueTextContainerVisible());
                
                StartCoroutine(EndDialogue());
                
                yield break;
            }
            
            _player.UiController.DialogueOverlay.SetDialogue(_inkStory.Continue());
            _player.UiController.DialogueOverlay.ShowDialogueText();
            _player.UiController.DialogueOverlay.SetDialogueChoices(_inkStory.currentChoices);

            if (_inkStory.currentChoices.Count > 0)
            {
                // We need to `yield` before starting the "show dialogue choices" coroutine.
                // If we don't include the delay that comes with the `yield`,
                // the transition animation for the first dialogue choice won't work.
                yield return new WaitUntil(() => _player.UiController.DialogueOverlay.IsDialogueTextVisible(opacityThreshold_DialogueText));

                StartCoroutine(_player.UiController.DialogueOverlay.ShowDialogueChoicesWithDelay(visibilityDelay_DialogueChoice));
                
                yield return new WaitUntil(() => _player.UiController.DialogueOverlay.AreDialogueChoicesVisible(advancementOpacity_DialogueChoice));
                
                _advanceableState = AdvanceableState.CanSelectDialogueChoice;
            }
            else
            {
                yield return new WaitUntil(() => _player.UiController.DialogueOverlay.IsDialogueTextVisible(advancementOpacity_DialogueText));
                
                _advanceableState = AdvanceableState.CanAdvanceDialogue;
            }
        }

        private IEnumerator TransitionToNextDialogue()
        {
            _player.UiController.DialogueOverlay.HideDialogueText();
            
            StartCoroutine(_player.UiController.DialogueOverlay.HideDialogueChoicesWithDelay(visibilityDelay_DialogueChoice));
            
            yield return new WaitUntil(() => _player.UiController.DialogueOverlay.IsDialogueTextHidden());
            yield return new WaitUntil(() => _player.UiController.DialogueOverlay.AreDialogueChoicesHidden());

            StartCoroutine(ShowNextDialogue());
        }

        private IEnumerator EndDialogue()
        {
            _player.UiController.DialogueOverlay.HideDialogueTextContainer();
            _player.UiController.DialogueOverlay.HideDialogueImage();
            
            yield return new WaitUntil(() => _player.UiController.DialogueOverlay.IsDialogueTextContainerHidden());
            yield return new WaitUntil(() => _player.UiController.DialogueOverlay.IsDialogueImageHidden());
            
            _player.UiController.DestroyDialogueOverlay();
            
            _inkStory = null;
            _npc = null;
            
            _player.PlayerInput.SwitchCurrentActionMap("Player");
        }
    }
}
