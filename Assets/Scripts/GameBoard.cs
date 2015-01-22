using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameBoard : MonoBehaviour {

    public int width;
    public int height;

    public GameObject[,] board;

    void Awake()
    {
        board = new GameObject[width, height];
    }

	// Use this for initialization
	void Start ()
    {
        List<GameObject> background = Resources.LoadAll<GameObject>("Background").ToList();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var bgTile = Instantiate(
                	background[0],
               		new Vector3(transform.position.x + i, transform.position.y + j, 0.1f),
                	Quaternion.identity) as GameObject;
                	
                board[i, j] = null;
            }
        }
	}

    public void ClearRows()
    {
        for (int i = 0; i < height; i++)
        {
            // assume the row is full
            bool rowFull = true;
            for (int j = 0; j < width; j++)
            {
                if (board[j, i] == null)
                {
                    rowFull = false;
                    break;
                }
            }

            if (rowFull)
            {
            	// drop all the pieces above the row by down by 1
                for (int l = i+1; l <= height; l++)
                {
                    if (l < 0)
                    {
                        continue;
                    }
                    for (int m = 0; m < width; m++)
                    {
                        if (l < height && l > 0)
                        {
                            if (board[m, l] != null)
                            {
                                board[m, l].transform.Translate(0.0f, -1.0f, 0.0f);
                            }
                            board[m, l - 1] = board[m, l];
                        }
                        else if (l == height)
                        {
                            board[m, l - 1] = null;
                        }
                    }
                }

                i--;  // essentially don't update when we have dropped all other rows
            }
        }
    }
}
