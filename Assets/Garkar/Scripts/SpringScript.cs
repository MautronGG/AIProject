using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringScript : ItemScript
{
    [SerializeField] private float m_springForce;
    public float SpringForce { get { return m_springForce; } }
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void OnMouseOver()
    {
        base.OnMouseOver();
    }
    public override void ResetDeafualts()
    {
        base.ResetDeafualts();
    }
    public override void SaveDefaults()
    {
        base.SaveDefaults();
    }
}
