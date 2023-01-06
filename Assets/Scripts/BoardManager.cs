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
using System.IO;

public class BoardManager : MonoBehaviour
{
    [SerializeField] GameObject queenFab;
    [SerializeField] GameObject heuristicFab;
    [SerializeField] Transform chessBoard;


    [SerializeField] TMP_InputField customBoardConfig;
    [SerializeField] TMP_InputField MutationRate;
    [SerializeField] TMP_InputField PopulationSize;

    [SerializeField] TMP_InputField Temperature;
    [SerializeField] TMP_InputField CoolingFactor;

    [SerializeField] TMP_InputField IterationNum;
    [SerializeField] TMP_InputField StateNum;

    [SerializeField] TMP_InputField RandomRestartIterations;

    [SerializeField] Button randomBoardBtn;
    [SerializeField] Button customBoardBtn;
    [SerializeField] Button calculateHeuristicBtn;
    [SerializeField] Button clearHeuristicBtn;
    [SerializeField] Button geneticAlgorithm;
    [SerializeField] Button pGeneticAlgorithm;


    [SerializeField] Button backBtn;

    [SerializeField] Button algoBackBtn;
    [SerializeField] Button avaialableAlgorithms;
    [SerializeField] Button runAnalysisAlgorithms;

    [SerializeField] Button simpleHillClimbing;
    [SerializeField] Button simpleHillClimbingRun;
    [SerializeField] Button simpleHillClimbingBack;
    [SerializeField] TextMeshProUGUI SimpleHCMsgTxt;

    [SerializeField] Button stochasticHillClimbing;
    [SerializeField] Button stochasticHillClimbingRun;
    [SerializeField] Button stochasticHillClimbingBack;

    [SerializeField] Button simulatedAnnealing;
    [SerializeField] Button annealingRun;
    [SerializeField] Button annealingBack;

    [SerializeField] Button localBeam;
    [SerializeField] Button beamRun;
    [SerializeField] Button beamBack;

    [SerializeField] Button randomRestart;
    [SerializeField] Button randomRestartRun;
    [SerializeField] Button randomRestartBack;

    [SerializeField] TextMeshProUGUI boardConfigTxt;
    [SerializeField] TextMeshProUGUI messageTxt;
    [SerializeField] TextMeshProUGUI attacksTxt;
    [SerializeField] TextMeshProUGUI geneticInitialPopulationTxt;
    [SerializeField] TextMeshProUGUI geneticPopulationTxt;
    [SerializeField] TextMeshProUGUI geneticTitleTxt;

    [SerializeField] GameObject algoPanel;
    [SerializeField] GameObject analysisPanel;
    [SerializeField] GameObject geneticPanel;
    [SerializeField] GameObject annealingPanel;
    [SerializeField] GameObject beamPanel;
    [SerializeField] GameObject simpleHillClimbingPanel;
    [SerializeField] GameObject stochasticHillPanel;
    [SerializeField] GameObject randomRestartPanel;

    [SerializeField] Button RunAnalysisBtn;
    [SerializeField] Button exitAnalysis;

    [SerializeField] TextMeshProUGUI analysisIterations;
    [SerializeField] TMP_InputField  analysisIterationLimit;
    [SerializeField] Toggle HC_Toogle;
    [SerializeField] Toggle SC_Toogle;
    [SerializeField] Toggle LB_Toogle;
    [SerializeField] Toggle SA_Toogle;
    [SerializeField] Toggle RR_Toogle;
    [SerializeField] Toggle GA_Toogle;

    [SerializeField] TextMeshProUGUI HC_Time;
    [SerializeField] TextMeshProUGUI HC_USol;
    [SerializeField] TextMeshProUGUI HC_sucR;
    [SerializeField] TextMeshProUGUI HC_Step;

    [SerializeField] TextMeshProUGUI SC_Time;
    [SerializeField] TextMeshProUGUI SC_USol;
    [SerializeField] TextMeshProUGUI SC_sucR;
    [SerializeField] TextMeshProUGUI SC_Step;

