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
        [SerializeField] private VisualTreeAsset patientPageUxml;
        [SerializeField] private VisualTreeAsset itemPageUxml;
        
        private const string ActiveClass = "active";
        private const string FocusableClass = "focusable";
        private const string SuccessClass = "success";
        
        private const string GoalAchievedText = "Blood sample acquired";
        private const string GoalNotAchievedText = "Blood sample needed";
        private const string UnknownNpcLabel = "???";

        private PauseMenu _pauseMenu;
        
        private VisualElement _rootContainer;
        private VisualElement _patientImageContainer;
        private VisualElement _itemsContainer;
        private VisualElement _patientRecordContentContainer;
        
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
        }
        
        private void DisplayLogBookEntries()
        {
            _pauseMenu.LogBookTab.Focus();
            _pauseMenu.HideBackButton();
            _rootContainer.Clear();
            
            foreach (var npcData in SceneManager.Instance.NpcDataInstances)
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
            _patientImageContainer.RegisterCallback<ClickEvent, NpcData>(SelectPatient, npcData);
            _patientImageContainer.RegisterCallback<KeyDownEvent, NpcData>(SelectPatient, npcData);

            _itemsContainer = patientRecord.Q("patient-items-container");
            _patientRecordContentContainer = patientRecord.Q("patient-record-content-container");

            foreach (var item in npcData.items)
            {
                var itemButton = patientRecordItemButtonUxml.Instantiate();

                if (Inventory.Instance.HasItem(item.itemId))
                {
                    itemButton.Q("item-button-image").style.backgroundImage = new StyleBackground(item.itemIcon);
                    itemButton.Q("patient-record-item-button").AddToClassList(FocusableClass);
                    
                    itemButton.Q("patient-record-item-button")
                        .RegisterCallback<ClickEvent, Item>(SelectItem, item);
                    itemButton.Q("patient-record-item-button")
                        .RegisterCallback<KeyDownEvent, Item>(SelectItem, item);
                }

                _itemsContainer.Add(itemButton);
            }

            _rootContainer.Add(patientRecord);
            _patientImageContainer.Focus();
            PopulatePatientPage(npcData);
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

        private void PopulatePatientPage(NpcData npcData)
        {
            var page = patientPageUxml.Instantiate();
            var contentContainer = page.Q("patient-page-text-container");
            
            foreach (var historyTag in Player.Instance.HistoryController.GetAcquiredHistoryTags())
            {
                if (!npcData.LogBookEntryMap.TryGetValue(historyTag, out var entry)) continue;
                
                var text = new Label(entry);
                text.AddToClassList("patient-page-text");
                contentContainer.Add(text);
            }
            
            _patientRecordContentContainer.Clear();
            _patientRecordContentContainer.Add(page);
        }
        
        private void PopulateItemPage(Item item)
        {
            var page = itemPageUxml.Instantiate();
            page.Q<Label>("item-page-title").text = item.itemName;
            page.Q("item-page-image").style.backgroundImage = new StyleBackground(item.itemIcon);
            
            var contentContainer = page.Q("item-page-text-container");

            foreach (var str in item.descriptionLines)
            {
                var text = new Label(str);
                text.AddToClassList("item-page-text");
                contentContainer.Add(text);
            }
            
            _patientRecordContentContainer.Clear();
            _patientRecordContentContainer.Add(page);
        }
        
        private void SelectPatient(ClickEvent evt, NpcData npcData)
        {
            SelectElement(evt.target);
            PopulatePatientPage(npcData);
        }
        
        private void SelectPatient(KeyDownEvent evt, NpcData npcData)
        {
            if (!UiManager.IsSubmitKeyDown(evt)) return;
            
            SelectElement(evt.target);
            PopulatePatientPage(npcData);
        }

        private void SelectItem(ClickEvent evt, Item item)
        {
            SelectElement(evt.target);
            PopulateItemPage(item);
        }
        
        private void SelectItem(KeyDownEvent evt, Item item)
        {
            if (!UiManager.IsSubmitKeyDown(evt)) return;
            
            SelectElement(evt.target);
            PopulateItemPage(item);
        }
    }
}
