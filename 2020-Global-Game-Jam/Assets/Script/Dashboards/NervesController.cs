using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dashboards
{
    public class NervesController : BaseCellController
    {
        private Vector3 m_screenPoint;
        private Vector3 m_offset;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnMouseDown()
        {
            m_screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            m_offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_screenPoint.z));
        }

        private void OnMouseDrag()
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + m_offset;
            transform.position = curPosition;
        }
    }
}