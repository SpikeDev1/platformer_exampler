namespace Gameplay.Round
{
    public class TokensCollectorInfo
    {
        public int Total { get; private set; }
        public int Collected { get; private set; }

        public void SetTotal(int v)
        {
            Total = v < 0 ? 0 : v;
            Collected = 0;
        }

        public void Inc()
        {
            if (Collected < Total) Collected++;
        }

        public void Reset()
        {
            Collected = 0;
        }
    }
}