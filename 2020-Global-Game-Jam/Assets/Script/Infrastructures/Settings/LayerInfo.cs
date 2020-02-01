using System;
using UnityEngine;

namespace Repair.Infrastructures.Settings
{
    public struct LayerInfo : IEquatable<LayerInfo>
    {
        public static readonly int MaxBuildInLayerIndex = 8;
        public static readonly int MaxLayerIndex = 32;

        public bool IsBuildInTag
        {
            get;
#if !NET_4_6
            private set;
#endif
        }

        public int Index
        {
            get;
#if !NET_4_6
            private set;
#endif
        }

        public string Name
        {
            get;
#if !NET_4_6
            private set;
#endif
        }

        public LayerInfo(int index, string name)
        {
            Validate(index, name);

            IsBuildInTag = index < MaxBuildInLayerIndex;
            Index = index;
            Name = name;
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private static void Validate(int index, string name)
        {
#if UNITY_EDITOR
            Debug.Assert(index >= 0 && index < MaxLayerIndex);
            Debug.Assert(!string.IsNullOrEmpty(name));

            if (Array.FindIndex(UnityEditorInternal.InternalEditorUtility.layers, (layer) => layer == name) == -1)
            {
                Debug.LogError(string.Format("Layer \"{0}\" was deleted!! Please re-generate again.", name));
            }
#endif
        }

        public bool Equals(LayerInfo other)
        {
            return GetHashCode() == other.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            LayerInfo other = (LayerInfo)obj;
            return this.Index == other.Index && this.Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Index ^ Name.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[Layer] #{0} {1}", Index, Name);
        }
    }
}
