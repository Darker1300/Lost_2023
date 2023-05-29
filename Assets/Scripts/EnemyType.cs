using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType : MonoBehaviour
{
    public enum Type { Fish, Tentacle }
    public Type type;

    private static readonly Dictionary<Type, Vector3> rippleScale = new Dictionary<Type, Vector3>
    {
        {Type.Fish, new Vector3(2f, 2f, 2f)},
        {Type.Tentacle, new Vector3(4f, 4f, 4f)}
    };

    public Vector3 GetScale()
    {
        return rippleScale[type];
    }
}

