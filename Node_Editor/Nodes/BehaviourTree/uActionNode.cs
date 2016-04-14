using UnityEngine;
using System.Collections;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
using System.Linq;

[System.Serializable]
[Node (false, "BehaviourTree/ActionNode")]
public class uActionNode : uBehaviourTreeNode {
	public const string ID = "ActionNode";

	public override string GetID { get { return ID; } }

	public int selectIndex {
		get;set;
	}

	int m_selectIndex = 0;
	int m_tmpIndex;


	public uActionNode(){
		nodeType = NodeType.ActionNode;
	}

	public override Node Create (Vector2 pos) 
	{
		uActionNode node = CreateInstance <uActionNode> ();
		node.name = "BehaviourTree Node";
		node.rect = new Rect (pos.x, pos.y, 200, 100);
		node.CreateInput ("Input 1", "Float");

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

		var methodNames = BTreeUtil.GetMethods<BTreeActionBase>().ToArray();
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
		return true;
	}

	public void UpdateAfterFocus (){
		m_selectIndex = m_tmpIndex;
		var methodNames = BTreeUtil.GetMethods<BTreeActionBase>().ToArray();
		if(methodNames.Length < m_selectIndex + 1 || m_selectIndex < 0){
			MethodName = "";
		}
		else {
			MethodName = methodNames[m_selectIndex];	
		}
	}
}
