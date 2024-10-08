using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carry_Key : Carriable
{
    int color;

    
    protected override void Start()
    {
        //base.Start();
        m_type = E_CARRY_TYPE.KEY;
    }


    protected override void Update()
    {
        base.Update();
    }
}
