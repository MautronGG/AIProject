using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCharacterSpawner : MonoBehaviour
{
    public enum CharaColor { Red, Green, Blue };
    public enum CharaType { Player, Door }
    public CharaColor m_color;
    public CharaType m_type;
}
