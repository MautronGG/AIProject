using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EditorBorderManager : MonoBehaviour
{
    public Transform m_leftBorder;
    public Transform m_rightBorder;
    public Transform m_topBorder;
    public Transform m_bottomBorder;

    public Vector3 BottomLeft => new Vector3(m_leftBorder.position.x + 2, m_bottomBorder.position.y + 2, 0);
    public Vector3 TopRight => new Vector3(m_rightBorder.position.x - 2, m_topBorder.position.y - 2, 0);

}
