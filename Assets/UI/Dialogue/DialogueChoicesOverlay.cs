using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Dialogue
{
    public class DialogueChoicesOverlay : MonoBehaviour
    {
        [Header("Assets")]
        [SerializeField] private VisualTreeAsset dialogueChoiceUxml;
        
        private VisualElement _choicesContainer;
        
        public void SetDialogueChoices(List<Choice> choices)
        {
            foreach (var choice in choices)
            {
                var dialogueChoice = dialogueChoiceUxml.Instantiate();
                
                var button = dialogueChoice.Q<Button>("root");
                button.text = choice.text;
                button.RegisterCallback<ClickEvent, string>(SelectDialogueChoice, choice.text);

                _choicesContainer.Add(dialogueChoice);
            }
        }
        
        private void OnEnable()
        {
            var uiDoc = GetComponent<UIDocument>();

            _choicesContainer = uiDoc.rootVisualElement.Q("choices-container");
        }

        private void SelectDialogueChoice(ClickEvent evt, string text)
        {
            Debug.Log(text);
        }
    }
}
