using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Utilities;
using TextAsset = UnityEngine.TextAsset;

namespace Characters.NPCs
{
    [CreateAssetMenu(fileName = "NpcData", menuName = "ScriptableObjects/NpcData")]
    public class NpcData : ScriptableObject
    {
        [field: SerializeField] public NpcId NpcId { get; private set; }
        [field: SerializeField] public string CharacterName { get; private set; }
        [field: SerializeField] public TextAsset InkAsset { get; private set; }
        [field: SerializeField, Tooltip("The compiled Ink JSON file!")] public FontAsset FontAsset { get; private set; }
        [field: SerializeField] public List<Item> items { get; private set;  }
        
        [Header("Mappings/Demeanors")]
        [SerializeField] private List<Demeanor> demeanors;
        [SerializeField] private List<Sprite> portraits;
        
        [Header("Mappings/Log Book Entries")]
        [SerializeField] private List<HistoryTag> logBookHistoryTags;
        [SerializeField] private List<string> logBookEntries;

        [NonSerialized] private Sprite _defaultPortrait;
        
        [field: NonSerialized] public Dictionary<HistoryTag, string> LogBookEntryMap { get; private set; }
        [field: NonSerialized] public Dictionary<Demeanor, Sprite> PortraitMap { get; private set; }
        
        public Sprite DefaultPortrait => _defaultPortrait;
        
        private void Awake()
        {
            LogBookEntryMap = new Dictionary<HistoryTag, string>();
            for (var i = 0; i < logBookHistoryTags.Count; i++)
            {
                LogBookEntryMap.Add(logBookHistoryTags[i], logBookEntries[i]);
            }
            
            PortraitMap = new Dictionary<Demeanor, Sprite>();
            for (var i = 0; i < demeanors.Count; i++) PortraitMap.Add(demeanors[i], portraits[i]);
            
            PortraitMap.TryGetValue(Demeanor.Neutral, out _defaultPortrait);
        }
    }
}
