using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using TextAsset = UnityEngine.TextAsset;

namespace Characters.NPCs
{
    [CreateAssetMenu(fileName = "NpcData", menuName = "ScriptableObjects/NpcData", order = 1)]
    public class NpcData : ScriptableObject
    {
        public enum Demeanor
        {
            Neutral
        }
        
        [field: SerializeField] public string CharacterName { get; private set; }
        [field: SerializeField] public TextAsset InkAsset { get; private set; }
        [field: SerializeField, Tooltip("The compiled Ink JSON file!")] public FontAsset FontAsset { get; private set; }
        
        [Header("Mappings")]
        [SerializeField] private List<Demeanor> demeanors;
        [SerializeField] private List<Sprite> portraits;

        [NonSerialized] private Sprite _defaultPortrait;
        public Sprite DefaultPortrait => _defaultPortrait;
        [field: NonSerialized] public Dictionary<Demeanor, Sprite> PortraitMap { get; private set; }
        
        private void Awake()
        {
            PortraitMap = new Dictionary<Demeanor, Sprite>();
            for (var i = 0; i < demeanors.Count; i++) PortraitMap.Add(demeanors[i], portraits[i]);
            
            PortraitMap.TryGetValue(Demeanor.Neutral, out _defaultPortrait);
        }
    }
}
