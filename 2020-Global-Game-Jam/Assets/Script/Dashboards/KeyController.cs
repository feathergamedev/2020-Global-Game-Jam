using System.Collections.Generic;
using UnityEngine;

namespace Repair.Dashboards
{
    public class KeyController : BaseCellController
    {
        [SerializeField]
        private KeyCode m_keyCode;

        public KeyCode KeyCode => m_keyCode;

        public void Clear()
        {
            IsPowerUp = false;
            foreach (var cell in m_closeCell)
            {
                cell.IsPowerUp = false;
            }
        }

        public void Trigger()
        {
            m_closeCell.Clear();
            CheckLinkedCells(this, true);
        }
    }
}