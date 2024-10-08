using Common;
using Interfaces;
using UnityEngine;
using UnityEngine.Pool;

namespace Factories
{
    public class ProjectilePoolItem : AObjectPoolItem
    {
        private IObjectPool<ProjectilePoolItem> _objectPool;
        private float _damage;
        private bool _isInit;

        public void Init(ServiceLocator serviceLocator, IObjectPool<ProjectilePoolItem> objectPool, float velocity, float damage)
        {
            if (_isInit)
                return;
            
            _isInit = true;
            
            _objectPool = objectPool;
            _damage = damage;

            var movement = new ObjectMovement(transform, velocity);
            serviceLocator.GetService<GameUpdater>().AddListener(movement);
        }

        private void OnTriggerEnter(Collider other)
        {
            var collidedObject = other.gameObject.GetComponent<IDamagable>();
            collidedObject?.TakeDamage(_damage);

            if (ObjectPoolFactory.CanRelease(this))
                _objectPool.Release(this);
        }
    }
}