using Model.Configs;

namespace Systems.Movement
{
    public class Movement2DAbility : IMovement2D
    {
        private readonly IRigidbody2DAdapter body;
        private readonly MoveConfig move;

        public Movement2DAbility(IRigidbody2DAdapter body, MoveConfig move)
        {
            this.body = body;
            this.move = move;
        }

        public float Speed => move.maxSpeed;

        public void Left()
        {
            var v = body.Velocity;
            v.x = -move.maxSpeed;
            body.Velocity = v;
        }

        public void Right()
        {
            var v = body.Velocity;
            v.x = move.maxSpeed;
            body.Velocity = v;
        }

        public void Stop()
        {
            var v = body.Velocity;
            v.x = 0f;
            body.Velocity = v;
        }
    }
}