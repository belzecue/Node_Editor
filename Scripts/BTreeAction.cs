using UnityEngine;
using System.Collections;
using BehaviourTrees;
using UniRx;
using System;

/// <summary>
/// B tree action.
/// </summary>
public class BTreeAction : BTreeActionBase {

	ReactiveProperty<bool> m_finishRP;

	public void AtackEmeny(ReactiveProperty<bool> finishRP)
	{
		m_finishRP = finishRP;
		Observable.Return(Unit.Default)
			.Delay(TimeSpan.FromMilliseconds(100))
			.Subscribe(_ => EndAction(()=>{Debug.Log("敵を攻撃する");}));
	}


	public void MoveNearTower(ReactiveProperty<bool> finishRP)
	{
		m_finishRP = finishRP;
		Observable.Return(Unit.Default)
			.Delay(TimeSpan.FromMilliseconds(100))
			.Subscribe(_ => EndAction(()=>{Debug.Log("一番近くのタワーにいく。");}));
	}


	public void Wait(ReactiveProperty<bool> finishRP)
	{
		m_finishRP = finishRP;
		Observable.Return(Unit.Default)
			.Delay(TimeSpan.FromMilliseconds(100))
			.Subscribe(_ => EndAction(()=>{Debug.Log("待機。");}));
	}


	void EndAction(System.Action action){
		if(action!=null) {
			action();
			m_finishRP.Value = true;
		}
	}
}
