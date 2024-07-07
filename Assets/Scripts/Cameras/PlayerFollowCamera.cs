using Characters.Player;
using Cinemachine;
using UnityEngine;
using Utilities;

namespace Cameras
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class PlayerFollowCamera : PersistedSingleton<PlayerFollowCamera>
    {
        public CinemachineVirtualCamera CineVirtualCamera { get; private set; }
        
        protected override void InitializeSingleton()
        {
            base.InitializeSingleton();
            
            CineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
            CineVirtualCamera.m_Lens.OrthographicSize = 5.4f;
            CineVirtualCamera.m_Lens.NearClipPlane = 0f;
            CineVirtualCamera.m_Lens.FarClipPlane = 50f;
        }

        private void Start()
        {
            CineVirtualCamera.Follow = Player.Instance.transform;
        }
    }
}
