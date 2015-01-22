using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]

public class Ingot : MonoBehaviour {

	public float X { get; private set; }
	public float Y { get; private set; }
	
	public Sprite SpriteBlock { get; private set; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Create(float x, float y, Sprite s)
	{
		X = x;
		Y = y;
		var renderer = GetComponent<SpriteRenderer>();
		SpriteBlock = s;
		renderer.sprite = SpriteBlock;
	}
	
	public void Rotate(InputManager.Rotation dir)
	{
		if (dir == InputManager.Rotation.Clockwise)
		{
			float temp = Y;
			Y = -X;
			X = temp;
		}
		else
		{
			float temp = X;
			X = -Y;
			Y = temp;	
		}
	}
	
}
