using UnityEngine;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof(GameBoard))]
[RequireComponent(typeof(Spawner))]

public class InputManager : MonoBehaviour 
{
    private GameObject _currentAlloy = null;
    private Spawner _spawner = null;
    private GameBoard _gameBoard = null;
    private Vector3 _initalPos;
    
    public float timeIncrements;

    public enum Direction { Up, Down, Left, Right };
    public enum Rotation { Clockwise, CounterClockwise };

    void Awake()
    {
        _spawner = this.GetComponentInChildren<Spawner>();
        _gameBoard = this.GetComponent<GameBoard>();

        foreach (Transform item in transform)
        {
            if (item.name == "AlloySpawner")
            {
                _initalPos = item.position;
            }
        }
    }

	// Use this for initialization
	void Start ()
    {
        MakeNewAlloy();
        InvokeRepeating("DropAlloy", 0.5f, timeIncrements);
	}

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Quit"))
        {
            Application.Quit();
        }
        
        if (Input.GetButtonDown("Next"))
        {
        	Destroy(_currentAlloy);
        	_currentAlloy = _spawner.NewAlloy();
        	return;
        }
        
        bool left = Input.GetButtonDown("Left");
        bool right = Input.GetButtonDown("Right");
        bool down = Input.GetButtonDown("Down");
       	bool up = Input.GetButtonDown("Up");

        bool counterClockwise = Input.GetButtonDown("Counter Clockwise");
        bool clockwise = Input.GetButtonDown("Clockwise");
        
        var ingots = _currentAlloy.GetComponentsInChildren<Transform>().Where(transform => transform.tag == "Ingot");

        if (left)
        {
            MoveAlloy(_currentAlloy, Direction.Left);
			ingots = _currentAlloy.GetComponentsInChildren<Transform>().Where(transform => transform.tag == "Ingot");
			if (Any(OutOfBounds, ingots, _gameBoard) || Any (Collides, ingots, _gameBoard))
			{
				MoveAlloy(_currentAlloy, Direction.Right);
			}
        }
        if (right)
        {
            MoveAlloy(_currentAlloy, Direction.Right);
			ingots = _currentAlloy.GetComponentsInChildren<Transform>().Where(transform => transform.tag == "Ingot");
			if (Any(OutOfBounds, ingots, _gameBoard) || Any (Collides, ingots, _gameBoard))
			{
				MoveAlloy(_currentAlloy, Direction.Left);
			}
        }
        if (down)
        {
            MoveAlloy(_currentAlloy, Direction.Down);
			ingots = _currentAlloy.GetComponentsInChildren<Transform>().Where(transform => transform.tag == "Ingot");
			if (Any(OutOfBounds, ingots, _gameBoard) || Any (Collides, ingots, _gameBoard))
            {
            	MoveAlloy(_currentAlloy, Direction.Up);
            }
        }
        if (up)
        {
            MoveAlloy(_currentAlloy, Direction.Up);
        }

        if (counterClockwise)
        {
        	RotateAlloy(_currentAlloy, Rotation.CounterClockwise);
			ingots = _currentAlloy.GetComponentsInChildren<Transform>().Where(transform => transform.tag == "Ingot");
			if (Any(OutOfBounds, ingots, _gameBoard) || Any (Collides, ingots, _gameBoard))
			{
				RotateAlloy(_currentAlloy, Rotation.Clockwise);
			}
       	}
        if (clockwise)
        {
        	RotateAlloy(_currentAlloy, Rotation.Clockwise);
			ingots = _currentAlloy.GetComponentsInChildren<Transform>().Where(transform => transform.tag == "Ingot");
			if (Any(OutOfBounds, ingots, _gameBoard) || Any (Collides, ingots, _gameBoard))
			{
				RotateAlloy(_currentAlloy, Rotation.CounterClockwise);
			}
      	}
      	
		ingots = _currentAlloy.GetComponentsInChildren<Transform>().Where(t => t.tag == "Ingot");
      	if (Any (Stops, ingots, _gameBoard))
      	{
      		SaveCurrent(ingots);
      		MakeNewAlloy();
      	}
	}

    public void DropAlloy()
    {
        MoveAlloy(_currentAlloy, Direction.Down);
    }

    private void RotateAlloy(GameObject alloy, Rotation rot)
    {
		_currentAlloy.GetComponent<Alloy>().Rotate(rot);
    }

    private void MoveAlloy(GameObject alloy, Direction d)
    {
        _currentAlloy.GetComponent<Alloy>().Translate(d);
    }

    private TResult Map<TResult>(Func<Transform, GameBoard, TResult> f, 
    	IEnumerable<Transform> ingots,
    	GameBoard board, TResult initial) where TResult : IEquatable<TResult>
    {
        foreach (var ingot in ingots)
        {
            TResult temp = f(ingot, board);
            if (!initial.Equals(temp))
            {
                return temp;
            }
        }
        return initial;
    }

    private bool Any(Func<Transform, GameBoard, bool> f, IEnumerable<Transform> ingots, GameBoard board)
    {
        return Map<bool>(f, ingots, board, false);
    }

    private bool OutOfBounds(Transform ingot, GameBoard board)
    {
        int x = Convert.ToInt32(ingot.transform.position.x);
        int y = Convert.ToInt32(ingot.transform.position.y);
        //Debug.Log(string.Format("{0}, {1}", x, y));
        if (x < 0 || x >= board.width)
        {
            return true;
        }
        if (y < 0)
        {
            return true;
        }
        return false;
    }

    private bool Collides(Transform ingot, GameBoard board)
    {
        int x = Convert.ToInt32(ingot.transform.position.x);
        int y = Convert.ToInt32(ingot.transform.position.y);
        if ((x >= 0 && x < board.width) &&
            (y >= 0 && y < board.height) && 
            (board.board[x, y] != null))
        {
            return true;
        }
        return false;
    }

    // assume the ingot is on the board
    private bool Stops(Transform ingot, GameBoard board)
    {
        int x = Convert.ToInt32(ingot.transform.position.x);
        int y = Convert.ToInt32(ingot.transform.position.y) - 1;
		//Debug.Log(string.Format("{0}, {1}", x, y));
        // last line
        if (y < 0)
        {
            return true;
        }
        // above the board
        if (y >= board.height)
        {
            return false;
        }
        // piece below
        if (board.board[x, y] != null)
        {
            return true;
        }
        return false; 
    }

    private bool SaveCurrent(IEnumerable<Transform> pieces)
    {
        bool done = false;
        foreach (var piece in pieces)
        {
            int x = Convert.ToInt32(piece.position.x);
            int y = Convert.ToInt32(piece.position.y);
			Debug.Log(string.Format("{0}, {1}", x, y));
            if (x < 0 || x >= _gameBoard.width || y < 0)
            {
                continue;
            }
            if (y >= _gameBoard.height)
            {
                done = true;
                continue;
            }
            _gameBoard.board[x, y] = piece.gameObject;
        }
        return done;
    }

    private void MakeNewAlloy()
    {
        _currentAlloy = _spawner.NewAlloy();
    }

    private string MakeGameBoard()
    {
        string board = "0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 " + Environment.NewLine;
        for (int i = 0; i < _gameBoard.height; i++)
        {
            board += string.Format("{0,3}", i);
            for (int j = 0; j < _gameBoard.width; j++)
            {
                board += _gameBoard.board[j, i] == null ? ". " : "x ";
            }
            board += Environment.NewLine;
        }
        return board;
    }
}
