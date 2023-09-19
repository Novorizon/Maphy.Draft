using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maphy.Mathematics;
using UnityEngine.Profiling;
using Unity.Mathematics;

public class Performance : MonoBehaviour
{
    public int count = 100000;
    void Start()
    {
        List<float> a = new List<float>();
        a.Add(2);
        a.Add(123);
        a.Add(12345);
        a.Add(12345678);
        a.Add(fix.Max32);
        for (int i = 0; i < a.Count; i++)
        {
            Debug.LogError(Maphy.Mathematics.math.sqrt(a[i]));
            Debug.LogError(Unity.Mathematics.math.sqrt(a[i]));
        }

        BoxCollider
        fix aaa = 2;
        fix b = 1/aaa;
    }

    // Update is called once per frame
    void Update()
    {
        Profiler.BeginSample("fix3");
        for (int i = 0; i < count; i++)
        {
            fix3 f3 = new fix3(1, 1, 1);
        }
        Profiler.EndSample();

        Profiler.BeginSample("float3");
        for (int i = 0; i < count; i++)
        {
            float3 f3 = new float3(1, 1, 1);
        }
        Profiler.EndSample();

        //Sqrt
        //Profiler.BeginSample("fix sqrt");
        //for (int i = 0; i < count; i++)
        //{
        //    Mathematica.math.sqrt(count);
        //}
        //Profiler.EndSample();

        //Profiler.BeginSample("float dqrt");
        //for (int i = 0; i < count; i++)
        //{
        //    Unity.Mathematics.math.sqrt(count);
        //}
        //Profiler.EndSample();


        //abs
        fix fix1 = 12345;
        float f1 = 12345;
        Profiler.BeginSample("fix abs");
        for (int i = 0; i < count; i++)
        {
            Maphy.Mathematics.math.abs(fix1);
        }
        Profiler.EndSample();

        Profiler.BeginSample("float abs");
        for (int i = 0; i < count; i++)
        {
            Unity.Mathematics.math.abs(f1);
        }
        Profiler.EndSample();



        //sin
        Profiler.BeginSample("fix sin");
        for (int i = 0; i < count; i++)
        {
            Maphy.Mathematics.math.sin(fix1);
        }
        Profiler.EndSample();

        Profiler.BeginSample("float sin");
        for (int i = 0; i < count; i++)
        {
            Unity.Mathematics.math.sin(f1);
        }
        Profiler.EndSample();

        //+
        Profiler.BeginSample("fix +");
        for (int i = 0; i < count; i++)
        {
            fix result = fix1 + fix1;
        }
        Profiler.EndSample();

        Profiler.BeginSample("float +");
        for (int i = 0; i < count; i++)
        {
            float result = f1 + f1;
        }
        Profiler.EndSample();

        //*
        Profiler.BeginSample("fix *");
        for (int i = 0; i < count; i++)
        {
            fix result = fix1 * fix1;
        }
        Profiler.EndSample();

        Profiler.BeginSample("float *");
        for (int i = 0; i < count; i++)
        {
            float result = f1 * f1;
        }
        Profiler.EndSample();
    }
}
