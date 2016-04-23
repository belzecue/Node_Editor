using UnityEngine;
using System.Collections;
using BehaviourTrees;

public class BTreeDecoratorFunc : BTreeDecoratorFuncBase {

	Pawn m_myPawn;
	Pawn m_enemyPawn;
	Pawn m_placePawn;

	void Start(){
		m_myPawn =gameObject.GetComponent<Pawn>();
		m_enemyPawn = GameManager.Instance.GetEnemey(m_myPawn.uuid).GetComponent<Pawn>();
		m_placePawn = GameManager.Instance.GetSpawnPosition(m_myPawn.uuid).GetComponent<Pawn>();
	}


	public ExecutionResult IsHpMoreThan(BehaviourTreeInstance instance){
		if(m_myPawn.hp > 2){
			return new ExecutionResult(true);
		}
		return new ExecutionResult(false);
	}



	public ExecutionResult IsHpLessThan(BehaviourTreeInstance instance){
		if(m_myPawn.hp <= 2){
			return new ExecutionResult(true);
		}
		return new ExecutionResult(false);
	}


	public ExecutionResult IsNearEnemy(BehaviourTreeInstance instance){
		var distance = Vector3.Distance(m_myPawn.transform.localPosition, m_enemyPawn.transform.localPosition);

		if(distance < 1f){
			return new ExecutionResult(true);
		}

		return new ExecutionResult(false);
	}

	public ExecutionResult IsNearHome(BehaviourTreeInstance instance){
		var distance = Vector3.Distance(m_myPawn.transform.localPosition, m_placePawn.transform.localPosition);

		if(distance < 1f){
			return new ExecutionResult(true);
		}

		return new ExecutionResult(false);
	}


	public ExecutionResult IsRandom50(BehaviourTreeInstance instance){
		var rand = Random.Range(0,1.0f);

		if(rand < 0.5f){
			return new ExecutionResult(true);
		}

		return new ExecutionResult(false);
	}
}
