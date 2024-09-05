using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionMovement : MonoBehaviour
{
    public Vector3 m_defaultPosition;

    [HideInInspector] public bool m_portaled = false;
    [HideInInspector] public float m_portalTime = 0f;
    [HideInInspector] bool m_bomb = false;
    [HideInInspector] float m_bombTimer = 0f;

    public bool m_reachedGoal = false;
    public Rigidbody2D m_Rigidbody;

    [Header("Movement")]
    public bool m_canMove = false;
    private Vector3 m_initialVelocity;
    private Vector2 m_moveVector;
    Vector3 m_newMove = new Vector2(1f, 0f);

    public float m_moveSpeed = 4.0f;
    public float m_speed;
    private float m_verticalVelocity;

    private bool m_isGrounded;
    private float m_groundedOffset = -0.14f;
    private float m_groundedRadius = 0.5f;
    private LayerMask m_groundLayers;

    private float m_gravity = -15.0f;
    private float m_speedChangeRate = 10.0f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    private float m_fallTimeout = 0.15f;
    private float m_fallTimeoutDelta;

    private float m_terminalVelocity = 53.0f;

    private LevelManager m_levelManager;
    private float m_mass;

    [SerializeField] private float m_timeToDeath = 5;
    private bool m_flipped;
    private bool m_canKillEnemy;
    Collider m_Collider;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_fallTimeoutDelta = m_fallTimeout;
        m_levelManager = FindObjectOfType<LevelManager>();
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
        //if (transform.position.y <= -4f)
        //{
        //  this.gameObject.SetActive(false);
        //}
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
        float currentHorizontalSpeed = new Vector2(m_Rigidbody.velocity.x, 0.0f).magnitude;

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

        Vector2 moveDirection = new Vector2(m_moveVector.x, 0.0f).normalized;
        if (m_moveVector != Vector2.zero)
        {

            // move
            moveDirection = transform.right * m_moveVector.x;
        }

        // move the player
        m_Rigidbody.velocity = (moveDirection.normalized * (m_speed * Time.deltaTime) + new Vector2(0.0f, m_verticalVelocity) * Time.deltaTime);
    }
    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector2 spherePosition = new Vector2(transform.position.x, transform.position.y - m_groundedOffset);
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
    //private void OnDisable()
    //{
    //  m_levelEditorManager.m_finishedBoids++;
    //}


    private void OnCollisionEnter2D(Collision2D collision)
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
                m_canMove = false;
                ///Animation
                DeathCountdown(this.gameObject);
                //gameObject.SetActive(false);
            }
        }
        if (collision.transform.tag.Equals("Floor"))
        {
            m_isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Floor"))
        {
            m_isGrounded = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals("Destiny"))
        {

            m_reachedGoal = true;
            m_levelManager.m_reachedGoals++;
            m_levelManager.m_playerEnded++;
            this.gameObject.SetActive(false);

        }
        //Si se encuentra con un resorte, obtener su fuerza y aplicarla a la esfera.
        if (collision.transform.tag.Equals("Spring"))
        {
            m_isGrounded = false;
            float springForce = collision.transform.GetComponent<SpringScript>().SpringForce;
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, springForce);
        }
        if (collision.transform.tag.Equals("Portal") && !m_portaled)
        {
            transform.position = collision.GetComponent<NewPortalScript>().otherObject.transform.position;
            m_portaled = true;
        }
        if (collision.transform.tag.Equals("Door") && !m_bomb)
        {
            FlipVelocity();
        }
        if (collision.transform.tag.Equals("Wall"))
        {
            FlipVelocity();
        }
        if (collision.transform.tag.Equals("Void"))
        {
            m_levelManager.m_playerEnded++;
            this.gameObject.SetActive(false);
        }
    }
    //Función para invertir la velocidad de la esfera en X.
    private void FlipVelocity()
    {
        m_flipped = !m_flipped;
        m_newMove = -m_newMove;
        ///FlipSprite
    }

    //Llamar a esta función para activar el movimiento.
    public void EnableMovement(bool state)
    {
        m_canMove = state;
        m_flipped = false;
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
        gameObject.SetActive(true);
        m_verticalVelocity = 0f;
        m_moveSpeed = 0;
        m_moveVector = Vector2.zero;
        m_newMove = new Vector2(1, 0);
        m_canMove = false;
        m_reachedGoal = false;
        m_Rigidbody.angularVelocity = 0;
        EnableMovement(false);
        transform.position = m_defaultPosition;
    }
}
