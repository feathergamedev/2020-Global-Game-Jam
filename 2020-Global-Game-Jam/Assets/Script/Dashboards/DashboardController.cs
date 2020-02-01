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


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
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

            EventEmitter.Emit(GameEvent.Action,
                new ListEvent(m_actions.Where(e => e.IsPowerUp).Select(e => e.ActionType)));
        }
    }
}