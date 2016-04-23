using UnityEngine;
using System.Collections;
using UniRx;

public class ActionController : MonoBehaviour {

	private static readonly string hashAttack = "Attack";
	private static readonly string hashCure = "Cure";

	StateMachineObservalbes m_stateMachineObservables;

	public ReactiveProperty<bool> finishRP {
		get; private set;
	}

	public bool IsKick {
		set {
			if(value){
				this.finishRP.Value = false;
				Debug.Log("Do Attack");
				m_anim.SetBool(hashAttack,true);
			}
		}
	}


	public bool IsCure {
		set {
			if(value){
				this.finishRP.Value = false;
				Debug.Log("Do Attack");
				m_anim.SetBool(hashCure,true);
			}
		}
	}


	public bool IsDamage {
		set {
			if(value){
				this.finishRP.Value = false;
				Debug.Log("Damage");
				m_anim.SetBool("Damage",true);
			}
		}
	}
		
	Animator m_anim;


	void Start(){
		this.finishRP = new ReactiveProperty<bool>(false);
		m_anim = GetComponent<Animator>();
		m_stateMachineObservables = m_anim.GetBehaviour<StateMachineObservalbes>();

		m_stateMachineObservables
			.OnStateEnterObservable
			.Where(p=>p.IsName("Base Layer."+hashAttack))
			.Subscribe(p =>Stop(hashAttack));

		m_stateMachineObservables
			.OnStateEnterObservable
			.Where(p=>p.IsName("Base Layer."+hashCure))
			.Subscribe(p =>Stop(hashCure));
	}

	void Stop(string anim){
		m_anim.SetBool(anim,false);
		this.finishRP.Value = true;
	}

}