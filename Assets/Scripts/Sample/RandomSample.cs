using Maphy.Mathematics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class RandomSample : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
        Maphy.Mathematics.Random random = new Maphy.Mathematics.Random(100);
        Unity.Mathematics.Random random1 = new Unity.Mathematics.Random(100);

        Debug.LogError(math.sin(30));
        Debug.LogError(Unity.Mathematics.math.sin(30));
        Debug.LogError(math.sin(12.5));
        Debug.LogError(Unity.Mathematics.math.sin(12.5));
        Debug.LogError(math.sin(365));
        Debug.LogError(Unity.Mathematics.math.sin(365));

        int length = 100000;
        fix a = 0;
        fix b = 0;
        Profiler.BeginSample("RandomSample");
        for (int i = 0; i < length; i++)
        {
            a = random.NextFix();
            b = random1.NextFloat();
            if (a - b > 1)
                Debug.LogWarning(a + b);
        }
        Profiler.EndSample();
    }
}
