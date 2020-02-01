﻿using UnityEngine;

namespace Dashboards
{
    public class NervesController : BaseCellController
    {
        private Vector3 m_screenPoint;
        private Vector3 m_offset;

        void OnMouseDown()
        {
            m_screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            m_offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_screenPoint.z));
        }

        void OnMouseDrag()
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + m_offset;
            transform.position = curPosition;
        }

        protected override void RemoveLinkCell(BaseCellController cell)
        {
            m_closeCell.Clear();
            base.RemoveLinkCell(cell);
            CheckLinkedCells(this, IsPowerUp);
        }
    }
}