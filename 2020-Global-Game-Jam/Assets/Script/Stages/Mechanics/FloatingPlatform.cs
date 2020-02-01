using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    [SerializeField]
    private GameObject m_platform;

    [SerializeField]
    private float m_moveSpeed;

    [SerializeField]
    private float m_curMoveDirection;

    [SerializeField]
    private Transform m_boundaryL, m_boundaryR;

    private 

    // Start is called before the first frame update
    void Start()
    {
        m_curMoveDirection = m_moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_platform.transform.position.x <= m_boundaryL.position.x)
        {
            m_curMoveDirection = m_moveSpeed;
        }
        else if(m_platform.transform.position.x >= m_boundaryR.position.x)
        {
            m_curMoveDirection = -m_moveSpeed;
        }
    }

    private void FixedUpdate()
    {
        m_platform.transform.position += new Vector3(m_curMoveDirection * Time.fixedDeltaTime, 0);
    }
}
