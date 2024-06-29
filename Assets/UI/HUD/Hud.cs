using Characters.Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.HUD
{
    public class Hud : MonoBehaviour
    {
        private ProgressBar _progressBar;
        
        private Player _player;
        
        private void OnEnable()
        {
            var uiDoc = GetComponent<UIDocument>();

            _progressBar = uiDoc.rootVisualElement.Q<ProgressBar>("wellness-bar");

            _player = FindAnyObjectByType<Player>();
        }

        /**
         * According to the Unity docs (https://docs.unity3d.com/Manual/ExecutionOrder.html):
         *   "[Y]ou can’t rely on one object’s Awake being called before another object’s OnEnable.
         *   Any work that depends on Awake having been called
         *   for all objects in the scene should be done in Start."
         */
        private void Start()
        {
            // _progressBar.value = _player.StatsController.Wellness;
        }
    }
}
