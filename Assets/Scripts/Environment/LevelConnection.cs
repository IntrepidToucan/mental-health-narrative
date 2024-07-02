using Characters.Player;
using Interaction;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Environment
{
    public class LevelConnection : MonoBehaviour, IInteractable
    {
        [SerializeField] private string targetSceneName;

        public IInteractable.InteractionData GetInteractionData(Player player)
        {
            return new IInteractable.InteractionData("Enter");
        }

        public void Interact(Player player)
        {
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
