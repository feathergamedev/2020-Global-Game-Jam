using System.Collections.Generic;
using UnityEngine;

namespace Dashboards
{
    public class KeyController : BaseCellController
    {
        [SerializeField]
        private KeyCode m_keyCode;

        //protected HashSet<BaseCellController> m_closeCell = new HashSet<BaseCellController>();


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