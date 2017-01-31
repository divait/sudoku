using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SudokuGenerator : MonoBehaviour {

	public Color error;
	public Color normal;

	public enum Level {EXEasy, Easy, Medium, Hard, EXHard};

	public Level level;

	private Vector2[] posArray;

	[HideInInspector]
	public int answers;

    // The sudoku matrix have all the numbers of the game
    // Every row is consider a 3x3 matrix     
    int[,] sudoku;
	int[,] answer;
    int size = 9;

    public MultiText[] boxes;

   [System.Serializable]
    public class MultiText
    {
        public Text[] matrix3x3;
    }


    // Use this for initialization
    void Start()
    {
        sudoku = new int[size, size];
		answer = new int[size, size];

        createSudoku();
        printMatrix(sudoku);

		level = stringToLevel (PlayerPrefs.GetString ("Level"));
		answers = getLevel (level);
		List<int> array = newRandomArray (answers);
		int k = 0;

		answers = (9 * 9) - answers;

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
				if (array.Contains (k)) {
					boxes [i].matrix3x3 [j].text = sudoku [i, j].ToString ();
					boxes [i].matrix3x3 [j].transform.parent.GetComponent<Button> ().interactable = false;
					answer [i, j] = sudoku[i, j];
				}
				k++;
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }

    void printMatrix(int[,] sudoku) 
    {
        // Init String
        string matrix = "";

        //Init Helpers for x and y in the matrix of the sudoku
        int x = 0, y = 0;
        matrix += " " + sudoku[x, y];

        // The loop that get all numbers in the sudoku matrix
        for (int i = 1; i < sudoku.Length; i++)
        {         
            // Every 9 numbers a line ends        
            if(i%9 == 0)
            {
                matrix += "\n"; // End line

                // Check if there was print already 3 lines (the complete 3x3 matrix)
                if (y+1 >= 9)
                {
                    x++;
                    y = 0;

                    // Add a horizontal divider
                    for (int w = 0; w < size; w++)
                    {
                        matrix += " - ";
                        if (w != 0 && (w + 1) % 3 == 0)
                            matrix += " ";
                    }
                    matrix += "\n";
                } else { // Move to the next row
                    x -= 2;
                    y++;
                }
            } else if (i % 3 == 0) { // Change to the next 3x3 matrix in the same roww
                matrix += " |"; // The vertical divider
                x++;
                y -= 2;
            } else { // Move to the next index
                y++;
            }
            // sudoku[x, y] = x;
            // Add the number of the sudoku in the String
            matrix += " " + sudoku[x, y]; 
        }

        // Show the matrix
        print(matrix);
    }

    void createSudoku()
    {
        initArray(); // Create init 3x3 matrix (Row 4)

        // Create the 3x3 matrix beside the center
        int type = Random.Range(0, 2); // Create a random to select the way to shuffle the matrix
        shuffleRows(4, 3, type); // Creta row 3 matrix 3x3
        type = type == 0 ? 1 : 0;
        shuffleRows(4, 5, type); // Creta row 5 matrix 3x3

        // Create the 3x3 matrix top and bottom of the center
        type = Random.Range(0, 2); // Create a random to select the way to shuffle the matrix
        shuffleColumns(4, 1, type); // Creta row 1 matrix 3x3
        type = type == 0 ? 1 : 0;
        shuffleColumns(4, 7, type); // Creta row 7 matrix 3x3

        // Create the corner matrix 3x3
		int rOc = Random.Range(0, 2);
		type = Random.Range(0, 2);

		// Shuffle by Row or Column
		if(rOc == 0) { // Shuffle by Row
			shuffleRows(1, 0, type);

			type = type == 0 ? 1 : 0;

			shuffleRows(1, 2, type);

			type = Random.Range(0, 2);

			shuffleRows(7, 6, type);

			type = type == 0 ? 1 : 0;

			shuffleRows(7, 8, type);
		} else { // Shuffle by Column
			shuffleColumns(3, 0, type);

			type = type == 0 ? 1 : 0;

			shuffleColumns(3, 6, type);

			type = Random.Range(0, 2);

			shuffleColumns(5, 2, type);

			type = type == 0 ? 1 : 0;

			shuffleColumns(5, 8, type);
		}

    }


	/*
	 int[] misingMatrix = { 0, 2, 6, 8 };
        for (int j = 0; j < misingMatrix.Length; j++)
        {
            int row = 0, column = 0;
            switch(misingMatrix[j])
            {
                case 0:
                    row = 1;
                    column = 3;
                    break;
                case 2:
                    row = 1;
                    column = 5;
                    break;
                case 6:
                    row = 7;
                    column = 3;
                    break;
                case 8:
                    row = 7;
                    column = 5;
                    break;
            }
            int rOc = Random.Range(0, 2);
            type = Random.Range(0, 2);

            // Shuffle by Row or Column
            if(rOc == 0) { // Shuffle by Row
                shuffleRows(row, misingMatrix[j], type);
            } else { // Shuffle by Column
                shuffleColumns(column, misingMatrix[j], type);
            }
        }
        */


	/**
	 * Shuffle the init central matrix 3x3
	 * 
	 * */
    void initArray ()
    {
        int[] init = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        int i = init.Length;
        int temp;
        while (i > 0)
        {
            int j = Random.Range(0, i);
            temp = init[--i];
            init[i] = init[j];
            init[j] = temp;
        }

        for (int j = 0; j < init.Length; j++)
        {
            sudoku[4, j] = init[j];
        }
    }

    /**
    *  shuffle rows in the origin matrix 3x3 in the result matrix.
    *
    * 012 top
    * 345 mid
    * 678 bot
    *
    **/
    void  shuffleRows(int origin, int result, int type)
    {
        // 0 = top, 1 = bottom

        // top
        if(type == 0)
        {
            sudoku[result, 0] = sudoku[origin, 6];
            sudoku[result, 1] = sudoku[origin, 7];
            sudoku[result, 2] = sudoku[origin, 8];

            sudoku[result, 3] = sudoku[origin, 0];
            sudoku[result, 4] = sudoku[origin, 1];
            sudoku[result, 5] = sudoku[origin, 2];

            sudoku[result, 6] = sudoku[origin, 3];
            sudoku[result, 7] = sudoku[origin, 4];
            sudoku[result, 8] = sudoku[origin, 5];
        } else { // Bottom
            sudoku[result, 0] = sudoku[origin, 3];
            sudoku[result, 1] = sudoku[origin, 4];
            sudoku[result, 2] = sudoku[origin, 5];

            sudoku[result, 3] = sudoku[origin, 6];
            sudoku[result, 4] = sudoku[origin, 7];
            sudoku[result, 5] = sudoku[origin, 8];

            sudoku[result, 6] = sudoku[origin, 0];
            sudoku[result, 7] = sudoku[origin, 1];
            sudoku[result, 8] = sudoku[origin, 2];
        }
    }

    /**
    *  shuffle columns in the origin matrix 3x3 in the result matrix.
    *
    * 036 left
    * 147 mid
    * 258 right
    *
    **/
    void shuffleColumns(int origin, int result, int type)
    {
        // 0 = left, 1 = right

        // left
        if (type == 0)
        {
            sudoku[result, 0] = sudoku[origin, 2];
            sudoku[result, 3] = sudoku[origin, 5];
            sudoku[result, 6] = sudoku[origin, 8];

            sudoku[result, 1] = sudoku[origin, 0];
            sudoku[result, 4] = sudoku[origin, 3];
            sudoku[result, 7] = sudoku[origin, 6];

            sudoku[result, 2] = sudoku[origin, 1];
            sudoku[result, 5] = sudoku[origin, 4];
            sudoku[result, 8] = sudoku[origin, 7];
        }
        else { // right
            sudoku[result, 0] = sudoku[origin, 1];
            sudoku[result, 3] = sudoku[origin, 4];
            sudoku[result, 6] = sudoku[origin, 7];

            sudoku[result, 1] = sudoku[origin, 2];
            sudoku[result, 4] = sudoku[origin, 5];
            sudoku[result, 7] = sudoku[origin, 8];

            sudoku[result, 2] = sudoku[origin, 0];
            sudoku[result, 5] = sudoku[origin, 3];
            sudoku[result, 8] = sudoku[origin, 6];
        }
    }


	/**
	 * Create a Array of unique numbers 
	 * 
	* */
	private List<int> newRandomArray(int size)
	{
		List<int> numbs = new List<int> ();
		int i = 0;

		int num;
		do {
			num = Random.Range (0, 81);
			if (!numbs.Contains (num)) {
				numbs.Add (num);
				i++;
			}
		} while(i < size);

		return numbs;
	}

	/**
	 * Get the difficult level
	 * 
	**/
	int getLevel (Level level) {
		switch(level) {
		default:
		case Level.EXEasy:
			return Random.Range(50, 72);
		case Level.Easy:
			return Random.Range(36, 50);
		case Level.Medium:
			return Random.Range(32, 36);
		case Level.Hard:
			return Random.Range(28, 32);
		case Level.EXHard:
			return Random.Range(22, 27);
		}
	}

	private Level stringToLevel(string lvl) {
		switch(lvl) {
		default:
		case "EXEasy":
			return Level.EXEasy;
		case "Easy":
			return Level.Easy;
		case "Medium":
			return Level.Medium;
		case "Hard":
			return Level.Hard;
		case "EXHard":
			return Level.EXHard;

		}
	}

	public bool checkedNumber(int number, int x, int y) {
		if (sudoku [x, y] == number) {

			boxes [x].matrix3x3 [y].text = sudoku [x, y].ToString ();
			boxes [x].matrix3x3 [y].transform.parent.GetComponent<Button> ().interactable = false;
			answer [x, y] = sudoku [x, y];
			answers--;

			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{		
					boxes [i].matrix3x3 [j].color = normal;
				}
			}
			return true;
		} else {
			posArray = new Vector2[9];
			int w = 0;

			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{		
					boxes [i].matrix3x3 [j].color = normal;
					if (answer[i, j] == number) {
						posArray[w] = new Vector2 (i, j);
						w++;

						boxes [i].matrix3x3 [j].color = error;
					}
				}
			}
			return false;
		}
	}
}
