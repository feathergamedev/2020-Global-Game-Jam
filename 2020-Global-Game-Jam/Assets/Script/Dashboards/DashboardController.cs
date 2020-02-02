using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Repair.Dashboard.Events;
using Repair.Dashboards.Helpers;
using Repair.Dashboards.Settings;
using Repair.Infrastructures.Events;
using UnityEngine;
using Random = UnityEngine.Random;

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


        private HashSet<BaseCellController> m_closeCells = new HashSet<BaseCellController>();
        private HashSet<BaseCellController> m_openCells = new HashSet<BaseCellController>();
        private HashSet<BaseCellController> m_allCells = new HashSet<BaseCellController>();

        private HashSet<int> m_powerUpGroups = new HashSet<int>();

        private Dictionary<int, HashSet<BaseCellController>> m_linkGroup = new Dictionary<int, HashSet<BaseCellController>>();


        private void Awake()
        {
            RotationHelper.I.Initialize();
            m_actions = m_actionContainer.GetComponentsInChildren<ActionController>();
            m_keys = m_keyContainer.GetComponentsInChildren<KeyController>();

            foreach (var cell in m_keys)
            {
                m_allCells.Add(cell);
            }

            m_nerves = new NerveController[m_nerveCount];
            for (var i = 0; i < m_nerveCount; i++)
            {
                var nerve = Instantiate(m_nerverSettings.GetRandomNerve(), m_nerveContainer);
                nerve.transform.localPosition = new Vector3(
                    Random.Range(m_nerverSettings.MinInitX, m_nerverSettings.MaxInitX), 0, 0);

                nerve.SetInitRotation(Random.Range(0f, m_nerverSettings.InitRotationRange));
                m_nerves[i] = nerve;
                m_allCells.Add(nerve);
            }

            foreach (var cell in m_actions)
            {
                m_allCells.Add(cell);
            }

            EventEmitter.Add(GameEvent.Killed, OnKilled);
        }

        private void OnDestroy()
        {
            EventEmitter.Remove(GameEvent.Killed, OnKilled);
        }

        private void ResetList()
        {
            m_openCells = new HashSet<BaseCellController>(m_allCells);
            m_closeCells.Clear();
            m_linkGroup.Clear();
            m_powerUpGroups.Clear();
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

            if (Input.GetKeyUp(KeyCode.A))
            {
                m_pressedKeyCodes.Remove(KeyCode.A);
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                m_pressedKeyCodes.Remove(KeyCode.D);
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

            if (Input.GetKeyDown(KeyCode.A))
            {
                m_pressedKeyCodes.Add(KeyCode.A);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                m_pressedKeyCodes.Add(KeyCode.D);
            }

            ResetList();

            foreach (var cell in m_allCells)
            {
                cell.Clear();
            }

            var linkGroup = 0;
            foreach (var keyController in m_keys)
            {
                m_linkGroup.Add(linkGroup, new HashSet<BaseCellController>());
                CheckLinkedCells(keyController, linkGroup);
                linkGroup++;
            }

            SetLinkedCells();


            foreach (var keyController in m_keys.Where(e => m_pressedKeyCodes.Contains(e.KeyCode)))
            {
                m_powerUpGroups.Add(keyController.LinkGroup);
                foreach (var cell in m_linkGroup[keyController.LinkGroup])
                {
                    cell.IsPowerUp = true;
                }
            }

            foreach (var pair in m_linkGroup)
            {
                if (!m_powerUpGroups.Contains(pair.Key))
                {
                    foreach (var cell in m_linkGroup[pair.Key])
                    {
                        cell.IsPowerUp = false;
                    }
                }
            }

            var action = ActionType.None;
            foreach (var cell in m_actions.Where(e => e.IsPowerUp))
            {
                action |= cell.ActionType;
            }

            if (m_pressedKeyCodes.Contains(KeyCode.A) && m_pressedKeyCodes.Contains(KeyCode.D))
            {
                EventEmitter.Emit(GameEvent.NerversRotation, new RotationEvent(RotationStatus.None));
            }
            else if (m_pressedKeyCodes.Contains(KeyCode.A))
            {
                EventEmitter.Emit(GameEvent.NerversRotation, new RotationEvent(RotationStatus.Left));
            }
            else if (m_pressedKeyCodes.Contains(KeyCode.D))
            {
                EventEmitter.Emit(GameEvent.NerversRotation, new RotationEvent(RotationStatus.Right));
            }
            else
            {
                EventEmitter.Emit(GameEvent.NerversRotation, new RotationEvent(RotationStatus.None));
            }

            EventEmitter.Emit(GameEvent.Action, new ActionEvent(action));
        }

        protected void CheckLinkedCells(BaseCellController baseCell, int group)
        {
            if (m_closeCells.Add(baseCell))
            {
                baseCell.LinkGroup = group;
                m_linkGroup[group].Add(baseCell);
                foreach (var cell in baseCell.LinkedCells)
                {
                    CheckLinkedCells(cell, group);
                }
            }
        }

        protected void SetLinkedCells()
        {
            foreach (var pair in m_linkGroup)
            {

                if (pair.Value.Any(e => e is ActionController) && pair.Value.Any(e => e is KeyController))
                {
                    foreach (var cells in pair.Value)
                    {
                        cells.IsLinked = true;
                    }
                }
                else
                {
                    foreach (var cells in pair.Value)
                    {
                        cells.IsLinked = false;
                    }
                }
            }
        }

        protected void OnKilled(IEvent @event)
        {
            const float r = 2000f;
            const float duration = 0.8f;
            var ao = 360f / m_nerves.Count();
            var idx = 0;
            var startAo = Random.Range(0, ao);
            foreach (var nerve in m_nerves)
            {
                if (nerve == null)
                {
                    continue;
                }

                var x = r * Math.Cos((startAo + ao * Math.PI * idx) / 180);
                var y = r * Math.Sin((ao * Math.PI * idx) / 180);
                var rotate = Random.Range(0, 720);

                nerve.transform.DOLocalMove(new Vector3((float)x, (float)y), duration);
                nerve.transform.DOLocalRotate(new Vector3(0, 0, rotate), duration);

                idx++;
            }
        }
    }
}