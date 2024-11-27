using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTrap : Object_Parent
{

    //[SerializeField] private Vector3 m_Direction = Vector3.zero;
    [SerializeField] private float m_Speed = 0.0f;
    [SerializeField] private bool m_canShoot = false;
    [SerializeField] private float m_shootCooldown = 1.0f;
    [SerializeField] private GameObject bulletPrefab;

    private GameObject m_muzzle;
    private float m_shootTimer = 0.0f;




    // Start is called before the first frame update
    public override void Start()
    {
        m_muzzle = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (m_canShoot)
        {
            m_shootTimer += Time.deltaTime;
            if (m_shootTimer >= m_shootCooldown)
            {
                Shoot();
                m_shootTimer = 0.0f;
            }
        }
        else
        {
            m_shootTimer = 0f;
        }
        
    }
    public override void StartObject()
    {
        base.StartObject();
        m_canShoot = true;
    }
    void Shoot()
    {
        //var dir = m_muzzle.transform.rotation * Vector3.forward;
        var bullet = Instantiate(bulletPrefab, m_muzzle.transform);
        var laser = bullet.GetComponent<Laser>();

        var scale = laser.transform.localScale;
        //if (dir == Vector3.zero)
        //{
            var dir = new Vector3(1.0f, 0.0f, 0.0f);
       // }

        laser.transform.localScale = new Vector3(scale.x * -1.0f, scale.y, scale.z);
        laser.direction = dir;
        laser.speed = m_Speed;
    }
}
