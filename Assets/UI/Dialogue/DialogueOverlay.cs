using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters.NPCs;
using Characters.Player;
using Ink.Runtime;
using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Dialogue
{
    public class DialogueOverlay : MonoBehaviour
    {
        [Header("UXML")]
        [SerializeField] private VisualTreeAsset dialogueBoxUxml;
        [SerializeField] private VisualTreeAsset dialogueChoiceUxml;

        private const string VisibleClass = "visible";

        private Npc _npc;
        private UIDocument _uiDoc;
        
        private VisualElement _dialogueChoicesContainer;
        private VisualElement _dialogueContainer;

        private Button _advanceDialogueButton;
        private VisualElement _dialogueImage;
        private Label _dialogueText;
        private VisualElement _dialogueTextContainer;
        
        public bool IsDialogueImageHidden(float opacityThreshold = 0f) =>
            IsElementHidden(_dialogueImage, opacityThreshold);
        public bool IsDialogueImageVisible(float opacityThreshold = 1f) =>
            IsElementVisible(_dialogueImage, opacityThreshold);
        public void HideDialogueImage() => _dialogueImage.RemoveFromClassList(VisibleClass);
        public void ShowDialogueImage() => _dialogueImage.AddToClassList(VisibleClass);
        
        public bool IsDialogueTextContainerHidden(float opacityThreshold = 0f) =>
            IsElementHidden(_dialogueTextContainer, opacityThreshold);
        public bool IsDialogueTextContainerVisible(float opacityThreshold = 1f) =>
            IsElementVisible(_dialogueTextContainer, opacityThreshold);
        public void HideDialogueTextContainer() => _dialogueTextContainer.RemoveFromClassList(VisibleClass);
        public void ShowDialogueTextContainer() => _dialogueTextContainer.AddToClassList(VisibleClass);
        
        public bool IsDialogueTextHidden(float opacityThreshold = 0f) =>
            IsElementHidden(_dialogueText, opacityThreshold);
        public bool IsDialogueTextVisible(float opacityThreshold = 1f) =>
            IsElementVisible(_dialogueText, opacityThreshold);
        public void HideDialogueText() =>_dialogueText.RemoveFromClassList(VisibleClass);
        public void ShowDialogueText() => _dialogueText.AddToClassList(VisibleClass);
        
        public void SetNpc(Npc npc)
        {
            _npc = npc;

            TrySetDialogueFont();
        }

        public void SetDialogue(string text)
        {
            _dialogueText.text = text.Trim();
        }

        public void SetDialogueChoices(List<Choice> choices)
        {
            _dialogueChoicesContainer.Clear();

            if (choices.Count <= 0)
            {
                _advanceDialogueButton.Focus();
                return;
            }

            foreach (var choice in choices)
            {
                var dialogueChoice = dialogueChoiceUxml.Instantiate();
                
                var button = dialogueChoice.Q<Button>("dialogue-choice-box");
                button.text = choice.text;
                button.RegisterCallback<MouseOverEvent>(FocusDialogueChoice);
                button.RegisterCallback<ClickEvent, Choice>(SelectDialogueChoice, choice);
                button.RegisterCallback<KeyDownEvent, Choice>(SelectDialogueChoice, choice);
                button.SetEnabled(false);

                _dialogueChoicesContainer.Add(dialogueChoice);
            }
        }
        
        public bool AreDialogueChoicesHidden(float opacityThreshold = 0f)
        {
            if (_dialogueChoicesContainer.ClassListContains(VisibleClass)) return false;

            var choiceBoxes = _dialogueChoicesContainer.Query(className: "dialogue-choice-box").ToList();

            if (choiceBoxes.Count <= 0) return true;

            return choiceBoxes.Sum(element => element.resolvedStyle.opacity) / choiceBoxes.Count <=
                   opacityThreshold + Mathf.Epsilon;
        }
        
        public bool AreDialogueChoicesVisible(float opacityThreshold = 1f)
        {
            if (!_dialogueChoicesContainer.ClassListContains(VisibleClass)) return false;

            var choiceBoxes = _dialogueChoicesContainer.Query(className: "dialogue-choice-box").ToList();

            if (choiceBoxes.Count <= 0) return false;

            return choiceBoxes.Sum(element => element.resolvedStyle.opacity) / choiceBoxes.Count >=
                   opacityThreshold + Mathf.Epsilon;
        }
        
        public IEnumerator HideDialogueChoicesWithDelay(float delay = 0f)
        {
            _dialogueChoicesContainer.RemoveFromClassList(VisibleClass);

            var i = 0;
            
            foreach (var element in _dialogueChoicesContainer.Query(className: "dialogue-choice-box").ToList())
            {
                if (i > 0 && delay > Mathf.Epsilon)
                {
                    yield return new WaitForSeconds(delay);
                }

                element.RemoveFromClassList(VisibleClass);
                i++;
            }
        }
        
        public IEnumerator ShowDialogueChoicesWithDelay(float delay = 0f)
        {
            _dialogueChoicesContainer.AddToClassList(VisibleClass);
            
            var i = 0;
            
            foreach (var element in _dialogueChoicesContainer.Query(className: "dialogue-choice-box").ToList())
            {
                element.SetEnabled(true);

                if (i == 0)
                {
                    element.Focus();
                } else if (delay > Mathf.Epsilon)
                {
                    yield return new WaitForSeconds(delay);
                }

                element.AddToClassList(VisibleClass);
                i++;
            }
        }
        
        private static bool IsElementHidden(VisualElement element, float opacityThreshold)
        {
            return !element.ClassListContains(VisibleClass) &&
                   element.resolvedStyle.opacity <= opacityThreshold + Mathf.Epsilon;
        }

        private static bool IsElementVisible(VisualElement element, float opacityThreshold)
        {
            // For some reason, `resolvedStyle.opacity` returns `1` when the UI is first created
            // even though the element doesn't have the `visible` class, so always check for the class first.
            return element.ClassListContains(VisibleClass) &&
                   element.resolvedStyle.opacity >= opacityThreshold + Mathf.Epsilon;
        }

        private void Awake()
        {
            _uiDoc = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            _uiDoc.rootVisualElement.RegisterCallback<ClickEvent>(HandleOverlayClick);
            _uiDoc.rootVisualElement.RegisterCallback<KeyDownEvent>(HandleOverlayKeyDown);

            _dialogueContainer = _uiDoc.rootVisualElement.Q("dialogue-container");
            _dialogueChoicesContainer = _uiDoc.rootVisualElement.Q("dialogue-choices-container");
            
            _advanceDialogueButton = _uiDoc.rootVisualElement.Q<Button>("advance-dialogue-button");
            _advanceDialogueButton.RegisterCallback<KeyDownEvent>(HandleAdvanceDialogueButtonKeyDown);

            _dialogueContainer.Clear();
            _dialogueContainer.Add(dialogueBoxUxml.Instantiate());
            
            _dialogueImage = _dialogueContainer.Q("dialogue-image");
            _dialogueTextContainer = _dialogueContainer.Q("dialogue-text-container");
            _dialogueText = _dialogueContainer.Q<Label>("dialogue-text");

            TrySetDialogueFont();
        }

        private void TrySetDialogueFont()
        {
            if (_npc == null || _dialogueText == null) return;
            
            _dialogueText.style.unityFontDefinition = new StyleFontDefinition(FontDefinition.FromSDFFont(_npc.Font));
        }
        
        private void HandleOverlayClick(ClickEvent evt)
        {
            if (evt.target is Button && evt.target != _advanceDialogueButton) return;
            
            Player.Instance.DialogueController.TryAdvanceDialogue();
        }
        
        private static void HandleOverlayKeyDown(KeyDownEvent evt)
        {
            if (!UiManager.IsSubmitKeyDown(evt) || evt.target is Button) return;
            
            Player.Instance.DialogueController.TryAdvanceDialogue();
        }

        private static void HandleAdvanceDialogueButtonKeyDown(KeyDownEvent evt)
        {
            if (!UiManager.IsSubmitKeyDown(evt)) return;
            
            Player.Instance.DialogueController.TryAdvanceDialogue();
        }

        private static void FocusDialogueChoice(MouseOverEvent evt)
        {
            if (evt.target is Button button) button.Focus();
        }
        
        private static void SelectDialogueChoice(ClickEvent evt, Choice choice)
        {
            Player.Instance.DialogueController.TrySelectDialogueChoice(choice);
        }
        
        private static void SelectDialogueChoice(KeyDownEvent evt, Choice choice)
        {
            if (!UiManager.IsSubmitKeyDown(evt)) return;
            
            Player.Instance.DialogueController.TrySelectDialogueChoice(choice);
        }
    }
}
