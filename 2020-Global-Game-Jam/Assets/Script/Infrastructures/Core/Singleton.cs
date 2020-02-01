using UnityEngine;
using UnityEngine.Assertions;

namespace Repair.Infrastructures.Core
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T I => (instance == null) ? CreateInstance() : instance;

        private static T CreateInstance()
        {
            Assert.IsNull(instance);

            var go = new GameObject();
            go.name = $"(Singleton) {typeof(T).Name}";
            DontDestroyOnLoad(go);
            instance = go.AddComponent<T>();

            return instance;
        }
    }
}
