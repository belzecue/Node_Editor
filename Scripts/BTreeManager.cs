using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NodeEditorFramework;
using NodeEditorFramework.Utilities;
using System.Linq;
using BehaviourTrees;
using System;
using UniRx;

public class BTreeManager : MonoBehaviour {

	[SerializeField, FilePath]
	string filePath;

	BehaviourTreeInstance node;

	RuntimeNodeEditor runtimeNodeEditor;

	Action m_task;

	// Use this for initialization
	void Start () {
		m_task = ChangeColorTask;
		runtimeNodeEditor = GetComponent<RuntimeNodeEditor>();
		var path = filePath.Replace (Application.dataPath, "Assets");
		// Load the NodeCanvas
		var canvas = NodeEditorSaveManager.LoadNodeCanvas (path);
		var startNode = FindStartNode(canvas);
		if(startNode==null || startNode.Outputs[0].connections==null){
			return;
		}
		var topNode = startNode.Outputs[0].connections[0].body as uBehaviourTreeNode;
		var btree = CreateBehaviourTreeBase(topNode);
		node = new BehaviourTreeInstance(btree);
		node.finishRP.Where(p=>p!=BehaviourTreeInstance.NodeState.READY).Subscribe(p=>ResetCoroutineStart());
		node.Excute();
	}


	void Update(){
		if(m_task!=null){
			m_task();
		}
	}


	void ChangeColorTask(){
		if(runtimeNodeEditor!=null && runtimeNodeEditor.canvas!=null){
			foreach(var one in runtimeNodeEditor.canvas.nodes){
				var bnode = one as uBehaviourTreeNode;
				if(bnode!=null){
					//Debug.Log("Excute Node = "+node.nowExcuteUuid);
					if(bnode.Uuid == node.nowExcuteUuid){
						bnode.setColorFlag = true;
					}
					else {
						bnode.setColorFlag = false;
					}
				}
			}
		}
	}

	/// <summary>
	/// 開始ノードを検索する。
	/// </summary>
	/// <returns>The start node.</returns>
	/// <param name="canvas">Canvas.</param>
	Node FindStartNode(NodeCanvas canvas){
		return canvas.nodes.Where(p=>p.GetType() == typeof(uStartNode)).FirstOrDefault();
	}

	/// <summary>
	/// BehaviourTreeを構築する。
	/// </summary>
	/// <returns>The behaviour tree base.</returns>
	/// <param name="node">Node.</param>
	BehaviourTreeBase CreateBehaviourTreeBase(uBehaviourTreeNode bnode){
		var type = Type.GetType("BehaviourTrees."+ bnode.nodeType.ToString());
		var ins =  Activator.CreateInstance(type) as BehaviourTreeBase;

		switch (bnode.nodeType) {
		case uBehaviourTreeNode.NodeType.ActionNode:
			{
				var nodeIns = ins as ActionNode;
				nodeIns.Initialize(BTreeUtil.GetAction<ReactiveProperty<bool>>(gameObject, typeof(BTreeActionBase),bnode.MethodName),bnode.Uuid);
			}
			break;
		case uBehaviourTreeNode.NodeType.DecoratorNode:
			{
				var nodeIns = ins as DecoratorNode;
				var actionNode = bnode.Outputs[0].connections[0].body as uBehaviourTreeNode;
				var bbase = CreateBehaviourTreeBase(actionNode);
				var func = BTreeUtil.GetFunc<BehaviourTreeInstance,ExecutionResult>(gameObject, typeof(BTreeDecoratorFuncBase),bnode.MethodName);
				nodeIns.Initialize(func, bbase,bnode.Uuid);
			}
			break;

		case uBehaviourTreeNode.NodeType.SelectorNode:
			{
				var nodeIns = ins as SelectorNode;
				var list = new List<BehaviourTreeBase>();
				foreach(var one in bnode.Outputs){
					if(one.connections.Count==0) continue;
					var childnode = CreateBehaviourTreeBase(one.connections[0].body  as uBehaviourTreeNode);
					list.Add(childnode);
				}
				nodeIns.Initialize(list.ToArray(),bnode.Uuid);
				list.Clear();
			}
			break;
		case uBehaviourTreeNode.NodeType.SequencerNode:
			{
				var nodeIns = ins as SequencerNode;
				var list = new List<BehaviourTreeBase>();
				foreach(var one in bnode.Outputs){
					if(one.connections.Count==0) continue;
					var childnode = CreateBehaviourTreeBase(one.connections[0].body as uBehaviourTreeNode);
					list.Add(childnode);
				}
				nodeIns.Initialize(list.ToArray(),bnode.Uuid);
				list.Clear();
			}
			break;
		}

		return ins;
	}


	/// <summary>
	/// Resets the coroutine start.
	/// </summary>
	void ResetCoroutineStart(){
		StartCoroutine(WaitCoroutine());
	}

	/// <summary>
	/// Waits the coroutine.
	/// </summary>
	/// <returns>The coroutine.</returns>
	IEnumerator WaitCoroutine(){
		yield return new WaitForSeconds(1.5f);
		node.Reset();
	}
}
