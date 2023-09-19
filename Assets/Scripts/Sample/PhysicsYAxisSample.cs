using Maphy.Mathematics;
using Maphy.Physics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Entity = Maphy.Physics.Entity;

public class PhysicsYAxisSample : MonoBehaviour
{
    public int length = 10;
    Dictionary<GameObject, AABB> AABBs = new Dictionary<GameObject, AABB>();
    Dictionary<GameObject, OBB> OBBs = new Dictionary<GameObject, OBB>();
    Dictionary<GameObject, Sphere> Spheres = new Dictionary<GameObject, Sphere>();
    Dictionary<GameObject, Capsule> Capsules = new Dictionary<GameObject, Capsule>();

    Color[] color = new Color[]
    {
                new Color(127 / 255f, 232 / 255f, 10 / 255f),//
                new Color(34 / 255f, 139 / 255f, 34 / 255f),//
                new Color(50 / 255f, 150 / 255f, 200 / 255f),//
                new Color(255 / 255f, 250 / 255f, 250 / 255f),//
    };
    void Start()
    {
        for (int i = 0; i < length; i++)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.position = UnityEngine.Random.insideUnitSphere * 10;
            float x = Mathf.Abs(UnityEngine.Random.insideUnitSphere.x);
            float y = Mathf.Abs(UnityEngine.Random.insideUnitSphere.y);
            float z = Mathf.Abs(UnityEngine.Random.insideUnitSphere.z);
            go.transform.localScale = go.transform.localScale + new Vector3(x, y, z) * 2;
            AABB aabb = new AABB(UnityUtils.math.fix3(go.transform.position - go.transform.localScale / 2), UnityUtils.math.fix3(go.transform.position + go.transform.localScale / 2));
            go.name = "AABB" + i;
            AABBs.Add(go, aabb);
        }
        for (int i = 0; i < length; i++)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.position = UnityEngine.Random.insideUnitSphere * 10;

