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
    }

    public override void onExit()
    {

    }

    public override void onUpdate()
    {
        if (!m_levelManager.m_canPlay)
        {
            m_stateMachine.SetState(m_stateMachine.m_onEditorState);
        }
    }
}
