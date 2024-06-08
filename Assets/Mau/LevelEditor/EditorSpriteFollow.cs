using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorSpriteFollow : MonoBehaviour
{
  EditorManager m_editor;
  public bool m_follow = true;
  [SerializeField] bool m_isParent = false;
  //public GameObject m_child;

  private void Awake()
  {
    m_editor = GameObject.FindGameObjectWithTag("EditorManager").GetComponent<EditorManager>();
    //ChangeDefaults(m_defaultRotation, m_defaultScale, 7);
  }

  void Update()
  {
    if (m_follow)
    {
      Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
      Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
      transform.position = worldPosition;
      //transform.rotation = Quaternion.Euler(m_defaultRotation.x + m_rotationDegree, m_defaultRotation.y, m_defaultRotation.z);
      if (Input.GetMouseButtonDown(0) && m_isParent)
      {
        m_follow = false;
        m_editor.m_HUDCanvas.SetActive(true);
        m_editor.m_isEditing = false;
      }
    }

  }
}
