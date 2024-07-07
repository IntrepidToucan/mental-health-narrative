using Cinemachine;
using UnityEngine;
using Utilities;

namespace Cameras
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(CinemachineBrain))]
    public class MainCamera : PersistedSingleton<MainCamera>
    {
        protected override void InitializeSingleton()
        {
            base.InitializeSingleton();
            
            GetComponent<Camera>().orthographic = true;
        }
    }
}
