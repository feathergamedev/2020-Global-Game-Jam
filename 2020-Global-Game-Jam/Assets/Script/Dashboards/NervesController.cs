using Repair.Dashboard.Events;
using Repair.Dashboards.Helpers;
using Repair.Infrastructures.Events;
using UnityEngine;

namespace Repair.Dashboards
{
    public class NervesController : BaseCellController
    {
        private Vector3 m_screenPoint;
        private Vector3 m_offset;
        private bool m_isDragging;
        private float z;



        void OnMouseDown()
        {
            m_isDragging = true;
            m_screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            m_offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_screenPoint.z));
        }

        void OnMouseDrag()
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + m_offset;
            transform.position = curPosition;
        }

        private void OnMouseUp()
        {
            m_isDragging = false;
        }

        protected override void RemoveLinkCell(BaseCellController cell)
        {
            m_closeCell.Clear();
            base.RemoveLinkCell(cell);
            CheckLinkedCells(this, IsPowerUp);
        }

        void FixedUpdate()
        {
            if (m_isDragging && RotationHelper.I.RotationStatus != RotationStatus.None)
            {

                z += Time.deltaTime * RotationHelper.I.RotationSpeed;

                if (z > 360.0f)
                {
                    z = 0.0f;
                }

                transform.localRotation = Quaternion.Euler(0, 0, z);
            }
        }

    }
}