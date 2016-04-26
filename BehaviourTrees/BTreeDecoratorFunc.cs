using UnityEngine;
using System.Collections;
using BehaviourTrees;

/// <summary>
/// uDecoratorNodeの条件式に設定できるメソッド群
/// Methods that can be set in the conditional expression of Decorator Node
/// </summary>
public class BTreeDecoratorFunc : BTreeDecoratorFuncBase {

	public ExecutionResult IsNearEnemy(BehaviourTreeInstance instance){
		return new ExecutionResult(true);
	}

	public ExecutionResult IsRandom50(BehaviourTreeInstance instance){
		var rand = Random.Range(0,1.0f);

		if(rand < 0.5f){
			return new ExecutionResult(true);
		}

		return new ExecutionResult(false);
	}
}
