namespace Gameplay.Actors
{
    public interface IMoveableActorView
    {
        void LookLeft();
        void LookRight();
        void Present(bool grounded, float normalizedSpeedHorz, float velocityVertc);
    }
}