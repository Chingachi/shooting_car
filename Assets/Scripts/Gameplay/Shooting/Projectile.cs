using Pools;
using Pools.Interfaces;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Gameplay.Shooting
{
    public class Projectile : Poolable
    {
        [SerializeField]
        private ProjectileModel _projectileModel;
        [SerializeField]
        private Rigidbody _rigidbody;
        [Inject]
        private GameConfigSo _gameConfig;
        [Inject]
        private IPool<Projectile> _pool;

        private bool _hasBeenShot;
        private Vector3 _direction;

        private void Start()
        {
            _projectileModel.OnBecomeInvisibleEvent += Despawn;
        }

        public void Shoot(Vector3 direction)
        {
            _hasBeenShot = true;
            _direction = direction;
            transform.rotation = Quaternion.LookRotation(_direction);
        }

        public override void ToPool()
        {
            base.ToPool();
            _hasBeenShot = false;
            _direction = Vector3.zero;
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        private void OnTriggerEnter(Collider other)
        {
            Despawn();
        }

        private void FixedUpdate()
        {
            if(!_hasBeenShot)
            {
                return;
            }

            Vector3 nextPosition = _rigidbody.position + _direction.normalized * (_gameConfig.BaseProjectileSpeed * Time.fixedDeltaTime);
            _rigidbody.MovePosition(nextPosition);
        }

        private void Despawn()
        {
            if(_pool == null)
            {
                Debug.LogError("Pool for projectile has not been injected!");
                return;
            }

            _pool.Return(this);
        }

        private void OnValidate()
        {
            _rigidbody = transform.GetOrAddComponent<Rigidbody>();

            if(_projectileModel == null)
            {
                _projectileModel = GetComponentInChildren<ProjectileModel>();
            }
        }
    }
}