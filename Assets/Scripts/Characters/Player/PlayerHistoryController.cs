using System;
using System.Collections.Generic;
using Characters.NPCs;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerHistoryController : MonoBehaviour
    {
        public enum HistoryTag
        {
            None,
            EncounteredErol,
            EncounteredMandy,
            EncounteredJanus
        }
        
        private Dictionary<HistoryTag, int> _historyMap;

        public static bool IsDefaultHistoryTag(HistoryTag historyTag) => historyTag == HistoryTag.None;
        public static string GetEncounteredNpcHistoryTagString(NpcData npcData) =>
            $"Encountered{Enum.GetName(typeof(Npc.NpcId), npcData.NpcId)}";

        public static HistoryTag TryParseHistoryTag(string tagString)
        {
            if (!Enum.TryParse(tagString, out HistoryTag result))
            {
                Debug.LogError($"Invalid history tag string {tagString}");
            }

            return result;
        }

        public void AddHistory(HistoryTag historyTag, int delta = 1)
        {
            if (IsDefaultHistoryTag(historyTag))
            {
                Debug.LogError($"History tag {historyTag} should never be updated");
                return;
            }
            
            if (delta < 1) Debug.LogError($"Unexpected delta for AddHistory: {delta}");

            if (!_historyMap.TryAdd(historyTag, delta))
            {
                _historyMap[historyTag] += delta;
            }
        }

        public int GetHistoryCount(HistoryTag historyTag)
        {
            if (_historyMap.TryGetValue(historyTag, out var result)) return result;
            
            Debug.LogError($"Tag not found in history map: {historyTag}");
            return 0;
        }
        
        public bool HasHistory(HistoryTag historyTag) => GetHistoryCount(historyTag) > 0;

        private void Awake()
        {
            _historyMap = new Dictionary<HistoryTag, int>();
            
            foreach (var historyTag in (HistoryTag[])Enum.GetValues(typeof(HistoryTag)))
            {
                _historyMap.Add(historyTag, 0);
            }
        }
    }
}
