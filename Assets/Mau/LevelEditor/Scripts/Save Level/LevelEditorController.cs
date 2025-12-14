using System.Collections.Generic;
using UnityEngine;

public class LevelEditorController : MonoBehaviour
{
    // optionally assign a parent that contains all placed editor objects
    public Transform placedObjectsParent;

    [ContextMenu("Save Current Level")]
    public void SaveCurrentLevelToFile()
    {
        SaveCurrentLevel("level_saved"); // default filename; or pass name param
    }

    public void SaveCurrentLevel(string filenameWithoutExt)
    {
        var level = new LevelData();
        level.levelName = filenameWithoutExt;
        level.version = 1;

        var parent = placedObjectsParent ? placedObjectsParent : transform;

        var placedVoids = parent.GetComponentsInChildren<EditorBorderDragger>();
        foreach (var obj in placedVoids)
        {
            int idInt;
            if (!int.TryParse(obj.id, out idInt) || idInt < 0)
            {
                continue;
            }
            var d = new LevelObjectData();
            d.id = obj.id;
            d.position = SerializableVector3.From(obj.transform.position);
            d.rotation = SerializableQuaternion.From(obj.transform.rotation);
            d.scale = SerializableVector3.From(obj.transform.localScale);
            level.objects.Add(d);
        }

        var placedObjects = parent.GetComponentsInChildren<EditorItem>();
        foreach (var obj in placedObjects)
        {
            int idInt;
            if (!int.TryParse(obj.id, out idInt) || idInt < 0)
            {
                continue;
            }

            bool isPairedObject = false;
            var editorPairedObjects = obj.gameObject.GetComponent<EditorPairedObject>();

            if (editorPairedObjects != null)
            {
                isPairedObject = true;
            }

            if (isPairedObject)
            {
                var d = new LevelObjectData();
                d.id = obj.id;
                d.position = SerializableVector3.From(obj.transform.position);
                d.rotation = SerializableQuaternion.From(obj.transform.rotation);
                d.scale = SerializableVector3.From(obj.transform.localScale);

                d.children = new List<LevelObjectData>();

                d.children.Add(new LevelObjectData()
                {
                    id = obj.id,
                    position = SerializableVector3.From(editorPairedObjects.m_childA.transform.localPosition),
                    rotation = SerializableQuaternion.From(editorPairedObjects.m_childA.transform.localRotation),
                    scale = SerializableVector3.From(editorPairedObjects.m_childA.transform.localScale)
                });

                d.children.Add(new LevelObjectData()
                {
                    id = obj.id,
                    position = SerializableVector3.From(editorPairedObjects.m_childB.transform.localPosition),
                    rotation = SerializableQuaternion.From(editorPairedObjects.m_childB.transform.localRotation),
                    scale = SerializableVector3.From(editorPairedObjects.m_childB.transform.localScale)
                });

                level.objects.Add(d);
            }
            else
            {
                var d = new LevelObjectData();
                d.id = obj.id;
                d.position = SerializableVector3.From(obj.transform.position);
                d.rotation = SerializableQuaternion.From(obj.transform.rotation);
                d.scale = SerializableVector3.From(obj.transform.localScale);
                level.objects.Add(d);
            }

        }


        SaveLoadManager.SaveLevel(level, filenameWithoutExt);
    }
}