using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{

    public static int[] generateAllOneState(int n) {

        return new int[n];
    }

    public static int[] randomizeState(int[] r) {

        for (int i = 0; i < r.Length; i++)
            r[i] = (int)(UnityEngine.Random.Range(0.0f, 1.0f) * r.Length);

        return r;
    }

    public static int[] generateRandomState(int n) {

        return randomizeState(generateAllOneState(n));
    }

    public static int getHeuristicCost(int[] r) {
        int h = 0;
        
        for (int i = 0; i < r.Length; i++)
            for (int j = i + 1; j < r.Length; j++)
                if (r[i] == r[j] || Mathf.Abs(r[i] - r[j]) == j - i)
                    h += 1;

        return h;
    }
}
