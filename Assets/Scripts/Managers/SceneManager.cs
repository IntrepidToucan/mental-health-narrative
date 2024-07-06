using System.Collections;
using Characters.Player;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class SceneManager : Singleton<SceneManager>
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

        public void LoadScene(string sceneName)
        {
            Player.Instance.PlayerInput.currentActionMap.Disable();
            
            StartCoroutine(LoadSceneWithFadeOut(sceneName));
        }
        
        protected override void Awake()
        {
            PersistAcrossScenes = true;
            
            base.Awake();
        }

        protected override void InitializeSingleton()
        {
            base.InitializeSingleton();

            Instantiate(eventSystemPrefab, gameObject.transform);
            Instantiate(uiManagerPrefab);
            
            var playerStart = GameObject.Find("PlayerStart");
            Instantiate(playerPrefab, playerStart.transform.position, playerStart.transform.rotation);
            Instantiate(playerFollowCameraPrefab);

            _sceneFader = Instantiate(sceneFaderPrefab, gameObject.transform).GetComponent<SceneFader>();

            // We need a delay before updating the UI element classes,
            // or else the transition animation won't work.
            StartCoroutine(FadeInAfterDelay());
        }

        private void Start()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (arg0, mode) =>
            {
                Player.Instance.PlayerInput.currentActionMap.Enable();
                
                StartCoroutine(FadeInAfterDelay(loadSceneFadeInDelay));
            };
        }

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
