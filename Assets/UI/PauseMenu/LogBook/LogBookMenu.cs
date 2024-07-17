using System;
using System.Collections.Generic;
using System.Linq;
using Characters.NPCs;
using Characters.Player;
using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.PauseMenu.LogBook
{
    public class LogBookMenu : MonoBehaviour
    {
        [Header("UXML")]
        [SerializeField] private VisualTreeAsset logBookMenuUxml;
        [SerializeField] private VisualTreeAsset logBookEntryUxml;
        [SerializeField] private VisualTreeAsset patientRecordUxml;
        [SerializeField] private VisualTreeAsset patientRecordItemButtonUxml;

        [Header("Data")]
        [SerializeField] private List<NpcData> npcDataOriginals;
        
        private const string ActiveClass = "active";
        private const string FocusableClass = "focusable";
        private const string SuccessClass = "success";
        
        private const string GoalAchievedText = "Blood sample acquired";
        private const string GoalNotAchievedText = "Blood sample needed";
        private const string UnknownNpcLabel = "???";

        private PauseMenu _pauseMenu;
        private List<NpcData> _npcDataInstances;
        
        private VisualElement _rootContainer;
        private VisualElement _patientImageContainer;
        private VisualElement _itemsContainer;
        
        public void HandleBackAction() => DisplayLogBookEntries();
        
        public void AddToContainer(VisualElement container)
        {
            var logBookMenu = logBookMenuUxml.Instantiate();
            logBookMenu.style.flexGrow = 1;
            container.Add(logBookMenu);
            
            _rootContainer = logBookMenu.Q("log-book-menu");
            DisplayLogBookEntries();
        }

        private void Awake()
        {
            _pauseMenu = GetComponent<PauseMenu>();
            
            _npcDataInstances = new List<NpcData>();
            foreach (var npcData in npcDataOriginals) _npcDataInstances.Add(Instantiate(npcData));
        }
        
        private void DisplayLogBookEntries()
        {
            _pauseMenu.LogBookTab.Focus();
            _pauseMenu.HideBackButton();
            _rootContainer.Clear();
            
            foreach (var npcData in _npcDataInstances)
            {
                var hasMetNpc = Player.Instance.HistoryController.HasHistory(
                    PlayerHistoryController.TryParseHistoryTag(
                        PlayerHistoryController.GetEncounteredNpcHistoryTagString(npcData)));
                var hasAchievedNpcGoal = Inventory.Instance.HasItem(
                    Inventory.TryParseItemId(
                        Inventory.GetNpcGoalItemIdString(npcData)));
                
                var logBookEntry = logBookEntryUxml.Instantiate();
                logBookEntry.Q("log-book-entry-image").style.backgroundImage =
                    new StyleBackground(npcData.DefaultPortrait);
                logBookEntry.Q<Label>("log-book-entry-label").text =
                    hasMetNpc ? npcData.CharacterName : UnknownNpcLabel;

                var goalText = logBookEntry.Q<Label>("log-book-entry-goal-text");
                goalText.text = hasAchievedNpcGoal ? GoalAchievedText : GoalNotAchievedText;
                
                if (hasAchievedNpcGoal) goalText.AddToClassList(SuccessClass);

                var logBookEntryRoot = logBookEntry.Q("log-book-entry");

                if (hasMetNpc)
                {
                    logBookEntryRoot.AddToClassList(FocusableClass);
                    
                    logBookEntry.RegisterCallback<ClickEvent>(evt => DisplayPatientRecord(npcData));
                    logBookEntry.RegisterCallback<KeyDownEvent>(evt =>
                    {
                        if (UiManager.IsSubmitKeyDown(evt)) DisplayPatientRecord(npcData);
                    });
                }
                else
                {
                    logBookEntryRoot.focusable = false;
                }

                _rootContainer.Add(logBookEntry);
            }
        }

        private void DisplayPatientRecord(NpcData npcData)
        {
            _pauseMenu.ShowBackButton();
            _rootContainer.Clear();

            var patientRecord = patientRecordUxml.Instantiate();
            patientRecord.style.flexGrow = 1;
            patientRecord.Q<Label>("patient-name").text = npcData.CharacterName;
            patientRecord.Q("patient-image").style.backgroundImage = new StyleBackground(npcData.DefaultPortrait);

            _patientImageContainer = patientRecord.Q("patient-image-container");
            _patientImageContainer.AddToClassList(ActiveClass);
            _patientImageContainer.RegisterCallback<ClickEvent>(SelectPatient);
            _patientImageContainer.RegisterCallback<KeyDownEvent>(SelectPatient);

            var npcIdString = Enum.GetName(typeof(Npc.NpcId), npcData.NpcId);
            var npcItems = string.IsNullOrEmpty(npcIdString) ?
                Array.Empty<Item>() :
                Inventory.Instance.items.Where(item =>
                {
                    var itemIdString = Enum.GetName(typeof(Inventory.ItemId), item.itemId);

                    return !string.IsNullOrEmpty(itemIdString) && itemIdString.StartsWith(npcIdString);
                }).ToArray();
            
            _itemsContainer = patientRecord.Q("patient-items-container");

            if (npcItems.Length > 0)
            {
                // Remove the default empty message element.
                _itemsContainer.Clear();
                
                foreach (var item in npcItems)
                {
                    var itemButton = patientRecordItemButtonUxml.Instantiate();
                    itemButton.Q("item-button-image").style.backgroundImage = new StyleBackground(item.itemIcon);
                    itemButton.Q("patient-record-item-button")
                        .RegisterCallback<ClickEvent, Item>(SelectItem, item);
                    itemButton.Q("patient-record-item-button")
                        .RegisterCallback<KeyDownEvent, Item>(SelectItem, item);
                    
                    _itemsContainer.Add(itemButton);
                }
            }

            _rootContainer.Add(patientRecord);
            _patientImageContainer.Focus();
        }
        
        private void ClearActiveClass()
        {
            _patientImageContainer.RemoveFromClassList(ActiveClass);
            _itemsContainer.Query(className: "item-button")
                .ForEach(element => element.RemoveFromClassList(ActiveClass));
        }

        private void SelectElement(IEventHandler target)
        {
            if (target is not VisualElement element) return;
            
            ClearActiveClass();
            element.AddToClassList(ActiveClass);
        }
        
        private void SelectPatient(ClickEvent evt)
        {
            SelectElement(evt.target);
        }
        
        private void SelectPatient(KeyDownEvent evt)
        {
            if (!UiManager.IsSubmitKeyDown(evt)) return;
            
            SelectElement(evt.target);
        }

        private void SelectItem(ClickEvent evt, Item item)
        {
            SelectElement(evt.target);
        }
        
        private void SelectItem(KeyDownEvent evt, Item item)
        {
            if (!UiManager.IsSubmitKeyDown(evt)) return;
            
            SelectElement(evt.target);
        }
    }
}
