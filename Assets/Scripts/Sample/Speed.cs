
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Entities;

public class Speed : MonoBehaviour
{
    public float speed;
}
public class Axis : MonoBehaviour
{
    public Vector3 value;
}
public class Center : MonoBehaviour
{
    public Vector3 value;
}

public class compo : IComponentData
{
    public Vector3 value;
}