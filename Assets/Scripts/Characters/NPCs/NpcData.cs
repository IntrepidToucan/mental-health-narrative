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
        
        [Header("Mappings")]
        [SerializeField] private List<Demeanor> demeanors;
        [SerializeField] private List<Sprite> portraits;

        [NonSerialized] private Sprite _defaultPortrait;
        
        [field: NonSerialized] public Dictionary<Demeanor, Sprite> PortraitMap { get; private set; }
        
        public Sprite DefaultPortrait => _defaultPortrait;
        
        private void Awake()
        {
            PortraitMap = new Dictionary<Demeanor, Sprite>();
            for (var i = 0; i < demeanors.Count; i++) PortraitMap.Add(demeanors[i], portraits[i]);
            
            PortraitMap.TryGetValue(Demeanor.Neutral, out _defaultPortrait);
        }
    }
}
