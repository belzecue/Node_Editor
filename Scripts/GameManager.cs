using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : SingletonMonoBehaviour<GameManager> {



	public List<GameObject> pawnObjList {
		get {
			return m_pawnObjList;
		}
	}



	List<GameObject> m_pawnObjList = new List<GameObject>();

	[SerializeField] GameObject m_unityChanPrefab;

	[SerializeField] GameObject[] m_poses;

	// Use this for initialization
	void Start () {
		for(int i=0;i<2;i++){
			var obj = Instantiate(m_unityChanPrefab) as GameObject;
			obj.transform.position = m_poses[i].transform.position;
			obj.GetComponent<Pawn>().uuid = i;
			obj.GetComponent<Pawn>().hp = 10;
			m_poses[i].GetComponent<Pawn>().uuid = i;
			m_pawnObjList.Add(obj);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject GetEnemey(int pawnUUID){
		return m_pawnObjList.Where(p=>p.GetComponent<Pawn>().uuid != pawnUUID).FirstOrDefault();
	}

	public GameObject GetSpawnPosition(int pawnUUID){
		return m_poses.Where(p=>p.GetComponent<Pawn>().uuid == pawnUUID).FirstOrDefault();
	}
}
