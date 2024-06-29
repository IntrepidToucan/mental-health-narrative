using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters.Player;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Dialogue
{
    public class DialogueOverlay : MonoBehaviour
    {
        [Header("UXML")]
        [SerializeField] private VisualTreeAsset dialogueBoxUxml;
        [SerializeField] private VisualTreeAsset dialogueChoiceUxml;

        private const string VisibilityClass = "visible";
        
        private Player _player;
        
        private VisualElement _choicesContainer;
        private VisualElement _dialogueContainer;

        private Button _advanceButton;
        private VisualElement _dialogueImage;
        private Label _dialogueText;
        private VisualElement _dialogueTextContainer;
        
        public bool IsDialogueImageHidden(float opacityThreshold = 0f) => IsElementHidden(_dialogueImage, opacityThreshold);
        public bool IsDialogueImageVisible(float opacityThreshold = 1f) => IsElementVisible(_dialogueImage, opacityThreshold);
        public void HideDialogueImage() => _dialogueImage.RemoveFromClassList(VisibilityClass);
        public void ShowDialogueImage() => _dialogueImage.AddToClassList(VisibilityClass);
        
        public bool IsDialogueTextContainerHidden(float opacityThreshold = 0f) => IsElementHidden(_dialogueTextContainer, opacityThreshold);
        public bool IsDialogueTextContainerVisible(float opacityThreshold = 1f) => IsElementVisible(_dialogueTextContainer, opacityThreshold);
        public void HideDialogueTextContainer() => _dialogueTextContainer.RemoveFromClassList(VisibilityClass);
        public void ShowDialogueTextContainer() => _dialogueTextContainer.AddToClassList(VisibilityClass);
        
        public bool IsDialogueTextHidden(float opacityThreshold = 0f) => IsElementHidden(_dialogueText, opacityThreshold);
        public bool IsDialogueTextVisible(float opacityThreshold = 1f) => IsElementVisible(_dialogueText, opacityThreshold);
        public void HideDialogueText() =>_dialogueText.RemoveFromClassList(VisibilityClass);
        public void ShowDialogueText() => _dialogueText.AddToClassList(VisibilityClass);
        
        public void Init(Player player)
        {
            _player = player;
            
            var uiDoc = GetComponent<UIDocument>();
            uiDoc.rootVisualElement.RegisterCallback<ClickEvent>(HandleOverlayClick);
            uiDoc.rootVisualElement.RegisterCallback<KeyDownEvent>(HandleOverlayKeyDown);
            
            _advanceButton = uiDoc.rootVisualElement.Q<Button>("advance-dialogue-button");
            _advanceButton.RegisterCallback<KeyDownEvent>(HandleAdvanceDialogueButtonKeyDown);
            
            _dialogueContainer.Clear();
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
            _choicesContainer.Clear();

            if (choices.Count <= 0)
            {
                _advanceButton.Focus();
                return;
            }

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
            if (_choicesContainer.ClassListContains(VisibilityClass)) return false;

            var choiceBoxes = _choicesContainer.Query(className: "dialogue-choice-box").ToList();

            if (choiceBoxes.Count <= 0) return true;

            return choiceBoxes.Sum(element => element.resolvedStyle.opacity) / choiceBoxes.Count <= opacityThreshold + Mathf.Epsilon;
        }
        
        public bool AreDialogueChoicesVisible(float opacityThreshold = 0f)
        {
            if (!_choicesContainer.ClassListContains(VisibilityClass)) return false;

            var choiceBoxes = _choicesContainer.Query(className: "dialogue-choice-box").ToList();

            if (choiceBoxes.Count <= 0) return false;

            return choiceBoxes.Sum(element => element.resolvedStyle.opacity) / choiceBoxes.Count >= opacityThreshold + Mathf.Epsilon;
        }
        
        public IEnumerator HideDialogueChoicesWithDelay(float delay = 0f)
        {
            _choicesContainer.RemoveFromClassList(VisibilityClass);

            var i = 0;
            
            foreach (var element in _choicesContainer.Query(className: "dialogue-choice-box").ToList())
            {
                if (i > 0 && delay > Mathf.Epsilon)
                {
                    yield return new WaitForSeconds(delay);
                }

                element.RemoveFromClassList(VisibilityClass);
                element.SetEnabled(false);
                i++;
            }
        }
        
        public IEnumerator ShowDialogueChoicesWithDelay(float delay = 0f)
        {
            _choicesContainer.AddToClassList(VisibilityClass);
            
            var i = 0;
            
            foreach (var element in _choicesContainer.Query(className: "dialogue-choice-box").ToList())
            {
                element.SetEnabled(true);

                if (i == 0)
                {
                    element.Focus();
                } else if (delay > Mathf.Epsilon)
                {
                    yield return new WaitForSeconds(delay);
                }

                element.AddToClassList(VisibilityClass);
                i++;
            }
        }
        
        private static bool IsContinueKeyPress(KeyDownEvent evt)
        {
            return evt.keyCode is KeyCode.Space or KeyCode.Return;
        }
        
        private static bool IsElementHidden(VisualElement element, float opacityThreshold)
        {
            return !element.ClassListContains(VisibilityClass) && element.resolvedStyle.opacity <= opacityThreshold + Mathf.Epsilon;
        }

        private static bool IsElementVisible(VisualElement element, float opacityThreshold)
        {
            // For some reason, `resolvedStyle.opacity` returns `1` when the UI is first created
            // even though the element doesn't have the `visible` class, so always check for the class first.
            return element.ClassListContains(VisibilityClass) && element.resolvedStyle.opacity >= opacityThreshold + Mathf.Epsilon;
        }
        
        private void OnEnable()
        {
            var uiDoc = GetComponent<UIDocument>();
            _choicesContainer = uiDoc.rootVisualElement.Q("dialogue-choices-container");
            _dialogueContainer = uiDoc.rootVisualElement.Q("dialogue-container");
        }
        
        private void HandleOverlayClick(ClickEvent evt)
        {
            if (evt.target is Button && evt.target != _advanceButton) return;
            
            _player.DialogueController.TryAdvanceDialogue();
        }
        
        private void HandleOverlayKeyDown(KeyDownEvent evt)
        {
            if (!IsContinueKeyPress(evt) || evt.target is Button) return;
            
            _player.DialogueController.TryAdvanceDialogue();
        }

        private void HandleAdvanceDialogueButtonKeyDown(KeyDownEvent evt)
        {
            if (!IsContinueKeyPress(evt)) return;
            
            _player.DialogueController.TryAdvanceDialogue();
        }
        
        private void SelectDialogueChoice(ClickEvent evt, Choice choice)
        {
            _player.DialogueController.TrySelectDialogueChoice(choice);
        }
        
        private void SelectDialogueChoice(KeyDownEvent evt, Choice choice)
        {
            if (!IsContinueKeyPress(evt)) return;
            
            _player.DialogueController.TrySelectDialogueChoice(choice);
        }
    }
}
