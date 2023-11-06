using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFollowMouse : MonoBehaviour
{
  public float m_rotationDegree = 0f;
  public float m_defaultRotateSpeed;
  public float m_runRotateSpeed;
  float m_rotationSpeed;

  public float m_scaleDegree = 0f;
  public float m_defaultScaleSpeed;
  public float m_runScaleSpeed;
  float m_scaleSpeed = 1;

  public Vector3 m_defaultRotation;
  public Vector3 m_defaultScale;
  public Material m_defaultMaterial;

  public int m_colorID = 7;
  LevelEditorManager m_levelEditor;
  //public GameObject m_child;

  private void Awake()
  {
    m_levelEditor = GameObject.FindGameObjectWithTag("LevelEditorManager").GetComponent<LevelEditorManager>();
    ChangeDefaults(m_defaultRotation, m_defaultScale, 7);
  }
  private void Start()
  {
    m_rotationSpeed = m_defaultRotateSpeed;
    m_scaleSpeed = m_defaultScaleSpeed;
  }
  private void OnEnable()
  {
    m_rotationDegree = 0f;
    m_scaleDegree = 0f;
  }

  void Update()
  {
    m_scaleDegree = 0f;
    Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
    transform.position = worldPosition;
    transform.rotation = Quaternion.Euler(m_defaultRotation.x + m_rotationDegree, m_defaultRotation.y, m_defaultRotation.z);

    if (Input.GetKey(KeyCode.E))
    {
      m_rotationDegree -= m_rotationSpeed * Time.deltaTime;
    }
    if (Input.GetKey(KeyCode.Q))
    {
      m_rotationDegree += m_rotationSpeed * Time.deltaTime;
    }
    if (Input.GetKey(KeyCode.F))
    {
      m_scaleDegree += m_scaleSpeed * Time.deltaTime;
      transform.localScale += new Vector3(0f, m_scaleDegree, 0f);
    }
    if (Input.GetKey(KeyCode.G))
    {
      m_scaleDegree -= m_scaleSpeed * Time.deltaTime;
      transform.localScale += new Vector3(0f, m_scaleDegree, 0f);
    }
    if (Input.GetKeyDown(KeyCode.LeftShift))
    {
      m_scaleSpeed = m_runScaleSpeed;
      m_rotationSpeed = m_runRotateSpeed;
    }
    if (Input.GetKeyUp(KeyCode.LeftShift))
    {
      m_scaleSpeed = m_defaultScaleSpeed;
      m_rotationSpeed = m_defaultRotateSpeed;
    }
  }
  public void ChangeDefaults(Vector3 newRot, Vector3 newScale, int color)
  {
    m_defaultRotation = newRot;
    m_defaultScale = newScale;
    transform.rotation = Quaternion.Euler(m_defaultRotation.x, m_defaultRotation.y, m_defaultRotation.z);
    transform.localScale = m_defaultScale;
    m_colorID = color;
    m_levelEditor.ChangeSpriteColor(m_colorID, this.gameObject);
    //m_colorID = material;
    //this.GetComponent<Renderer>().material = m_colorID;
  }
}
