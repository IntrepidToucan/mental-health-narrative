using System.Collections;
using System.Linq;
using Cameras;
using Characters.Player;
using Environment;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Managers
{
    public class SceneManager : PersistedSingleton<SceneManager>
    {
        [Header("Params")]
        [SerializeField] private float loadSceneFadeInDelay = 0.1f;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject eventSystemPrefab;
        [SerializeField] private GameObject playerFollowCameraPrefab;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject sceneFaderPrefab;
        [SerializeField] private GameObject uiManagerPrefab;

        private SceneFader _sceneFader;
        private string _activeSceneConnectionId;

        public void SwitchToScene(string sceneName, ISceneConnectable sceneConnectable)
        {
            _activeSceneConnectionId = sceneConnectable.GetSceneConnectionId();
            
            Player.Instance.PlayerInput.currentActionMap.Disable();

            StartCoroutine(LoadSceneWithFadeOut(sceneName));
        }
        
        protected override void InitializeSingleton()
        {
            base.InitializeSingleton();

            Instantiate(eventSystemPrefab, gameObject.transform);
            Instantiate(uiManagerPrefab);

            var playerStart = GetPlayerStart();
            
            Instantiate(playerPrefab, playerStart == null ? Vector3.zero : playerStart.transform.position,
                playerStart == null ? Quaternion.identity : playerStart.transform.rotation);
            Instantiate(playerFollowCameraPrefab);

            _sceneFader = Instantiate(sceneFaderPrefab, gameObject.transform).GetComponent<SceneFader>();

            // We need a delay before updating the UI element classes,
            // or else the transition animation won't work.
            StartCoroutine(FadeInAfterDelay());
        }

        private void OnEnable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void OnDisable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= HandleSceneLoaded;
        }
        
        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (!string.IsNullOrEmpty(_activeSceneConnectionId))
            {
                var targetPosition = Vector3.zero;
                // TODO: There's probably a more efficient way to do this, like keeping track of all ISceneConnectables
                // (see https://stackoverflow.com/questions/49329764/get-all-components-with-a-specific-interface-in-unity).
                var sceneConnectables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                    .OfType<ISceneConnectable>()
                    .Where(item => item.GetSceneConnectionId() == _activeSceneConnectionId).ToArray();

                if (sceneConnectables.Length > 0)
                {
                    targetPosition = sceneConnectables[0].gameObject.transform.position;
                }
                else
                {
                    var playerStart = GetPlayerStart();

                    if (playerStart != null)
                    {
                        targetPosition = playerStart.transform.position;
                    }
                }

                var playerTransform = Player.Instance.gameObject.transform;
                
                // Warp the Cinemachine follow camera.
                PlayerFollowCamera.Instance.CineVirtualCamera.OnTargetObjectWarped(
                    playerTransform, targetPosition - playerTransform.position);
                playerTransform.position = targetPosition;
            }
            
            Player.Instance.PlayerInput.currentActionMap.Enable();
                
            StartCoroutine(FadeInAfterDelay(loadSceneFadeInDelay));
        }

        private static GameObject GetPlayerStart() => GameObject.Find("PlayerStart");

        private IEnumerator FadeInAfterDelay(float delay = 0.1f)
        {
            yield return new WaitForSeconds(delay);
            
            _sceneFader.FadeIn();
        }

        private IEnumerator LoadSceneWithFadeOut(string sceneName)
        {
            _sceneFader.FadeOut();

            yield return new WaitUntil(() => _sceneFader.IsFadedOut());

            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
