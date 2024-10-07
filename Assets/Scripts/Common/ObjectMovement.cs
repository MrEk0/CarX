using Interfaces;
using UnityEngine;

namespace Common
{
    public class ObjectMovement : IGameUpdatable
    {
        private readonly float _velocity;
        private readonly Transform _owner;

        public ObjectMovement(Transform owner, float velocity)
        {
            _owner = owner;
            _velocity = velocity;
        }

        public void OnUpdate(float deltaTime)
        {
            var newPos = _owner.forward * (_velocity * deltaTime);
            _owner.position += newPos;
        }
    }
}