using System;
using UnityEngine;

namespace Repair.Infrastructures.Settings
{
    public struct SortingLayerInfo : IEquatable<SortingLayerInfo>
    {
        public int Value
        {
            get
            {
                return SortingLayer.GetLayerValueFromName(Name);
            }
        }

        public int Id
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

        public SortingLayerInfo(int id, string name)
        {
            Validate(id, name);

            Id = id;
            Name = name;
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private static void Validate(int id, string name)
        {
            Debug.Assert(!string.IsNullOrEmpty(name));

            if (Array.FindIndex(SortingLayer.layers, (layer) => layer.name == name) == -1)
            {
                Debug.LogError(string.Format("SortingLayer \"{0}\" was deleted or changed!! Please re-generate again.", name));
            }

            if (Array.FindIndex(SortingLayer.layers, (layer) => layer.id == id) == -1)
            {
                Debug.LogError(string.Format("SortingLayer \"{0}\" was deleted or changed!! Please re-generate again.", id));
            }
        }

        public bool Equals(SortingLayerInfo other)
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

            SortingLayerInfo other = (SortingLayerInfo)obj;
            return this.Id == other.Id && this.Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Id ^ Name.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[SortingLayer] #{0} {1} (Id: {2})", Value, Name, Id);
        }
    }
}
