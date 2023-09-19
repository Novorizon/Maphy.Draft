using Maphy.Mathematics;
using Maphy.Physics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Entity = Maphy.Physics.Entity;

public class PhysicsWorldSample : MonoBehaviour
{
    World world=new World();
    void Start()
    {
        world.settings.gravity=-9.8;  
    }

    void Update()
    {
        world.Update();
    }

}