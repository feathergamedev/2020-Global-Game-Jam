using System.Collections.Generic;
using System.Linq;
using Repair.Infrastructures.Events;
using UnityEngine;

namespace Dashboards
{
    public class DashboardController : MonoBehaviour
    {
        [SerializeField]
        private List<NervesController> m_nerves;

        [SerializeField]
        private List<ActionController> m_actions;

        [SerializeField]
        private List<KeyController> m_keyControllers;

        private HashSet<KeyCode> m_pressedKeyCodes = new HashSet<KeyCode>();


        void Update()
        {

            if (Input.GetKeyUp(KeyCode.Z))
            {
                m_pressedKeyCodes.Remove(KeyCode.Z);
            }

            if (Input.GetKeyUp(KeyCode.X))
            {
                m_pressedKeyCodes.Remove(KeyCode.X);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                m_pressedKeyCodes.Add(KeyCode.Z);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                m_pressedKeyCodes.Add(KeyCode.X);
            }

            foreach (var keyController in m_keyControllers)
            {
                keyController.Trigger(m_pressedKeyCodes);
            }

            var action = 0;
            foreach (var cell in m_actions.Where(e => e.IsPowerUp))
            {
                action |= (int)cell.ActionType;
            }

            EventEmitter.Emit(GameEvent.Action, new IntEvent(action));
        }
    }
}