using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class SphereScript : MonoBehaviour
{

    [SerializeField] private float m_speed = 5;
    [SerializeField] private float m_timeToDeath = 5;
    private bool m_flipped;
    private bool m_grounded;
    private bool m_canKillEnemy;
    public bool m_isActive;
    private Vector3 m_desiredMovement;
    Rigidbody m_Rigidbody;
    LevelManager m_levelManager;
    Vector3 m_defaultPosition;
    Quaternion m_defaultRotation;
    // Start is called before the first frame update
    void Start()
    {
        //Establecer los parámetros iniciales
        m_Rigidbody = GetComponent<Rigidbody>();
        m_flipped = false;
        m_canKillEnemy = false;
        m_isActive = false;
        m_desiredMovement = Vector3.zero;
        m_levelManager = FindAnyObjectByType<LevelManager>();
        m_defaultPosition = GetComponent<Transform>().position;
        m_defaultRotation = GetComponent<Transform>().rotation;
    }
    private void FixedUpdate()
    {
        if (m_isActive)
        {
            //Si no está en el piso, que mantenga su momentum en Y pero que mantenga la velocidad constante en X.
            if (!m_grounded)
            {
                float desiredSpeed = m_speed;
                if (m_flipped)
                {
                    desiredSpeed = -m_speed;
                }
                m_desiredMovement = new Vector3(desiredSpeed, m_Rigidbody.velocity.y, m_Rigidbody.velocity.z);
            }

            //Establecer la velocidad esperada.
            m_Rigidbody.velocity = m_desiredMovement;

        }
        else
        {
            m_Rigidbody.velocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Si es un muro u otra bola, cambiar la dirección del personaje.
        if (collision.gameObject.tag.Equals("Wall"))
        {
            FlipVelocity();
        }
        if (collision.gameObject.tag.Equals("Player"))
        {
            FlipVelocity();
        }

        //Si es un enemigo, desactivar al jugador.
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            if (m_canKillEnemy)
            {
                collision.gameObject.SetActive(false);
            }
            else
            {
                m_isActive = false;
                ///Animation
                DeathCountdown(this.gameObject);
                //gameObject.SetActive(false);
            }
        }

        //Si es un piso, determinar si se puede escalar o no.
        if (collision.gameObject.tag.Equals("Floor"))
        {
            m_grounded = true;
            //Determinar con qué parte del piso está chocando.
            float rotationAngle = collision.transform.rotation.eulerAngles.z;
            Vector3 hit = collision.contacts[0].normal;
            float angle = Vector3.Angle(hit, collision.transform.up);

            //Colisión con la parte de atrás
            if (Mathf.Approximately(angle, 0))
            {
                //Si el piso se encuentra rotado a un ángulo aceptable, se puede escalar, por lo que se ocupa el right vector del objeto para seguir con una velocidad constante.
                if ((rotationAngle <= 45f && rotationAngle >= 0) || rotationAngle >= 315)
                {
                    //Si está volteado, hacer el cambio correspondiente.
                    float desiredSpeed = m_speed;
                    if (m_flipped)
                    {
                        desiredSpeed = -m_speed;
                    }
                    m_desiredMovement = collision.transform.right * desiredSpeed;
                }
                else
                {
                    FlipVelocity();
                }
            }
            //Colisión con la parte frontal (El lado por el que no se puede escalar)
            else if (Mathf.Approximately(angle, 180))
            {
                FlipVelocity();
            }
            //Colisión por los lados (No debería suceder)
            else if (Mathf.Approximately(angle, 90))
            {
                //Vector3 cross = Vector3.Cross(Vector3.forward, hit);
                //if (cross.y > 0) //Derecha
                //{
                //}
                //else //Izquierda
                //{
                //}
                FlipVelocity();
            }
            //Colisión con esquinas, no se puede escalar.
            else
            {
                FlipVelocity();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Si se separa del piso, marcar la bandera correspondiente.
        if (collision.transform.tag.Equals("Floor"))
        {
            m_grounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Si se encuentra con un resorte, obtener su fuerza y aplicarla a la esfera.
        if (other.transform.tag.Equals("Spring"))
        {
            m_grounded = false;
            float springForce = other.transform.GetComponent<SpringScript>().SpringForce;
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, springForce, m_Rigidbody.velocity.z);
        }
        if (other.transform.tag.Equals("Destiny"))
        {
            m_isActive = false;
            m_levelManager.m_reachedGoals++;
            m_levelManager.m_playerEnded++;
        }
        //add Enemy
    }

    //Función para invertir la velocidad de la esfera en X.
    private void FlipVelocity()
    {
        m_flipped = !m_flipped;
        m_desiredMovement.x = -m_desiredMovement.x;
        ///FlipSprite
    }

    //Llamar a esta función para activar el movimiento.
    public void EnableMovement(bool state)
    {
        m_isActive = state;
    }

    //Timer para que la esfera sea desactivada. Se puede configurar con el tiempo deseado.
    IEnumerator DeathCountdown(GameObject obj)
    {
        float counter = m_timeToDeath;
        while (counter > 0)
        {
            counter -= Time.deltaTime;
            yield return null;
        }
        obj.SetActive(false);
    }
    public void ResetTransform()
    {
        m_Rigidbody.angularVelocity = Vector3.zero;
        transform.position = m_defaultPosition;
        transform.rotation = m_defaultRotation;
    }
}
