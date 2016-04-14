using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

[System.Serializable]
[Node (false, "BehaviourTree/StartNode")]
public class uStartNode : uBehaviourTreeNode {

	public const string ID = "StartNode";

	public override string GetID { get { return ID; } }

	public uStartNode(){
		nodeType = NodeType.StartNode;
	}

	public override Node Create (Vector2 pos) 
	{
		uStartNode node = CreateInstance <uStartNode> ();
		node.name = "Start Node";
		node.rect = new Rect (pos.x, pos.y, 200, 100);
		node.CreateOutput ("Output 1", "Float");

		return node;
	}

	protected internal override void NodeGUI () 
	{
		GUILayout.Label (GetID);

		GUILayout.BeginHorizontal ();
		GUILayout.BeginVertical ();


		GUILayout.EndVertical ();
		GUILayout.BeginVertical ();

		Outputs [0].DisplayLayout ();
	
		GUILayout.EndVertical ();
		GUILayout.EndHorizontal ();

	}

	public override bool Calculate () 
	{
		return false;
	}
}
