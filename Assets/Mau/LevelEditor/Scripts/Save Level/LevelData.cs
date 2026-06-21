using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public string levelName;
    public int version = 1;
    public int bridges = 0;
    public List<LevelObjectData> objects = new();
}

[Serializable]
public class LevelObjectData
{
    public string id;
    public SerializableVector3 position;
    public SerializableQuaternion rotation;
    public SerializableVector3 scale;
    public List<LevelObjectData> children = new();

    [NonSerialized] public GameObject editorInstance;
}

[Serializable]
public struct SerializableVector3
{
    public float x, y, z;

    public static SerializableVector3 From(Vector3 v)
        => new SerializableVector3 { x = v.x, y = v.y, z = v.z };

    public Vector3 ToVector3()
        => new Vector3(x, y, z);
}

[Serializable]
public struct SerializableQuaternion
{
    public float x, y, z, w;

    public static SerializableQuaternion From(Quaternion q)
        => new SerializableQuaternion { x = q.x, y = q.y, z = q.z, w = q.w };

    public Quaternion ToQuaternion()
        => new Quaternion(x, y, z, w);
}