    [SerializeField] TextMeshProUGUI LB_Time;
    [SerializeField] TextMeshProUGUI LB_USol;
    [SerializeField] TextMeshProUGUI LB_sucR;
    [SerializeField] TextMeshProUGUI LB_Step;

    [SerializeField] TextMeshProUGUI SA_Time;
    [SerializeField] TextMeshProUGUI SA_USol;
    [SerializeField] TextMeshProUGUI SA_sucR;
    [SerializeField] TextMeshProUGUI SA_Step;

    [SerializeField] TextMeshProUGUI RR_Time;
    [SerializeField] TextMeshProUGUI RR_USol;
    [SerializeField] TextMeshProUGUI RR_sucR;
    [SerializeField] TextMeshProUGUI RR_Step;

    [SerializeField] TextMeshProUGUI GA_Time;
    [SerializeField] TextMeshProUGUI GA_USol;
    [SerializeField] TextMeshProUGUI GA_sucR;
    [SerializeField] TextMeshProUGUI GA_Step;

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
    SimulatedAnnealing sAAlgo;
    LocalBeam lBAlgo;

    List<Tuple<string[], int, int>> InitPopulation = new List<Tuple<string[], int, int>>();

    private int maxiterations = 0;

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
        simulatedAnnealing.onClick.AddListener(simulatedAnnealingPanel);
        localBeam.onClick.AddListener(localBeamPanel);
        simpleHillClimbing.onClick.AddListener(simpleHillClimbingAPanel);
        stochasticHillClimbing.onClick.AddListener(stochasticHillAPanel);
        randomRestart.onClick.AddListener(randomRestartAPanel);

        avaialableAlgorithms.onClick.AddListener(availableAlgorithmsPanel);
        runAnalysisAlgorithms.onClick.AddListener(openAnalysisPanel);
        pGeneticAlgorithm.onClick.AddListener(geneticAlgorithmRun);
        annealingRun.onClick.AddListener(simulatedAnnealingRun);
        beamRun.onClick.AddListener(localBeamRun);
        randomRestartRun.onClick.AddListener(perfromRandomRestart);
        simpleHillClimbingRun.onClick.AddListener(perfromHillClimbing);

        // Back button Listeners
        algoBackBtn.onClick.AddListener(exitAllPanels);
        backBtn.onClick.AddListener(exitAllPanels);
        simpleHillClimbingBack.onClick.AddListener(exitAllPanels);
        stochasticHillClimbingBack.onClick.AddListener(exitAllPanels);
        beamBack.onClick.AddListener(exitAllPanels);
        annealingBack.onClick.AddListener(exitAllPanels);
        randomRestartBack.onClick.AddListener(exitAllPanels);

        RunAnalysisBtn.onClick.AddListener(RunAnalysis);
        exitAnalysis.onClick.AddListener(exitAllPanels);

        MutationRate.text = 5 + "";
        PopulationSize.text = 10 + "";

        Temperature.text = 120 + "";
        CoolingFactor.text = 0.5 + "";

        IterationNum.text = 50000 + "";
        StateNum.text = 5 + "";

        MUTATION_RATE = mutationRate;
        algorithmG = new GeneticAlgorithm();
        sAAlgo = new SimulatedAnnealing();
        lBAlgo = new LocalBeam();

        maxiterations = System.Convert.ToInt32(analysisIterationLimit.text);

