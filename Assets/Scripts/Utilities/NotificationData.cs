using UnityEngine;

namespace Utilities
{
    [CreateAssetMenu(fileName = "NotificationData", menuName = "ScriptableObjects/NotificationData")]
    public class NotificationData : ScriptableObject
    {
        public string text;
        public float ttl = 3f;
    }
}
