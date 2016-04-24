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

	void Awake(){
		uuid = -1;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos()
	{
		#if UNITY_EDITOR

		if(hp<0){
			return;
		}

		if(uuid == 0){
			GUIStyle myStyle = new GUIStyle();
			myStyle.normal.textColor = Color.blue;
			UnityEditor.Handles.Label(transform.position + new Vector3(0,2,0), "Player1",myStyle);
		}
		else if (uuid == 1) {
			GUIStyle myStyle = new GUIStyle();
			myStyle.normal.textColor = Color.red;
			UnityEditor.Handles.Label(transform.position+ new Vector3(0,2,0), "Player2",myStyle);
		}

		#endif
	}
}
