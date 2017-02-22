using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LitJson;
using System.IO;
using UnityEditor;

public class ipfsJson : MonoBehaviour {

    private JsonData itemData;

    //Help:http://answers.unity3d.com/questions/11021/how-can-i-send-and-receive-data-to-and-from-a-url.html
    //Verify IPFS in Daemon state working
    //Usando il gateway: https://gateway.ipfs.io/ipfs/QmcdH1f3mMZDVKGwhXW5nyBrSjK74o6r9G5xLcqetnhGeM

    // Use this for initialization
    void Start()
    {
        string url = "http://localhost:5001/api/v0/cat?arg=QmeABshHo61b8xgEUAkxXQN3qstfNKJPPy6eRmbUMd3Vtc"; //Fbx file
        //string url = "http://localhost:5001/api/v0/get?arg=QmcdH1f3mMZDVKGwhXW5nyBrSjK74o6r9G5xLcqetnhGeM"; //Cat txt file
        //string url = "http://localhost:5001/api/v0/id"; //ID Ipfs
        WWW www = new WWW(url);
        StartCoroutine(WaitForRequest(www));
    }

    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            /* OK FUNZIONA! ESP 10/01/2017
            Debug.Log("WWW Ok!: " + www.text);
            //https://www.youtube.com/watch?v=OyQQ-7-22Hw&t=825s //LitJson
            itemData = JsonMapper.ToObject(www.text);
            Debug.Log(itemData["ID"]);
            */

            //Response Header
            if (www.responseHeaders.Count > 0)
            {
                foreach (KeyValuePair<string, string> entry in www.responseHeaders)
                {
                    Debug.Log(entry.Value + "=" + entry.Key);
                }
            }

            /*
            //Response Header
            Debug.Log(www.responseHeaders["Content-Type"]);
            */

            string fullPath = Application.dataPath + "/Resources/duck.fbx";
            File.WriteAllBytes(fullPath, www.bytes);
            //http://stackoverflow.com/questions/36869444/programmatically-import-asset-in-hierarchy-unity
            //https://forum.unity3d.com/threads/loading-a-mesh-from-an-fbx-through-resource-load.80472/
            //GameObject testModel = (GameObject)Resources.Load("duck.fbx");
            //GameObject.Instantiate(testModel, new Vector3(0, 0, 0), Quaternion.identity);

            //Questo dovrebbe ricaricare, ma non lo fa. Il file viene salvato, ma non appare. Solo dopo refresh appare :-( e quindi credo sia caricabile.
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            Object prefab = Resources.Load("duck"); // Assets/Resources/Prefabs/prefab1.FBX
            GameObject t = (GameObject)Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);

            /*
            var loadedObjects = Resources.LoadAll("GameObjects", typeof(GameObject)).Cast<GameObject>();
            foreach (var go in loadedObjects)
            {
                Debug.Log(go.name);
            }
            */

        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
}