using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsMovement : MonoBehaviour
{
  public bool m_portaled = false;
  public float m_portalTime = 0f;
  Vector3 m_newMove = new Vector2(1f, 0f);
  public bool m_bomb = false;
  public float m_bombTimer = 0f;
  public Vector3 m_defaultPosition;
  public float m_moveSpeed = 4.0f;
  public float m_speed;
  public bool m_reachedGoal = false;
  public Rigidbody rig;
  public bool m_isGrounded;
  public bool m_canMove = false;
  public Vector3 m_initialVelocity;
  public Vector2 m_moveVector;
  public CharacterController m_controller;
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

  [Header("FollowPath")]
  public List<Transform> m_pathList;
  public int m_pathIndex = 0;
  public int m_pathIndexNext = 0;
  public int m_pathIndexPrev = 0;
  public bool m_isFirstPoint = true;
  public float m_followPathImpetu;
  public float m_pathRatio;
  public float m_pathArriveRatio;
  public bool m_isFollowPath;

  [Header("Seek")]
  public Vector3 m_target;
  public float m_seekImpetu;
  public Transform m_seekTarget;
  public float m_seekRatio;
  public bool m_seekByRatio = false;

  public Vector3 m_Force;
  public Vector3 m_moveForce = Vector3.zero;
  public Vector3 m_currentForce = Vector3.zero;
  public float m_mass;
  // Start is called before the first frame update
  void Start()
  {
    transform.Rotate(new Vector3(1f, 0f, 0f));
    rig = GetComponent<Rigidbody>();
    m_controller = GetComponent<CharacterController>();
    m_fallTimeoutDelta = m_fallTimeout;
    m_levelEditorManager = FindObjectOfType<LevelEditorManager>();
    m_defaultPosition = transform.position;
  }

  // Update is called once per frame
  void Update()
  {
    if (m_bomb)
    {
      m_bombTimer += Time.deltaTime;
      if (m_bombTimer >= 0.5f)
      {
        m_bombTimer = 0f;
        m_bomb = false;
      }
    }
    if (m_portaled)
    {
      m_portalTime += Time.deltaTime;
      if (m_portalTime >= 1.5f)
      {
        m_portalTime = 0f;
        m_portaled = false;
      }
    }
    if (transform.position.y <= -4f)
    {
      this.gameObject.SetActive(false);
    }
    else
    {
      m_moveSpeed = 0;
      m_moveVector = Vector2.zero;
    }
    if (m_canMove)
    {
      m_moveSpeed = 2.7f;
      //rig.velocity = new Vector3(m_speed,m_speed,m_speed);
      m_moveVector = m_newMove;
    }
    if (m_reachedGoal)
    {
      m_canMove = false;
      //SoundEffect
      //AddPoints
      //FinishLevel
    }
    if (m_isFollowPath)
    {
      m_Force = Vector3.zero;
      m_Force += FollowPath(m_pathList, m_pathRatio, m_pathArriveRatio, m_followPathImpetu);
      m_Force += Inertia(m_currentForce, m_Force, m_mass);
      m_moveForce = Truncar(m_Force, m_speed);
      //transform.forward = m_moveForce.normalized;
      transform.position += m_moveForce * Time.deltaTime;
    }
    else
    {
      //m_controller.enabled = true;
      Move();
      GroundedCheck();
      Gravity();
    }
    
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
      if (m_verticalVelocity < -10f)
      {
        m_verticalVelocity = -10f;
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
    if (other.transform.tag == "BombWall" && !m_bomb)
    {
      ChangeDirection();
    }
    if (other.transform.tag == "Wall")
    {
      ChangeDirection();
    }
  }
  public void ChangeDirection()
  {
    m_newMove *= -1;
  }

  public void ResetDefaults()
  {
    if (!gameObject.activeInHierarchy)
    {
      gameObject.SetActive(true);
    }
    m_isFollowPath = false;
    m_verticalVelocity = 0f;
    m_moveSpeed = 0;
    m_moveVector = Vector2.zero;
    m_newMove = new Vector2(1, 0);
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

  Vector3 FollowPath(List<Transform> path, float pathRatio, float arriveRatio, float impetu)
  {
    Vector3 nextPoint = path[m_pathIndexNext].position;
    Vector3 prevPoint = path[m_pathIndexPrev].position;

    var v1 = nextPoint - transform.position;

    if (v1.magnitude <= arriveRatio)
    {
      m_pathIndexNext++;
      if (m_pathIndexNext > path.Count - 1)
      {
        //m_pathIndexNext = 0;
        m_isFollowPath = false;
        m_controller.enabled = true;
        return Vector3.zero;
      }
      m_pathIndexPrev = m_pathIndexNext - 1;
      if (m_pathIndexPrev < 0)
      {
        m_pathIndexPrev = path.Count - 1;
      }
      nextPoint = path[m_pathIndexNext].position;
      prevPoint = path[m_pathIndexPrev].position;
    }

    var v2 = prevPoint - transform.position;
    var v3 = prevPoint - nextPoint;
    var dir = nextPoint - prevPoint;

    var R = Vector3.Dot(v2.normalized, v3.normalized) / v3.magnitude;
    Vector3 PP = dir * R + prevPoint;

    var v4 = PP - transform.position;
    Vector3 F = Vector3.zero;
    if (v4.magnitude > pathRatio)
    {
      F += Seek(PP, m_followPathImpetu);
    }

    F += Seek(nextPoint, m_followPathImpetu);

    return F;
  }
  Vector3 Seek(Vector3 Target, float Impetu)
  {
    Vector3 Force = Target - transform.position;
    if (m_seekByRatio)
    {
      if (Force.magnitude <= m_seekRatio)
      {
        Force = Force.normalized;
        Force *= Impetu;
        return Force;
      }
      else
      {
        return Vector3.zero;
      }
    }
    else
    {
      Force = Force.normalized;
      Force *= Impetu;
      return Force;
    }
    //float magnitude = Force.magnitude;
  }
  Vector3 Inertia(Vector3 currentForce, Vector3 newForce, float mass)
  {
    Vector3 direction = (currentForce * mass) + (newForce * (1 - mass));
    return direction;
  }
  Vector3 Truncar(Vector3 force, float speed)
  {
    Vector3 F = force.normalized;
    F *= speed;
    return F;
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
