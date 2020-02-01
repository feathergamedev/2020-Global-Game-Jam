using System;
using System.IO;
using UnityEngine;

namespace Repair.Infrastructures.Settings
{
    public struct SceneInfo : IEquatable<SceneInfo>
    {
        public static readonly int INVALID_BUILD_INDEX = -1;

        public int BuildIndex
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

        public string Path
        {
            get;
#if !NET_4_6
            private set;
#endif
        }

        public string BundleName
        {
            get;
#if !NET_4_6
            private set;
#endif
        }

        public SceneInfo(int buildIndex, string name, string path, string bundleName)
        {
            Validate(buildIndex, name, path, bundleName);

            BuildIndex = buildIndex;
            Name = name;
            Path = path;
            BundleName = bundleName;
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        private static void Validate(int buildIndex, string name, string path, string bundleName)
        {
            Debug.Assert(buildIndex == INVALID_BUILD_INDEX || buildIndex >= 0);
            Debug.Assert(!string.IsNullOrEmpty(name));
            Debug.Assert(!string.IsNullOrEmpty(path));

            Debug.Assert(File.Exists(path));
            if (buildIndex == INVALID_BUILD_INDEX)
            {
                Debug.Assert(!string.IsNullOrEmpty(bundleName));
            }
        }

        public bool Equals(SceneInfo other)
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

            SceneInfo other = (SceneInfo)obj;
            return GetHashCode() == other.GetHashCode();
        }

        public override int GetHashCode()
        {
            return BuildIndex ^ Name.GetHashCode() ^ Path.GetHashCode() ^ BundleName.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[Scene] #{0} {1} @ \"{2}\" (Bundle: {3})", BuildIndex, Name, Path, BundleName);
        }
    }
}
