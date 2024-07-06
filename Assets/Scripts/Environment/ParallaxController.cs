using Characters.Player;
using UnityEngine;

namespace Environment
{
    public class ParallaxController : MonoBehaviour
    {
        private Camera _camera;
        private Vector2 _startPosition;

        private void Start()
        {
            _camera = Camera.main;
            _startPosition = transform.position;
        }

        private void Update()
        {
            var travelDistance = (Vector2)_camera.transform.position - _startPosition;
            var distanceFromPlayer = transform.position.z - Player.Instance.transform.position.z;
            var clippingPlane = _camera.transform.position.z +
                                (distanceFromPlayer > Mathf.Epsilon ? _camera.farClipPlane : _camera.nearClipPlane);
            var parallaxFactor = Mathf.Abs(distanceFromPlayer) / clippingPlane;
            var newPosition = _startPosition + travelDistance * parallaxFactor;
                
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }
    }
}
