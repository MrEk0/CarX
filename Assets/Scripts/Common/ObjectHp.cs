using System;
using Interfaces;

namespace Common
{
    public class ObjectHp : IDamagable
    {
        public event Action DeathEvent = delegate { };
        
        private float _hp;

        public ObjectHp(float maxHp)
        {
            _hp = maxHp;
        }

        public void SetHp(float maxHp)
        {
            _hp = maxHp;
        }

        public void TakeDamage(float damageValue)
        {
            _hp -= damageValue;
            if (_hp <= 0f)
                DeathEvent();
        }
    }
}
