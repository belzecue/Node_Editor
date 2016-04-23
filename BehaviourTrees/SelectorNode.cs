using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UniRx;

namespace BehaviourTrees
{
	/// <summary>
	/// Selector node.
	/// 成功する子が見つかるまで子を実行する 
	/// 成功するのが見つかったらこれ以降の処理は行わず Successを返す
	/// すべて実行しすべてが失敗に終わったら Failureを返す
	/// </summary>
	public class SelectorNode : BehaviourTreeBase {

		private IEnumerator<BehaviourTreeBase> actions;

		private IDisposable	m_dispose;

		public void Initialize(BehaviourTreeBase[] actionArray, int uuid=0){
			this.Init(uuid);
			this.actions = actionArray.ToList().GetEnumerator();
		}


		public override void ObserveInit(BehaviourTreeInstance behaviourTreeInstance){

			m_dispose = behaviourTreeInstance.nodeStateDict.ObserveReplace()
				.Where(p=>p.Key == actions.Current.key).Subscribe(p=>NextState(p.NewValue, behaviourTreeInstance));
			while(this.actions.MoveNext()){
				this.actions.Current.ObserveInit(behaviourTreeInstance);
			}
			this.actions.Reset();
		}

		public override void Execute(BehaviourTreeInstance behaviourTreeInstance)
		{
			base.Execute(behaviourTreeInstance);
			behaviourTreeInstance.nodeStateDict[this.key] = BehaviourTreeInstance.NodeState.READY;
		
			this.actions.MoveNext();
			this.actions.Current.Execute(behaviourTreeInstance);
		}

		public override void Reset ()
		{
			this.actions.Reset();
			while(this.actions.MoveNext()){
				this.actions.Current.Reset();
			}
			this.actions.Reset();
		}

		public override void Delete ()
		{
			m_dispose.Dispose();
			this.actions.Reset();
			while(this.actions.MoveNext()){
				this.actions.Current.Delete();
			}
		}

		void NextState(BehaviourTreeInstance.NodeState state, BehaviourTreeInstance behaviourTreeInstance){
			if(state == BehaviourTreeInstance.NodeState.SUCCESS){
				behaviourTreeInstance.nodeStateDict[this.key] = BehaviourTreeInstance.NodeState.SUCCESS;
			}
			else if (state == BehaviourTreeInstance.NodeState.FAILURE){
				if(actions.MoveNext()){
					actions.Current.Execute(behaviourTreeInstance);
				}
				else {
					behaviourTreeInstance.nodeStateDict[this.key] = BehaviourTreeInstance.NodeState.FAILURE;
				}
			}
		}
	}
}
