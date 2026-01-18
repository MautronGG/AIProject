using UnityEngine;

public class EditorItem : MonoBehaviour
{
    [Tooltip("Unique ID used to save this object. Must match an entry in PrefabDatabase.")]
    public string id;
    [HideInInspector] public LevelObjectData data;

    public EditorManager m_editor;

    public int m_personalID;
    public bool m_canOpenOptions = true;
    public bool m_canDelete = true;

    public bool m_canClick = true;
    public bool m_created = true;

    EditorSpriteFollow m_spriteFollow;

    private void Awake()
    {
        m_editor = GameObject.FindGameObjectWithTag("EditorManager").GetComponent<EditorManager>();

        m_spriteFollow = GetComponent<EditorSpriteFollow>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && m_spriteFollow.m_follow && m_canClick)
        {
            PlaceDown();
        }

        m_canClick = true;
    }

    //private void LateUpdate()
    //{
    //    if (data == null) return;
    //
    //    data.position = SerializableVector3.From(transform.position);
    //    data.rotation = SerializableQuaternion.From(transform.rotation);
    //    data.scale = SerializableVector3.From(transform.localScale);
    //}

    private void OnMouseOver()
    {
        if (m_spriteFollow.m_follow) return;

        if (Input.GetMouseButtonDown(0) && CanInteract())
        {
            PickUp();
        }

        if (Input.GetMouseButtonDown(1) && m_canDelete && CanInteract())
        {
            DeleteItem();
        }
    }

    bool CanInteract()
    {
        if (m_editor.currentMode != EditorMode.Edit) return false;
        if (!m_canOpenOptions) return false;
        if (m_editor.m_isEditing) return false;
        //if (m_editor.m_optionsCanvas.activeInHierarchy) return false;
        if (m_editor.m_pauseCanvas.activeInHierarchy) return false;

        foreach (var button in m_editor.m_buttonSelectionTrackers)
            if (button.IsSelected) return false;

        return true;
    }

    public void PickUp()
    {
        m_editor.m_HUDCanvas.SetActive(false);
        m_spriteFollow.m_follow = true;
        m_editor.m_isEditing = true;
        m_canClick = false;
        m_spriteFollow.m_lastValidPosition = transform.position;
    }

    public void PlaceDown()
    {
        m_spriteFollow.m_follow = false;
        m_editor.m_isEditing = false;
        m_editor.m_HUDCanvas.SetActive(true);

        Vector3 oldPos = m_spriteFollow.m_lastValidPosition;
        Vector3 newPos = transform.position;

        if (m_created)
        {
            m_editor.DoAction(new CreateObjectAction(data));
            m_created = false;
            return;
        }

        if (oldPos != newPos)
        {
            m_editor.DoAction(
                new MoveAction(
                    data,
                    
                    oldPos,
                    newPos
                )
            );
        }
    }

    public void DeleteItem()
    {
        //gameObject.SetActive(false);
        var parent = transform.parent.GetComponent<EditorPairedObject>();
        if (parent)
            m_editor.DoAction(new DeleteAction(parent.GetComponent<EditorItem>()));

        else
            m_editor.DoAction(new DeleteAction(this));
    }

    public void ForceSyncData()
    {
        //if (data == null) return;
        //
        //data.position = SerializableVector3.From(transform.position);
        //data.rotation = SerializableQuaternion.From(transform.rotation);
        //data.scale = SerializableVector3.From(transform.localScale);

        if (data == null) return;

        data.position = SerializableVector3.From(transform.position);
        data.rotation = SerializableQuaternion.From(transform.rotation);
        data.scale = SerializableVector3.From(transform.localScale);

        // Paired object case
        if (TryGetComponent(out EditorPairedObject pair))
        {
            if (data.children == null || data.children.Count <= 0)
                return;

            // CHILD A (WORLD SPACE)
            data.children[0].position = SerializableVector3.From(pair.m_childA.transform.localPosition);
            data.children[0].rotation = SerializableQuaternion.From(pair.m_childA.transform.localRotation);
            data.children[0].scale = SerializableVector3.From(pair.m_childA.transform.localScale);

            // CHILD B (WORLD SPACE)
            data.children[1].position = SerializableVector3.From(pair.m_childB.transform.localPosition);
            data.children[1].rotation = SerializableQuaternion.From(pair.m_childB.transform.localRotation);
            data.children[1].scale = SerializableVector3.From(pair.m_childB.transform.localScale);
        }
    }

}
