using System;
using System.Collections.Generic;
using UnityEngine;

namespace Repair.Stages.Puzzles
{
    public class StagePuzzle : MonoBehaviour
    {
        [Serializable]
        private class TransformSetting
        {
            public string name;
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
        private class DecorationSettings
        {
            public string name;
            public Vector3 position = Vector3.zero;

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
                    instance.name = string.IsNullOrEmpty(decorationSetting.name) ? decorationSetting.type.ToString() : decorationSetting.name;
                    ResetTransform(instance.transform, decorationSetting);
                }
            }
        }

        private void CreateFloor()
        {
            foreach (var floorSetting in floorSettings)
            {
                var floor = Instantiate(floorPrefab, floorsRoot);
                floor.name = floorSetting.name;
                ResetTransform(floor.transform, floorSetting);
            }
        }

        private void ResetTransform(Transform transform, TransformSetting setting)
        {
            transform.localPosition = setting.position;
            transform.rotation = setting.rotation;
            transform.localScale = setting.scale;
        }

        private void ResetTransform(Transform transform, DecorationSettings setting)
        {
            transform.localPosition = setting.position;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var color = Gizmos.color;
            foreach (var setting in decorationSettings)
            {
                switch (setting.type)
                {
                    case DecorationType.Mountain:
                        DrawGizmo(mountainPrefab, Color.red, setting);
                        break;

                    case DecorationType.Cloud:
                        DrawGizmo(cloudPrefab, Color.blue, setting);
                        break;

                    case DecorationType.Tree:
                        DrawGizmo(treePrefab, Color.green, setting);
                        break;
                }
            }

            foreach (var setting in floorSettings)
            {
                DrawGizmoWithScale(floorPrefab, Color.gray, setting);
            }

            Gizmos.color = color;

            void DrawGizmo(GameObject prefab, Color gizmoColor, DecorationSettings setting)
            {
                var prefabTransform = prefab.transform as RectTransform;
                var size = prefabTransform.rect.size;
                var offset = (prefabTransform.pivot - new Vector2(0.5f, 0.5f)) * size;
                var center = transform.position + setting.position + new Vector3(offset.x, -offset.y, 0);
                Gizmos.color = gizmoColor;
                Gizmos.DrawWireCube(center, new Vector3(size.x, size.y, 1));
            }

            void DrawGizmoWithScale(GameObject prefab, Color gizmoColor, TransformSetting setting)
            {
                var prefabTransform = prefab.transform as RectTransform;
                var size = prefabTransform.rect.size * setting.scale;
                var offset = (prefabTransform.pivot - new Vector2(0.5f, 0.5f)) * size;
                var center = transform.position + setting.position + new Vector3(offset.x, -offset.y, 0);
                Gizmos.color = gizmoColor;
                Gizmos.DrawWireCube(center, new Vector3(size.x, size.y, 1));
            }
        }
#endif
    }
}
