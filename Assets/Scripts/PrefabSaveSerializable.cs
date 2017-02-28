using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SerializableComponent))]
[Save(true,true,true)]
public class PrefabSaveSerializable : MonoBehaviour {
    //Save rot, trans, scale 

	void Update() {
		//If under the ground destroy
		if (transform.position.y < -2) {
			Destroy(gameObject);
		}
	}
}
