using System;
using Characters.Player;
using Interaction;
using Managers;
using UnityEngine;

namespace Environment
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class RoomDoor : MonoBehaviour, IInteractable
    {
        [Header("Params")]
        [SerializeField] private string targetSceneName;
        
        public IInteractable.InteractionData GetInteractionData(Player player)
        {
            return new IInteractable.InteractionData("Enter");
        }

        public void Interact(Player player)
        {
            SceneManager.LoadScene(targetSceneName);
        }

        private void Awake()
        {
            GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("NonPlayerObjects");
        }
    }
}
