using System.Collections;
using System.Collections.Generic;
using Repair.Infrastructures.Events;
using UnityEngine;

namespace Dashboards
{
    public class ActionController : BaseCellController
    {
        [SerializeField]
        private ActionType m_actionType;
    }
}