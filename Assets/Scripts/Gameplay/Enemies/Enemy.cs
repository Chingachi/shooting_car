using System;
using Pools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Enemies
{
    public class Enemy : Poolable
    {
        public event Action<Enemy> OnDeath;
        [SerializeField]
        private Animator _animator;

        public override void FromPool()
        {
            base.FromPool();
            transform.rotation = Quaternion.Euler(0.0f, Random.Range(0, 360), 0.0f);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Enemy death");
            OnDeath?.Invoke(this);
        }
    }
}