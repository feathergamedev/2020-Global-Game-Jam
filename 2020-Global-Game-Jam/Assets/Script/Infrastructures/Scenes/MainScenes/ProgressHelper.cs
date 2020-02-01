using Repair.Infrastructures.Core;

namespace Repair.Infrastructures.Scenes.MainScenes
{
    internal class ProgressHelper : Singleton<ProgressHelper>
    {
        private int maxStage = 0;
        private int currentStage = 0;

        internal void Initialize(int max)
        {
            maxStage = max;
        }

        internal int GetStage()
        {
            if (currentStage >= maxStage)
            {
                currentStage = 0;
            }

            return currentStage;
        }

        internal int SetStage(int stage)
        {
            currentStage = stage;
            if (currentStage >= maxStage)
            {
                currentStage = 0;
            }

            return currentStage;
        }

        internal int NextStage()
        {
            return SetStage(currentStage + 1);
        }
    }
}
