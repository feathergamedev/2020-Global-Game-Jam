using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Repair.Stages.Puzzles
{
    public class StagePuzzle : MonoBehaviour
    {
        [Serializable]
        private class TransformSetting
        {
            public Vector3 position = Vector3.zero;

            public Quaternion rotation;

            public Vector3 scale = Vector3.one;
        }

        public enum DecorationType
        {
            None,
            Sky,
            Mountain,
            Cloud,
            Tree,
        }

        [Serializable]
        private class DecorationSettings : TransformSetting
        {
            public DecorationType type = DecorationType.None;
        }

        [SerializeField]
        private Transform backgroundsRoot;

        [SerializeField]
        private GameObject mountainPrefab;

        [SerializeField]
        private GameObject cloudPrefab;

        [SerializeField]
        private GameObject treePrefab;

        [SerializeField]
        private List<DecorationSettings> decorationSettings;

        [SerializeField]
        private Transform floorsRoot;

        [SerializeField]
        private GameObject floorPrefab;

        [SerializeField]
        private List<TransformSetting> floorSettings;

        public void Awake()
        {
            CreateDecoration();
            CreateFloor();
        }

        private void CreateDecoration()
        {
            foreach (var decorationSetting in decorationSettings)
            {
                GameObject instance = null;
                switch (decorationSetting.type)
                {
                    case DecorationType.Sky:
                        break;

                    case DecorationType.Mountain:
                        instance = Instantiate(mountainPrefab, backgroundsRoot);
                        break;

                    case DecorationType.Cloud:
                        instance = Instantiate(cloudPrefab, backgroundsRoot);
                        break;

                    case DecorationType.Tree:
                        instance = Instantiate(treePrefab, backgroundsRoot);
                        break;

                    default:
                        break;
                }

                if (instance != null)
                {
                    ResetTransform(instance.transform, decorationSetting);
                }
            }
        }

        private void CreateFloor()
        {
            foreach (var floorSetting in floorSettings)
            {
                var floor = Instantiate(floorPrefab, floorsRoot);
                ResetTransform(floor.transform, floorSetting);
            }
        }

        private void ResetTransform(Transform transform, TransformSetting setting)
        {
            transform.localPosition = setting.position;
            transform.rotation = setting.rotation;
            transform.localScale = setting.scale;
        }

    }
}
