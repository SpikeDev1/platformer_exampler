namespace Systems.Movement
{
    public interface IJump
    {
        bool Enabled { get; }
        void Enable();
        void Disable();
        bool TryJump(float power);
    }
}