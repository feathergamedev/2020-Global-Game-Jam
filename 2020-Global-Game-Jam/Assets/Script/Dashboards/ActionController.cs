using Repair.Infrastructures.Events;
using UnityEngine;

namespace Dashboards
{
    public class ActionController : BaseCellController
    {
        [SerializeField]
        private ActionType m_actionType;
        public ActionType ActionType => m_actionType;
    }
}