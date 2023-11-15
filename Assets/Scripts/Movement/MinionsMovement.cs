using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsMovement : MonoBehaviour
{
  public Vector3 m_defaultPosition;
  public float m_moveSpeed = 4.0f;
  public float m_speed;
  public bool m_reachedGoal = false;
  public Rigidbody rig;
  public bool m_isGrounded;
  public bool m_canMove = false;
  public Vector3 m_initialVelocity;
  public Vector2 m_moveVector;
  private CharacterController m_controller;
  public float m_verticalVelocity;
  public float m_groundedOffset = -0.14f;
  public float m_groundedRadius = 0.5f;
  public LayerMask m_groundLayers;
  public float m_gravity = -15.0f;

  public float m_speedChangeRate = 10.0f;
  [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
  public float m_fallTimeout = 0.15f;
  public float m_fallTimeoutDelta;

  private float m_terminalVelocity = 53.0f;
  public LevelEditorManager m_levelEditorManager;
  // Start is called before the first frame update
  void Start()
  {
    transform.Rotate(new Vector3(1f, 0f, 0f));
    rig = GetComponent<Rigidbody>();
    m_controller = GetComponent<CharacterController>();
    m_fallTimeoutDelta = m_fallTimeout;
    m_levelEditorManager = FindObjectOfType<LevelEditorManager>();
  }

  // Update is called once per frame
  void Update()
  {
    if (transform.position.y <= -4f)
    {
      this.gameObject.SetActive(false);
    }
    if (m_canMove)
    {
      m_moveSpeed = 2.7f;
      //rig.velocity = new Vector3(m_speed,m_speed,m_speed);
      m_moveVector = new Vector2(1f, 0f);
    }
    else
    {
      m_moveSpeed = 0;
      m_moveVector = Vector2.zero;
    }
    if (m_reachedGoal)
    {
      m_canMove = false;
      //SoundEffect
      //AddPoints
      //FinishLevel
    }
    Move();
    GroundedCheck();
    Gravity();
  }
  public void SetMovement()
  {
    m_canMove = true;
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
    float currentHorizontalSpeed = new Vector3(m_controller.velocity.x, 0.0f, m_controller.velocity.z).magnitude;

    float speedOffset = 0.1f;

    // accelerate or decelerate to target speed
    if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
    {
      // creates curved result rather than a linear one giving a more organic speed change
      // note T in Lerp is clamped, so we don't need to clamp our speed
      m_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * m_speedChangeRate);

      // round speed to 3 decimal places
      m_speed = Mathf.Round(m_speed * 1000f) / 1000f;
    }
    else
    {
      m_speed = targetSpeed;
    }

    Vector3 moveDirection = new Vector3(m_moveVector.x, 0.0f, 0f).normalized;
    if (m_moveVector != Vector2.zero)
    {

      // move
      moveDirection = transform.right * m_moveVector.x;
    }

    // move the player
    m_controller.Move(moveDirection.normalized * (m_speed * Time.deltaTime) + new Vector3(0.0f, m_verticalVelocity, 0.0f) * Time.deltaTime);
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
      if (m_verticalVelocity < 0.0f)
      {
        m_verticalVelocity = -2f;
      }


    }
    else
    {
      // fall timeout
      if (m_fallTimeoutDelta >= 0.0f)
      {
        m_fallTimeoutDelta -= Time.deltaTime;
      }

    }

    // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
    if (m_verticalVelocity < m_terminalVelocity)
    {
      m_verticalVelocity += m_gravity * Time.deltaTime;
    }
  }
  private void OnDisable()
  {
    m_levelEditorManager.m_finishedBoids++;
  }
  private void OnTriggerEnter(Collider other)
  {
    if (other.transform.tag == "Destiny")
    {
      m_reachedGoal = true;
      m_levelEditorManager.m_reachedGoals++;
      this.gameObject.SetActive(false);
    }
  }

  public void ResetDefaults()
  {
    if (!gameObject.activeInHierarchy)
    {
      gameObject.SetActive(true);
    }
    m_verticalVelocity = 0f;
    m_moveSpeed = 0;
    m_moveVector = Vector2.zero;
    m_canMove = false;
    m_reachedGoal = false;
    m_controller.enabled = false;
    transform.position = m_defaultPosition;
    m_controller.enabled = true;
    
  }
  public void PortalTrigger(Vector3 newPos)
  {
    m_controller.enabled = false;
    transform.position = newPos;
    m_controller.enabled = true;
  }

  //private void OnCollisionEnter2D(Collision2D collision)
  //{
  //  if (collision.transform.tag == "Floor")
  //  {
  //    m_isGrounded = true;
  //  }
  //}
  //private void OnCollisionExit2D(Collision2D collision)
  //{
  //  if (collision.transform.tag == "Floor")
  //  {
  //    m_isGrounded = false;
  //  }
  //}
}
