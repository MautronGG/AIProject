using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Enemy : Object_Parent
{
    [HideInInspector] public bool m_portaled = false;
    [HideInInspector] public float m_portalTime = 0f;
    public Rigidbody2D m_rigidBody;
    public CircleCollider2D m_collider;

    [Header("Movement")]
    public bool m_canMove = false;
    private Vector3 m_moveVector = new Vector3(1f, 0f);

    [SerializeField] private float m_speed;
    [SerializeField] private float m_verticalVelocity;

    [SerializeField] private bool m_isGrounded;

    private float m_gravity = -15.0f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    private float m_fallTimeout = 0.15f;
    private float m_fallTimeoutDelta;

    private float m_terminalVelocity = 53.0f;

    [SerializeField] private float m_timeToDeath = 5;
    private bool m_flipped;

    Vector3 AdvanceDirection = Vector3.right;
    [SerializeField] List<GameObject> list = new List<GameObject>();
    List<Vector3> m_rights = new List<Vector3>();
    List<Collider2D> m_colliders = new List<Collider2D>();

    [Header("StairStep")]
    Vector3 m_normal;
    [SerializeField] float m_stepHeight;
    [SerializeField] float m_maxStepHeight = .7f; // Maximum height difference the character can step
    
    bool m_applyDirection = false;
    public override void Awake()
    {
        base.Awake();
    }
    public override void Start()
    {
        base.Start();
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<CircleCollider2D>();
        m_fallTimeoutDelta = m_fallTimeout;
    }

    public override void Update()
    {
        base.Update();
        if (m_portaled)
        {
            m_portalTime += Time.deltaTime;
            if (m_portalTime >= 1.5f)
            {
                m_portalTime = 0f;
                m_portaled = false;
            }
        }
    }
    private void FixedUpdate()
    {
        Move();
        Gravity();
    }

    public override void OnMouseOver()
    {
        base.OnMouseOver();
    }
    public override void SaveDefaults()
    {
        base.SaveDefaults();
    }
    public void Move()
    {
        if (m_canMove)
        {
            m_moveVector = AdvanceDirection;
        }
        else
        {
            m_moveVector = Vector3.zero;
            //m_verticalVelocity = 0f;
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_applyDirection = false;
        if (collision == null)
        {
            return;
        }
        //Si es un muro u otra bola, cambiar la dirección del personaje.
        if (collision.gameObject.tag.Equals("Wall"))
        {
            Collider2D colliderWall = collision.collider;
            Collider2D colliderMinion = GetComponent<Collider2D>();
            //Vector2 m_Center = collider.bounds.center;
            //Vector2 m_Size = collider.bounds.size;
            //Vector2 m_Min = collider.bounds.min;

            if (colliderMinion.bounds.min.y + .2 <= colliderWall.bounds.max.y)
            {
                FlipVelocity();
            }
            else
            {
                m_applyDirection = true;
            }
        }
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), true);
        }
        if (collision.transform.tag.Equals("Floor"))
        {
            m_applyDirection = true;
        }
        if (m_applyDirection)
        {
            Vector2 rightContactPoint = new Vector2();
            Vector2 ContactPoint = new Vector2();
            if (!list.Contains(collision.gameObject))
            {
                // Check if the character is colliding from above
                ContactPoint2D[] contactPoints = new ContactPoint2D[collision.contactCount];
                collision.GetContacts(contactPoints);
                bool isFromAbove = false;

                foreach (ContactPoint2D contact in contactPoints)
                {
                    //Debug.DrawLine(contact.point, contact.point + (contact.normal * 10), Color.yellow, 100);
                    rightContactPoint = new Vector2(contact.normal.y, -contact.normal.x);
                    ContactPoint = contact.point;
                    //Debug.DrawLine(contact.point, contact.point + (rightContactPoint * 10), Color.magenta, 100);
                    // Check if the collision normal points upward, meaning the character is above the floor
                    if (contact.normal.y >= 0f)
                    {
                        isFromAbove = true;
                        m_normal = contact.normal;
                        break;
                    }
                }
                list.Add(collision.gameObject);
                m_rights.Add(collision.transform.right);
                // Only apply movement direction if the character is on top of the floor
                if (isFromAbove)
                {
                    m_isGrounded = true;

                    Debug.Log(collision.gameObject.name);

                    Vector3 forwardDirection = rightContactPoint;
                    Vector3 upDirection = m_normal;
                    Vector3 finalPos = collision.gameObject.transform.position + (upDirection * 10);

                    //Debug.DrawLine(collision.gameObject.transform.position, finalPos, Color.blue, 100);
                    Debug.Log("Angle: " + Vector3.Angle(Vector3.up, upDirection));

                    if (Vector3.Angle(Vector3.up, upDirection) <= 66)
                    {
                        if (AdvanceDirection.x > 0f)
                        {
                            AdvanceDirection = forwardDirection;
                        }
                        else
                        {
                            AdvanceDirection = -forwardDirection;
                        }
                    }
                    else
                    {
                        if (list.Count > 1)
                        {
                            //HandleStepClimb();
                            Collider2D col1 = list[0].gameObject.GetComponent<Collider2D>();
                            BoxCollider2D col2 = list[1].gameObject.GetComponent<BoxCollider2D>();
                            DebugBounds og1 = list[0].gameObject.GetComponent<DebugBounds>();
                            DebugBounds og2 = list[0].gameObject.GetComponent<DebugBounds>();
                            Vector3 collision1 = m_rights[0];
                            Vector3 collision2 = m_rights[1];

                            //Vector3 col1Max = col1.bounds.max;
                            //Vector3 col1Min = col1.bounds.min;
                            //Vector3 col1Center = col1.bounds.center;
                            //
                            //Vector3 col2Max = col2.bounds.max;
                            //Vector3 col2Min = col2.bounds.min;
                            Vector3 col2Center = col2.bounds.center;

                            Vector3 col2Size = col2.size;

                            // Get half the width and height of the box
                            Vector2 col2HalfSize = col2Size * 0.5f;

                            Vector2[] col2LocalCorners = new Vector2[4];
                            col2LocalCorners[0] = col2.offset + new Vector2(-col2HalfSize.x, col2HalfSize.y);   // Top-left
                            col2LocalCorners[1] = col2.offset + new Vector2(col2HalfSize.x, col2HalfSize.y);    // Top-right
                            col2LocalCorners[2] = col2.offset + new Vector2(-col2HalfSize.x, -col2HalfSize.y);  // Bottom-left
                            col2LocalCorners[3] = col2.offset + new Vector2(col2HalfSize.x, -col2HalfSize.y);   // Bottom-right

                            // Use rotation to calculate the actual corners in world space
                            Vector2[] col2WorldCorners = new Vector2[4];
                            for (int i = 0; i < col2LocalCorners.Length; i++)
                            {
                                col2WorldCorners[i] = col2.transform.TransformPoint(col2LocalCorners[i]);
                                //Vector2 rotatedCorner = RotatePoint(col2LocalCorners[i], Vector2.zero, col2.transform.eulerAngles.z);
                                //col2WorldCorners[i] = col2CenterOff + rotatedCorner;
                            }
                            Debug.DrawLine(col2Center, col2WorldCorners[0], Color.blue, 100);
                            //Debug.DrawLine(this.gameObject.GetComponent<Collider2D>().bounds.center, (this.gameObject.GetComponent<Collider2D>().bounds.center - new Vector3(0f, this.gameObject.GetComponent<Collider2D>().bounds.extents.y)), Color.red, 100);
                            if (AdvanceDirection.x > 0f)
                            {
                                float difference = (col2WorldCorners[0].y - this.gameObject.GetComponent<Collider2D>().bounds.min.y);
                                if (difference <= m_maxStepHeight)
                                {
                                    m_stepHeight = difference;
                                    //col2WorldCorners[0].y - transform.position.y + 0.01f;
                                    transform.position += new Vector3(0, m_stepHeight);
                                }
                                else
                                {
                                    FlipVelocity();
                                }
                            }
                            else if (AdvanceDirection.x < 0f)
                            {
                                float difference = (col2WorldCorners[1].y - this.gameObject.GetComponent<Collider2D>().bounds.min.y);
                                if (difference <= m_maxStepHeight)
                                {
                                    m_stepHeight = difference;
                                    //col2WorldCorners[0].y - transform.position.y + 0.01f;
                                    transform.position += new Vector3(0, m_stepHeight);
                                }
                                else
                                {
                                    FlipVelocity();
                                }
                            }
                        }
                        else
                        {
                            FlipVelocity();
                        }
                    }
                }
                else if (list.Count > 1)
                {
                    if (AdvanceDirection.x > 0)
                    {
                        if (ContactPoint.x > transform.position.x)
                        {
                            FlipVelocity();
                        }
                    }
                    else if (AdvanceDirection.x < 0)
                    {
                        if (ContactPoint.x < transform.position.x)
                        {
                            FlipVelocity();
                        }
                    }
                }
                else if (list.Count == 1)
                {
                    m_verticalVelocity = 0f;
                }
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (list.Contains(collision.gameObject))
        {
            int index = list.IndexOf(collision.gameObject);
            list.RemoveAt(index);
            m_rights.Remove(collision.transform.right);
            if (list.Count <= 0)
            {
                m_isGrounded = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals("Void"))
        {
            this.gameObject.SetActive(false);
        }
        if (collision.transform.tag.Equals("Portal") && !m_portaled)
        {
            transform.position = collision.GetComponent<Object_Portal>().otherObject.transform.position;
            m_portaled = true;
        }
        if (collision.transform.tag.Equals("Spring"))
        {
            m_isGrounded = false;
            float springForce = collision.transform.GetComponent<Object_Spring>().SpringForce;
            m_verticalVelocity = springForce;
        }
        if (collision.transform.tag.Equals("Laser"))
        {
            Die(collision);
            Destroy(collision.gameObject);
        }
    }

    //Función para invertir la velocidad de la esfera en X.
    private void FlipVelocity()
    {
        m_flipped = !m_flipped;
        AdvanceDirection = -AdvanceDirection;
        ///FlipSprite
    }

    //Llamar a esta función para activar el movimiento.
    public override void StartObject()
    {
        m_canMove = true;
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
    public override void ResetDeafualts()
    {
        base.ResetDeafualts();
        m_verticalVelocity = 0f;
        //m_Rigidbody.angularVelocity = 0;
        m_canMove = false;
        m_flipped = false;
        AdvanceDirection = new Vector3(1f, 0f, 0f);
        list.Clear();
        m_rights.Clear();
        m_isGrounded = false;
        foreach (Collider2D collider in m_colliders)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider, false);
        }
        m_colliders.Clear();
    }
    public void Die(Collider2D collision)
    {
        m_canMove = false;
        ///Animation
        //gameObject.SetActive(false);
        m_colliders.Add(collision);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision, true);
        StartCoroutine(DeathCountdown(this.gameObject));
    }
}
