using UnityEngine;

public class EditorItemButton : MonoBehaviour
{
    public string objectId;

    public void OnClick()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

        var manager = EditorManager.Instance;
        var prefabs = manager.GetPrefab(objectId);

        LevelObjectData data = new()
        {
            id = objectId,
            position = SerializableVector3.From(pos),
            rotation = SerializableQuaternion.From(Quaternion.identity),
            scale = SerializableVector3.From(Vector3.one)
        };

        manager.currentLevel.objects.Add(data);

        GameObject editorObj = Instantiate(
            prefabs.editorPrefab,
            pos,
            Quaternion.identity,
            manager.editorRoot
        );

        EditorItem item = editorObj.GetComponent<EditorItem>();
        item.id = objectId;
        item.data = data;
        data.editorInstance = editorObj;

        manager.DoAction(new CreateObjectAction(data));
    }
}
