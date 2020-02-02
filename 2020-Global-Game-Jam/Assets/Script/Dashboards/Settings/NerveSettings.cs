using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Repair.Dashboards.Settings
{
    public class ScriptableSample : ScriptableObject
    {
#if UNITY_EDITOR

        [MenuItem("Assets/Resources/CreateNerveSettings")]
        public static void CreateAsset()
        {
            NerveSettings ObjSample = ScriptableObject.CreateInstance<NerveSettings>();
            string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/" + typeof(NerveSettings) + ".asset");

            AssetDatabase.CreateAsset(ObjSample, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
    }

    [CreateAssetMenu(fileName = "NerveSettings", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
    public class NerveSettings : ScriptableObject
    {
        [SerializeField]
        private List<NerveController> m_nervePrefabs;

        public int MinInitX;
        public int MaxInitX;
        public float InitRotationRange;

        public NerveController GetRandomNerve()
        {
            var randomIndex = UnityEngine.Random.Range(0, m_nervePrefabs.Count);
            return m_nervePrefabs[randomIndex];
        }
    }
}
