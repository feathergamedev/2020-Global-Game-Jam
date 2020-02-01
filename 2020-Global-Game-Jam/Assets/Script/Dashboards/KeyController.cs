using System.Collections.Generic;
using UnityEngine;

namespace Repair.Dashboards
{
    public class KeyController : BaseCellController
    {
        [SerializeField]
        private KeyCode m_keyCode;


        public void Trigger(HashSet<KeyCode> keyCodes)
        {
            var isKeyPressed = keyCodes.Contains(m_keyCode);

            foreach (var cell in m_closeCell)
            {
                cell.IsPowerUp = false;
            }

            m_closeCell.Clear();
            CheckLinkedCells(this, isKeyPressed);
        }
    }
}