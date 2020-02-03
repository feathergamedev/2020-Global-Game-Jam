using Repair.Infrastructures.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Repair.Dashboards
{
    public class KeyController : BaseCellController, IPointerDownHandler, IPointerUpHandler
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

        public void OnPointerDown(PointerEventData eventData)
        {
            EventEmitter.Emit(GameEvent.KeyPressed, new KeyCodeEvent(KeyCode));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            EventEmitter.Emit(GameEvent.KeyUp, new KeyCodeEvent(KeyCode));
        }
    }
}