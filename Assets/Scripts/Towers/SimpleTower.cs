using System.Collections.Generic;
using System.Linq;
using Common;
using Configs;
using Factories;
using Spawners;
using UnityEngine;

namespace Towers
{
    public class SimpleTower : Tower
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] protected SphereCollider attackCollider;
        
        private readonly List<MonsterPoolItem> _targets = new();
        
        public void Init(ServiceLocator serviceLocator, ProjectilePoolFactory projectilePoolFactory)
        {
            var data = serviceLocator.GetService<GameSettingsData>();
            attackCollider.isTrigger = true;
            attackCollider.radius = data.SimpleTowerAttackRange;
            
            var simpleTowerAttack = new SimpleTowerAttackSpawner(serviceLocator, projectilePoolFactory, this, spawnPoint);
            serviceLocator.GetService<GameUpdater>().AddListener(simpleTowerAttack);
        }

        private void OnTriggerEnter(Collider other)
        {
            var monster = other.GetComponent<MonsterPoolItem>();
            if (monster == null)
                return;

            _targets.Add(monster);
        }

        private void OnTriggerExit(Collider other)
        {
            var monster = other.GetComponent<MonsterPoolItem>();
            if (monster == null)
                return;

            _targets.Remove(monster);
        }

        public IReadOnlyList<MonsterPoolItem> GetTargets(float radius)
        {
            return _targets.Where(o => o.gameObject.activeSelf && Vector3.Distance(o.transform.position, transform.position) < radius).ToList();
        }
    }
}
