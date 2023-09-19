using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cross : MonoBehaviour
{
    public GameObject gameObject1;
    public GameObject gameObject2;
    public GameObject gameObject3;
    // Start is called before the first frame update
    void Start()
    {

    }

    float Cross(Vector2 a, Vector2 b)
    {
        return a.x * b.y - a.y * b.x;
    }
    // Update is called once per frame
    void Update()
    {
        gameObject1.transform.position = new Vector3(gameObject1.transform.position.x, 0, gameObject1.transform.position.z);
        gameObject2.transform.position = new Vector3(gameObject2.transform.position.x, 0, gameObject2.transform.position.z);
        gameObject3.transform.position = new Vector3(gameObject3.transform.position.x, 0, gameObject3.transform.position.z);
        Vector2 a = new Vector3(gameObject1.transform.position.x,gameObject1.transform.position.z).normalized;
        Vector2 b = new Vector3(gameObject2.transform.position.x,gameObject2.transform.position.z).normalized;
        Vector2 c = new Vector3(gameObject3.transform.position.x,  gameObject3.transform.position.z).normalized;

        float ac = Vector2.Dot(a, c);
        float cb = Vector2.Dot(c, b);
        float ab = Vector2.Dot(a, b);
        if (ac * cb > 0)
            print("中间");
        float ab1 = Cross(a, b);
        float ac1 = Cross(a, c);
        float cb1 = Cross(c, b);
        print(ab1);
        print(ac1);
        print(cb1);
        if (ab1 > 0 )//b在a逆时针
            if (ac1 >0&& cb1 > 0)
            print("中间1");

        if(ab1<0)
        if (ac1 < 0 && cb1 <0)
            print("中间2");
    }
}
