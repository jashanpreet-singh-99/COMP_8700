using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Text;
using System;
using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class BoardManager : MonoBehaviour
{
    [SerializeField] GameObject queenFab;
    [SerializeField] GameObject heuristicFab;
    [SerializeField] Transform chessBoard;


    [SerializeField] TMP_InputField customBoardConfig;
    [SerializeField] TMP_InputField MutationRate;
    [SerializeField] TMP_InputField PopulationSize;

    [SerializeField] Button randomBoardBtn;
    [SerializeField] Button customBoardBtn;
    [SerializeField] Button calculateHeuristicBtn;
    [SerializeField] Button clearHeuristicBtn;
    [SerializeField] Button geneticAlgorithm;
    [SerializeField] Button pGeneticAlgorithm;

    [SerializeField] Button backBtn;

    [SerializeField] TextMeshProUGUI boardConfigTxt;
    [SerializeField] TextMeshProUGUI messageTxt;
    [SerializeField] TextMeshProUGUI attacksTxt;
    [SerializeField] TextMeshProUGUI geneticInitialPopulationTxt;
    [SerializeField] TextMeshProUGUI geneticPopulationTxt;
    [SerializeField] TextMeshProUGUI geneticTitleTxt;

    [SerializeField] GameObject geneticPanel;

    [SerializeField] int x;
    [SerializeField] int y;

    [SerializeField] int mutationRate = 5;

    public static int MUTATION_RATE;

    private GameObject[] queensObj;
    private GameObject[,] heuristicObj;
    private string[] boardConfig;
    private int[,] heuristicMatrix;

    private bool hasHue = false;

    private int walkConst = 74;
    private int generation = 1;

    GeneticAlgorithm algorithmG;

    List<Tuple<string[], int, int>> InitPopulation = new List<Tuple<string[], int, int>>();

    void Start()
    {
        queensObj = new GameObject[8];
        heuristicObj = new GameObject[8,8];
        boardConfig = new string[8];
        randomBoardBtn.onClick.AddListener(generateRandomBoard);
        customBoardBtn.onClick.AddListener(generateCustomBoard);
        calculateHeuristicBtn.onClick.AddListener(calculateHeuristics);
        clearHeuristicBtn.onClick.AddListener(clearHeuristics);
        geneticAlgorithm.onClick.AddListener(geneticAlgorithmPanel);
        pGeneticAlgorithm.onClick.AddListener(geneticAlgorithmRun);
        backBtn.onClick.AddListener(exitAllPanels);

        MutationRate.text = 5 + "";
        PopulationSize.text = 10 + "";

        MUTATION_RATE = mutationRate;
        algorithmG = new GeneticAlgorithm();
        Debug.Log("Fac : " + getChildLimit(10));
    }

    void Update()
    {
        printInitPopulation();
        geneticTitleTxt.text = "Generation " +  generation; 
    }

    void generateRandomBoard() {
        clearBoard();
        Debug.Log("Generate Board");
        for (int i = 1; i < 9; i ++) {
            int num = UnityEngine.Random.Range(1, 9);
            Debug.Log("Number :" + num);
            boardConfig[i-1] = num.ToString();
            queensObj[i-1] = putQueenAt(i, num);
        }
        boardConfigTxt.text = string.Join("", boardConfig);
        messageTxt.color = new Color(0,0,0,1);
        messageTxt.text = "Board Generated .....";
        int attacks = calculateAttacks(boardConfig);
        attacksTxt.text = "" + attacks;
    }

    private string[] randomBoard() {
        string[] board = new string[8];
        for (int i = 1; i < 9; i ++) {
            int num = UnityEngine.Random.Range(1, 9);
            board[i-1] = num.ToString();
        }
        return board;
    }

    void generateCustomBoard() {

        clearBoard();
        Debug.Log("String : " + customBoardConfig.text);
        string data = customBoardConfig.text;
        if (data.Length < 8) {
            messageTxt.text = "Invalid board, Enter between 1-8.";
            messageTxt.color = new Color(1,0,0,1);
            return;
        }
        for (int i = 1; i < 9; i++) {
            int num = System.Convert.ToInt32(data[i-1] + " ");
            if (num < 1 && num > 8) {
                messageTxt.text = "Invalid board, Enter between 1-8.";
                messageTxt.color = new Color(1,0,0,1);
                break;
            }
            Debug.Log("num : " + i + " : " + num);
            boardConfig[i-1] = num.ToString();
            queensObj[i-1] = putQueenAt(i, num);
        }
        messageTxt.color = new Color(0,0,0,1);
        messageTxt.text = "Board Generated .....";
        int attacks = calculateAttacks(boardConfig);
        attacksTxt.text = "" + attacks;
    }

    void clearBoard() {
        if (hasHue) {
            clearHeuristics();
        }
        for (int i = 0 ; i < 8; i++) {
            GameObject.Destroy(queensObj[i]);
        }
    }

    GameObject putQueenAt(int x, int y) {
        var Q1 = Instantiate(queenFab, calculateBoxPosition(x, y), Quaternion.identity);
        Q1.transform.SetParent(chessBoard, false);
        Q1.transform.localScale  = new Vector3(1,1,1);
        return Q1;
    }

    // Convert board location to pixels.
    Vector3 calculateBoxPosition(int x, int y) {
        x -= 1;
        y = (8 - y) % 8;
        int multiX = 1;
        int multiY = 1;
        if (x < 4) {
            x = 3 - x;
            multiX = -1;
        }
        if (y < 4) {
            y = 3 - y;
            multiY = -1;
        }
            int posX = x % 4;
            int posY = y % 4;
            float tansX = (walkConst/2) + walkConst * posX;
            float tansY = (walkConst/2) + walkConst * posY;
            return new Vector3(multiX * tansX, multiY * tansY, 0);
    }

    static int[,] covertToMatrix(string[] data) {
        int [,] board = new int[8,8];
        for (int i = 0; i < 8; i++) {
            int queenP = System.Convert.ToInt32(data[i]);
            for (int j = 0; j < 8; j++) {
                if (j == queenP-1) {
                    board[j,i] = 1;
                } else {
                    board[j,i] = 0;
                }
            }
        }
        return board;
    }

    void printMatrix(int[,] board) {
        for (int i = 0; i < 8; i++) {
            string s = "";
            for (int j = 0; j < 8; j++) {
                s += board[i,j] + "   ";
            }
            Debug.Log(i + "     " + s);
        }
    }

    static bool add_check(HashSet<Tuple<int,int, int, int>> attacks, int x, int y, int x1, int y1) {
        Tuple<int,int, int, int> t1 = Tuple.Create(x, y, x1, y1);
        Tuple<int,int, int, int> t2 = Tuple.Create(x1, y1, x, y);
        if (attacks.Contains(t1) || attacks.Contains(t2)) {
            //Debug.Log("FALSE : " + x + " " + y + " " + x1 + " " + y1);
            return false;
        }
        //Debug.Log("TRUE  : " + x + " " + y + " " + x1 + " " + y1);
        return true;
    }

    public static int calculateAttacks(string[] board) {
        int[,] iBoard = covertToMatrix(board);
        HashSet<Tuple<int,int, int, int>> attacks = new HashSet<Tuple<int,int, int, int>>();

        for (int i = 0; i < 8; i ++ ) {
            
            int queenPos = System.Convert.ToInt32(board[i]) - 1;

            // Check right of the current queen
            for (int k = i+1; k < 8; k++) {
                if (iBoard[queenPos, k] == 1) {
                    if (add_check(attacks, queenPos, i,queenPos, k)) {
                        attacks.Add(Tuple.Create(queenPos, i,queenPos, k));
                    }
                }
            }

            // Check Diagonal Left Below
            int row = i -1;
            int col = queenPos + 1;
            while (row >= 0 && col < 8) {
                if (iBoard[col, row] == 1) {
                    if (add_check(attacks, queenPos, i, col, row)) {
                        attacks.Add(Tuple.Create(queenPos, i, col, row));
                    }
                }
                row -= 1;
                col += 1;
            }

            // Check Diagonal Left Above
            row = i - 1;
            col = queenPos - 1;
            while (row >= 0 && col >= 0) {
                if (iBoard[col, row] == 1) {
                    if (add_check(attacks, queenPos, i, col, row)) {
                        attacks.Add(Tuple.Create(queenPos, i, col, row));
                    }
                }
                row -= 1;
                col -= 1;
            }

            // Check Diagonal Right Below
            row = i + 1;
            col = queenPos + 1;
            while (row < 8 && col < 8) {
                if (iBoard[col, row] == 1) {
                    if (add_check(attacks, queenPos, i, col, row)) {
                        attacks.Add(Tuple.Create(queenPos, i, col, row));
                    }
                }
                row += 1;
                col += 1;
            }

            // Check Diagonal Right Above
            row = i + 1;
            col = queenPos - 1;
            while (row < 8 && col >= 0) {
                if (iBoard[col, row] == 1) {
                    if (add_check(attacks, queenPos, i, col, row)) {
                        attacks.Add(Tuple.Create(queenPos, i, col, row));
                    }
                }
                row += 1;
                col -= 1;
            }
        }
        return attacks.Count;
    }

    private void calculateHeuristics() {
        if (hasHue) {
            clearHeuristics();
        }
        if (System.Convert.ToInt32(boardConfig[7]) < 1 || System.Convert.ToInt32(boardConfig[7]) > 8) {
            messageTxt.text = "Please Generate Board.";
            messageTxt.color = new Color(1,0,0,1);
            return;
        }
        heuristicMatrix = new int[8,8];
        for (int i = 0; i < 8; i ++) {
            int queenP = System.Convert.ToInt32(boardConfig[i]);
            for (int j = 0; j < 8; j++) {
                string[] nBoard = (string[]) boardConfig.Clone();
                if (j == (queenP -1)) {
                    heuristicMatrix[j,i] = -1;
                    continue; 
                }
                nBoard[i] = (j+1).ToString();
                int attacks = calculateAttacks(nBoard);
                Debug.Log("New Board : " +  string.Join("", nBoard) + " " + attacks);
                heuristicMatrix[j,i] = attacks;
                GameObject hueObj = putTextAt(i+1, j+1, attacks.ToString());
                heuristicObj[i,j] = hueObj;
                hasHue = true;
            }
        }
        printMatrix(heuristicMatrix);
        calculateLowestHeuristic();
    }
    
    GameObject putTextAt(int x, int y, string text) {
        heuristicFab.GetComponent<TextMeshProUGUI>().text = text;
        var Q1 = Instantiate(heuristicFab, calculateBoxPosition(x, y), Quaternion.identity);
        Q1.transform.SetParent(chessBoard, false);
        Q1.transform.localScale  = new Vector3(1,1,1);
        return Q1;
    }

    private void clearHeuristics() {
        for (int i = 0 ; i < 8; i++) {
            for (int j=0; j < 8; j++) {
                GameObject.Destroy(heuristicObj[i, j]);
            }
        }
        hasHue = false;
    }

    private void calculateLowestHeuristic() {
        HashSet<Tuple<int,int>> lowestHeuristics = new HashSet<Tuple<int, int>>(); 
        int lowest = 1000;
        for (int i = 0; i < 8; i ++) {
            for (int j = 0; j < 8; j++) {
                if (lowest > heuristicMatrix[i,j] && heuristicMatrix[i,j] > -1) {
                    lowest = heuristicMatrix[i,j];
                    lowestHeuristics = new HashSet<Tuple<int, int>>();
                    lowestHeuristics.Add(Tuple.Create(i,j));
                } else if (lowest == heuristicMatrix[i,j]) {
                    lowestHeuristics.Add(Tuple.Create(i,j));
                }
            }
        }
        foreach (Tuple<int, int> position in lowestHeuristics) {
            int i = position.Item1;
            int j = position.Item2;
            heuristicObj[j,i].GetComponent<TextMeshProUGUI>().color =  new Color(1,1,1,1);
        }
    }

    private List<Tuple<string[], int, int>> getHighestFitness(List<Tuple<string[], int, int>> population) {
        population.Sort((x,y) => y.Item2.CompareTo(x.Item2));
        return population;
    }

    // void printPopulation() {
    //     string value = "";
    //     foreach (Tuple<string[], int, int> data in population) {
    //         Debug.Log("[" + string.Join(" ", data.Item1) + "]-" + data.Item2);
    //         value += "[" + string.Join(" ", data.Item1) + "]-" + data.Item2 + "\n";
    //     }
    //     geneticPopulationTxt.text = value;
    // }

    void printPopulation(List<Tuple<string[], int, int>> population) {
        string value = "";
        foreach (Tuple<string[], int, int> data in population) {
            Debug.Log("[" + string.Join(" ", data.Item1) + "]-" + data.Item2);
            value += "[" + string.Join(" ", data.Item1) + "]-" + data.Item2 + "\n";
        }
        geneticPopulationTxt.text = value;
    }

    void printInitPopulation() {
        string value = "";
        foreach (Tuple<string[], int, int> data in InitPopulation) {
            Debug.Log("[" + string.Join(" ", data.Item1) + "]-" + data.Item2);
            value += "[" + string.Join(" ", data.Item1) + "]-" + data.Item2 + "\n";
        }
        geneticInitialPopulationTxt.text = value;
    }

    private void geneticAlgorithmPanel() {
        geneticPanel.SetActive(true);
    }

    private void geneticAlgorithmRun() {
        int POPULATION_SIZE = System.Convert.ToInt32(PopulationSize.text);
        MUTATION_RATE = System.Convert.ToInt32(MutationRate.text);
        List<Tuple<string[], int, int>> population = new List<Tuple<string[], int, int>>();

        while (population.Count < POPULATION_SIZE) {
            var board = randomBoard();
            int fitnessScore = algorithmG.fitnessScore(board);
            population.Add(Tuple.Create(board, fitnessScore, -1));
        }
        population = getHighestFitness(population);
        InitPopulation = population;

        generation = 1;
        while (population.Count > 0 && population[0].Item2 < 28) {
            List<Tuple<string[], int, int>> newPopulation = new List<Tuple<string[], int, int>>();

            int limit = getChildLimit(population.Count);
            int i = 0;
            int j = 1;
            while (newPopulation.Count < population.Count) {
                newPopulation.AddRange(algorithmG.crossOver(population, i, j));
                //Debug.Log("Child : " + i + " " + j);
                j += 1;
                if (j == limit) {
                    i += 1;
                    j = i + 1;
                }
            }
            
            population = getHighestFitness(newPopulation);
            printPopulation(population);
            Debug.Log("Checking Generation : " + generation);
            generation += 1;

            if (generation == 50000) {
                break;
            }         
        }
        if (generation < 50000) {
            Debug.Log("Solution    : " + string.Join(" ", population[0].Item1) + "-" + population[0].Item2);
        } else {
            Debug.Log("No Solution : " + string.Join(" ", population[0].Item1) + "-" + population[0].Item2);
        }
    }

    private int getChildLimit(int populationSize) {
        int num = 1;
        while (populationSize > (factorial(num - 1)*2)) {
            num += 1;
        }
        return num;
    }

    private int factorial(int num) {
        if (num < 2) {
            return 1;
        }
        return num * factorial(num - 1);
    }

    void exitAllPanels() {
        if (geneticPanel.activeSelf) {
            geneticPanel.SetActive(false);
        }
    }
    
}
// 81163444
// 56745676
//
