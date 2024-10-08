using Common;
using Configs;
using Interfaces;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Pool;

namespace Factories
{
    public class MonsterPoolItem : AObjectPoolItem, IDamagable
    {
        [SerializeField] private LayerMask targetMask;

        [CanBeNull] private ObjectHp _objectHp;
        private IObjectPool<MonsterPoolItem> _objectPool;
        private bool _isInit;

        public void Init(ServiceLocator serviceLocator)
        {
            var data = serviceLocator.GetService<GameSettingsData>();
            
            if (_isInit)
            {
                _objectHp?.SetHp(data.MonsterMaxHp);
                return;
            }

            _isInit = true;

            var movement = new ObjectMovement(transform, data.MonsterVelocity);
            _objectHp = new ObjectHp(data.MonsterMaxHp);

            var gameUpdater = serviceLocator.GetService<GameUpdater>();
            gameUpdater.AddListener(movement);
            
            _objectPool = serviceLocator.GetService<MonsterPoolFactory>().ObjectPool;

            _objectHp.DeathEvent += OnMonsterDead;
        }

        private void OnDestroy()
        {
            if (_objectHp == null)
                return;
            
            _objectHp.DeathEvent += OnMonsterDead;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (targetMask.value != (targetMask.value | (1 << other.gameObject.layer)))
                return;
            
            if (ObjectPoolFactory.CanRelease(this))
                _objectPool.Release(this);
        }

        private void OnMonsterDead()
        {
            if (ObjectPoolFactory.CanRelease(this))
                _objectPool.Release(this);
        }

        public void TakeDamage(float damageValue)
        {
            if (_objectHp == null)
                return;
            
            _objectHp.TakeDamage(damageValue);
        }
    }
}