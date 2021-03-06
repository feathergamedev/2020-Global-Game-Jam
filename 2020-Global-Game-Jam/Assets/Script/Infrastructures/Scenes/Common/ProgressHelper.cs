﻿using Repair.Infrastructures.Core;

namespace Repair.Infrastructures.Scenes
{
    internal class ProgressHelper : Singleton<ProgressHelper>
    {
        private int maxStage;
        private int currentStage;
        private bool gameComplete;
        private bool sawStory;

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

        internal bool GetComplete() => gameComplete;

        internal void SetComplete(bool complete) => gameComplete = complete;

        internal bool GetSawStory() => sawStory;

        internal void SetSawStory(bool saw) => sawStory = saw;
    }
}
