using System.Collections.Generic;
using System.Linq;
using Repair.Dashboard.Events;
using Repair.Dashboards.Helpers;
using Repair.Dashboards.Settings;
using Repair.Infrastructures.Events;
using UnityEngine;

namespace Repair.Dashboards
{
    public class DashboardController : MonoBehaviour
    {
        [SerializeField]
        private Transform m_actionContainer;
        [SerializeField]
        private Transform m_keyContainer;
        [SerializeField]
        private Transform m_nerveContainer;
        [SerializeField]
        private NerveSettings m_nerverSettings;
        [SerializeField]
        private int m_nerveCount;

        private ActionController[] m_actions;
        private KeyController[] m_keys;
        private NerveController[] m_nerves;

        private HashSet<KeyCode> m_pressedKeyCodes = new HashSet<KeyCode>();

        private void Awake()
        {
            RotationHelper.I.Initialize();
            m_actions = m_actionContainer.GetComponentsInChildren<ActionController>();
            m_keys = m_keyContainer.GetComponentsInChildren<KeyController>();

            m_nerves = new NerveController[m_nerveCount];
            for (var i = 0; i < m_nerveCount; i++)
            {
                var nerve = Instantiate(m_nerverSettings.GetRandomNerve(), m_nerveContainer);
                nerve.transform.localPosition = new Vector3(
                    Random.Range(m_nerverSettings.MinInitX, m_nerverSettings.MaxInitX), 0, 0);

                nerve.SetInitRotation(Random.Range(0f, m_nerverSettings.InitRotationRange));
                m_nerves[i] = nerve;
            }
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

            if (Input.GetKeyUp(KeyCode.C))
            {
                m_pressedKeyCodes.Remove(KeyCode.C);
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

            if (Input.GetKeyDown(KeyCode.C))
            {
                m_pressedKeyCodes.Add(KeyCode.C);
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