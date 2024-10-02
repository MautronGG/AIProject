using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Enemy : Object_Parent
{
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
    List<Collision2D> m_collisions = new List<Collision2D>();

    [Header("StairStep")]
    float stepHeight = .3f; // Maximum height difference the character can step

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
    private void OnCollisionEnter2D(Collision2D collision)
    {
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
                    if (contact.normal.y > 0f)
                    {
                        isFromAbove = true;
                        break;
                    }
                }
                list.Add(collision.gameObject);
                m_collisions.Add(collision);
                // Only apply movement direction if the character is on top of the floor
                if (isFromAbove)
                {
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
                //HandleStepClimb();
                Collider2D col1 = list[0].gameObject.GetComponent<Collider2D>();
                Collider2D col2 = list[1].gameObject.GetComponent<Collider2D>();
                Collision2D collision1 = m_collisions[0];
                Collision2D collision2 = m_collisions[1];
                Vector3 col1Max = col1.bounds.max;
                Vector3 col1Min = col1.bounds.min;
                Vector3 col1Center = col1.bounds.center;

                Vector3 col2Max = col2.bounds.max;
                Vector3 col2Min = col2.bounds.min;
                Vector3 col2Center = col2.bounds.center;

                Vector3 point1 = Vector2.zero;
                Vector3 point2 = Vector2.zero;

                if (AdvanceDirection.x > 0f)
                {
                    point1 = col1Center + collision1.transform.right * (col1.bounds.size.x / 2);
                    point2 = col2Center - collision2.transform.right * (col2.bounds.size.x / 2);
                    if (point2.y <= point1.y + stepHeight)
                    {
                        transform.position += new Vector3(0, stepHeight);
                    }
                    else
                    {
                        FlipVelocity();
                    }
                }
                else
                {
                    point1 = col1Center - collision1.transform.right * (col1.bounds.size.x / 2);
                    point2 = col2Center + collision2.transform.right * (col2.bounds.size.x / 2);
                    if (point2.y <= point1.y + stepHeight)
                    {
                        transform.position += new Vector3(0, stepHeight);
                    }
                    else
                    {
                        FlipVelocity();
                    }
                }
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
            if (m_collisions.Contains(collision))
            {
                m_collisions.Remove(collision);
            }

        }
    }
   //private void OnTriggerEnter2D(Collider2D collision)
   //{
   //
   //}

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
    public override void ResetDeafualts()
    {
        base.ResetDeafualts();
        m_verticalVelocity = 0f;
        //m_Rigidbody.angularVelocity = 0;
        EnableMovement(false);
        AdvanceDirection = new Vector3(1f, 0f, 0f);
        list.Clear();
        m_collisions.Clear();
        m_isGrounded = false;
    }
}
