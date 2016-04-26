using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;

[System.Serializable]
[Node (false, "BehaviourTree/DecoratorNode")]
public class uDecoratorNode : uBehaviourTreeNode {
	public const string ID = "DecoratorNode";

	public override string GetID { get { return ID; } }

	int m_selectIndex = 0;
	int m_tmpIndex;


	public uDecoratorNode(){
		nodeType = NodeType.DecoratorNode;
	}

	public override Node Create (Vector2 pos) 
	{
		uDecoratorNode node = CreateInstance <uDecoratorNode> ();
		node.name = "BehaviourTree Node";
		node.rect = new Rect (pos.x, pos.y, 200, 100);
		node.CreateInput ("Input 1", "Float");
		node.CreateOutput ("Output 1", "Float");

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

		var methodNames = BTreeUtil.GetMethods<BTreeDecoratorFuncBase>().ToArray();
		if(!string.IsNullOrEmpty(MethodName)){
			m_selectIndex = methodNames.ToList().IndexOf(MethodName);
		}
		m_tmpIndex = RTEditorGUI.Popup(m_selectIndex,methodNames);

		if(NodeEditor.curEditorState.focusedNode != null && 
			NodeEditor.curEditorState.focusedNode.GetID == GetID){
			UpdateAfterFocus();
		}

		GUILayout.EndVertical ();
		GUILayout.EndHorizontal ();

	}

	public override bool Calculate () 
	{
		if (!allInputsReady ())
			return false;
		Outputs[0].SetValue<float> (Inputs[0].GetValue<float> () * 5);
		return true;
	}

	public void UpdateAfterFocus (){
		m_selectIndex = m_tmpIndex;
		var methodNames = BTreeUtil.GetMethods<BTreeDecoratorFuncBase>().ToArray();
		if(methodNames.Length < m_selectIndex + 1 || m_selectIndex < 0){
			MethodName = "";
		}
		else {
			MethodName = methodNames[m_selectIndex];	
		}
	}
}
