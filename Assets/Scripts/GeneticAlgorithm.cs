using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm
{

    public int fitnessScore(string[] board) {
        int attacks = BoardManager.calculateAttacks(board);
        return (28 - attacks);
    }

    public string[] mutateBoard(string[] board) {
        int mutationRate =  UnityEngine.Random.Range(0, 100);
        if (mutationRate < BoardManager.MUTATION_RATE) {
            int mutationP = UnityEngine.Random.Range(0, 8);
            int mutation = UnityEngine.Random.Range(1, 9);
            board[mutationP] = mutation.ToString();
            return board;
        }
        return board;
    }

    public List<Tuple<string[], int, int>> crossOver(List<Tuple<string[], int, int>> population, int parent1I, int parent2I) {
        string[] child = new string[8];
        string[] child2 = new string[8];

        var parent1 = population[parent1I].Item1;
        var parent2 = population[parent2I].Item1;
        int crossoverP = UnityEngine.Random.Range(1, 8);
        //Debug.Log("CrossOver Point : " + crossoverP);
        for (int i = 0; i < crossoverP; i++) {
            child[i] = parent1[i];
            child2[i] = parent2[i];
        }
        for (int i = crossoverP; i < 8; i++) {
            child[i] = parent2[i];
            child2[i] = parent1[i];
        }
        child = mutateBoard(child);
        child2 = mutateBoard(child2);
        List<Tuple<string[], int, int>> children = new List<Tuple<string[], int, int>>();
        children.Add(Tuple.Create(child, fitnessScore(child), crossoverP));
        children.Add(Tuple.Create(child2, fitnessScore(child2), crossoverP));
        return children;
    }


}
