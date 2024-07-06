using System.Collections;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class SceneManager : Singleton<SceneManager>
    {
        [Header("UI")]
        [SerializeField] private GameObject sceneFaderPrefab;

        private SceneFader _sceneFader;
        
        protected override void Awake()
        {
            persistAcrossScenes = true;
            
            base.Awake();

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
