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
    public Rigidbody2D m_rigidBody;
    public CircleCollider2D m_collider;

    [Header("Movement")]
    public bool m_canMove = false;
    private Vector3 m_initialVelocity;
    private Vector3 m_moveVector = new Vector3(1f, 0f);

    [SerializeField] private float m_speed;
    [SerializeField] private float m_verticalVelocity;

    [SerializeField] private bool m_isGrounded;

    private float m_gravity = -15.0f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    private float m_fallTimeout = 0.15f;
    private float m_fallTimeoutDelta;

    private float m_terminalVelocity = 53.0f;

    private LevelManager m_levelManager;

    [SerializeField] private float m_timeToDeath = 5;
    private bool m_flipped;
    private bool m_canKillEnemy = false;

    Vector3 AdvanceDirection = Vector3.right;
    List<GameObject> list = new List<GameObject>();

    [Header("StairStep")]
    public float stepHeight = 0.8f; // Maximum height difference the character can step
    public float stepDetectionDistance = 0.1f; // Distance to check in front of the player


    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<CircleCollider2D>();
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

        if (m_reachedGoal)
        {
            m_canMove = false;
            //SoundEffect
            //AddPoints
            //FinishLevel
        }
        else
        {
            Move();
            //GroundedCheck();
            Gravity();
        }

    }
    public void SetMovement()
    {
        m_canMove = true;

    }

    public void Move()
    {
        //Vector3 targetMovement = Vector3.zero;
        //if (m_canMove)
        //{
        //    targetMovement = m_moveVector;
        //}
        //transform.position = transform.position + targetMovement.normalized * (m_speed * Time.deltaTime) + (new Vector3(0f, m_verticalVelocity) * Time.deltaTime);
        if (m_canMove)
        {
            m_moveVector = AdvanceDirection;
        }
        else
        {
            m_moveVector = Vector3.zero;
        }
        transform.Translate((m_moveVector * m_speed * Time.deltaTime) + new Vector3(0f, m_verticalVelocity) * Time.deltaTime);

    }
    //private void GroundedCheck()
    //{
    //    // set sphere position, with offset
    //    Vector2 spherePosition = new Vector2(transform.position.x, transform.position.y - m_groundedOffset);
    //    m_isGrounded = Physics2D.OverlapCircle(spherePosition, m_groundedRadius, m_groundLayers);
    //}
    private void Gravity()
    {
        if (m_isGrounded)
        {
            // reset the fall timeout timer
            m_fallTimeoutDelta = m_fallTimeout;

            // stop our velocity dropping infinitely when grounded
            if (m_verticalVelocity != 0.0f)
            {
                m_verticalVelocity = 0f;
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
            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (m_verticalVelocity < m_terminalVelocity)
            {
                m_verticalVelocity += m_gravity * Time.deltaTime;
            }
        }


    }
    //private void OnDisable()
    //{
    //  m_levelEditorManager.m_finishedBoids++;
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null)
        {
            return;
        }
        //Si es un muro u otra bola, cambiar la dirección del personaje.
        if (collision.gameObject.tag.Equals("Wall"))
        {
            Collider2D collider = collision.collider;
            //Vector2 m_Center = collider.bounds.center;
            //Vector2 m_Size = collider.bounds.size;
            //Vector2 m_Min = collider.bounds.min;
            Vector2 m_Max = collider.bounds.max;
            if (transform.position.y <= m_Max.y)
            {
                FlipVelocity();
            }
        }
        //if (collision.gameObject.tag.Equals("Player"))
        //{
        //    FlipVelocity();
        //}

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
            if (!list.Contains(collision.gameObject))
            {
                // Check if the character is colliding from above
                ContactPoint2D[] contactPoints = new ContactPoint2D[collision.contactCount];
                collision.GetContacts(contactPoints);

                bool isFromAbove = false;

                foreach (ContactPoint2D contact in contactPoints)
                {
                    // Check if the collision normal points upward, meaning the character is above the floor
                    if (contact.normal.y > 0.5f)
                    {
                        isFromAbove = true;
                        break;
                    }
                }

                // Only apply movement direction if the character is on top of the floor
                if (isFromAbove)
                {
                    list.Add(collision.gameObject);
                    m_isGrounded = true;

                    Debug.Log(collision.gameObject.name);

                    Vector3 forwardDirection = collision.transform.right;
                    Vector3 upDirection = collision.transform.up;
                    Vector3 finalPos = collision.gameObject.transform.position + (upDirection * 10);

                    Debug.DrawLine(collision.gameObject.transform.position, finalPos, Color.blue, 100);
                    Debug.Log(Vector3.Dot(Vector3.up, upDirection));

                    if (Vector3.Dot(Vector3.up, upDirection) >= 0.5f)
                    {
                        if (AdvanceDirection.x > 0f)
                        {
                            AdvanceDirection = forwardDirection;
                        }
                        else
                        {
                            AdvanceDirection = new Vector3(-forwardDirection.x, forwardDirection.y);
                        }
                    }
                    else
                    {
                        FlipVelocity();
                    }
                }
            }
            if (list.Count > 1)
            {
                HandleStepClimb();
                //Collider2D col1 = list[0].gameObject.GetComponent<Collider2D>();
                //Collider2D col2 = list[1].gameObject.GetComponent<Collider2D>();
                //Vector3 col1Max = col1.bounds.max;
                //Vector3 col1Min = col1.bounds.min;
                //Vector3 col1Center = col1.bounds.center;
                //
                //Vector3 col2Max = col2.bounds.max;
                //Vector3 col2Min = col2.bounds.min;
                //Vector3 col2Center = col2.bounds.center;
                //
                //
                //Vector3 point1 = Vector2.zero;
                //Vector3 point2 = Vector2.zero;
                //
                //if (AdvanceDirection.x > 0f)
                //{
                //    point1 = col1Center + col1.gameObject.transform.right * (col1.bounds.size.x / 2);
                //    point2 = col2Center - col1.gameObject.transform.right * (col1.bounds.size.x / 2);
                //    if(point2.y <= point1.y + m_stepOffSet)
                //    {
                //        //transform.position += Vector3.up + Vector3.right;
                //    }
                //}
                //else
                //{
                //    point1 = col1Center - col1.gameObject.transform.right * (col1.bounds.size.x / 2);
                //    point2 = col2Center + col1.gameObject.transform.right * (col1.bounds.size.x / 2);
                //    if (point2.y <= point1.y + m_stepOffSet)
                //    {
                //        //transform.position -= Vector3.up + Vector3.right;
                //    }
                //}
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("Floor"))
        {
            if (list.Contains(collision.gameObject))
            {
                list.Remove(collision.gameObject); 
                if (list.Count <= 0)
                {
                    m_isGrounded = false;
                }
            }
            
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
        //if (collision.transform.tag.Equals("Spring"))
        //{
        //    m_isGrounded = false;
        //    float springForce = collision.transform.GetComponent<SpringScript>().SpringForce;
        //    m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, springForce);
        //}
        if (collision.transform.tag.Equals("Portal") && !m_portaled)
        {
            transform.position = collision.GetComponent<NewPortalScript>().otherObject.transform.position;
            m_portaled = true;
        }
        if (collision.transform.tag.Equals("Door") && !m_bomb)
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
        AdvanceDirection = new Vector3(-AdvanceDirection.x, AdvanceDirection.y);
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
        m_canMove = false;
        m_reachedGoal = false;
        //m_Rigidbody.angularVelocity = 0;
        EnableMovement(false);
        transform.position = m_defaultPosition;
        AdvanceDirection = new Vector3(1f, 0f, 0f);
        list.Clear();
        m_isGrounded = false;
    }
    void HandleStepClimb()
    {
        float adjustedRadius = m_collider.radius * transform.localScale.x; // Assuming uniform scaling

        // Cast two rays in front of the player: one at foot height and another at head height
        Vector2 footPosition = new Vector2(transform.position.x + adjustedRadius, transform.position.y - adjustedRadius);
        Vector2 headPosition = new Vector2(transform.position.x + adjustedRadius, transform.position.y + adjustedRadius);

        RaycastHit2D footRay = Physics2D.Raycast(footPosition, Vector2.right, stepDetectionDistance);
        RaycastHit2D headRay = Physics2D.Raycast(headPosition, Vector2.right, stepDetectionDistance);

        if (footRay.collider != null && !headRay.collider) // If foot detects a collision but head doesn't
        {
            // Check if the object hit by the foot ray has the correct tag
            if (footRay.collider.CompareTag("Floor") || footRay.collider.CompareTag("Wall") || footRay.collider.CompareTag("Bridge"))
            {
                float stepDifference = footRay.point.y - transform.position.y;
                if (stepDifference > 0 && stepDifference <= stepHeight)
                {
                    // Step is within the threshold height, step up
                    m_rigidBody.position = new Vector2(m_rigidBody.position.x, m_rigidBody.position.y + stepDifference);
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        float adjustedRadius = m_collider.radius * transform.localScale.x; // Assuming uniform scaling
        // Visualize raycasts in the editor
        Vector2 footPosition = new Vector2(transform.position.x + adjustedRadius, transform.position.y - adjustedRadius);
        Vector2 headPosition = new Vector2(transform.position.x + adjustedRadius, transform.position.y + adjustedRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(footPosition, footPosition + Vector2.right * stepDetectionDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(headPosition, headPosition + Vector2.right * stepDetectionDistance);
    }
}
