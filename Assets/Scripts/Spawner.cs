using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Spawner : MonoBehaviour
{
    public List<GameObject> alloys = new List<GameObject>();
    public List<GameObject> ingots = new List<GameObject>();
	public List<Sprite> sprites = new List<Sprite>();
    
    public void Awake()
    {
        alloys = Resources.LoadAll<GameObject>("Alloys").ToList();
        ingots = Resources.LoadAll<GameObject>("Ingots").ToList();
		sprites = Resources.LoadAll<Sprite>("Sprites").ToList();
    }

    public GameObject NewAlloy()
    {
    	// create the alloy object and get a reference to the alloy component
		GameObject alloyObject = Instantiate(
			alloys[Random.Range(0, alloys.Count)],
			this.transform.position,
			Quaternion.identity) as GameObject;
			
		Alloy alloy = alloyObject.GetComponent<Alloy>();
		
		// set the type of alloy
		alloy.Type = Alloy.AlloyTypes[Random.Range(0, Alloy.AlloyTypes.Count())];
		
		// get the relative initial positions of the ingots
		var positions = Alloy.IngotInitialPositions[alloy.Type];
    
    	// choose the sprite
    	int spriteIdx = Random.Range(0, sprites.Count);
    	
    	// chose the ingot
    	int ingotIdx = Random.Range(0, ingots.Count);
    	
    	// make the ingot objects and ingot components and attach them
    	List<GameObject> ingotObjects = new List<GameObject>();
    	
    	for (int i = 0; i < positions.Count; i++)
    	{
    		// create gameobject
    		var ingotObject = Instantiate(ingots[ingotIdx]) as GameObject;
    		// set the position
    		ingotObject.transform.position = alloyObject.transform.position + positions[i];
    		// attach parent
    		ingotObject.transform.parent = alloyObject.transform;
    		
    		// get ingot component
    		var block = ingotObject.GetComponent<Ingot>();
    		// set sprite and relative position
    		block.Create(positions[i].x, positions[i].y, sprites[spriteIdx]);
    		ingotObjects.Add(ingotObject);
    	}
		
		alloy.Ingots = ingotObjects;
    	
		return alloyObject;
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