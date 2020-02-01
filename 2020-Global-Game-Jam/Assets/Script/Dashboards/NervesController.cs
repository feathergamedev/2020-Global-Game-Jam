﻿using System;
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
        private float m_z, m_rotationSpeed;
        private RotationStatus m_rotationStatus = RotationStatus.None;

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
                if (m_rotationStatus != RotationHelper.I.RotationStatus)
                {
                    m_rotationStatus = RotationHelper.I.RotationStatus;
                    m_rotationSpeed = RotationHelper.I.RotationStatus == RotationStatus.Left ? RotationHelper.MIN_SPEED : -RotationHelper.MIN_SPEED;
                }

                if (RotationHelper.I.RotationStatus == RotationStatus.Left && m_rotationSpeed >= RotationHelper.MAX_SPEED)
                {
                    m_rotationSpeed = RotationHelper.MAX_SPEED;
                }
                else if (RotationHelper.I.RotationStatus == RotationStatus.Right && m_rotationSpeed <= -RotationHelper.MAX_SPEED)
                {
                    m_rotationSpeed = -RotationHelper.MAX_SPEED;
                }
                else
                {
                    if (RotationHelper.I.RotationStatus == RotationStatus.Left)
                    {
                        m_rotationSpeed += RotationHelper.INTERVAL_SPEED;
                    }
                    else
                    {
                        m_rotationSpeed -= RotationHelper.INTERVAL_SPEED;
                    }
                }

                m_z += Time.deltaTime * m_rotationSpeed;

                if (m_z > 360.0f)
                {
                    m_z = 0.0f;
                }

                transform.localRotation = Quaternion.Euler(0, 0, m_z);
            }
            else
            {
                m_rotationStatus = RotationStatus.None;
                m_rotationSpeed = 0;
            }
        }

    }
}