            float x = Mathf.Abs(UnityEngine.Random.insideUnitSphere.x);
            float y = Mathf.Abs(UnityEngine.Random.insideUnitSphere.y);
            float z = Mathf.Abs(UnityEngine.Random.insideUnitSphere.z);
            go.transform.localScale = go.transform.localScale + new Vector3(x, y, z) * 2;
            OBB obb = new OBB(UnityUtils.math.fix3(go.transform.position - go.transform.localScale / 2), UnityUtils.math.fix3(go.transform.position + go.transform.localScale / 2), quaternion.identity);
            go.AddComponent<Speed>().speed = UnityEngine.Random.Range(1, 3);
            go.AddComponent<Center>().value = UnityEngine.Random.insideUnitSphere * 5;
            go.AddComponent<Axis>().value = Vector3.up;
            go.name = "OBB" + i;
            OBBs.Add(go, obb);
        }
        for (int i = 0; i < length; i++)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = UnityEngine.Random.insideUnitSphere * 10;

            go.transform.localScale = go.transform.localScale * UnityEngine.Random.Range(1, 3);
            Sphere sphere = new Sphere(UnityUtils.math.fix3(go.GetComponent<SphereCollider>().center), new fix(go.transform.localScale.x / 2));
            go.AddComponent<Speed>().speed = UnityEngine.Random.Range(1, 3);
            go.AddComponent<Center>().value = UnityEngine.Random.insideUnitSphere * 5;
            go.AddComponent<Axis>().value = UnityEngine.Random.insideUnitSphere;
            go.name = "Sphere" + i;
            Spheres.Add(go, sphere);
        }
        for (int i = 0; i < length; i++)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            go.transform.position = UnityEngine.Random.insideUnitSphere * 10;

            go.transform.localScale = go.transform.localScale * UnityEngine.Random.Range(1, 3);
            Capsule sphere = new Capsule(UnityUtils.math.fix3(go.GetComponent<CapsuleCollider>().center),
                go.GetComponent<CapsuleCollider>().radius * go.transform.localScale.x,
                go.GetComponent<CapsuleCollider>().height * go.transform.localScale.y,
                quaternion.identity,
                fix3.up);

            go.AddComponent<Speed>().speed = UnityEngine.Random.Range(1, 3);
            go.AddComponent<Center>().value = UnityEngine.Random.insideUnitSphere * 5;
            go.AddComponent<Axis>().value = Vector3.up;
            go.name = "Capsule" + i;
            Capsules.Add(go, sphere);
        }
    }
    void Update()
    {
        foreach (var item in AABBs.Keys.ToList())
        {
            AABB aabb = AABBs[item];
            aabb.Update(UnityUtils.math.fix3(item.transform.position));
            AABBs[item] = aabb;
            item.GetComponent<MeshRenderer>().material.color = color[0];
        }

        foreach (var item in OBBs.Keys.ToList())
        {
            item.transform.RotateAround(item.GetComponent<Center>().value, item.GetComponent<Axis>().value, item.GetComponent<Speed>().speed * Time.deltaTime * 10);
            OBB obb = OBBs[item];
            obb.Update(UnityUtils.math.fix3(item.transform.position), UnityUtils.math.quaternion(item.transform.rotation));
            OBBs[item] = obb;
            item.GetComponent<MeshRenderer>().material.color = color[1];
        }

        foreach (var item in Spheres.Keys.ToList())
        {
            item.transform.RotateAround(item.GetComponent<Center>().value, item.GetComponent<Axis>().value, item.GetComponent<Speed>().speed * Time.deltaTime * 10);
            Sphere sphere = Spheres[item];
            sphere.Update(UnityUtils.math.fix3(item.transform.position));
            Spheres[item] = sphere;
            item.GetComponent<MeshRenderer>().material.color = color[2];
        }


        foreach (var item in Capsules.Keys.ToList())
        {
            item.transform.RotateAround(item.GetComponent<Center>().value, item.GetComponent<Axis>().value, item.GetComponent<Speed>().speed * Time.deltaTime * 10);
            Capsule obb = Capsules[item];
            obb.Update(UnityUtils.math.fix3(item.transform.position), UnityUtils.math.quaternion(item.transform.rotation));
            Capsules[item] = obb;
            item.GetComponent<MeshRenderer>().material.color = color[3];
        }
        AABBDetect();
        OBBDetect();
        SphereDetect();
        CapsuleDetect();
    }

    void AABBDetect()
    {
        foreach (var item in AABBs)
        {
            AABB aabb = item.Value;

            foreach (var item1 in AABBs)
            {
                if (item.Key == item1.Key)
                    continue;
                if (Entity.IsOverlap(item.Value, item1.Value))
                {
                    item.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                    item1.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }

            foreach (var item1 in OBBs)
            {
                if (Entity.IsOverlap(item.Value, item1.Value))
                {
                    item.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                    item1.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }

            foreach (var item1 in Spheres)
            {
                if (Entity.IsOverlap(item.Value, item1.Value))
                {
                    item.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                    item1.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }

            foreach (var item1 in Capsules)
            {
                if (Entity.IsOverlap(item.Value, item1.Value))
                {
                    item.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                    item1.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
        }
    }

    void OBBDetect()
    {
        foreach (var item in OBBs)
        {
            OBB aabb = item.Value;

            foreach (var item1 in OBBs)
            {
                if (item.Key == item1.Key)
                    continue;
                //Profiler.BeginSample("aaa");
                //for (int i = 0; i < 100; i++)
                //{
                //    Physics.IsOverlap(item.Value, item1.Value);
                //}
                //Profiler.EndSample();
                //Profiler.BeginSample("bbb");
                //for (int i = 0; i < 100; i++)
                //{
                //    Physics.IsOverlap(item.Value, item1.Value);
                //}
                //Profiler.EndSample();
                if (Entity.IsOverlap(item.Value, item1.Value))
                {
                    item.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                    item1.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }

            foreach (var item1 in AABBs)
            {
                if (Entity.IsOverlap(item.Value, item1.Value))
                {
                    item.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                    item1.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }

            foreach (var item1 in Spheres)
            {
                if (Entity.IsOverlap(item.Value, item1.Value))
                {
                    item.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                    item1.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }

            foreach (var item1 in Capsules)
            {
                if (Entity.IsOverlap(item.Value, item1.Value))
                {
                    item.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                    item1.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
        }
    }

    void SphereDetect()
    {
        foreach (var item in Spheres)
        {
            Sphere aabb = item.Value;

            foreach (var item1 in Spheres)
            {
                if (item.Key == item1.Key)
                    continue;
                if (Entity.IsOverlap(item.Value, item1.Value))
                {
                    item.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                    item1.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }

            foreach (var item1 in AABBs)
            {
                if (Entity.IsOverlap(item.Value, item1.Value))
                {
                    item.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                    item1.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }

            foreach (var item1 in OBBs)
            {
                if (Entity.IsOverlap(item.Value, item1.Value))
                {
                    item.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                    item1.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }

            foreach (var item1 in Capsules)
            {
                if (Entity.IsOverlap(item.Value, item1.Value))
                {
                    item.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                    item1.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
        }
    }

    void CapsuleDetect()
    {

        foreach (var item in Capsules)
        {
            foreach (var item1 in Capsules)
            {
                if (item.Key == item1.Key)
                    continue;
                if (Entity.IsOverlap(item.Value, item1.Value))
                {
                    item.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                    item1.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }

            foreach (var item1 in AABBs)
            {
                if (Entity.IsOverlap(item.Value, item1.Value))
                {
                    item.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                    item1.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }

            foreach (var item1 in OBBs)
            {
                if (Entity.IsOverlap(item.Value, item1.Value))
                {
                    item.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                    item1.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }

            foreach (var item1 in Spheres)
            {
                if (Entity.IsOverlap(item.Value, item1.Value))
                {
                    item.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                    item1.Key.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
        }
    }
}