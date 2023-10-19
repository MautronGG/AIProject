using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mau
{
  public class Boid : MonoBehaviour
  {
    public Vector3 m_Force;

    [Header("Seek")]
    public Vector3 m_target;
    public float m_seekImpetu;
    public Transform m_seekTarget;
    public float m_seekRatio;
    public bool m_seekByRatio = false;

    [Header("Wander")]
    public Vector3 m_wanderTarget;
    public bool m_isWandering = false;
    public bool m_canWander = false;
    public float m_wanderTimer = 0f;
    public float m_wanderImpetu;

    [Header("Flee")]
    public Transform m_fleeTarget;
    public float m_fleeImpetu;
    public float m_fleeRatio;
    public bool m_fleeByRatio;

    [Header("Arrive")]
    public Transform m_arriveTarget;
    public float m_arriveImpetu;
    public float m_arriveRatio;

    [Header("Persue")]
    public Boid m_persueTarget;
    public float m_persueImpetu;
    public float m_persueTime;

    [Header("Evade")]
    public Boid m_evadeTarget;
    public float m_evadeImpetu1;
    public float m_evadeImpetu2;
    public float m_evadeTime;

    [Header("Floacking")]
    public bool m_isFloacking;
    public List<Boid> m_allBoids;
    public List<Boid> m_nearbyBoids;
    public float m_floackingRatio = 10f;
    public float m_floackingImpetu = 1f;
    public float m_floackingSeparateImpetu = 1000f;
    int m_nearbyBoidsIteration = 0;
    int m_nearbyBoidsIndex = 1;
    bool nearbyWhile = true;
    int foreachIteration = 0;

    [Header("FollowPath")]
    public List<Transform> m_pathList;
    public int m_pathIndex = 0;
    public int m_pathIndexNext = 0;
    public int m_pathIndexPrev = 0;
    public bool m_isFirstPoint = true;
    public float m_followPathImpetu;
    public float m_pathRatio;
    public float m_pathArriveRatio;

    public Vector3 m_moveForce = Vector3.zero;
    public Vector3 m_currentForce = Vector3.zero;
    public float m_speed = 10;
    public float m_mass;
    public float m_personalSpace;

    public GameObject m_destiny;
    Vector3 m_direction;

    public float m_ratio;
    public float m_magnitude;
    public bool m_arrived = false;
    int m_behavior = 1;
    public GameObject m_floor;

    // Start is called before the first frame update
    void Start()
    {
      m_personalSpace = GetComponent<BoxCollider>().bounds.size.x;
      var boids = FindObjectsOfType<Boid>();
      foreach (Boid boid in boids)
      {
        m_allBoids.Add(boid);
      }
      //m_behavior = ((int)Random.Range(0, 1.9f));
      //m_ratio = GetComponent<BoxCollider>().bounds.size.x;
      m_target = Wander();

      m_pathIndexNext = 0;
      m_pathIndexPrev = m_pathIndexNext - 1;
      if(m_pathIndexPrev < 0)
      {
        m_pathIndexPrev = m_pathList.Count - 1;
      }
    }
    private void FixedUpdate()
    {
      //foreach (Boid boid1 in m_allBoids)
      //{
      //  if (boid1 != this)
      //  {
      //    var pos = boid1.transform.position - transform.position;
      //    if (m_nearbyBoids.Count > 0)
      //    {
      //      if (pos.magnitude <= m_floackingRatio)
      //      {
      //        foreach (Boid boid2 in m_nearbyBoids)
      //        {
      //          if (boid2 != boid1)
      //          {
      //            m_nearbyBoidsIteration++;
      //            continue;
      //          }
      //        }
      //        if (m_nearbyBoidsIteration == m_nearbyBoids.Count)
      //        {
      //          m_nearbyBoids.Add(boid1);
      //          m_nearbyBoidsIteration = 0;
      //        }
      //      }
      //      else
      //      {
      //        foreach (Boid boid2 in m_nearbyBoids)
      //        {
      //          if (boid2 != boid1)
      //          {
      //            m_nearbyBoidsIteration++;
      //            continue;
      //          }
      //        }
      //        if (m_nearbyBoidsIteration <= m_nearbyBoids.Count)
      //        {
      //          m_nearbyBoids.Remove(boid1);
      //          m_nearbyBoidsIteration = 0;
      //        }
      //      }
      //    }
      //    else
      //    {
      //      if (pos.magnitude <= m_floackingRatio)
      //      {
      //        m_nearbyBoids.Add(boid1);
      //      }
      //    }
      //  }
      //}
      foreach (Boid allBoid in m_allBoids)
      {
        if (allBoid != this)
        {
          var pos = allBoid.transform.position - transform.position;
          if (pos.magnitude <= m_floackingRatio)
          {
            foreach (Boid nearBoid in m_nearbyBoids)
            {
              if (nearBoid != allBoid)
              {
                m_nearbyBoidsIteration++;
                continue;
              }
            }
            if (m_nearbyBoidsIteration == m_nearbyBoids.Count)
            {
              m_nearbyBoidsIteration = 0;
              m_nearbyBoids.Add(allBoid);
            }
            else
            {
              m_nearbyBoidsIteration = 0;
              continue;
            }
          }
          else
          {
            foreach (Boid nearBoid in m_nearbyBoids)
            {
              if (nearBoid == allBoid)
              {
                m_nearbyBoids.Remove(nearBoid);
                m_nearbyBoidsIteration++;
                continue;
              }
            }
          }
        }
      }
      foreachIteration = 0;
      foreach (Boid nearBoid in m_nearbyBoids)
      {
        while (nearbyWhile)
        {
          if (m_nearbyBoidsIndex < m_nearbyBoids.Count)
          {
            if (foreachIteration == m_nearbyBoidsIndex)
            {
              m_nearbyBoidsIndex++;
            }
            if (nearBoid != m_nearbyBoids[m_nearbyBoidsIndex])
            {
              m_nearbyBoidsIndex++;
            }
            else
            {
              m_nearbyBoids.Remove(nearBoid);
              m_nearbyBoidsIndex++;
            }
          }
          else
          {
            m_nearbyBoidsIndex = 1;
            nearbyWhile = false;
          }
        }
        foreachIteration++;
      }
    }
    // Update is called once per frame
    void Update()
    {
      m_Force = Vector3.zero;
      if (m_seekTarget)
      {
        m_Force += Seek(m_seekTarget.position, m_seekImpetu);
      }
      if (m_canWander)
      {
        m_Force += Wander();
      }
      if (m_fleeTarget)
      {
        m_Force += Flee(m_fleeTarget.position, m_fleeImpetu);
      }
      if (m_persueTarget)
      {
        m_Force += Persue(m_persueTarget, m_persueImpetu);
      }
      if (m_evadeTarget)
      {
        m_Force += Evade(m_evadeTarget, m_evadeImpetu1, m_evadeImpetu2);
      }
      if (m_isFloacking)
      {
        m_Force += Floacking_Cohesion();
        m_Force += Floacking_Direction();
        m_Force += Floacking_Separate();
      }
      if (m_followPathImpetu > 0)
      {
        m_Force += FollowPath(m_pathList, m_pathRatio, m_pathArriveRatio, m_followPathImpetu);
      }
      if (m_arriveTarget)
      {
        m_moveForce = Arrive(m_arriveTarget.position, m_arriveImpetu, m_speed, m_Force);
      }
      else
      {
        m_Force += Inertia(m_currentForce, m_Force, m_mass);
        m_moveForce = Truncar(m_Force, m_speed);
      }
      transform.forward = m_moveForce.normalized;
      transform.position += m_moveForce * Time.deltaTime;



      //if (m_behavior == 1)
      //{
      //  m_direction = Seek(m_target, m_impetu);
      //  Arrive();
      //  if (!m_arrived)
      //  {
      //    transform.position += m_direction * Time.deltaTime;
      //  }
      //  if (Input.GetKeyDown(KeyCode.Space))
      //  {
      //    m_behavior = 0;
      //  }
      //}
      //else if (m_behavior == 0)
      //{
      //  m_direction = Flee(m_target, m_impetu);
      //  transform.position += m_direction * Time.deltaTime;
      //  if (Input.GetKeyDown(KeyCode.Space))
      //  {
      //    m_behavior = 1;
      //  }
      //}
      //if (Vector3.Distance(transform.position, m_destiny.transform.position) <= m_destiny.GetComponent<Destiny>().m_ratio)
      //{
      //  m_target = m_destiny.transform.position;
      //}
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
    Vector3 Flee(Vector3 Target, float Impetu)
    {
      Vector3 Force = Target - transform.position;
      if (m_fleeByRatio)
      {
        if (Force.magnitude <= m_fleeRatio)
        {
          return -Seek(Target, Impetu);
        }
        else
        {
          Force = Vector3.zero;
          return Force;
        }
      }
      else
      {
        return -Seek(Target, Impetu);
      }
    }

    Vector3 Arrive(Vector3 Target, float Impetu, float Speed, Vector3 newForce)
    {
      Vector3 Force = Target - transform.position;
      float magnitude = Force.magnitude;
      Force = Force.normalized;
      Force *= Impetu;
      float addSpeed = 1f;
      if (m_magnitude <= m_arriveRatio)
      {
        addSpeed = magnitude / m_arriveRatio;
      }
      newForce += Force;
      m_currentForce = Inertia(m_currentForce, newForce, m_mass);
      Vector3 moveForce = Truncar(newForce, m_speed * addSpeed);

      return moveForce;


      //if (m_magnitude < m_ratio)
      //{
      //  m_arrived = true;
      //  m_target = Wander();
      //}
      //else
      //{
      //  m_arrived = false;
      //}
      //return Target;

    }
    Vector3 Wander()
    {
      if (!m_isWandering)
      {
        var x = m_floor.GetComponent<MeshCollider>().bounds.size.x;
        var z = m_floor.GetComponent<MeshCollider>().bounds.size.z;
        var randomx = Random.Range(m_floor.transform.position.x - x, m_floor.transform.position.x + x);
        var randomz = Random.Range(m_floor.transform.position.z - z, m_floor.transform.position.z + z);
        //m_wanderTarget.y = transform.position.y;
        m_wanderTarget = new Vector3(randomx, transform.position.y, randomz);
        m_isWandering = true;
      }
      else
      {
        m_wanderTimer += Time.deltaTime;
        if (m_wanderTimer >= 3f)
        {
          m_wanderTimer = 0f;
          m_isWandering = false;
        }
      }
      return Seek(m_wanderTarget, m_wanderImpetu);
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

    Vector3 Persue(Boid Target, float impetu)
    {
      Vector3 tragetPosition = Target.gameObject.transform.position;
      Vector3 PP = (Target.m_moveForce.normalized * Target.m_speed * Target.m_persueTime);
      Vector3 distance = tragetPosition - transform.position;
      if (distance.magnitude <= PP.magnitude)
      {
        float Percentage = distance.magnitude / PP.magnitude;
        PP *= Percentage;
      }
      PP += tragetPosition;
      return Seek(PP, impetu);
    }

    Vector3 Evade(Boid Target, float impetu1, float impetu2)
    {
      Vector3 tragetPosition = Target.gameObject.transform.position;
      Vector3 PP = (Target.m_moveForce.normalized * Target.m_speed * Target.m_evadeTime);
      Vector3 distance = tragetPosition - transform.position;
      PP += tragetPosition;
      Vector3 F = Flee(PP, impetu1) + Flee(Target.gameObject.transform.position, impetu2);
      return F;
    }

    Vector3 Floacking_Cohesion()
    {
      Vector3 Target = Vector3.zero;
      foreach (Boid boid in m_nearbyBoids)
      {
        Target += boid.transform.position;
      }
      Target += transform.position;
      Target /= m_nearbyBoids.Count + 1;
      return Seek(Target, m_floackingImpetu);
    }

    Vector3 Floacking_Direction()
    {
      Vector3 Direction = Vector3.zero;
      foreach (Boid boid in m_nearbyBoids)
      {
        Direction += boid.transform.forward;
      }
      Direction += transform.forward;
      Direction /= m_nearbyBoids.Count + 1;
      return Direction * m_floackingImpetu;
    }

    Vector3 Floacking_Separate()
    {
      Vector3 Force = Vector3.zero;
      foreach (Boid boid in m_nearbyBoids)
      {
        var distance = boid.transform.position - transform.position;
        if (distance.magnitude <= m_personalSpace)
        {   
          Force += Flee(boid.transform.position, m_floackingSeparateImpetu);
        }
      }
      return Force;
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
          m_pathIndexNext = 0;
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
    private void OnDrawGizmos()
    {
      Color color1 = Color.white;
      Color color2 = Color.red;
      Color color3 = Color.yellow;
      color1.a = 0.5f;
      color2.a = 0.5f;
      color3.a = 0.5f;
      Gizmos.color = color1;
      Gizmos.DrawSphere(transform.position, m_seekRatio);
      Gizmos.color = color2;
      Gizmos.DrawSphere(transform.position, m_fleeRatio);
      Gizmos.color = color3;
      Gizmos.DrawSphere(transform.position, m_arriveRatio);
      Gizmos.color = color1;
      Gizmos.DrawSphere(transform.position, m_floackingRatio);
    }
  }
}

