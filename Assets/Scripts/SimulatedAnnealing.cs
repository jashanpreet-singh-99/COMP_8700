using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatedAnnealing : MonoBehaviour {
    
    public int[] solve(int n, int maxNumOfIterations, float temperature, float coolingFactor) {
        
        int[] randomState = Utils.generateRandomState(n);

        int costToBeat = Utils.getHeuristicCost(randomState);

        for (int x = 0; x < maxNumOfIterations && costToBeat > 0; x++) {
            randomState = makeMove(randomState, costToBeat, temperature);
            costToBeat = Utils.getHeuristicCost(randomState);
            temperature = Mathf.Max(temperature * coolingFactor, 0.01f);
        }

        return costToBeat == 0 ? randomState : null; 
    }

    private int[] makeMove(int[] r, int costToBeat, float temp) {
        int n = r.Length;
        

        while (true) {
            int nCol = (int)(Random.Range(0.0f, 1.0f) * n);
            int nRow = (int)(Random.Range(0.0f, 1.0f) * n);
            int tmpRow = r[nCol];
            r[nCol] = nRow;

            int cost = Utils.getHeuristicCost(r);
            if (cost < costToBeat)
                return r;

            int dE = costToBeat - cost;
            double acceptProb = Mathf.Min(1, Mathf.Exp(dE / temp));

            if (UnityEngine.Random.Range(0.0f, 1.0f) < acceptProb)
                return r;

            r[nCol] = tmpRow;
        }

    }   

}
