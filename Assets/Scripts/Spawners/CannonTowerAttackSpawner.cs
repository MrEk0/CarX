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
                _towerHead.rotation = Quaternion.Lerp(_towerHead.rotation, GetAttackDirection(), _data.CannonRotateSpeed * deltaTime);
                
                var distance = Vector3.Distance(_target.transform.position, _attackPoint.position);
                if (distance >= _data.CannonTowerAttackRange)
                {
                    _target = null;
                    return;
                }

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
            tr.rotation = GetAttackDirection();

            projectile.Init(_serviceLocator, _projectilesFactory.ObjectPool, _data.CannonTowerProjectileSpeed, _data.CannonTowerDamage);

            _timer = 0f;
        }

        private bool TryGetTarget(out MonsterPoolItem target)
        {
            target = null;

            var targets = _monsterPoolFactory.ActiveMonsters;
            if (targets.Count == 0)
                return false;

            target = targets.FirstOrDefault(o =>
                Vector3.Distance(o.transform.position, _towerHead.position) <= _data.CannonTowerAttackRange);

            return target != null;
        }

        private Quaternion GetAttackDirection()
        {
            var tr = _target.transform;
            var direction = _attackPoint.position - tr.position;

            var velocity = _data.CannonTowerProjectileSpeed;
            var velocityDir = tr.forward;
            var a = velocityDir.sqrMagnitude - velocity;
            var b = 2f * Vector3.Dot(direction, velocityDir);
            var c = direction.sqrMagnitude;

            var discriminant = b * b - 4f * a * c;

            if (discriminant < 0f || a == 0f)
                return Quaternion.identity;

            var time = (-b - Mathf.Sqrt(discriminant)) / (2f * a);

            if (time <= 0f)
                return Quaternion.identity;
            
            var crossPoint = (_target.transform.position + velocityDir * time - _attackPoint.position).normalized;
            return Quaternion.LookRotation(crossPoint);
        }
    }
}