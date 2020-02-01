using System.Collections.Generic;
using UnityEngine;

namespace Dashboards
{
    public class KeyController : BaseCellController
    {
        [SerializeField]
        private KeyCode m_keyCode;

        private HashSet<BaseCellController> m_openCell = new HashSet<BaseCellController>();
        private HashSet<BaseCellController> m_closeCell = new HashSet<BaseCellController>();

        public void Trigger(HashSet<KeyCode> keyCodes)
        {
            var isKeyPressed = keyCodes.Contains(m_keyCode);
            TriggerPowerUp(isKeyPressed);

            if (!isKeyPressed)
            {
                return;
            }

            foreach (var cell in m_linkedCells)
            {

            }
        }
    }
}