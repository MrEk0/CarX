using UnityEngine;
using UnityEngine.Pool;

namespace Factories
{
    public class ProjectilePoolFactory : ObjectPoolFactory
    {
        [SerializeField] private ProjectilePoolItem poolItem;

        public IObjectPool<ProjectilePoolItem> ObjectPool { get; private set; }

        private void Start()
        {
            ObjectPool = new ObjectPool<ProjectilePoolItem>(CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject);
        }

        private ProjectilePoolItem CreateProjectile()
        {
            var spawnerItem = Instantiate(poolItem, transform);
            return spawnerItem;
        }

        private void OnDestroyPooledObject(ProjectilePoolItem pooledObject)
        {
            Destroy(pooledObject.gameObject);
        }
    }
}
