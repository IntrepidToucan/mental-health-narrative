using UnityEngine;

namespace Utilities
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        protected static bool PersistAcrossScenes;
        
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
                
                if (PersistAcrossScenes) DontDestroyOnLoad(gameObject);
            }

            if (_isSingletonInitialized) return;

            InitializeSingleton();
            _isSingletonInitialized = true;
        }

        protected virtual void InitializeSingleton() {}
    }
}
