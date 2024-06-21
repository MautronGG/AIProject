using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFixObject : MonoBehaviour
{
    [Tooltip("Manager object Fix")]
    [SerializeField]
    FixColorManager m_fixColorManager;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            spriteRenderer.sprite = m_fixColorManager.getSprite("Red");
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            spriteRenderer.sprite = m_fixColorManager.getSprite("White");
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            spriteRenderer.sprite = m_fixColorManager.getSprite("Blue");
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            spriteRenderer.sprite = m_fixColorManager.getSprite("Green");
        }
    }
}
