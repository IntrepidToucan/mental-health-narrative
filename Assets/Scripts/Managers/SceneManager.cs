using System.Collections;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class SceneManager : Singleton<SceneManager>
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject eventSystemPrefab;
        [SerializeField] private GameObject playerFollowCameraPrefab;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject sceneFaderPrefab;
        [SerializeField] private GameObject uiManagerPrefab;

        private SceneFader _sceneFader;

        public static void LoadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
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
            StartCoroutine(FadeInWithDelay());
        }

        private void Start()
        {
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += (arg0, scene) =>
            {
                _sceneFader.FadeOut();
            };
            
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += (arg0, mode) =>
            {
                _sceneFader.FadeIn();
            };
        }

        private IEnumerator FadeInWithDelay()
        {
            yield return new WaitForSeconds(0.1f);
            
            _sceneFader.FadeIn();
        }
    }
}
