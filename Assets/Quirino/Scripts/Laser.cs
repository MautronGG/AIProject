using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float Lifespam = 3.0f;
    private Vector3 m_Direction = Vector3.zero;
    private float m_Speed = 0.0f;

    public Vector3 direction
    {
        get { return m_Direction; }
        set { m_Direction = value; }
    }

    public float speed
    {
        get { return m_Speed; }
        set { m_Speed = value; }
    }


    void Start()
    {
        Destroy(gameObject, Lifespam);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += m_Direction * m_Speed * Time.deltaTime;
    }
}
