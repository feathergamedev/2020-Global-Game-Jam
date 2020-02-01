using System;
using System.Collections.Generic;
using System.Linq;
using Repair.Infrastructures.Events;
using UnityEngine;

namespace Dashboards
{
    public class DashboardController : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_nervesContainer;
        [SerializeField]
        private GameObject m_actionContainer;
        [SerializeField]
        private GameObject m_keyContainer;

        private NervesController[] m_nerves;
        private ActionController[] m_actions;
        private KeyController[] m_keys;

        private HashSet<KeyCode> m_pressedKeyCodes = new HashSet<KeyCode>();

        private void Awake()
        {
            m_nerves = m_nervesContainer.GetComponentsInChildren<NervesController>();
            m_actions = m_actionContainer.GetComponentsInChildren<ActionController>();
            m_keys = m_keyContainer.GetComponentsInChildren<KeyController>();
        }


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

            foreach (var keyController in m_keys)
            {
                keyController.Trigger(m_pressedKeyCodes);
            }

            var action = ActionType.None;
            foreach (var cell in m_actions.Where(e => e.IsPowerUp))
            {
                action |= cell.ActionType;
            }

            EventEmitter.Emit(GameEvent.Action, new ActionEvent(action));
        }
    }
}