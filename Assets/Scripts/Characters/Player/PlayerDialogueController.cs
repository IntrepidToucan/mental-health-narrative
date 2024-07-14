using System.Collections;
using System.Collections.Generic;
using Characters.NPCs;
using Ink.Runtime;
using Interaction;
using Managers;
using UnityEngine;

namespace Characters.Player
{
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

        private InkScript _inkScript;
        private Npc _npc;
        private AdvanceableState _advanceableState = AdvanceableState.CanNotAdvance;

        public void StartDialogue(Npc npc, InkScript inkScript)
        {
            Player.Instance.PlayerInput.currentActionMap.Disable();
            
            _inkScript = inkScript;
            _npc = npc;
            _advanceableState = AdvanceableState.CanNotAdvance;

            UiManager.Instance.ShowDialogueOverlay(_npc);
            
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
            
            _inkScript.Story.ChooseChoiceIndex(choice.index);

            StartCoroutine(TransitionToNextDialogue());
        }

        private IEnumerator ShowDialogueBox()
        {
            // We need to delay before showing the dialogue image.
            // If we add the visibility class immediately, the animation transition won't work.
            yield return new WaitForSeconds(visibilityDelay_DialogueImage);
            
            UiManager.Instance.DialogueOverlay.ShowDialogueImage();
            
            yield return new WaitUntil(() => UiManager.Instance.DialogueOverlay.IsDialogueImageVisible(opacityThreshold_DialogueImage));
            
            UiManager.Instance.DialogueOverlay.ShowDialogueTextContainer();
            
            yield return new WaitUntil(() => UiManager.Instance.DialogueOverlay.IsDialogueTextContainerVisible(opacityThreshold_DialogueTextContainer));
            
            StartCoroutine(ShowNextDialogue());
        }

        private IEnumerator ShowNextDialogue()
        {
            if (debugInk)
            {
                Debug.Log($"Ink canContinue? {_inkScript.Story.canContinue}, {_inkScript.Story.currentChoices.Count} choices, currentText: {_inkScript.Story.currentText}");
            }

            if (!_inkScript.Story.canContinue)
            {
                yield return new WaitUntil(() => UiManager.Instance.DialogueOverlay.IsDialogueTextContainerVisible());
                
                StartCoroutine(EndDialogue());
                
                yield break;
            }
            
            UiManager.Instance.DialogueOverlay.SetDialogue(_inkScript.Story.Continue(),
                _npc.NpcData.PortraitMap.GetValueOrDefault(NpcData.Demeanor.Neutral, _npc.NpcData.DefaultPortrait));
            UiManager.Instance.DialogueOverlay.ShowDialogueText();
            UiManager.Instance.DialogueOverlay.SetDialogueChoices(_inkScript.Story.currentChoices);

            if (_inkScript.Story.currentChoices.Count > 0)
            {
                // We need to `yield` before starting the "show dialogue choices" coroutine.
                // If we don't include the delay that comes with the `yield`,
                // the transition animation for the first dialogue choice won't work.
                yield return new WaitUntil(() => UiManager.Instance.DialogueOverlay.IsDialogueTextVisible(opacityThreshold_DialogueText));

                StartCoroutine(UiManager.Instance.DialogueOverlay.ShowDialogueChoicesWithDelay(visibilityDelay_DialogueChoice));
                
                yield return new WaitUntil(() => UiManager.Instance.DialogueOverlay.AreDialogueChoicesVisible(advancementOpacity_DialogueChoice));
                
                _advanceableState = AdvanceableState.CanSelectDialogueChoice;
            }
            else
            {
                yield return new WaitUntil(() => UiManager.Instance.DialogueOverlay.IsDialogueTextVisible(advancementOpacity_DialogueText));
                
                _advanceableState = AdvanceableState.CanAdvanceDialogue;
            }
        }

        private IEnumerator TransitionToNextDialogue()
        {
            UiManager.Instance.DialogueOverlay.HideDialogueText();
            
            StartCoroutine(UiManager.Instance.DialogueOverlay.HideDialogueChoicesWithDelay(visibilityDelay_DialogueChoice));
            
            yield return new WaitUntil(() => UiManager.Instance.DialogueOverlay.IsDialogueTextHidden());
            yield return new WaitUntil(() => UiManager.Instance.DialogueOverlay.AreDialogueChoicesHidden());

            StartCoroutine(ShowNextDialogue());
        }

        private IEnumerator EndDialogue()
        {
            UiManager.Instance.DialogueOverlay.HideDialogueTextContainer();
            UiManager.Instance.DialogueOverlay.HideDialogueImage();
            
            yield return new WaitUntil(() => UiManager.Instance.DialogueOverlay.IsDialogueTextContainerHidden());
            yield return new WaitUntil(() => UiManager.Instance.DialogueOverlay.IsDialogueImageHidden());
            
            UiManager.Instance.HideDialogueOverlay();
            
            _inkScript = null;
            _npc = null;
            
            Player.Instance.PlayerInput.currentActionMap.Enable();
        }
    }
}
