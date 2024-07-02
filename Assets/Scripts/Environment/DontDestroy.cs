using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Environment
{
    public class DontDestroy : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
        }

        private void Start()
        {
            var confiner = GetComponent<CinemachineConfiner2D>();
            
            if (confiner is not null)
            {
                SceneManager.sceneLoaded += (arg0, mode) =>
                {
                    var confinerCollider = GameObject.Find("Camera Confiner");
                    
                    confiner.m_BoundingShape2D = confinerCollider.GetComponent<PolygonCollider2D>();
                };
            }
        }
    }
}
