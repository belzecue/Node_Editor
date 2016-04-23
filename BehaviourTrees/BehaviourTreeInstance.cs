using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using System;

namespace BehaviourTrees
{
	public class BehaviourTreeInstance  {
		public enum NodeState
		{
			READY,
			SUCCESS,
			FAILURE,
		};

		/// <summary>
		/// 終了検知ReactiveProperty
		/// </summary>
		public ReactiveProperty<NodeState> finishRP;

		/// <summary>
		/// 各nodeのNodeStateの状態変化を監視するReactiveProperty
		/// </summary>
		public ReactiveDictionary<string,NodeState> nodeStateDict = new ReactiveDictionary<string, NodeState>();

		private BehaviourTreeBase	rootNode;

		private IDisposable	m_addDispose;

		private IDisposable	m_replaceDispose;

		public int nowExcuteUuid {
			set;get;
		}

		public BehaviourTreeInstance(BehaviourTreeBase rootNode)
		{
			this.finishRP = new ReactiveProperty<NodeState>(NodeState.READY);

			this.rootNode = rootNode;

			m_addDispose = this.nodeStateDict.ObserveAdd()
				.Where(p=>p.Value == NodeState.READY)
				.Subscribe(p=>SetCurrentNodeKey(p.Key));

			m_replaceDispose = this.nodeStateDict.ObserveReplace()
				.Where(p=>p.Key == rootNode.key)
				.Where(p=>p.NewValue == NodeState.FAILURE || p.NewValue == NodeState.SUCCESS)
				.Subscribe(p=>Finish(p.NewValue));

			this.rootNode.ObserveInit(this);
		}

		/// <summary>
		/// 実行する
		/// </summary>
		public void Excute(){
			this.rootNode.Execute(this);
		}


		/// <summary>
		/// nodeを削除する。
		/// </summary>
		public void Delete(){
			this.nodeStateDict.Clear();
			this.rootNode.Delete();
			this.finishRP.Dispose();
			this.m_addDispose.Dispose();
			this.m_replaceDispose.Dispose();
		}

			
		void Finish(NodeState state){
			this.finishRP.Value = state;
		}


		void SetCurrentNodeKey(string key){
			//Debug.Log(key);
		}
	}
}
