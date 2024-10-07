using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Factories
{
    public class MonsterPoolFactory : ObjectPoolFactory
    {
        [SerializeField] private MonsterPoolItem poolItem;

        public IObjectPool<MonsterPoolItem> ObjectPool { get; private set; }
        public IReadOnlyList<MonsterPoolItem> ActiveMonsters => _monsters.Where(o => o.isActiveAndEnabled).ToList();

        private List<MonsterPoolItem> _monsters = new List<MonsterPoolItem>();

        private void Start()
        {
            ObjectPool = new ObjectPool<MonsterPoolItem>(CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject);
        }

        private MonsterPoolItem CreateProjectile()
        {
            var spawnerItem = Instantiate(poolItem, transform);
            _monsters.Add(spawnerItem);
            return spawnerItem;
        }

        private void OnDestroyPooledObject(MonsterPoolItem pooledObject)
        {
            _monsters.Remove(pooledObject);
            Destroy(pooledObject.gameObject);
        }
    }
}
