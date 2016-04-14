using UnityEngine;
using System.Collections;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

[System.Serializable]
[Node (false, "BehaviourTree/SequencerNode/3")]
public class uSequencerNode3 : uBehaviourTreeNode {
	public const string ID = "SequencerNode3";

	public override string GetID { get { return ID;} }

	int		m_selectIndex = 0;
	int		m_size = 1;

	public uSequencerNode3(){
		nodeType = NodeType.SequencerNode;
	}

	public override Node Create (Vector2 pos) 
	{

		var node = CreateInstance <uSequencerNode3> ();

		node.name = "BehaviourTree Node";
		node.rect = new Rect (pos.x, pos.y, 200, 100);
		node.CreateInput ("Input 1", "Float");
		node.CreateOutput ("OutPut 1", "Float");
		node.CreateOutput ("OutPut 2", "Float");
		node.CreateOutput ("OutPut 3", "Float");
		return node;
	}

	protected internal override void NodeGUI () 
	{
		GUILayout.Label (GetID+ "#"+Uuid.ToString());
		GUILayout.BeginHorizontal ();
		GUILayout.BeginVertical ();
		Inputs [0].DisplayLayout ();
		GUILayout.EndVertical ();
		GUILayout.BeginVertical ();
		Outputs [0].DisplayLayout ();
		Outputs [1].DisplayLayout ();
		Outputs [2].DisplayLayout ();
		GUILayout.EndVertical ();
		GUILayout.EndHorizontal ();
	}

	public override bool Calculate () 
	{
		if (!allInputsReady ())
			return false;
		return true;
	}
}
