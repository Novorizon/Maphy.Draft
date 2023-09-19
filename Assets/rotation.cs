using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

using Unity.Mathematics;
public class rotation : MonoBehaviour
{
    public GameObject obj;
    void Start()
    {
        Quaternion q= obj. transform.rotation;
        quaternion quaternion= obj.transform.rotation;
        print(quaternion);
        print(q.eulerAngles);
    }

    // Update is called once per frame
    void Update()
    {
        //quaternion.Euler(0,0,0);
        Quaternion q = obj.transform.rotation;
        quaternion quaternion = obj.transform.rotation;
        //print(quaternion);
        //print(q.eulerAngles);
        Unity.Mathematics.quaternion.Euler(0, 0, 10);

        q = Quaternion.Euler(1.56f, 1.62f, 59.23f);
        //quaternion(0.01881894f, 0.005562271f, 0.493907f, 0.8692933f);
        var angle = q.eulerAngles;
        print(q);
        print(angle);
    }
}
