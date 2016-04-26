using UnityEngine;
using System.Collections;

using NodeEditorFramework;
using NodeEditorFramework.Utilities;

public abstract class uBehaviourTreeNode : Node {
	/// <summary>
	/// Get the ID of the Node
	/// </summary>
	public override string GetID {
		get {
			return "";
		}
	}

	/// <summary>
	/// Create an instance of this Node at the given position
	/// </summary>
	public override Node Create (Vector2 pos){
		return null;
	}


	internal protected override void DrawNode ()
	{
		if(setColorFlag){
			ChangeColor();
		}
		else {
			ResetDefaultColor();
		}
		base.DrawNode ();
	}
		
	/// <summary>
	/// Calculate the outputs of this Node
	/// Return Success/Fail
	/// Might be dependant on previous nodes
	/// </summary>
	public override bool Calculate (){
		return false;
	}


	public enum NodeType {
		StartNode,
		ActionNode,
		DecoratorNode,
		SelectorNode,
		SequencerNode,
	}


	public NodeType nodeType;

	public string MethodName = "";

	public bool setColorFlag = false;

	Color defaultColor = Color.gray;

	void ChangeColor(){
		GUI.backgroundColor = Color.white;
	}

	void ResetDefaultColor() {
		GUI.backgroundColor = defaultColor;
	}


}
