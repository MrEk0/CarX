using System.Linq;
using Common;
using Configs;
using Factories;
using Interfaces;
using Towers;
using UnityEngine;

namespace Spawners
{
    public class SimpleTowerAttackSpawner : Spawner, IGameUpdatable
    {
        private readonly ProjectilePoolFactory _projectilesFactory;
        private readonly ServiceLocator _serviceLocator;
        private readonly SimpleTower _ownerTower;
        private readonly Vector3 _position;
        private readonly float _attackRange;
        private readonly float _damage;
        private readonly float _velocity;
        private readonly float _spawnInterval;
        
        private MonsterPoolItem _target;
        private float _timer;

        public SimpleTowerAttackSpawner(ServiceLocator serviceLocator, ProjectilePoolFactory projectilesFactory, SimpleTower tower, Transform attackPoint)
        {
            _ownerTower = tower;
            _serviceLocator = serviceLocator;
            _projectilesFactory = projectilesFactory;
            _position = attackPoint.position;

            var data = serviceLocator.GetService<GameSettingsData>();
            _spawnInterval = data.SimpleTowerFireInterval;
            _damage = data.SimpleTowerDamage;
            _velocity = data.SimpleTowerProjectileSpeed;
            _attackRange = data.SimpleTowerAttackRange;
            
            _timer = _spawnInterval;
        }

        public void OnUpdate(float deltaTime)
        {
            _timer += deltaTime;
            
            if (_target != null)
            {
                var distance = Vector3.Distance(_target.transform.position, _ownerTower.transform.position);
                if (distance >= _attackRange)
                    _target = TryGetTarget(out var newTarget) ? newTarget : null;

                if (_target != null && _timer >= _spawnInterval)
                    Spawn();
                
                return;
            }

            if (TryGetTarget(out var target))
                _target = target;
        }

        public override void Spawn()
        {
            var projectile = _projectilesFactory.ObjectPool.Get();
            var tr = projectile.transform;
            tr.position = _position;
            tr.rotation = Quaternion.LookRotation(_target.transform.position - _position);

            projectile.Init(_serviceLocator, _projectilesFactory.ObjectPool, _velocity, _damage);

            _timer = 0f;
        }

        private bool TryGetTarget(out MonsterPoolItem target)
        {
            target = null;
            
            var targets = _ownerTower.GetTargets(_attackRange);
            if (targets.Count == 0)
                return false;
            
            target = targets.FirstOrDefault();

            return target != null;
        }
    }
}