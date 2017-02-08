using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSystem : MonoBehaviour {

	public GameObject cloud_system1;
	public GameObject cloud_system2;
	public GameObject cloud_system3;
	public GameObject cloud_system4;

	private GameObject cur_cloud_system;
	// Use this for initialization
	void Start () {
		cur_cloud_system = cloud_system3;
		cur_cloud_system = Instantiate(cur_cloud_system,this.transform.position,Quaternion.identity) as GameObject;
		cur_cloud_system.name = cloud_system3.name;
	}
}
