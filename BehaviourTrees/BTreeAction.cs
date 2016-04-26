using UnityEngine;
using System.Collections;
using BehaviourTrees;
using UniRx;
using System;

/// <summary>
/// uActionNodeのActionに設定できるメソッド群
/// Methods that can be set to Action of uActionNode
/// </summary>
public class BTreeAction : BTreeActionBase {

	public void RandomMove(ReactiveProperty<bool> finishRP)
	{
		Debug.Log("RandomMove");
		StartCoroutine(WaitCoroutine(finishRP));
	}


	public void MoveToEnemy(ReactiveProperty<bool> finishRP)
	{
		Debug.Log("MoveToEnemy");
		StartCoroutine(WaitCoroutine(finishRP));
	}


	IEnumerator WaitCoroutine(ReactiveProperty<bool> finishRP){
		yield return new WaitForSeconds(2f);
		finishRP.Value = true;
	}
}
