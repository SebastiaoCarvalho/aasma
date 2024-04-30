using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.AI;

public class LevelCreator : MonoBehaviour
{
    //Grid configuration
    private int width;
    private int height;
    private float cellSize;

    public string gridName;
    private string gridPath;
    string[,] textLines;

    [Header("Prefabs")]
    public GameObject guardPrefab;
    public GameObject prisonerPrefab;
    public GameObject wallPrefab;

    [Header("Spawn Conglomerates")]
    public Transform wallSpawn;
    public Transform agentsSpawn;

    // Create the grid according to the text file set in the "Assets/Resources/grid.txt"
    public  void GridMapVisual()
    {

        int prisonerCounter = 0;
        int guardCounter = 0;

        int width = textLines.GetLength(0);
        int height = textLines.GetLength(1);

        //Informing the grid of nodes that are not walkable
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++) {

                // We are reading the textLines from the top left till the bottom right, we need to adjust accordingly
                var x = j;
                var y = height - i - 1;

                if (textLines[i, j] == "1")
                {
                    var wall = Instantiate(wallPrefab, wallSpawn);
                    wall.transform.localPosition = new Vector3(x * cellSize, 1.0f, y * cellSize);
                    wall.transform.localScale *= cellSize;

                }

                else if (textLines[i, j] == "P")
                {
                    // Prisoner
                    var prisoner = Instantiate(prisonerPrefab, agentsSpawn);
                    prisoner.transform.localPosition = new Vector3(x * cellSize, 0.0f, y * cellSize);
                    prisoner.transform.localScale *= cellSize * 2;
                    prisoner.name = "Prisoner" + prisonerCounter;
                    prisonerCounter++;
                }
                    
                else if (textLines[i, j] == "G")
                {
                    // Guard
                    var guard = Instantiate(guardPrefab, agentsSpawn);
                    guard.transform.localPosition = new Vector3(x * cellSize, 0.0f, y * cellSize);
                    guard.transform.localScale *= cellSize * 2;
                    guard.name = "Guard" + guardCounter;
                    guardCounter++;
                }

            }

    }

    public void CleanMap()
    {
        var wallNumber = wallSpawn.childCount;

        List<GameObject> toDelete = new List<GameObject>();

        for(int i = 0; i <wallNumber  ;i++)
        { 
           toDelete.Add(wallSpawn.GetChild(i).gameObject);
        }

        var monsterNumber = agentsSpawn.childCount;

        for (int i = 0; i < monsterNumber; i++)
        {
            toDelete.Add(agentsSpawn.GetChild(i).gameObject);
        }

        foreach(var d in toDelete)
        {
            DestroyImmediate(d);
        }

    }

    // Reading the text file that where the map "definition" is stored
    public void LoadGrid()
    {
        gridPath = "Assets/Resources/" + gridName + ".txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(gridPath);
        var fileContent = reader.ReadToEnd();
        reader.Close();
        var lines = fileContent.Split("\n"[0]);

        //Calculating Height and Width from text file
        height = lines.Length;
        width = lines[0].Length - 1;

        // CellSize Formula 
        cellSize = 100.0f / (width);

        textLines = new string[height, width];
        int i = 0;
        foreach (var l in lines)
        {
            var words = l.Split();
            var j = 0;

            var w = words[0];

            foreach (var letter in w)
            {
                textLines[i, j] = letter.ToString();
                j++;

                if (j == textLines.GetLength(1))
                    break;
            }

            i++;
            if (i == textLines.GetLength(0))
                break;
        }

    }

}