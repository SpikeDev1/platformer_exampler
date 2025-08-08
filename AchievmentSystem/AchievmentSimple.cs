using System;


namespace Gamemaker.AchievmentSystem
{
    public class AchievmentSimple
    {
        public string Name
        {
            get
            {
                return this.data.name;
            }
        }

        public float CurrentScore
        {
            get
            {
                return this.StateData.currentScore;
            }
        }

        public bool IsCompleted
        {
            get
            {
                return this.StateData.isCompleted;
            }
        }

        public string ID
        {
            get
            {
                return this.StateData.id;
            }
        }

        public AchievmentSimpleState StateData { get; private set; }


        private event Action<AchievmentSimple> onComplete;
        private AchievmentSimpleData data;

        [System.Serializable]
        public class AchievmentSimpleData
        {
            public string id;
            public string name; // it is better to be unique
            public float targetScore = 1f;
        }

        [System.Serializable]
        public class AchievmentSimpleState
        {
            public string id;
            public float currentScore = 1f;
            public bool isCompleted;
        }

        public AchievmentSimple(AchievmentSimpleState currentState, AchievmentSimpleData data, Action<AchievmentSimple> onComplete)
        {
            this.StateData = currentState;
            this.data = data;

            this.onComplete = onComplete;
        }

        public AchievmentSimple(AchievmentSimpleData data, Action<AchievmentSimple> onComplete)
        {
            this.StateData = new AchievmentSimpleState();
            this.StateData.id = data.id;
            this.data = data;

            this.onComplete = onComplete;
        }

        public void Update(int score)
        {
            if (IsCompleted)
            {
                return;
            }

            this.StateData.currentScore = score;

            if (this.CurrentScore >= this.data.targetScore)
            {
                this.StateData.isCompleted = true;
                this.onComplete?.Invoke(this);
            }
        }
    }
}
