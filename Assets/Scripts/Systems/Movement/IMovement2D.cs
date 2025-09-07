namespace Systems.Movement
{
    public interface IMovement2D
    {
        float Speed { get; }
        void Left();
        void Right();
        void Stop();
    }
}