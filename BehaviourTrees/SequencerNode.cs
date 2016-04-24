using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UniRx;

namespace BehaviourTrees
{
	/// <summary>
	/// Sequencer node.
	/// 子が成功したら次の子を実行させる。
	/// 子が失敗したら Failure を返す。
	/// すべての子の処理が終わったら Success を返す
	/// </summary>
	public class SequencerNode : BehaviourTreeBase {

		private IEnumerator<BehaviourTreeBase> actions;

		private IDisposable	m_dispose;

		public void Initialize(BehaviourTreeBase[] actionArray, int uuid){
			this.Init(uuid);
			this.actions = actionArray.ToList().GetEnumerator();
		}


		public override void ObserveInit (BehaviourTreeInstance behaviourTreeInstance)
		{
			m_dispose = behaviourTreeInstance.nodeStateDict.ObserveReplace()
				.Where(p=>p.Key == actions.Current.key)
				.Subscribe(p=>NextState(p.NewValue, behaviourTreeInstance));
			while(this.actions.MoveNext()){
				this.actions.Current.ObserveInit(behaviourTreeInstance);
			}
			this.actions.Reset();
		}


		override public void Execute(BehaviourTreeInstance behaviourTreeInstance)
		{
			base.Execute(behaviourTreeInstance);
			behaviourTreeInstance.nodeStateDict[this.key] = BehaviourTreeInstance.NodeState.READY;

			this.actions.MoveNext();
			this.actions.Current.Execute(behaviourTreeInstance);

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
			if(state == BehaviourTreeInstance.NodeState.FAILURE){
				behaviourTreeInstance.nodeStateDict[this.key] = BehaviourTreeInstance.NodeState.FAILURE;
			}
			else if (state == BehaviourTreeInstance.NodeState.SUCCESS){
				if(this.actions.MoveNext()){
					this.actions.Current.Execute(behaviourTreeInstance);
				}
				else {
					behaviourTreeInstance.nodeStateDict[this.key] = BehaviourTreeInstance.NodeState.SUCCESS;
				}
			}
		}
			
	}
}
