
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Entities;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

using Collider = Maphy.Physics.Collider;
using World = Unity.Entities.World;
using Entity = Unity.Entities.Entity;

public class Position:IComponentData
{

}

public class PhysicsECS : MonoBehaviour
{
    public float speed;

    public void Start()
    {
        Unity.Physics.
        World world= GetComponent<World>();
       

        Entity entity = world.EntityManager.CreateEntity();
        Position position = new Position();
        world.EntityManager.AddComponent<Collider>(entity);
        world.EntityManager.AddComponent<Rigid>(entity);

    }
}