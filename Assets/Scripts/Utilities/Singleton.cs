using UnityEngine;

namespace Utilities
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        [SerializeField] protected bool persistAcrossScenes;
        
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
                
                if (persistAcrossScenes) DontDestroyOnLoad(gameObject);
            }
        }
    }
}
