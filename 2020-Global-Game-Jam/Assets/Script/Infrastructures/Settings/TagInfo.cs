using System;
using UnityEngine;

namespace Repair.Infrastructures.Settings
{
    public struct TagInfo : IEquatable<TagInfo>
    {
        public static readonly int MaxBuildInTagIndex = 7;

        public bool IsBuildInTag
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

        public TagInfo(int index, string name)
        {

            Validate(index, name);

            IsBuildInTag = index < MaxBuildInTagIndex;
            Name = name;
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private static void Validate(int index, string name)
        {
#if UNITY_EDITOR
            Debug.Assert(index >= 0);
            Debug.Assert(!string.IsNullOrEmpty(name));

            if (Array.FindIndex(UnityEditorInternal.InternalEditorUtility.tags, (tag) => tag == name) == -1)
            {
                Debug.LogError(string.Format("Tag \"{0}\" was deleted!! Please re-generate again.", name));
            }
#endif
        }

        public bool Equals(TagInfo other)
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

            TagInfo other = (TagInfo)obj;
            return this.Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[Tag] {0}", Name);
        }
    }
}
