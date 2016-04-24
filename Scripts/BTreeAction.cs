using UnityEngine;
using System.Collections;
using BehaviourTrees;
using UniRx;
using System;
using UnityStandardAssets.Characters.ThirdPerson;

/// <summary>
/// uActionNodeのActionに設定できるメソッド群
/// Methods that can be set to Action of uActionNode
/// </summary>
public class BTreeAction : BTreeActionBase {

	[SerializeField]  ThirdPersonCharacter	m_Character;
	[SerializeField]  ActionController		m_Action;

	Action m_task;

	ReactiveProperty<bool> m_finishRP;

	Pawn m_myPawn;
	Pawn m_enemyPawn;
	Pawn m_placePawn;

	IDisposable m_dispose;

	void Start(){
		m_myPawn =gameObject.GetComponent<Pawn>();
		m_enemyPawn = GameManager.Instance.GetEnemey(m_myPawn.uuid).GetComponent<Pawn>();
		m_placePawn = GameManager.Instance.GetSpawnPosition(m_myPawn.uuid).GetComponent<Pawn>();
	}


	public void RandomMove(ReactiveProperty<bool> finishRP)
	{
		m_finishRP = finishRP;
		Debug.Log("RandomMove");
		var dir = GetRandomDir();
		StartCoroutine(MoveCroutine(0.5f, dir, MoveDir));
	}


	public void MoveToEnemy(ReactiveProperty<bool> finishRP)
	{
		m_finishRP = finishRP;
		Debug.Log("MoveToEnemy");
		var heading = m_myPawn.transform.position - m_enemyPawn.transform.position;
		var distance = heading.magnitude;
		var dir = -heading / distance;
		StartCoroutine(MoveCroutine(0.9f, dir, MoveDir ,m_enemyPawn.transform.localPosition, 1.5f));
	}


	public void GoHome(ReactiveProperty<bool> finishRP)
	{
		m_finishRP = finishRP;
		Debug.Log("GoHome");
		var heading = m_myPawn.transform.position - m_placePawn.transform.position;
		var distance = heading.magnitude;
		var dir = -heading / distance;
		StartCoroutine(MoveCroutine(3f, dir, MoveDir,  m_placePawn.transform.localPosition, 0.8f));
	}


	public void Hikick(ReactiveProperty<bool> finishRP)
	{
		m_finishRP = finishRP;
		Debug.Log("Kick");
		DoHikick();
	}


	public void Cure(ReactiveProperty<bool> finishRP)
	{
		m_finishRP = finishRP;
		Debug.Log("Cure");
		DoCure();
	}


	IEnumerator MoveCroutine(float startTime, Vector3 dir, Action<Vector3> moveDir){
		var currentTime = startTime;
		while (currentTime > 0)
		{
			moveDir(dir);
			currentTime -= Time.deltaTime;
			yield return null;
		}
		m_finishRP.Value = true;
	}


	IEnumerator MoveCroutine(float startTime, Vector3 dir, Action<Vector3> moveDir, Vector3 target, float distance){
		var currentTime = startTime;
		while (currentTime > 0)
		{
			moveDir(dir);
			currentTime -= Time.deltaTime;
			if(Vector3.Distance(transform.localPosition, target) < distance){
				break;
			}
			yield return null;
		}
		m_finishRP.Value = true;
	}


	Vector3 GetRandomDir(){
		var val = UnityEngine.Random.Range(0,5);
		var move = Vector3.zero;
		switch(val){
		case 0:
			move = Vector3.left;
			break;
		case 1:
			move = Vector3.right;
			break;
		case 2:
			move = Vector3.forward;
			break;
		case 3:
			move = Vector3.back;
			break;
		default:
			move = Vector3.zero;
			break;
		}
		return move;
	}

	void MoveDir(Vector3 dir){
		dir *= 2;
		m_Character.Move(dir, false, false);
	}

	void MoveDir(Vector3 dir, float speed){
		dir *= speed;
		m_Character.Move(dir, false, false);
	}
		
	void DoCure(){
		m_Action.finishRP.Value = false;
		m_dispose = m_Action.finishRP.Where(p=>p==true).Subscribe(_=>EndAction(CureHP));
		m_Action.IsCure = true;
	}

	void CureHP(){
		m_myPawn.hp = m_myPawn.maxHp;
	}

	void DoHikick(){
		m_Action.finishRP.Value = false;
		m_dispose = m_Action.finishRP.Where(p=>p==true).Subscribe(_=>EndAction(GetDamage));
		m_Action.IsKick = true;
	}
		
	void GetDamage(){
		m_enemyPawn.hp -= 2;
		Debug.Log("HP=" + m_enemyPawn.hp.ToString());
	}

	void EndAction(Action action){
		action();
		m_dispose.Dispose();
		m_finishRP.Value = true;
	}

}
