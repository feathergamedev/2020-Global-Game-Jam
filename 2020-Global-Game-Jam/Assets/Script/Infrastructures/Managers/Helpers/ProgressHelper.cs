using System.Collections;
using System.Collections.Generic;
using Repair.Infrastructures.Core;
using UnityEngine;

namespace Repair.Infrastructures.Managers.Helpers
{
    internal class ProgressHelper : Singleton<ProgressHelper>
    {
        private const string STAGE = "stage_id";

        private static int maxStage = 0;

        internal void Initialize(int max)
        {
            maxStage = max;
        }

        internal int GetStage()
        {
            var currentStage = PlayerPrefs.GetInt(STAGE);
            if (currentStage >= maxStage)
            {
                currentStage = 0;
            }

            return currentStage;
        }

        internal int SetStage(int stage)
        {
            var currentStage = stage;
            if (currentStage >= maxStage)
            {
                currentStage = 0;
            }

            PlayerPrefs.SetInt(STAGE, currentStage);
            return currentStage;
        }

        internal int NextStage()
        {
            var currentStage = GetStage();
            return SetStage(++currentStage);
        }
    }
}
