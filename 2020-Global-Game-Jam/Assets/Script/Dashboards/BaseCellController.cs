using System.Collections.Generic;
using UnityEngine;

namespace Repair.Dashboards
{
    public class BaseCellController : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_linkEffect;
        [SerializeField]
        private GameObject m_powerUpEffect;

        protected HashSet<BaseCellController> m_closeCell = new HashSet<BaseCellController>();
        private HashSet<BaseCellController> m_linkedCells = new HashSet<BaseCellController>();
        public HashSet<BaseCellController> LinkedCells => m_linkedCells;

        public int LinkGroup;
        protected bool m_isLinked;
        public bool IsLinked
        {
            get => m_isLinked;
            set
            {
                m_isLinked = value;
                TriggerLinked(m_isLinked);
            }
        }

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

        public void Clear()
        {
            IsLinked = false;
            m_isPowerUp = false;
        }

        void OnTriggerEnter2D(Collider2D triggerCollider)
        {
            var cell = triggerCollider.gameObject.GetComponent<BaseCellController>();
            AddLinkCell(cell);
        }

        void OnTriggerExit2D(Collider2D triggerCollider)
        {
            var cell = triggerCollider.gameObject.GetComponent<BaseCellController>();
            RemoveLinkCell(cell);
        }

        protected virtual void AddLinkCell(BaseCellController cell)
        {
            if (cell == null)
            {
                return;
            }

            if (!m_linkedCells.Contains(cell))
            {
                m_linkedCells.Add(cell);
            }
        }

        protected virtual void RemoveLinkCell(BaseCellController cell)
        {
            if (cell == null)
            {
                return;
            }

            if (m_linkedCells.Contains(cell))
            {
                m_linkedCells.Remove(cell);
            }
        }

        protected void CheckPoweredCells(BaseCellController baseCell, bool isPowerUp)
        {
            baseCell.IsPowerUp = isPowerUp;

            foreach (var cell in baseCell.LinkedCells)
            {
                cell.IsPowerUp = isPowerUp;

                if (m_closeCell.Add(cell))
                {
                    CheckPoweredCells(cell, isPowerUp);
                }
            }
        }

        public void TriggerLinked(bool active)
        {
            if (m_linkEffect == null)
            {
                return;
            }

            m_linkEffect.SetActive(active);
        }

        public virtual void TriggerPowerUp(bool active)
        {
            if (m_powerUpEffect == null)
            {
                return;
            }

            m_powerUpEffect.SetActive(active);
        }
    }
}