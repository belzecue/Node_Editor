using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BehaviourTrees
{

	public class UniqNum {
		public static int num = 0;
	}

	/// <summary>
	/// 全てのBehaviourTreeが継承するクラス
	/// Class that all of BehaviorTree inherits
	/// </summary>
	public abstract class BehaviourTreeBase {

		public int uuid {
			get {
				return m_uuid;
			}
		}
		
		int m_uuid;

		protected virtual void Init(int uuid = 0){
			this.m_uuid = uuid;
			this.key =  this.ToString() + uuid.ToString();
		}

		public string key {
			private set; get;
		}


		public abstract void ObserveInit(BehaviourTreeInstance behaviourTreeInstance);

		public virtual void Execute(BehaviourTreeInstance behaviourTreeInstance) {
			#if UNITY_EDITOR
			behaviourTreeInstance.nowExcuteUuid = m_uuid;
			#endif
		}
			

		public abstract void Delete();
			
	}
}

