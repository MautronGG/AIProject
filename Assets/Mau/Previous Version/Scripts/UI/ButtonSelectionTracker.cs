using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ButtonSelectionTracker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
  public bool IsSelected = false;
  LevelEditorManager m_levelEditor;
  private void Start()
  {
    m_levelEditor = FindObjectOfType<LevelEditorManager>();
  }
  public void OnPointerEnter(PointerEventData eventData)
  {
    IsSelected = true;
  }
  public void OnPointerExit(PointerEventData eventData)
  {
    IsSelected = false;
  }
  public void Clicked()
  {
    IsSelected = false;
  }
  private void Update()
  {
    if (m_levelEditor.m_pauseCanvas.activeInHierarchy)
    {
      IsSelected = false;
    }
  }
}
