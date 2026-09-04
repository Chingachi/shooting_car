using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class MainCamera : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _offset;
        [SerializeField]
        private float _smoothTime = 0.5f;

        [Inject]
        private Car _car;

        private Vector3 _velocity = Vector3.zero;

        private void LateUpdate()
        {
            if(_car == null)
            {
                return;
            }

            Vector3 finalPosition = _car.transform.position + _offset;
            transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref _velocity, _smoothTime);
        }
    }
}