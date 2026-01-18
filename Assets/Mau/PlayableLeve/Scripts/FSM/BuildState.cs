using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildState : State
{
    private void Awake()
    {
        m_levelManager = GetComponent<LevelManager>();
    }
    public override void onEnter()
    {
        m_levelManager.m_currentStateCanvas = m_levelManager.m_HUDBuildCanvas;
        m_levelManager.m_restartEvents.Invoke();
    }

    public override void onExit()
    {

    }

    public override void onUpdate()
    {
    }
}
