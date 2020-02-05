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
#if UNITY_WEBGL || UNITY_EDITOR || UNITY_STANDALONE
            EventEmitter.Add(GameEvent.NerversDraging, OnNeverDragging);
#endif
            gameObject.SetActive(!IsRotationKey);
        }

        private void OnDestroy()
        {
#if UNITY_WEBGL || UNITY_EDITOR || UNITY_STANDALONE
            EventEmitter.Remove(GameEvent.NerversDraging, OnNeverDragging);
#endif
        }

        private void OnNeverDragging(IEvent @event)
        {
#if UNITY_WEBGL || UNITY_EDITOR || UNITY_STANDALONE
            if (!IsRotationKey)
            {
                return;
            }

            var isDragging = (@event as BoolEvent).Value;
            gameObject.SetActive(isDragging);
#endif
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