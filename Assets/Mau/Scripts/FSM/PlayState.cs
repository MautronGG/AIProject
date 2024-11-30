using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : State
{
    private void Awake()
    {
        m_levelManager = FindObjectOfType<LevelManager>();
    }
    public override void onEnter()
    {
        m_levelManager.m_playEvents.Invoke();
        m_levelManager.m_currentStateCanvas = m_levelManager.m_HUDPlayCanvas;
    }

    public override void onExit()
    {

    }

    public override void onUpdate()
    {
    }
}
