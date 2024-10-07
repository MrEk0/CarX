using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/GameSettingsData")]
    public class GameSettingsData : ScriptableObject
    {
        [SerializeField] private float monsterMovementVelocity;
        [SerializeField] private float monsterMaxHp;
        [SerializeField] private float monsterSpawnInterval;
        [SerializeField] private float cannonTowerFireInterval;
        [SerializeField] private float simpleTowerFireInterval;
        [SerializeField] private float cannonTowerAttackRange;
        [SerializeField] private float simpleTowerAttackRange;
        [SerializeField] private float cannonTowerDamage;
        [SerializeField] private float simpleTowerDamage;
        [SerializeField] private float simpleTowerProjectileSpeed;
        [SerializeField] private float cannonTowerProjectileSpeed;
        [SerializeField] private float cannonRotateSpeed;

        public float MonsterVelocity => monsterMovementVelocity;
        public float MonsterMaxHp => monsterMaxHp;
        public float MonsterSpawnInterval => monsterSpawnInterval;
        public float CannonTowerFireInterval => cannonTowerFireInterval;
        public float SimpleTowerFireInterval => simpleTowerFireInterval;
        public float CannonTowerAttackRange => cannonTowerAttackRange;
        public float SimpleTowerAttackRange => simpleTowerAttackRange;
        public float CannonTowerDamage => cannonTowerDamage;
        public float SimpleTowerDamage => simpleTowerDamage;
        public float SimpleTowerProjectileSpeed => simpleTowerProjectileSpeed;
        public float CannonTowerProjectileSpeed => cannonTowerProjectileSpeed;
        public float CannonRotateSpeed => cannonRotateSpeed;
    }
}