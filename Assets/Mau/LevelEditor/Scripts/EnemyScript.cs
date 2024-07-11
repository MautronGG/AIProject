using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
  [SerializeField] private float m_speed = 5;
  [SerializeField] private float m_timeToDeath = 5;
  private bool m_flipped;
  private bool m_grounded;
  private bool m_isActive;
  private Vector3 m_desiredMovement;
  Rigidbody m_Rigidbody;
  void Start()
  {
    //Establecer los par�metros iniciales
    m_Rigidbody = GetComponent<Rigidbody>();
    m_flipped = false;
    m_isActive = false;
    m_desiredMovement = Vector3.zero;
  }
  private void FixedUpdate()
  {
    if (m_isActive)
    {
      //Si no est� en el piso, que mantenga su momentum en Y pero que mantenga la velocidad constante en X.
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
  }
  private void OnCollisionEnter(Collision collision)
  {
    //Si es un muro u otra bola, cambiar la direcci�n del personaje.
    if (collision.gameObject.tag.Equals("Wall"))
    {
      FlipVelocity();
    }
    if (collision.gameObject.tag.Equals("Enemy"))
    {
      FlipVelocity();
    }
    //Si es un piso, determinar si se puede escalar o no.
    if (collision.gameObject.tag.Equals("Floor"))
    {
      m_grounded = true;
      //Determinar con qu� parte del piso est� chocando.
      float rotationAngle = collision.transform.rotation.eulerAngles.z;
      Vector3 hit = collision.contacts[0].normal;
      float angle = Vector3.Angle(hit, collision.transform.up);

      //Colisi�n con la parte de atr�s
      if (Mathf.Approximately(angle, 0))
      {
        //Si el piso se encuentra rotado a un �ngulo aceptable, se puede escalar, por lo que se ocupa el right vector del objeto para seguir con una velocidad constante.
        if ((rotationAngle <= 45f && rotationAngle >= 0) || rotationAngle >= 315)
        {
          //Si est� volteado, hacer el cambio correspondiente.
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
      //Colisi�n con la parte frontal (El lado por el que no se puede escalar)
      else if (Mathf.Approximately(angle, 180))
      {
        FlipVelocity();
      }
      //Colisi�n por los lados (No deber�a suceder)
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
      //Colisi�n con esquinas, no se puede escalar.
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
    private void FlipVelocity()
  {
    m_flipped = !m_flipped;
    m_desiredMovement.x = -m_desiredMovement.x;
    ///FlipSprite
  }
}