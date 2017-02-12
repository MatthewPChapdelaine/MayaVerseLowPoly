using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerStart : MonoBehaviour {

    public GameObject Prefab;

    bool quitting;

    // Use this for initialization
    void Start () {
        
        //Make game working in background

        //Load all serializable objects
        SerializableManager.LoadAll();

        //Create object
        //GameObject newObject = SerializableManager.PrefabInstantiate(Prefab);
        //newObject.transform.position = new Vector3(9.908f, 0.645f, 8.2f);

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
