using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ButtonSelectionTracker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool IsSelected = false;
    //LevelEditorManager m_levelEditor;
    LevelManager m_levelManager;
    EditorManager m_editorManager;
    Button button;

    bool m_onLevel = true;
    private void Start()
    {
        //m_levelEditor = FindObjectOfType<LevelEditorManager>();
        m_levelManager = FindObjectOfType<LevelManager>();
        if (!m_levelManager)
        {
            m_editorManager = FindObjectOfType<EditorManager>();
            m_onLevel = false;
        }
        button = GetComponent<Button>();
        //button.onClick.AddListener(() =>
        //{
        //    IsSelected = false;
        //});
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        IsSelected = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        IsSelected = false;
    }

    private void OnDisable()
    {
        IsSelected = false;
    }

    private void Update()
    {
        //if (m_levelEditor.m_pauseCanvas.activeInHierarchy)
        //{
        //    IsSelected = false;
        //}
        if (m_onLevel)
        {

            if (m_levelManager.m_pauseCanvas.activeInHierarchy)
            {
                IsSelected = false;
            }
            if (IsSelected && Input.GetMouseButtonDown(0))
            {
                Debug.Log("Button Clicked: " + gameObject.name);
                m_levelManager.m_audioManager.PlaySFX(m_levelManager.m_audioManager.m_sfx_ButtonClick);
            }
        }
        else
        {
            if (m_editorManager.m_pauseCanvas.activeInHierarchy)
            {
                IsSelected = false;
            }
        }
    }
}
