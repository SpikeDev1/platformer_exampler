using UnityEngine;

namespace Systems.Movement
{
    public interface IRigidbody2DAdapter : IContactGround
    {
        Vector2 Velocity { get; set; }
        Vector3 Position { get; }
        void AddImpulse(Vector2 force);
        void Teleport(Vector3 position);
        void Stop();
    }

    public interface IContactGround
    {
        bool IsGrounded { get; }
    }
}