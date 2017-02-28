using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerStart : MonoBehaviour {

    public GameObject Prefab;

    bool quitting;

    // Use this for initialization
    void Start () {

        //Make game working in background
        Application.runInBackground = true;
		//PlayerPrefsX.SetBool ("ManasSpawned", false); //DEBUG

		INIParser ini = new INIParser();
		// Open the save file. If the save file does not exist, INIParser automatically create
		// one
		ini.Open(Application.dataPath + "/MayaVerseLowPoly.ini");

		//Check all objects spawned
		if (ini.ReadValue("ObjectSpawned","Spawned",false) == false)
		{	
        	//Create object
        	GameObject newObject = SerializableManager.PrefabInstantiate(Prefab);
        	//newObject.transform.position = new Vector3(9.908f, 0.645f, 8.2f);
			newObject.transform.position = new Vector3(0, 0, 0);
			//Assign identifier
			newObject.GetComponent<NetworkObject> ().objectID = 0;
			ini.WriteValue ("ObjectSpawned", "Spawned", true);
		}
		else
		{
			//Load all serializable objects
			SerializableManager.LoadAll();
		}

		//Check GUI Debug Log 
		if (ini.ReadValue ("DebugLog", "GUIDebug", false) == false) {
			GetComponent<Log> ().enabled = false;
			//ini.WriteValue ("DebugLog", "GUIDebug", true);
		} else {
			GetComponent<Log> ().enabled = true;
			//ini.WriteValue ("DebugLog", "GUIDebug", false);
		}

		/*
		//Delete objects
		//to erase those now!
		foreach (PrefabSaveSerializable Manas in GameObject.FindObjectsOfType<PrefabSaveSerializable>()) {
			Destroy(Manas.gameObject);
		}
		*/

		//Close file
		ini.Close();
    }

    // On quiting Application save all
    void OnApplicationQuit()
    {
        quitting = true;
        //Save all serializable objects	
        SerializableManager.SaveAll();
    }
    
	void OnDestroy()
    {
        if (quitting)
        {
            //application is quitting; don't spawn more stuff
        }
        else
        {
            //application is running, proceed normally
        }
    }
}
