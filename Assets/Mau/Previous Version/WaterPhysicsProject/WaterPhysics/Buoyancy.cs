using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Buoyancy : MonoBehaviour
{
  [Header("Buoyancy")]
  [SerializeField] bool m_kgM;
  [SerializeField] public float m_mass;
  [SerializeField] public float m_volume;
  [SerializeField] public float m_density;
  [SerializeField] public float m_waterDensity = 1000;
  [SerializeField] float m_densitiesDifference;
  [SerializeField] float m_force;
  [SerializeField] bool m_isSpherical;
  [SerializeField] Water m_water;
  [SerializeField] float m_radius;
  [SerializeField] bool m_onWater;
  [SerializeField] bool m_firstTime;
  [SerializeField] float m_buoyForce;
  [SerializeField] bool m_arquimedes;
  [SerializeField] bool m_resetVelocity;

  [Header("Movement")]
  [SerializeField] float m_speed;
  [SerializeField] float m_moveSpeed = 4.0f;
  [SerializeField] float m_speedChangeRate = 10.0f;
  [SerializeField] Vector2 m_moveVector;
  [SerializeField] float m_verticalVelocity;
  //[SerializeField] CharacterController m_controller;

  [Header("Ground")]
  [SerializeField] bool m_isGrounded;
  [SerializeField] float m_groundedOffset = -0.14f;
  [SerializeField] float m_groundedRadius = 0.5f;
  [SerializeField] LayerMask m_groundLayers;

  [Header("Falling")]
  [SerializeField] float m_fallTimeoutDelta;
  [SerializeField] float m_fallTimeout = 0.15f;
  [SerializeField] float m_terminalVelocity = 53.0f;
  [SerializeField] float m_gravity = -9.8f;
  [SerializeField] float m_secondGravity = -9.8f;

  public TextMeshProUGUI m_densityText;
  public TextMeshProUGUI m_velocityText;
  private void Start()
  {
    m_force = m_gravity;
    m_waterDensity = m_water.m_density;
  }
  private void Update()
  {
    //OnWater(m_water.gameObject);
    Gravity();
    Move();
    GroundedCheck();
  }
  public void Move()
  {
    // set target speed based on move speed, sprint speed and if sprint is pressed
    float targetSpeed = m_moveSpeed;

    // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

    // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
    // if there is no input, set the target speed to 0
    if (m_moveVector == Vector2.zero) targetSpeed = 0.0f;

    // a reference to the players current horizontal velocity
    //float currentHorizontalSpeed = new Vector3(m_controller.velocity.x, 0.0f, m_controller.velocity.z).magnitude;

    float speedOffset = 0.1f;

    // accelerate or decelerate to target speed
    //if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
    //{
    //  // creates curved result rather than a linear one giving a more organic speed change
    //  // note T in Lerp is clamped, so we don't need to clamp our speed
    //  m_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * m_speedChangeRate);
    //
    //  // round speed to 3 decimal places
    //  m_speed = Mathf.Round(m_speed * 1000f) / 1000f;
    //}
    //else
    //{
    //  m_speed = targetSpeed;
    //}
    //
    //Vector3 moveDirection = new Vector3(m_moveVector.x, 0.0f, 0f).normalized;
    //if (m_moveVector != Vector2.zero)
    //{
    //
    //  // move
    //  moveDirection = transform.right * m_moveVector.x;
    //}

    // move the player
    //m_controller.Move(moveDirection.normalized * (m_speed * Time.deltaTime) + new Vector3(0.0f, m_verticalVelocity, 0.0f) * Time.deltaTime);
    transform.position += new Vector3(0.0f, m_verticalVelocity, 0.0f) * Time.deltaTime;
    //transform.position += new Vector3(0f, m_gravity, 0f) * Time.deltaTime;
  }
  private void GroundedCheck()
  {
    // set sphere position, with offset
    Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - m_groundedOffset, transform.position.z);
    m_isGrounded = Physics.CheckSphere(spherePosition, m_groundedRadius, m_groundLayers, QueryTriggerInteraction.Ignore);
  }
  private void Gravity()
  {
    if (m_isGrounded)
    {
      // reset the fall timeout timer
      m_fallTimeoutDelta = m_fallTimeout;

      // stop our velocity dropping infinitely when grounded
      if(m_force < 0)
      {
        m_verticalVelocity = 0f;

      }
      m_gravity = 0f;
    }
    else
    {
      // fall timeout
      if (m_fallTimeoutDelta >= 0.0f)
      {
        m_fallTimeoutDelta -= Time.deltaTime;
      }
      m_gravity = -9.81f;
    }

    // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
    if (m_onWater)
    {
      m_verticalVelocity += m_force * Time.deltaTime;
      if (m_arquimedes)
      {
        m_verticalVelocity += m_buoyForce * Time.deltaTime;
      }

        if (m_force < 0 && m_verticalVelocity < m_force)
      {
        m_verticalVelocity = m_force;
      }
      else if(m_force > 0 && m_verticalVelocity > m_force)
      {
        m_verticalVelocity = m_force;
      }
      if (m_force <0 && m_isGrounded)
      {
        m_verticalVelocity = 0f;
      }
    }
    else
    {
      m_verticalVelocity += m_gravity * Time.deltaTime;
      m_force -= Time.deltaTime;
      if (m_force < 0)
      {
        m_force = 0f;
      }
    }
  }

  public void CalculatePhysics()
  {
    if (m_resetVelocity)
    {
      m_verticalVelocity = 0f;
    }
    var mass = m_mass;
    var volume = m_volume;
    var waterDensity = m_waterDensity;
    //var distanceY = other.transform.position.y + other.GetComponent<Water>().m_radius;
    //Vector3 distance = other.transform.position - transform.position;
    if (m_kgM)
    {
      mass = m_mass / 1000;
      volume = m_volume / 1000000;
      waterDensity = m_waterDensity * 1000;
    }
    m_density = mass / volume;
    if(Mathf.Approximately(m_density, waterDensity))
    {
      m_density = waterDensity;
    }
    m_densitiesDifference = waterDensity - m_density;
    //if (m_densitiesDifference < 0 || m_densitiesDifference > 0)
    //
    // m_secondGravity = 9.81f;
    //
    if (m_densitiesDifference == 0)
    {
      m_secondGravity = 0;
    }

    m_secondGravity = 9.81f;
    var Cubicradius = (volume * 3) / (4 * 3.1416);
    var radius = System.Math.Pow(Cubicradius, (double)1 / 3);
    m_force = m_densitiesDifference * m_secondGravity * (4 / 3 * 3.1416f) * (float)Cubicradius;
    m_buoyForce = m_density * 9.81f * volume / 10;
    m_terminalVelocity = m_force;
    m_densityText.text = "Density : " + m_density + "Kg/m3";
    m_velocityText.text = "Velocity : " + m_force + "m/s";
    //var viscocity = (2 * m_densitiesDifference * m_gravity * (radius * radius)) / (9 * )
    //var velocity = (2/9) * (m_density - m_waterDensity)
  }
  private void OnTriggerEnter(Collider other)
  {

    if (other.transform.tag == "Water")
    {
      m_verticalVelocity = 0f;
      m_onWater = true;
      if (!m_firstTime)
      {
        m_firstTime = true;
        CalculatePhysics();
      }
    }
  }
  private void OnTriggerStay(Collider other)
  {
    
  }
  private void OnTriggerExit(Collider other)
  {
    m_onWater = false;
    m_secondGravity = -9.81f;
    //m_secondGravity = m_gravity - m_force;
  }

  //void OnWater(GameObject other)
  //{
  //  if (other.transform.tag == "Water")
  //  {
  //    var mass = m_mass;
  //    var volume = m_volume;
  //    var waterDensity = m_waterDensity;
  //    //var distanceY = other.transform.position.y + other.GetComponent<Water>().m_radius;
  //    Vector3 distance = other.transform.position - transform.position;
  //    if (distance.magnitude <= m_radius)
  //    {
  //      m_onWater = true;
  //      if (m_kgM)
  //      {
  //        mass = m_mass / 1000;
  //        volume = m_volume / 1000000;
  //        waterDensity = m_waterDensity * 1000;
  //      }
  //      m_density = mass / volume;
  //      m_densitiesDifference = m_density - waterDensity;
  //
  //      if (m_densitiesDifference < 0 || m_densitiesDifference > 0)
  //      {
  //        m_gravity = 9.81f;
  //      }
  //      if (m_densitiesDifference == 0)
  //      {
  //        m_gravity = 0;
  //      }
  //      if (m_isSpherical)
  //      {
  //        var Cubicradius = (volume * 3) / (4 * 3.1416);
  //        var radius = System.Math.Pow(Cubicradius, (double)1 / 3);
  //        m_force = m_densitiesDifference * m_gravity * (4 / 3 * 3.1416f) * (float)Cubicradius;
  //        //var viscocity = (2 * m_densitiesDifference * m_gravity * (radius * radius)) / (9 * )
  //        //var velocity = (2/9) * (m_density - m_waterDensity)
  //      }
  //    }
  //    else if(!m_isGrounded)
  //    {
  //      m_onWater = false;
  //      m_gravity = -9.81f;
  //    }
  //  }
  //}
  private void OnDrawGizmos()
  {
    Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - m_groundedOffset, transform.position.z);
    Color color1 = Color.white;
    color1.a = 0.5f;
    Gizmos.color = color1;
    Gizmos.DrawSphere(spherePosition, m_groundedRadius);
  }
}
