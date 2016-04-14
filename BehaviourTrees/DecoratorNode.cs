using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System.Linq;

namespace BehaviourTrees
{
	/// <summary>
	/// Dicorator node.
	/// </summary>
	public class DecoratorNode : BehaviourTreeBase {

		private Func<BehaviourTreeInstance, ExecutionResult> conditionFunction;
		private BehaviourTreeBase action;

		public void Initialize(Func<BehaviourTreeInstance, ExecutionResult> func, BehaviourTreeBase action, int uuid = 0){
			this.Init(uuid);
			this.conditionFunction = func;
			this.action = action;
		}


		public override void Reset ()
		{
			//this.actions.Reset();
		}

		public override void Execute(BehaviourTreeInstance behaviourTreeInstance)
		{
			base.Execute(behaviourTreeInstance);
			behaviourTreeInstance.nodeStateDict[this.key] = BehaviourTreeInstance.NodeState.READY;

			if(this.conditionFunction(behaviourTreeInstance).BooleanResult){

				behaviourTreeInstance.nodeStateDict.ObserveReplace()
					.Where(p=>p.Key ==this.action.key)
					.Subscribe(p=>NextState(p.NewValue, behaviourTreeInstance));
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
