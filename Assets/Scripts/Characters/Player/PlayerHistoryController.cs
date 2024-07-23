using System;
using System.Collections.Generic;
using System.Linq;
using Characters.NPCs;
using Managers;
using UnityEngine;
using Utilities;

namespace Characters.Player
{
    public class PlayerHistoryController : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private List<HistoryTag> historyTags_Countable;
        
        private Dictionary<HistoryTag, int> _historyMap;

        public static bool IsDefaultHistoryTag(HistoryTag historyTag) => historyTag == HistoryTag.None;
        public static string GetEncounteredNpcHistoryTagString(NpcData npcData) =>
            $"Encountered{Enum.GetName(typeof(NpcId), npcData.NpcId)}";

        public static HistoryTag TryParseHistoryTag(string tagString)
        {
            if (!Enum.TryParse(tagString, out HistoryTag result))
            {
                Debug.LogError($"Invalid history tag string {tagString}");
            }

            return result;
        }
        
        public List<HistoryTag> GetAcquiredHistoryTags() =>
            _historyMap.Keys.Where(key => _historyMap[key] > 0).ToList();

        public void AddHistory(HistoryTag historyTag, int delta = 1)
        {
            if (IsDefaultHistoryTag(historyTag))
            {
                Debug.LogError($"History tag {historyTag} should never be updated");
                return;
            }

            var hasKey = _historyMap.TryGetValue(historyTag, out var oldCount);
            var newCount = oldCount + delta;

            if (!historyTags_Countable.Contains(historyTag) && newCount > 1) return;

            if (hasKey)
            {
                _historyMap[historyTag] = newCount;
            }
            else
            {
                _historyMap.Add(historyTag, newCount);
            }

            if (SceneManager.Instance.NpcDataInstances.Any(npcData => npcData.LogBookEntryMap.ContainsKey(historyTag)))
            {
                EventManager.TriggerNotification(UiManager.Instance.NotificationData_LogBookUpdated);
            }
        }

        public int GetHistoryCount(HistoryTag historyTag)
        {
            if (_historyMap.TryGetValue(historyTag, out var result)) return result;
            
            Debug.LogError($"Tag not found in history map: {historyTag}");
            return 0;
        }
        
        private void Awake()
        {
            _historyMap = new Dictionary<HistoryTag, int>();
            
            foreach (var historyTag in (HistoryTag[])Enum.GetValues(typeof(HistoryTag)))
            {
                _historyMap.Add(historyTag, 0);
            }
        }
        
        public bool HasHistory(HistoryTag historyTag) => GetHistoryCount(historyTag) > 0;
    }
}
