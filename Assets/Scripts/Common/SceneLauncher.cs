using Configs;
using Factories;
using Spawners;
using Towers;
using UnityEngine;

namespace Common
{
    public class SceneLauncher : MonoBehaviour
    {
        [SerializeField] private GameSettingsData data;
        [SerializeField] private GameUpdater gameUpdater;
        [SerializeField] private ProjectilePoolFactory cannonBulletFactory;
        [SerializeField] private ProjectilePoolFactory simpleBulletFactory;
        [SerializeField] private MonsterPoolFactory monsterPoolFactory;
        [SerializeField] private Transform monstersStartPosition;
        [SerializeField] private SimpleTower simpleTower;
        [SerializeField] private CannonTower cannonTower;

        private ServiceLocator _serviceLocator;

        private void Start()
        {
            _serviceLocator = new ServiceLocator();
            _serviceLocator.AddService(data);
            _serviceLocator.AddService(cannonBulletFactory);
            _serviceLocator.AddService(simpleBulletFactory);
            _serviceLocator.AddService(monsterPoolFactory);
            _serviceLocator.AddService(gameUpdater);

            var monsterSpawner = new MonsterSpawner(_serviceLocator, monstersStartPosition);
            gameUpdater.AddListener(monsterSpawner);

            simpleTower.Init(_serviceLocator, simpleBulletFactory);
            cannonTower.Init(_serviceLocator, cannonBulletFactory);
        }

        private void OnDestroy()
        {
            gameUpdater.RemoveAll();
            _serviceLocator.RemoveAll();
        }
    }
}
