using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters.Player;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace UI.Dialogue
{
    public class DialogueOverlay : MonoBehaviour
    {
        [Header("UXML")]
        [SerializeField] private VisualTreeAsset dialogueBoxUxml;
        [SerializeField] private VisualTreeAsset dialogueChoiceUxml;

        private const string ClassName_Visible = "visible";
        
        private PlayerDialogueController _dialogueController;
        
        private VisualElement _choicesContainer;
        private VisualElement _dialogueContainer;
        
        private Button _continueButton;
        private VisualElement _dialogueImage;
        private Label _dialogueText;
        private VisualElement _dialogueTextContainer;
        
        public bool IsDialogueImageHidden(float opacityThreshold = 0f) => IsElementHidden(_dialogueImage, opacityThreshold);
        public bool IsDialogueImageVisible(float opacityThreshold = 1f) => IsElementVisible(_dialogueImage, opacityThreshold);
        public void HideDialogueImage() => _dialogueImage.RemoveFromClassList(ClassName_Visible);
        public void ShowDialogueImage() => _dialogueImage.AddToClassList(ClassName_Visible);
        
        public bool IsDialogueTextContainerHidden(float opacityThreshold = 0f) => IsElementHidden(_dialogueTextContainer, opacityThreshold);
        public bool IsDialogueTextContainerVisible(float opacityThreshold = 1f) => IsElementVisible(_dialogueTextContainer, opacityThreshold);
        public void HideDialogueTextContainer() => _dialogueTextContainer.RemoveFromClassList(ClassName_Visible);
        public void ShowDialogueTextContainer() => _dialogueTextContainer.AddToClassList(ClassName_Visible);
        
        public bool IsDialogueTextHidden(float opacityThreshold = 0f) => IsElementHidden(_dialogueText, opacityThreshold);
        public bool IsDialogueTextVisible(float opacityThreshold = 1f) => IsElementVisible(_dialogueText, opacityThreshold);
        public void HideDialogueText() =>_dialogueText.RemoveFromClassList(ClassName_Visible);
        public void ShowDialogueText() => _dialogueText.AddToClassList(ClassName_Visible);
        
        public void Init(PlayerDialogueController dialogueController)
        {
            _dialogueController = dialogueController;
            
            var uiDoc = GetComponent<UIDocument>();
            _continueButton = uiDoc.rootVisualElement.Q<Button>("continue-dialogue-button");
            _continueButton.RegisterCallback<KeyDownEvent>(ContinueDialogue);
            _continueButton.RegisterCallback<FocusOutEvent>(RefocusContinueButton);
            
            _dialogueContainer.Add(dialogueBoxUxml.Instantiate());
            _dialogueText = _dialogueContainer.Q<Label>("dialogue-text");
            _dialogueTextContainer = _dialogueContainer.Q("dialogue-text-container");
            _dialogueImage = _dialogueContainer.Q("dialogue-image");
        }

        public void SetDialogue(string text)
        {
            _dialogueText.text = text.Trim();
        }
        
        public void SetDialogueChoices(List<Choice> choices)
        {
            _choicesContainer.Query(className: "dialogue-choice-box").ForEach(element =>
            {
                _choicesContainer.Remove(element.parent);
            });

            foreach (var choice in choices)
            {
                var dialogueChoice = dialogueChoiceUxml.Instantiate();
                
                var button = dialogueChoice.Q<Button>("dialogue-choice-box");
                button.text = choice.text;
                button.RegisterCallback<ClickEvent, Choice>(SelectDialogueChoice, choice);
                button.RegisterCallback<KeyDownEvent, Choice>(SelectDialogueChoice, choice);
                button.SetEnabled(false);

                _choicesContainer.Add(dialogueChoice);
            }
        }
        
        public bool AreDialogueChoicesHidden(float opacityThreshold = 0f)
        {
            if (_choicesContainer.ClassListContains(ClassName_Visible)) return false;

            var choiceBoxes = _choicesContainer.Query(className: "dialogue-choice-box").ToList();

            if (choiceBoxes.Count <= 0) return true;

            return choiceBoxes.Sum(element => element.resolvedStyle.opacity) / choiceBoxes.Count <= opacityThreshold + Mathf.Epsilon;
        }
        
        public IEnumerator HideDialogueChoicesWithDelay(float delay = 0f)
        {
            _choicesContainer.RemoveFromClassList(ClassName_Visible);

            var i = 0;
            
            foreach (var element in _choicesContainer.Query(className: "dialogue-choice-box").ToList())
            {
                if (i > 0 && delay > Mathf.Epsilon)
                {
                    yield return new WaitForSeconds(delay);
                }

                element.RemoveFromClassList(ClassName_Visible);
                element.SetEnabled(false);
                i++;
            }
        }
        
        public IEnumerator ShowDialogueChoicesWithDelay(float delay = 0f)
        {
            _choicesContainer.AddToClassList(ClassName_Visible);
            
            var choiceBoxes = _choicesContainer.Query(className: "dialogue-choice-box").ToList();

            if (choiceBoxes.Count <= 0)
            {
                _continueButton.Focus();
                yield break;
            }
            
            var i = 0;
            
            foreach (var element in choiceBoxes)
            {
                element.SetEnabled(true);

                if (i == 0)
                {
                    element.Focus();
                } else if (delay > Mathf.Epsilon)
                {
                    yield return new WaitForSeconds(delay);
                }

                element.AddToClassList(ClassName_Visible);
                i++;
            }
        }
        
        private static bool IsElementHidden(VisualElement element, float opacityThreshold)
        {
            return !element.ClassListContains(ClassName_Visible) && element.resolvedStyle.opacity <= opacityThreshold + Mathf.Epsilon;
        }

        private static bool IsElementVisible(VisualElement element, float opacityThreshold)
        {
            // For some reason, `resolvedStyle.opacity` returns `1` when the UI is first created
            // even though the element doesn't have the `visible` class, so always check for the class first.
            return element.ClassListContains(ClassName_Visible) && element.resolvedStyle.opacity >= opacityThreshold + Mathf.Epsilon;
        }
        
        private void OnEnable()
        {
            var uiDoc = GetComponent<UIDocument>();

            _choicesContainer = uiDoc.rootVisualElement.Q("dialogue-choices-container");
            _dialogueContainer = uiDoc.rootVisualElement.Q("dialogue-container");
        }
        
        private void ContinueDialogue(KeyDownEvent evt)
        {
            if (evt.keyCode != KeyCode.Space && evt.keyCode != KeyCode.Return) return;
            
            _dialogueController.ContinueDialogue();
        }

        private void RefocusContinueButton(FocusOutEvent evt)
        {
            // If the continue button has been focused by the game,
            // don't let the player un-focus it
            // unless focus is being transferred to a dialogue choice button
            // (otherwise, the player will be stuck in the dialogue).
            if (evt.relatedTarget is VisualElement relatedTarget && relatedTarget.ClassListContains("dialogue-choice-box"))
            {
                return;
            }
            
            StartCoroutine(FocusContinueButtonAfterDelay());
        }

        private IEnumerator FocusContinueButtonAfterDelay()
        {
            yield return new WaitForSeconds(0.01f);
            
            _continueButton.Focus();
        }
        
        private void SelectDialogueChoice(ClickEvent evt, Choice choice)
        {
            _dialogueController.SelectDialogueChoice(choice);
        }
        
        private void SelectDialogueChoice(KeyDownEvent evt, Choice choice)
        {
            if (evt.keyCode != KeyCode.Space && evt.keyCode != KeyCode.Return) return;
            
            _dialogueController.SelectDialogueChoice(choice);
        }
    }
}
