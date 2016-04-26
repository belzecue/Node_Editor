using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System.Linq;

namespace BehaviourTrees
{
	/// <summary>
	/// 条件が通ったら子を実行して、子が返すステータスを返す。
	/// 通らなかったらFailureを返す。
	/// Run the child Once the condition is through, return the status returned by the children.
	/// It is not pass return a Failure.
	/// </summary>
	public class DecoratorNode : BehaviourTreeBase {

		private Func<BehaviourTreeInstance, ExecutionResult> conditionFunction;
		private BehaviourTreeBase action;
		private IDisposable	m_dispose;

		public void Initialize(Func<BehaviourTreeInstance, ExecutionResult> func, BehaviourTreeBase action, int uuid = 0){
			this.Init(uuid);
			this.conditionFunction = func;
			this.action = action;
		}


		public override void ObserveInit(BehaviourTreeInstance behaviourTreeInstance){
			m_dispose = behaviourTreeInstance.nodeStateDict.ObserveReplace()
				.Where(p=>p.Key ==this.action.key)
				.Subscribe(p=>NextState(p.NewValue, behaviourTreeInstance));
			this.action.ObserveInit(behaviourTreeInstance);
		}
			

		public override void Delete ()
		{
			this.m_dispose.Dispose();
			this.action.Delete();
			this.conditionFunction = null;
		}

			
		public override void Execute(BehaviourTreeInstance behaviourTreeInstance)
		{
			base.Execute(behaviourTreeInstance);
			behaviourTreeInstance.nodeStateDict[this.key] = BehaviourTreeInstance.NodeState.READY;

			if(this.conditionFunction(behaviourTreeInstance).BooleanResult){
				this.action.Execute(behaviourTreeInstance);
			}
			else {
				behaviourTreeInstance.nodeStateDict[this.key] = BehaviourTreeInstance.NodeState.FAILURE;
			}
		}


		void NextState(BehaviourTreeInstance.NodeState state, BehaviourTreeInstance behaviourTreeInstance){
			if(state == BehaviourTreeInstance.NodeState.SUCCESS){
				behaviourTreeInstance.nodeStateDict[this.key] = BehaviourTreeInstance.NodeState.SUCCESS;
			}
			else if (state == BehaviourTreeInstance.NodeState.FAILURE){
				behaviourTreeInstance.nodeStateDict[this.key] = BehaviourTreeInstance.NodeState.FAILURE;
			}
		}
			
	}
}
