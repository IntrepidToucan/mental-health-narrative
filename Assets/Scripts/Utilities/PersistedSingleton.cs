using UnityEngine;

namespace Utilities
{
    public class PersistedSingleton<T> : MonoBehaviour where T : PersistedSingleton<T>
    {
        private static bool _isSingletonInitialized;
        
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != this && Instance != null)
            {
                if (gameObject != null) Destroy(gameObject);
            }
            else
            {
                Instance = (T)this;
                
                DontDestroyOnLoad(gameObject);
            }

            if (_isSingletonInitialized) return;

            InitializeSingleton();
            _isSingletonInitialized = true;
        }

        protected virtual void InitializeSingleton() {}
    }
}
