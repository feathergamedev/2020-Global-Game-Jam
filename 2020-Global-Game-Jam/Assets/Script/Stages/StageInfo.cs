using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo : MonoBehaviour
{

    [SerializeField]
    private Vector3 m_initPlayerPos;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindWithTag("Player").transform.position = m_initPlayerPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
