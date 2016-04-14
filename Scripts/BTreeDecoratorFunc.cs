using UnityEngine;
using System.Collections;
using BehaviourTrees;

public class BTreeDecoratorFunc : BTreeDecoratorFuncBase {
	public ExecutionResult IsHpLessThan(BehaviourTreeInstance instance){
		var enemyhp = Random.Range(0,10);
		if(enemyhp <= 4){
			Debug.Log("敵のHPが4以下。チャンス。");
			return new ExecutionResult(true);
		}
		Debug.Log("敵のHPが4より大きい。まだ慌てる時間じゃない。");
		return new ExecutionResult(false);
	}
}
