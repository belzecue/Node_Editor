using UnityEngine;
using System.Collections;

public class Pawn : MonoBehaviour {

	public int maxHp {
		get {
			return 10;
		}
	}

	public int hp {
		get; set;
	}

	public int uuid {
		get; set;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
