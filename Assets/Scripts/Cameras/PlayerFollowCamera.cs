using Characters.Player;
using Cinemachine;
using UnityEngine;
using Utilities;

namespace Cameras
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    [RequireComponent(typeof(CinemachineConfiner2D))]
    public class PlayerFollowCamera : PersistedSingleton<PlayerFollowCamera>
    {
        public CinemachineConfiner2D CineConfiner { get; private set; }
        public CinemachineVirtualCamera CineVirtualCamera { get; private set; }

        public const float OrthoSizeNarrative = 4.2f;
        public const float OrthoSizePlatforming = 6.2f;

        [SerializeField]
        private float desiredYOffset = 2f; // Exposed to the editor

        private CinemachineFramingTransposer framingTransposer;

        protected override void InitializeSingleton()
        {
            base.InitializeSingleton();
            
            CineConfiner = GetComponent<CinemachineConfiner2D>();

            CineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
            CineVirtualCamera.m_Lens.OrthographicSize = OrthoSizeNarrative;
            CineVirtualCamera.m_Lens.NearClipPlane = 0f;
            CineVirtualCamera.m_Lens.FarClipPlane = 50f;
        }

        private void Start()
        {
            CineVirtualCamera.Follow = Player.Instance.transform;

            framingTransposer = CineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

            if (framingTransposer != null)
            {
                framingTransposer.m_TrackedObjectOffset.y = desiredYOffset; // Adjusting the Y offset
                Debug.Log("Adjusted Y Offset: " + framingTransposer.m_TrackedObjectOffset.y);
            }
            else
            {
                Debug.LogWarning("CinemachineFramingTransposer not found!");
            }
        }

        // Optionally, you can create a method to update this offset at runtime
        public void UpdateYOffset(float newYOffset)
        {
            if (framingTransposer != null)
            {
                framingTransposer.m_TrackedObjectOffset.y = newYOffset;
                Debug.Log("Updated Y Offset to: " + newYOffset);
            }
        }
    }
}
