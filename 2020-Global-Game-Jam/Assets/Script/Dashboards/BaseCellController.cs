using System.Collections.Generic;
using UnityEngine;

namespace Dashboards
{
    public class BaseCellController : MonoBehaviour
    {
        [SerializeField]
        protected Collider2D m_collider;

        [SerializeField]
        protected GameObject m_linkEffect;

        protected HashSet<BaseCellController> m_linkedCells = new HashSet<BaseCellController>();

        protected bool m_isPowerUp;
        public bool IsPowerUp
        {
            get => m_isPowerUp;
            set
            {
                m_isPowerUp = value;
                TriggerPowerUp(m_isPowerUp);
            }
        }

        void OnTriggerEnter2D(Collider2D triggerCollider)
        {
            Debug.Log($"[BaseCellController] OnTriggerEnter2D: {triggerCollider.gameObject.name}");

            var cell = triggerCollider.gameObject.GetComponent<BaseCellController>();
            if (cell == null)
            {
                Debug.LogError($"[BaseCellController] OnTriggerEnter2D: Can't find BaseCellController in {triggerCollider.gameObject.name}");
                return;
            }

            if (!m_linkedCells.Contains(cell))
            {
                m_linkedCells.Add(cell);
            }
        }

        void OnTriggerExit2D(Collider2D triggerCollider)
        {
            Debug.Log($"[BaseCellController] OnTriggerExit2D: {triggerCollider.gameObject.name}");

            var cell = triggerCollider.gameObject.GetComponent<BaseCellController>();
            if (cell == null)
            {
                Debug.LogError($"[BaseCellController] OnTriggerExit2D: Can't find BaseCellController in {triggerCollider.gameObject.name}");
                return;
            }

            if (m_linkedCells.Contains(cell))
            {
                m_linkedCells.Remove(cell);
            }
        }


        public virtual void TriggerPowerUp(bool active)
        {
            if (m_linkEffect == null)
            {
                return;
            }

            m_linkEffect.SetActive(active);
        }
    }
}