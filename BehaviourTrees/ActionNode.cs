using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UniRx;

namespace BehaviourTrees
{
	/// <summary>
	/// 処理を実行するだけのクラス
	/// </summary>
	public class ActionNode : BehaviourTreeBase
	{
		//private Func<BehaviourTreeInstance, ExecutionResult> action;

		private Action<ReactiveProperty<bool>> func;

		private ReactiveProperty<bool> finishRP = new ReactiveProperty<bool>(false);

		public void Initialize(Action<ReactiveProperty<bool>> action, int uuid = 0){
			this.Init(uuid);
			this.func = action;

		}

		public override void Reset ()
		{
			
		}

		override public void Execute(BehaviourTreeInstance behaviourTreeInstance)
		{
			base.Execute(behaviourTreeInstance);
			behaviourTreeInstance.nodeStateDict[this.key] = BehaviourTreeInstance.NodeState.READY;
			finishRP.Where(p=>p == true).Subscribe(p=> EndCB(behaviourTreeInstance));
			func(finishRP);
		}

		void EndCB(BehaviourTreeInstance behaviourTreeInstance){
			behaviourTreeInstance.nodeStateDict[this.key] = BehaviourTreeInstance.NodeState.SUCCESS;
			finishRP.Value = false;
		}
	}
}
