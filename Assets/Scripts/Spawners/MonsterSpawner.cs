using Common;
using Configs;
using Factories;
using Interfaces;
using UnityEngine;

namespace Spawners
{
    public class MonsterSpawner : Spawner, IGameUpdatable
    {
        private readonly ServiceLocator _serviceLocator;
        private readonly MonsterPoolFactory _monsterFactory;
        private readonly Vector3 _startPosition;
        private readonly float _spawnInterval;
        private readonly Quaternion _rotation;

        private float _timer;

        public MonsterSpawner(ServiceLocator serviceLocator, Transform startPoint)
        {
            _serviceLocator = serviceLocator;
            _monsterFactory = serviceLocator.GetService<MonsterPoolFactory>();
            _startPosition = startPoint.position;
            _rotation = startPoint.rotation;

            var data = serviceLocator.GetService<GameSettingsData>();
            _spawnInterval = data.MonsterSpawnInterval;

            _timer = _spawnInterval;
        }

        public void OnUpdate(float deltaTime)
        {
            _timer += deltaTime;
            if (_timer < _spawnInterval)
                return;

            Spawn();

            _timer = 0f;
        }

        public override void Spawn()
        {
            var monster = _monsterFactory.ObjectPool.Get();
           
            var tr = monster.transform;
            tr.position = _startPosition;
            tr.rotation = _rotation;
            
            monster.Init(_serviceLocator);
        }
    }
}
