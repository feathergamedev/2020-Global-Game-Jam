using UnityEngine;

namespace Repair.Infrastructures.Settings
{

    public static class ProjectExtension
    {
        #region TagInfo Extensions

        public static bool CompareTag(this GameObject gameObject, TagInfo info)
        {
            return gameObject.CompareTag(info.Name);
        }

        public static bool CompareTag(this MonoBehaviour monoBehaviour, TagInfo info)
        {
            return monoBehaviour.gameObject.CompareTag(info);
        }

        #endregion

        #region LayerInfo Extension

        public static void ChangeLayerIncludeChildren(this GameObject root, LayerInfo layer)
        {
            root.transform.ChangeLayerIncludeChildren(layer);
        }

        public static void ChangeLayerIncludeChildren(this Transform root, LayerInfo layer)
        {
            var max = root.childCount;
            root.gameObject.layer = layer.Index;
            for (int index = 0; index < max; ++index)
            {
                var child = root.GetChild(index);
                child.ChangeLayerIncludeChildren(layer);
            }
        }

        #endregion

        #region SortingLayerInfo Extension

        public static void ChangeSortingLayer(this Renderer renderer, SortingLayerInfo info)
        {
            renderer.sortingLayerName = info.Name;
        }

        public static void ChangeSortingLayer(this Renderer renderer, SortingLayerInfo info, int sortingOrder)
        {
            renderer.sortingLayerName = info.Name;
            renderer.sortingOrder = sortingOrder;
        }

        #endregion
    }
}
