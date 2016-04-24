using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UniRx;

namespace BehaviourTrees
{
	/// <summary>
	/// Class of only executes Action
	/// </summary>
	public class ActionNode : BehaviourTreeBase
	{
		//private Func<BehaviourTreeInstance, ExecutionResult> action;

		private Action<ReactiveProperty<bool>> func;

		private ReactiveProperty<bool> finishRP;

		private IDisposable	m_dispose;

		public void Initialize(Action<ReactiveProperty<bool>> action, int uuid = 0){
			this.finishRP = new ReactiveProperty<bool>(false);
			this.Init(uuid);
			this.func = action;

		}

		public override void ObserveInit (BehaviourTreeInstance behaviourTreeInstance)
		{
			this.m_dispose = this.finishRP.Where(p=>p == true).Subscribe(p=> EndCB(behaviourTreeInstance));
		}
			

		public override void Delete ()
		{
			this.m_dispose.Dispose();
			this.func = null;
		}

		override public void Execute(BehaviourTreeInstance behaviourTreeInstance)
		{
			base.Execute(behaviourTreeInstance);
			behaviourTreeInstance.nodeStateDict[this.key] = BehaviourTreeInstance.NodeState.READY;

			this.func(this.finishRP);
		}

		void EndCB(BehaviourTreeInstance behaviourTreeInstance){
			behaviourTreeInstance.nodeStateDict[this.key] = BehaviourTreeInstance.NodeState.SUCCESS;
			finishRP.Value = false;
		}
	}
}
