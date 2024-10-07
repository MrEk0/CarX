using System.Linq;
using Common;
using Configs;
using Factories;
using Interfaces;
using UnityEngine;

namespace Spawners
{
    public class CannonTowerAttackSpawner : Spawner, IGameUpdatable
    {
        private readonly ProjectilePoolFactory _projectilesFactory;
        private readonly MonsterPoolFactory _monsterPoolFactory;
        private readonly ServiceLocator _serviceLocator;
        private readonly Transform _towerHead;
        private readonly Transform _attackPoint;
        private readonly GameSettingsData _data;
        
        private MonsterPoolItem _target;
        private float _timer;

        public CannonTowerAttackSpawner(ServiceLocator serviceLocator, ProjectilePoolFactory projectilesFactory, Transform attackPoint, Transform towerHead)
        {
            _serviceLocator = serviceLocator;
            _projectilesFactory = projectilesFactory;
            _towerHead = towerHead;
            _attackPoint = attackPoint;
            _monsterPoolFactory = serviceLocator.GetService<MonsterPoolFactory>();
            _data = serviceLocator.GetService<GameSettingsData>();
            
            _timer = _data.CannonTowerFireInterval;
        }

        public void OnUpdate(float deltaTime)
        {
            _timer += deltaTime;
            
            if (_target != null && _target.gameObject.activeSelf)
            {
                _towerHead.rotation = Quaternion.Lerp(_towerHead.rotation, CalculatePredictedAttackRotation(), _data.CannonRotateSpeed * deltaTime);
                
                var distance = Vector3.Distance(_target.transform.position, _attackPoint.position);
                if (distance >= _data.CannonTowerAttackRange)
                    return;

                if (_timer >= _data.CannonTowerFireInterval)
                    Spawn();
                
                return;
            }

            _towerHead.rotation = Quaternion.Lerp(_towerHead.rotation, Quaternion.identity, _data.CannonRotateSpeed * deltaTime);
            
            if (TryGetTarget(out var target))
                _target = target;
        }

        public override void Spawn()
        {
            var projectile = _projectilesFactory.ObjectPool.Get();
            var tr = projectile.transform;
            tr.position = _attackPoint.position;
            tr.rotation = _attackPoint.rotation;

            projectile.Init(_serviceLocator, _projectilesFactory.ObjectPool, _data.CannonTowerProjectileSpeed, _data.CannonTowerDamage);

            _timer = 0f;
        }

        private bool TryGetTarget(out MonsterPoolItem target)
        {
            target = null;

            var targets = _monsterPoolFactory.ActiveMonsters;
            if (targets.Count == 0)
                return false;
            
            target = targets.FirstOrDefault();

            return target != null;
        }

        private Quaternion CalculatePredictedAttackRotation()
        {
            if (_data.MonsterVelocity == 0f || _data.CannonTowerProjectileSpeed == 0f)
                return Quaternion.identity;
            
            var targetSpeed = _data.MonsterVelocity;
            var bulletSpeed = _data.CannonTowerProjectileSpeed;

            var distanceRate = bulletSpeed / targetSpeed;

            var tr = _target.transform;
            var targetPos = tr.position;
            var attackPos = _attackPoint.position;

            var angle = Vector3.Angle(attackPos - targetPos, tr.forward) / distanceRate;

            var predictedDirection = Quaternion.AngleAxis(-angle, Vector3.up) * (targetPos - attackPos);
         
            return Quaternion.LookRotation(predictedDirection, Vector3.up);
        }
    }
}