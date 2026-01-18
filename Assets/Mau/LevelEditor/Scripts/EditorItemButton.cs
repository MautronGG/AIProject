using UnityEngine;
using System.Collections.Generic;

public class EditorItemButton : MonoBehaviour
{
    public string objectId;

    public void OnClick()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

        var manager = EditorManager.Instance;
        var prefabs = manager.GetPrefab(objectId);

        // --- Create editor object ---
        GameObject editorObj = Instantiate(
            prefabs.editorPrefab,
            pos,
            Quaternion.identity,
            manager.editorRoot
        );

        EditorItem item = editorObj.GetComponent<EditorItem>();

        // --- Create base level data ---
        LevelObjectData data = new LevelObjectData
        {
            id = objectId,
            position = SerializableVector3.From(editorObj.transform.position),
            rotation = SerializableQuaternion.From(editorObj.transform.rotation),
            scale = SerializableVector3.From(editorObj.transform.localScale)
        };

        //data.editorInstance = editorObj;

        // --- HANDLE PAIRED OBJECTS HERE ---
        if (editorObj.TryGetComponent(out EditorPairedObject paired))
        {
            data.children = new List<LevelObjectData>
            {
                new LevelObjectData
                {
                    id = objectId,
                    position = SerializableVector3.From(paired.m_childA.transform.localPosition),
                    rotation = SerializableQuaternion.From(paired.m_childA.transform.localRotation),
                    scale = SerializableVector3.From(paired.m_childA.transform.localScale)
                },
                new LevelObjectData
                {
                    id = objectId,
                    position = SerializableVector3.From(paired.m_childB.transform.localPosition),
                    rotation = SerializableQuaternion.From(paired.m_childB.transform.localRotation),
                    scale = SerializableVector3.From(paired.m_childB.transform.localScale)
                }
            };
        }
        else
        {
            data.children = null;
        }

        // --- Assign references ---
        item.id = objectId;
        item.data = data;

        manager.currentLevel.objects.Add(data);

        // --- Start dragging ---
        item.PickUp();

        // --- Undo support ---
        manager.DoAction(new CreateObjectAction(data));
    }
}
