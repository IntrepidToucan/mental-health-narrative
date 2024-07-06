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
            persistAcrossScenes = true;
            
            base.Awake();

            GetComponent<Camera>().orthographic = true;
        }
    }
}
