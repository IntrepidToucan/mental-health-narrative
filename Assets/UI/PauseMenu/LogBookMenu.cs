using System.Collections.Generic;
using Characters.NPCs;
using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.PauseMenu
{
    public class LogBookMenu : MonoBehaviour
    {
        [Header("UXML")]
        [SerializeField] private VisualTreeAsset logBookMenuUxml;
        [SerializeField] private VisualTreeAsset logBookEntryUxml;
        [SerializeField] private VisualTreeAsset patientRecordUxml;

        [Header("Data")]
        [SerializeField] private List<NpcData> npcDataOriginals;

        private PauseMenu _pauseMenu;
        private List<NpcData> _npcDataInstances;
        private VisualElement _rootContainer;
        
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
            _rootContainer.Clear();
            
            foreach (var npcData in _npcDataInstances)
            {
                var logBookEntry = logBookEntryUxml.Instantiate();
                logBookEntry.Q("log-book-entry-image").style.backgroundImage =
                    new StyleBackground(npcData.DefaultPortrait);
                logBookEntry.Q<Label>("log-book-entry-label").text = npcData.CharacterName;

                logBookEntry.RegisterCallback<ClickEvent>(evt => DisplayPatientRecord(npcData));
                logBookEntry.RegisterCallback<KeyDownEvent>(evt =>
                {
                    if (UiManager.IsSubmitKeyDown(evt)) DisplayPatientRecord(npcData);
                });
                
                _rootContainer.Add(logBookEntry);
            }
        }

        private void DisplayPatientRecord(NpcData npcData)
        {
            _rootContainer.Clear();
            
            var patientRecord = patientRecordUxml.Instantiate();
            patientRecord.Q("patient-image").style.backgroundImage = new StyleBackground(npcData.DefaultPortrait);
            patientRecord.Q<Label>("patient-name").text = npcData.CharacterName;

            _rootContainer.Add(patientRecord);
        }
    }
}
