using System.Collections.Generic;
using Repair.Dashboard.Events;
using Repair.Infrastructures.Events;
using UnityEngine;

namespace Repair.Dashboards
{
    public class KeyController : BaseCellController
    {
        [SerializeField]
        private KeyCode m_keyCode;

        public KeyCode KeyCode => m_keyCode;
        private bool IsRotationKey
        {
            get
            {
                return KeyCode == KeyCode.A || KeyCode == KeyCode.D;
            }
        }

        private void Start()
        {
            EventEmitter.Add(GameEvent.NerversDraging, OnNeverDragging);
            gameObject.SetActive(!IsRotationKey);
        }

        private void OnDestroy()
        {
            EventEmitter.Remove(GameEvent.NerversDraging, OnNeverDragging);
        }

        private void OnNeverDragging(IEvent @event)
        {
            if (!IsRotationKey)
            {
                return;
            }

            var isDragging = (@event as BoolEvent).Value;
            gameObject.SetActive(isDragging);
        }

        public void Trigger()
        {
            CheckPoweredCells(this, true);
        }
    }
}