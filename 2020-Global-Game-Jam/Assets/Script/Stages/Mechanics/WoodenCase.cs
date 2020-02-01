using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenCase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Eliminated()
    {
        Destroy(this.gameObject);
    }
}
