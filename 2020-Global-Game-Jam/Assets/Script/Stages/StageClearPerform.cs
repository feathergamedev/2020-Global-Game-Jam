using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClearPerform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Really really dirty code.
    public void RequestBoxClose()
    {
        var box = GameObject.FindWithTag("FinishPoint");
        var boxScript = box.GetComponent<FinishPoint>();
        boxScript.CloseBox();
    }

}
