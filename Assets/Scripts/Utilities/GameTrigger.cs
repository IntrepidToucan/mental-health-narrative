using System.Collections.Generic;
using System.Linq;
using Characters.Player;
using Managers;
using UnityEngine;

namespace Utilities
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class GameTrigger : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField] private bool debug;
        
        [Header("Params - Activation")]
        [SerializeField] private int maxNumTimesActivated = -1;
        [SerializeField] private List<string> activatorTags = new(){ "Player" };
        
        [Header("Params - Data")]
        [SerializeField] private List<HistoryTag> historyTags;
        [SerializeField] private TutorialData tutorialData;

        private uint _numTimesActivated;

        private void Awake()
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!activatorTags.Contains(other.gameObject.tag)) return;
            if (maxNumTimesActivated > -1 && _numTimesActivated >= maxNumTimesActivated) return;

            _numTimesActivated++;
            
            if (debug)
            {
                Debug.Log($"Trigger activated by {other.gameObject.name} ({_numTimesActivated} times total)");
            }
            
            foreach (var historyTag in historyTags.Where(historyTag =>
                         !PlayerHistoryController.IsDefaultHistoryTag(historyTag)))
            {
                Player.Instance.HistoryController.AddHistory(historyTag);
            }

            if (tutorialData != null) EventManager.TriggerTutorial(tutorialData);
        }
    }
}
