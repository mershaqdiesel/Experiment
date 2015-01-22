using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Alloy : MonoBehaviour
{
    public static readonly string[] AlloyTypes = { "Square", "Line", "L", "J", "S", "Z", "T" };
    public static readonly Dictionary<string, List<Vector3>> IngotInitialPositions = 
    	new Dictionary<string, List<Vector3>>()
        {
            { "Square", new List<Vector3>()
                {
                    new Vector3(0, 0),
                    new Vector3(1, 0),
                    new Vector3(0, 1),
                    new Vector3(1, 1)
                }
            },
            { "Line", new List<Vector3>()
                {
					new Vector3(0, 0),
                    new Vector3(0, 1),
					new Vector3(0, 2),
                    new Vector3(0, 3),
                }
            },
            { "L", new List<Vector3>()
                {
                    new Vector3(0, 0),
					new Vector3(1, 0),
					new Vector3(0, 1),
                    new Vector3(0, 2)
                }
            },
            { "J", new List<Vector3>()
                {
                    new Vector3(0, 0),
					new Vector3(-1, 0),
					new Vector3(0, 1),
                    new Vector3(0, 2)
                }
            },
            { "S", new List<Vector3>()
                {
                    new Vector3(0, 0),
					new Vector3(-1, 0),
					new Vector3(0, 1),
					new Vector3(1, 1)
                }
            },
            { "Z", new List<Vector3>()
                {
                    new Vector3(0, 0),
					new Vector3(1, 0),
					new Vector3(0, 1),
					new Vector3(-1, 1)
                }
            },
            { "T", new List<Vector3>()
                {
                    new Vector3(0, 0),
					new Vector3(0, 1),
					new Vector3(-1, 1),
					new Vector3(1, 1)
                }
            }
        };

    public string Type { get; set; }
    
    public IList<GameObject> Ingots { get; set; }
	
	public void Rotate(InputManager.Rotation rot)
	{
		float parentX = this.gameObject.transform.position.x;
		float parentY = this.gameObject.transform.position.y;
		foreach (var ingot in Ingots)
		{
			// set new ingot position based on parent position and the rotation
			var comp = ingot.GetComponent<Ingot>();
			comp.Rotate(rot);			
			
			ingot.transform.position = new Vector3(parentX + comp.X, parentY + comp.Y, 0f);
		}
	}
	
	public void Translate(InputManager.Direction dir)
	{
		float dx = 0, dy = 0;
		switch (dir)
		{
			case InputManager.Direction.Down:
				dy = -1f;
				break;
			case InputManager.Direction.Left:
				dx = -1f;
				break;
			case InputManager.Direction.Right:
				dx = 1f;
				break;
			case InputManager.Direction.Up:
				dy = 1f;
				break;
		}
		this.gameObject.transform.Translate(dx, dy, 0f);
		// don't need to worry about child positions
	}
    
	public void Awake()
	{
	}
	
	// Use this for initialization
	public void Start ()
	{
	}
	
	// Update is called once per frame
	public void Update ()
	{
	}
}