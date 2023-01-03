using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalBeam : MonoBehaviour {
    
    public int[] solve(int n, int maxNumOfIterations, int numOfStates) {
        int[][] states = new int[numOfStates][];

        for (int i = 0; i < numOfStates; i++)
            states[i] = Utils.generateRandomState(n);


        for (int x = 0; x < maxNumOfIterations; x++) {

            int[][] newStates = new int[n * numOfStates][];
            for (int i = 0; i < numOfStates; i++) {
                int costToBeat = Utils.getHeuristicCost(states[i]);

                // if solved
                if (costToBeat == 0)
                    return states[i];

                for (int col = 0; col < n; col++) {
                    newStates[i * n + col] = makeMove(states[i], col, costToBeat);

                    // if stuck
                    if (newStates[i * n + col] == null)
                        newStates[i * n + col] = Utils.generateRandomState(n);
                }

            }
            System.Array.Sort(newStates, Comparer<int[]>.Create((x, y) => Utils.getHeuristicCost(x).CompareTo(Utils.getHeuristicCost(y))));
            states = newStates.Take(numOfStates).ToArray();
        }

        return null;
    }

    private int[] makeMove(int[] r, int col, int costToBeat) {
        int n = r.Length;

        for (int row = 0; row < n; row++) {
            
            if (row == r[col])
                continue;

            int tmpRow = r[col];
            r[col] = row;
            int cost = Utils.getHeuristicCost(r);
            if (costToBeat > cost) {
                r[col] = row;
                return r;
            }
            r[col] = tmpRow;
        }

        return null;
    }

    
}
