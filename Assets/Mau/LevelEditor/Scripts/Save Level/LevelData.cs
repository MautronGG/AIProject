using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public string levelName;
    public int version = 1;
    public List<LevelObjectData> objects = new List<LevelObjectData>();
}

[Serializable]
public class LevelObjectData
{
    public string id;
    public SerializableVector3 position;
    public SerializableQuaternion rotation;
    public SerializableVector3 scale;

    public List<LevelObjectData> children;
}

[Serializable]
public struct SerializableVector3
{
    //public float x, y, z;
    //public SerializableVector3(float x,float y,float z){this.x=x;this.y=y;this.z=z;}
    //public SerializableVector3(Vector3 v){x=v.x;y=v.y;z=v.z;}
    //public Vector3 ToVector3() => new Vector3(x,y,z);
    //public static SerializableVector3 From(Vector3 v) => new SerializableVector3(v);

    public float x, y, z;

    public static SerializableVector3 From(Vector3 v)
        => new SerializableVector3 { x = v.x, y = v.y, z = v.z };

    public Vector3 ToVector3()
        => new Vector3(x, y, z);
}

[Serializable]
public struct SerializableQuaternion
{
    //public float x,y,z,w;
    //public SerializableQuaternion(float x,float y,float z,float w){this.x=x;this.y=y;this.z=z;this.w=w;}
    //public SerializableQuaternion(Quaternion q){x=q.x;y=q.y;z=q.z;w=q.w;}
    //public Quaternion ToQuaternion() => new Quaternion(x,y,z,w);
    //public static SerializableQuaternion From(Quaternion q) => new SerializableQuaternion(q);

    public float x, y, z, w;

    public static SerializableQuaternion From(Quaternion q)
        => new SerializableQuaternion { x = q.x, y = q.y, z = q.z, w = q.w };

    public Quaternion ToQuaternion()
        => new Quaternion(x, y, z, w);

}
