using Common;
using Factories;
using Spawners;
using UnityEngine;

namespace Towers
{
    public class CannonTower : Tower
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform cannonHead;

        public void Init(ServiceLocator serviceLocator, ProjectilePoolFactory projectilePoolFactory)
        {
            var cannonTowerAttackSpawner = new CannonTowerAttackSpawner(serviceLocator, projectilePoolFactory, spawnPoint, cannonHead);
            serviceLocator.GetService<GameUpdater>().AddListener(cannonTowerAttackSpawner);
        }
    }
}