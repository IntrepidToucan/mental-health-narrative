using UnityEngine;

namespace Characters.Player
{
    public class PlayerStatsController : MonoBehaviour
    {
        [Header("Primary Attributes")]
        [SerializeField, Range(0, 100)] private int wellness = 50;
        
        [Header("Relationship Attributes")]
        [SerializeField, Range(0, 100)] private int affinityErol = 50;

        public int Wellness => wellness;
    }
}
