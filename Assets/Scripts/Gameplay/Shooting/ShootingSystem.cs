using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Data;
using Pools.Interfaces;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Zenject;

namespace Gameplay.Shooting
{
    public class ShootingSystem : MonoBehaviour
    {
        [SerializeField]
        private Transform _turret;
        [SerializeField]
        private Transform _projectileSpawnPoint;
        [SerializeField]
        private float _maxRotationAngle = 45;

        private EventSystem _eventSystem;
        private PlayerData _playerData;
        private GameConfigSo _gameConfig;
        private IPool<Projectile> _projectilePool;

        private float _targetAngle;
        private float _halfWidth;

        private Touchscreen _touchscreen;

        private CancellationTokenSource _cts;


        [Inject]
        private void Construct(EventSystem eventSystem, PlayerData playerData, GameConfigSo gameConfig, IPool<Projectile> projectilePool)
        {
            _eventSystem = eventSystem;
            _playerData = playerData;
            _gameConfig = gameConfig;
            _projectilePool = projectilePool;
        }

        private void Start()
        {
            _halfWidth = Screen.width / 2f;
            _cts = new CancellationTokenSource();
            StartShooting(_cts.Token).Forget();
        }

        private void OnDestroy()
        {
            _cts.Dispose();
        }

        private async UniTaskVoid StartShooting(CancellationToken token)
        {
            try
            {
                while(!token.IsCancellationRequested)
                {
                    Shoot();
                    await UniTask.WaitForSeconds(_gameConfig.BaseFireRateTime, cancellationToken: token);
                }
            } catch(OperationCanceledException)
            {

            }
        }

        private void Shoot()
        {
            Projectile projectile = _projectilePool.Get();
            projectile.transform.position = _projectileSpawnPoint.position;
            Vector3 direction = new Vector3(_turret.forward.x, 0, _turret.forward.z).normalized;
            projectile.Shoot(direction);
        }

        private void Update()
        {
            ReadInput();
        }

        private void ReadInput()
        {
            _touchscreen ??= Touchscreen.current;

            if(_touchscreen == null)
            {
                return;
            }

            TouchControl touch = _touchscreen.primaryTouch;

            if(!touch.press.isPressed)
            {
                return;
            }

            int touchId = touch.touchId.ReadValue();
            bool touchIsOverUI = _eventSystem.IsPointerOverGameObject(touchId);

            if(touchIsOverUI)
            {
                return;
            }

            HandleTouch(touch);
        }

        private void HandleTouch(TouchControl touch)
        {
            Vector2 screenPosition = touch.position.ReadValue();
            float xPosition = screenPosition.x - _halfWidth;
            float xPositionPercents = xPosition / _halfWidth;
            xPositionPercents = Mathf.Clamp(xPositionPercents, -1.0f, 1.0f);
            _turret.rotation = Quaternion.Euler(0, xPositionPercents * _maxRotationAngle, 0);
        }
    }
}