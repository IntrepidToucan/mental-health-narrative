using UnityEngine;

namespace Utilities
{
    [CreateAssetMenu(fileName = "TutorialData", menuName = "ScriptableObjects/TutorialData")]
    public class TutorialData : ScriptableObject
    {
        [field: SerializeField] public string Text { get; private set; }
    }
}
