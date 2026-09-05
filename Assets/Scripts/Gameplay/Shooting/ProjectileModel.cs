using System;
using UnityEngine;

namespace Gameplay.Shooting
{
    public class ProjectileModel : MonoBehaviour
    {
        public event Action OnBecomeInvisibleEvent;

        private void OnBecameInvisible()
        {
            OnBecomeInvisibleEvent?.Invoke();
        }
    }
}