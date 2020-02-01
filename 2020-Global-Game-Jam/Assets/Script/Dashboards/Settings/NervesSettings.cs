using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Repair.Dashboards.Settings
{
    [CreateAssetMenu(menuName = "MySubMenu/Create NerveSettings")]
    public class NerveSettings : ScriptableObject
    {
        [SerializeField]
        private List<NerveController> m_nervePrefabs;

        public int MinInitX;
        public int MaxInitX;
        public float InitRotationRange;

        public NerveController GetRandomNerve()
        {
            var randomIndex = Random.Range(0, m_nervePrefabs.Count);
            return m_nervePrefabs[randomIndex];
        }
    }
}