        Debug.Log("Fac : " + getChildLimit(10));

    }

    void Update()
    {
        printInitPopulation();
        geneticTitleTxt.text = "Generation " +  generation;
        HC_Time.text = "" + timeHC;
        SC_Time.text = "" + timeSC;
        LB_Time.text = "" + timeLB;
        SA_Time.text = "" + timeSA;
        RR_Time.text = "" + timeRR;
        GA_Time.text = "" + timeGA;

        HC_USol.text = "" + solHC.Count;
        SC_USol.text = "" + solSC.Count;
        LB_USol.text = "" + solLB.Count;
        SA_USol.text = "" + solSA.Count;
        RR_USol.text = "" + solRR.Count;
        GA_USol.text = "" + solGA.Count;

        // Steps
        HC_Step.text = "" + stepHC;
        SC_Step.text = "" + stepSC;
        LB_Step.text = "" + stepLB;
        SA_Step.text = "" + stepSA;
        RR_Step.text = "" + stepRR;
        GA_Step.text = "" + stepGA;

        // Success Rate
        HC_sucR.text = "" + (100 - (errorHC/(maxiterations/100)));
        SC_sucR.text = "" + (100 - (errorSC/(maxiterations/100)));
        LB_sucR.text = "" + (100 - (errorLB/(maxiterations/100)));
        SA_sucR.text = "" + (100 - (errorSA/(maxiterations/100)));
        RR_sucR.text = "" + (100 - (errorRR/(maxiterations/100)));
        GA_sucR.text = "" + (100 - (errorGA/(maxiterations/100)));

        analysisIterations.text = "Iterations : " + iterations;
        // timeLB
        // timeSA
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
                //Debug.Log("New Board : " +  string.Join("", nBoard) + " " + attacks);
                heuristicMatrix[j,i] = attacks;
                GameObject hueObj = putTextAt(i+1, j+1, attacks.ToString());
                heuristicObj[i,j] = hueObj;
                hasHue = true;
            }
        }
        //printMatrix(heuristicMatrix);
        calculateLowestHeuristic();
    }

    private string[] calculateLHeuristics(string[] board) {
        var hMatrix = new int[8,8];
        int lowest = 1000;
        string[] lBoard = board;
        for (int i = 0; i < 8; i ++) {
            int queenP = System.Convert.ToInt32(board[i]);
            for (int j = 0; j < 8; j++) {
                string[] nBoard = (string[]) board.Clone();
                if (j == (queenP -1)) {
                    hMatrix[j,i] = -1;
                    continue;
                }
                nBoard[i] = (j+1).ToString();
                int attacks = calculateAttacks(nBoard);
                hMatrix[j,i] = attacks;
                if (attacks > -1 && attacks < lowest) {
                  lowest = attacks;
                  lBoard = nBoard;
                }
            }
        }
        return lBoard;
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
            //Debug.Log("[" + string.Join(" ", data.Item1) + "]-" + data.Item2);
            value += "[" + string.Join(" ", data.Item1) + "]-" + data.Item2 + "\n";
        }
        geneticPopulationTxt.text = value;
    }

    void printInitPopulation() {
        string value = "";
        foreach (Tuple<string[], int, int> data in InitPopulation) {
            //Debug.Log("[" + string.Join(" ", data.Item1) + "]-" + data.Item2);
            value += "[" + string.Join(" ", data.Item1) + "]-" + data.Item2 + "\n";
        }
        geneticInitialPopulationTxt.text = value;
    }

    private void geneticAlgorithmPanel() {
        geneticPanel.SetActive(true);
    }

    private void geneticAlgorithmRun() {
      StartCoroutine(geneticAlgorithmRunCo());
    }
    private IEnumerator geneticAlgorithmRunCo() {
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
            //Debug.Log("Checking Generation : " + generation);
            generation += 1;

            if (generation % 100 == 0) {
              yield return null;
            }

            if (generation == 50000) {
                break;
            }
        }

        if (generation < 50000) {
            Debug.Log("Solution    : " + string.Join(" ", population[0].Item1) + "-" + population[0].Item2);
            solGA.Add(string.Join(" ", population[0].Item1));
            printPopulation(population);
            if (stepGA == 0) {
              stepGA = generation;
            } else {
              stepGA = (stepGA + generation)/2;
            }
        } else {
            Debug.Log("No Solution : " + string.Join(" ", population[0].Item1) + "-" + population[0].Item2);
            errorGA++;
            printPopulation(population);
        }
    }

    private void perfromRandomRestart() {
      runRandomRestart(true);
    }

    private string[] sToA(string board) {
      string[] array = new string[8];
      for (int i = 0; i < 8; i ++) {
          array[i] = board[i] + "";
      }
      return array;
    }

    private string[] runRandomRestart(bool stat) {
      int s_steps = 0;
      int maxIterRR = System.Convert.ToInt32(RandomRestartIterations.text);
      string[] board = randomBoard();
      int boarH = calculateAttacks(board);

      for (int i = 0; i < maxIterRR; i++){
        while (true) {
          int hue = calculateAttacks(board);

          string[] nBoard = calculateLHeuristics(board);
          int nHue = calculateAttacks(nBoard);

          //Debug.Log("Hue : " + nHue + "-" + hue);

          if (nHue >= hue) {
            break;
          }

          board = nBoard;
          s_steps++;
          boarH = nHue;
          //Debug.Log("Steps : " + stepRR);
          //Debug.Log("Board : " + string.Join("", board));
        }
        if (boarH > 0) {
          board = randomBoard();
          boarH = calculateAttacks(board);
        } else {
          if (stat) {
            clearBoard();
            boardConfig = board;
            for (int k = 0; k < 8; k++) {
              //Debug.Log("KK " + k);
                queensObj[k] = putQueenAt(k + 1, System.Convert.ToInt32(board[k]));
            }
          }
          //Debug.Log("Solution : " + string.Join("", board));
          if (stepRR == 0) {
            stepRR = s_steps;
          } else {
            stepRR = (stepRR + s_steps)/2;
          }
          return board;
        }
      }
      errorRR += 3;
      //Debug.Log("Failed RR.");
      return null;
    }

    private void perfromHillClimbing() {
      SimpleHCMsgTxt.enabled = false;
      runHillClimbing(true);
    }

    private string[] runHillClimbing(bool stat) {
      int s_steps = 0;
      int maxIterRR = System.Convert.ToInt32(RandomRestartIterations.text);
      string[] board = randomBoard();
      int boarH = calculateAttacks(board);
      while (true) {
        int hue = calculateAttacks(board);

        string[] nBoard = calculateLHeuristics(board);
        int nHue = calculateAttacks(nBoard);

        //Debug.Log("Hue : " + nHue + "-" + hue);

        if (nHue >= hue) {
          break;
        }

        board = nBoard;
        s_steps++;
        boarH = nHue;
        //Debug.Log("Steps : " + stepRR);
        //Debug.Log("Board : " + string.Join("", board));
      }
      if (boarH > 0) {
        if (stat) {
          Debug.Log("Error");
          SimpleHCMsgTxt.gameObject.SetActive(true);
          SimpleHCMsgTxt.enabled = true;
        }
        return null;
      } else {
        if (stat) {
          clearBoard();
          boardConfig = board;
          for (int k = 0; k < 8; k++) {
            //Debug.Log("KK " + k);
              queensObj[k] = putQueenAt(k + 1, System.Convert.ToInt32(board[k]));
          }
        }
        //Debug.Log("Solution : " + string.Join("", board));
        if (stepHC == 0) {
          stepHC = s_steps;
        } else {
          stepHC = (stepHC + s_steps)/2;
        }
        return board;
      }
    }

    // private HashSet<string> getNeighbours(string[] board) {
    //     HashSet<string> neighbours = new HashSet<string>();
    //     for (int row = 0; row < 8; row++) {
    //       for (int col = 0; col < 8; col++) {
    //         if (col != System.Convert.ToInt32(board[row])) {
    //           var nBoard = board;
    //           nBoard[row] = "" + col;
    //           neighbours.Add(string.Join("", nBoard));
    //           Debug.Log("N " + string.Join("", nBoard));
    //         }
    //       }
    //     }
    //     return neighbours;
    // }



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
        if (annealingPanel.activeSelf) {
            annealingPanel.SetActive(false);
        }
        if (beamPanel.activeSelf) {
            beamPanel.SetActive(false);
        }
        if (randomRestartPanel.activeSelf) {
            randomRestartPanel.SetActive(false);
        }
        if (simpleHillClimbingPanel.activeSelf) {
            simpleHillClimbingPanel.SetActive(false);
        }
        if (stochasticHillPanel.activeSelf) {
            stochasticHillPanel.SetActive(false);
        }
        if (algoPanel.activeSelf) {
          algoPanel.SetActive(false);
        }
        if (analysisPanel.activeSelf) {
          analysisPanel.SetActive(false);
        }
    }

    private void availableAlgorithmsPanel() {
      algoPanel.SetActive(true);
    }

    private void openAnalysisPanel() {
      analysisPanel.SetActive(true);
    }

    private void simulatedAnnealingPanel() {
        annealingPanel.SetActive(true);
    }

    private void simulatedAnnealingRun() {
        SimulatedAnnealing run = new SimulatedAnnealing();
        float temperature = float.Parse(Temperature.text),
            coolingFactor = float.Parse(CoolingFactor.text);
        int[] res = run.solve(8, 50000, temperature, coolingFactor);
        clearBoard();
        for (int i = 0; i < 8; i++) {
            //Debug.Log(i + ":" + res[i]);
            queensObj[i] = putQueenAt(i + 1, res[i]);
        }
    }

    private void localBeamPanel() {
        beamPanel.SetActive(true);
    }

    private void localBeamRun() {
        LocalBeam run = new LocalBeam();
        int INum = System.Convert.ToInt32(IterationNum.text),
            SNum = System.Convert.ToInt32(StateNum.text);
        int[] res = run.solve(8, INum, SNum);
        clearBoard();
        for (int i = 0; i < 8; i++) {
            //Debug.Log(i + ":" + res[i] );
            queensObj[i] = putQueenAt(i + 1, res[i]+1);
        }
    }

    private void simpleHillClimbingAPanel() {
        simpleHillClimbingPanel.SetActive(true);
    }

    private void stochasticHillAPanel() {
        stochasticHillPanel.SetActive(true);
    }

    private void randomRestartAPanel() {
        randomRestartPanel.SetActive(true);
    }

    long timeHC = 0;
    long timeSC = 0;
    long timeLB = 0;
    long timeSA = 0;
    long timeRR = 0;
    long timeGA = 0;
    int iterations = 0;

    float errorHC = 0;
    float errorSC = 0;
    float errorLB = 0;
    float errorSA = 0;
    float errorRR = 0;
    float errorGA = 0;

    int stepHC = 0;
    int stepSC = 0;
    int stepLB = 0;
    int stepSA = 0;
    int stepRR = 0;
    int stepGA = 0;

    HashSet<string> solHC = new HashSet<string>();
    HashSet<string> solSC = new HashSet<string>();
    HashSet<string> solLB = new HashSet<string>();
    HashSet<string> solSA = new HashSet<string>();
    HashSet<string> solRR = new HashSet<string>();
    HashSet<string> solGA = new HashSet<string>();

    private void RunAnalysis() {
      timeHC = 0;
      timeSC = 0;
      timeLB = 0;
      timeSA = 0;
      timeRR = 0;
      timeGA = 0;
      iterations = 0;

      solHC.Clear();
      solSC.Clear();
      solLB.Clear();
      solSA.Clear();
      solRR.Clear();
      solGA.Clear();

      errorHC = 0;
      errorSC = 0;
      errorLB = 0;
      errorSA = 0;
      errorRR = 0;
      errorGA = 0;

      stepHC = 0;
      stepSC = 0;
      stepLB = 0;
      stepSA = 0;
      stepRR = 0;
      stepGA = 0;

      removeFiles();
      string title = "iterations,time,steps,error,solutions";
      writeString("report_HC", title);
      writeString("report_SC", title);
      writeString("report_LB", title);
      writeString("report_SA", title);
      writeString("report_RR", title);
      writeString("report_GA", title);

      StartCoroutine(RunAnalysisCo());
    }

    private IEnumerator RunAnalysisCo() {

      LocalBeam runLB = new LocalBeam();
      int INum = System.Convert.ToInt32(IterationNum.text),
          SNum = System.Convert.ToInt32(StateNum.text);

      SimulatedAnnealing runSA = new SimulatedAnnealing();
      float temperature = float.Parse(Temperature.text),
          coolingFactor = float.Parse(CoolingFactor.text);

      long start = 0;
      maxiterations = System.Convert.ToInt32(analysisIterationLimit.text);
      for (int i = 0; i < maxiterations; i++) {
        // Simple Hill climbing
        if (HC_Toogle.isOn) {
          try {
            start = nanoTime();
            solHC.Add(string.Join("", runHillClimbing(false)));
            timeHC = (timeHC + (nanoTime() - start)/1000)/2;
            string d_HC = iterations + "," + timeHC + "," + stepHC + "," + errorHC + "," + solHC.Count;
            writeString("report_HC", d_HC);
          } catch(Exception e) {
            Debug.Log("E : " + e);
            errorHC++;
          }
        }

        // stochastic Hill Climbing
        if (SC_Toogle.isOn) {
          try {
            string d_SC = iterations + "," + timeSC + "," + stepSC + "," + errorSC + "," + solSC.Count;
            writeString("report_SC", d_SC);
          } catch(Exception e) {
            Debug.Log("E : " + e);
            errorSC++;
          }
        }

        // Local Beam Search
        if (LB_Toogle.isOn) {
          try {
            start = nanoTime();
            int[] resLB = runLB.solve(8, INum, SNum);
            solLB.Add(string.Join("", resLB));
            timeLB = (timeLB + (nanoTime() - start)/1000)/2;
            errorLB = (errorLB + runLB.getSuccessRate())/2;
            stepLB = (stepLB + runLB.getSteps())/2;
            Debug.Log("Error Rate : " + errorLB);
            string d_LB = iterations + "," + timeLB + "," + stepLB + "," + errorLB + "," + solLB.Count;
            writeString("report_LB", d_LB);
          } catch (Exception e) {
            Debug.Log("E : " + e);
            errorLB++;
          }
        }

        // Simulated Annealing
        if (SA_Toogle.isOn) {
          try {
            start = nanoTime();
            int[] resSA = runSA.solve(8, 50000, temperature, coolingFactor);
            solSA.Add(string.Join("", resSA));
            timeSA = (timeSA + (nanoTime() - start)/1000)/2;
            stepSA = (stepSA + runSA.getSteps())/2;
            string d_SA = iterations + "," + timeSA + "," + stepSA + "," + errorSA + "," + solSA.Count;
            writeString("report_SA", d_SA);
          } catch (Exception e) {
            Debug.Log("E : " + e);
            errorSA++;
          }
        }

        // Random Restart
        if (RR_Toogle.isOn) {
          try {
            start = nanoTime();
            solRR.Add(string.Join("", runRandomRestart(false)));
            timeRR = (timeRR + (nanoTime() - start)/1000)/2;
            string d_RR = iterations + "," + timeRR + "," + stepRR + "," + errorRR + "," + solRR.Count;
            writeString("report_RR", d_RR);
          } catch(Exception e) {
            Debug.Log("E : " + e);
            errorRR++;
          }
        }

        // Genetic Algorithm
        if (GA_Toogle.isOn) {
          try {
            start = nanoTime();
            geneticAlgorithmRun();
            timeGA =  (timeGA + (nanoTime() - start)/1000)/2;
            string d_GA = iterations + "," + timeGA + "," + stepGA + "," + errorGA + "," + solGA.Count;
            writeString("report_GA", d_GA);
          } catch (Exception e) {
            Debug.Log("E : " + e);
          }
        }
        iterations++;
        yield return null;
      }
      generateRandomBoard();
    }

    private static long nanoTime() {
      long nano = 10000L * System.Diagnostics.Stopwatch.GetTimestamp();
      nano /= TimeSpan.TicksPerMillisecond;
      nano *= 100L;
      return nano;
    }

    private void writeString(string fileT, string msg) {
        StreamWriter writer = new StreamWriter(fileT + ".csv", true);
        writer.WriteLine(msg);
        writer.Close();
    }

    private void removeFiles() {
      if(File.Exists("report_HC.csv")) {
        File.Delete("report_HC.csv") ;
      }

      if(File.Exists("report_SC.csv")) {
        File.Delete("report_SC.csv") ;
      }

      if(File.Exists("report_LB.csv")) {
        File.Delete("report_LB.csv") ;
      }

      if(File.Exists("report_SA.csv")) {
        File.Delete("report_SA.csv") ;
      }

      if(File.Exists("report_RR.csv")) {
        File.Delete("report_RR.csv") ;
      }

      if(File.Exists("report_GA.csv")) {
        File.Delete("report_GA.csv") ;
      }

    }

}
// 81163444
// 56745676
//
