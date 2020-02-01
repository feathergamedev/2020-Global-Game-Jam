using System;
using System.Collections.Generic;
using System.Linq;
using Repair.Dashboard.Events;
using Repair.Dashboards.Helpers;
using Repair.Infrastructures.Events;
using UnityEngine;

namespace Repair.Dashboards
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
            RotationHelper.I.Initialize();
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

            if (Input.GetKeyUp(KeyCode.R))
            {
                m_pressedKeyCodes.Remove(KeyCode.R);
            }

            if (Input.GetKeyUp(KeyCode.T))
            {
                m_pressedKeyCodes.Remove(KeyCode.T);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                m_pressedKeyCodes.Add(KeyCode.Z);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                m_pressedKeyCodes.Add(KeyCode.X);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                m_pressedKeyCodes.Add(KeyCode.R);
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                m_pressedKeyCodes.Add(KeyCode.T);
            }

            foreach (var keyController in m_keys)
            {
                keyController.Clear();
            }

            foreach (var keyController in m_keys.Where(e => m_pressedKeyCodes.Contains(e.KeyCode)))
            {
                keyController.Trigger();
            }

            var action = ActionType.None;
            foreach (var cell in m_actions.Where(e => e.IsPowerUp))
            {
                action |= cell.ActionType;
            }

            if (m_pressedKeyCodes.Contains(KeyCode.R) && m_pressedKeyCodes.Contains(KeyCode.T))
            {
                EventEmitter.Emit(GameEvent.NerversRotation, new RotationEvent(RotationStatus.None));
            }
            else if (m_pressedKeyCodes.Contains(KeyCode.R))
            {
                EventEmitter.Emit(GameEvent.NerversRotation, new RotationEvent(RotationStatus.Left));
            }
            else if (m_pressedKeyCodes.Contains(KeyCode.T))
            {
                EventEmitter.Emit(GameEvent.NerversRotation, new RotationEvent(RotationStatus.Right));
            }
            else
            {
                EventEmitter.Emit(GameEvent.NerversRotation, new RotationEvent(RotationStatus.None));
            }

            EventEmitter.Emit(GameEvent.Action, new ActionEvent(action));
        }
    }
}