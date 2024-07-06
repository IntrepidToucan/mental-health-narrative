using Cinemachine;
using UnityEngine;
using Utilities;

namespace Cameras
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(CinemachineBrain))]
    public class MainCamera : Singleton<MainCamera>
    {
        protected override void Awake()
        {
            PersistAcrossScenes = true;
            
            base.Awake();
        }

        protected override void InitializeSingleton()
        {
            base.InitializeSingleton();
            
            GetComponent<Camera>().orthographic = true;
        }
    }
}
