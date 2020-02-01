using System.Collections;
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
                Debug.Log("z up");
            }

            if (Input.GetKeyUp(KeyCode.X))
            {
                m_pressedKeyCodes.Remove(KeyCode.X);
                Debug.Log("x up");
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                m_pressedKeyCodes.Add(KeyCode.Z);
                Debug.Log("z was pressed");
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                m_pressedKeyCodes.Add(KeyCode.X);
                Debug.Log("x was pressed");
            }

            foreach (var keyController in m_keyControllers)
            {
                keyController.Trigger(m_pressedKeyCodes);
            }

            EventEmitter.Emit(m_actions.Select(action => action.IsPowerUp));
        }
    }
}