using UnityEngine;
using System.Collections;

using DarkRift;

public class NetworkObject : MonoBehaviour {

	//The ID of the client that owns this player (so we can check if it's us updating)
	public ushort objectID;

	Vector3 lastPosition;
	Quaternion lastRotation;
	Vector3 lastScale;

	void Start(){
		//Tell the network to pass data to our RecieveData function so we can process it.
		DarkRiftAPI.onData += RecieveData;
	}

	void Update(){
		//Only send data if we're connected and we own this player
		if( DarkRiftAPI.isConnected ){
			//We're going to use a tag of 1 for movement messages
			//If we're conencted and have moved send our position with subject 0.
			if(( transform.position != lastPosition ) || ( transform.rotation != lastRotation ))
			{
				SerialisePosRot(transform.position, transform.rotation);
			}
			//Update stuff
			lastPosition = transform.position;
			lastRotation = transform.rotation;
		}
	}

	void RecieveData(byte tag, ushort subject, object data){
		//Right then. When data is recieved it will be passed here, 
		//we then need to process it if it's got a tag of 1 or 2 
		//(the tags for position and rotation), check it's for us 
		//and update ourself.

		//The catch is we need to do this quite quickly because data
		//is going to be comming in thick and fast and it'll create 
		//lag if we spend time here.

		//If it has a ObjectUpdate tag then...
		if( tag == TagIndex.ObjectUpdate ){

			//...update our position and rotation
			if( subject == TagIndex.ObjectUpdateSubjects.PosRot){
                Debug.Log("Data recieved!");
                DeserialisePosRot (data);
			}
		}
	}
		
	void SerialisePosRot(Vector3 pos, Quaternion rot)
	{
		//Here is where we actually serialise things manually. To do this we need to add
		//any data we want to send to a DarkRiftWriter. and then send this as we would normally.
		//The advantage of custom serialisation is that you have a much smaller overhead than when
		//the default BinaryFormatter is used, typically about 50 bytes.
		using(DarkRiftWriter writer = new DarkRiftWriter())
		{
			//Next we write any data to the writer
			writer.Write(pos.x);
			writer.Write(pos.y);
			writer.Write(pos.z);
			writer.Write(rot.x);
			writer.Write(rot.y);
			writer.Write(rot.w);
			writer.Write(rot.z);

			DarkRiftAPI.SendMessageToOthers(TagIndex.ObjectUpdate, TagIndex.ObjectUpdateSubjects.PosRot, writer);
            Debug.Log("Data sent!");
        }
	}

	void DeserialisePosRot(object data)
	{
		//Here we decode the stream, the data will arrive as a DarkRiftReader so we need to cast to it
		//and then read the data off in EXACTLY the same order we wrote it.
		if( data is DarkRiftReader )
		{
			//Cast in a using statement because we are using streams and therefore it 
			//is important that the memory is deallocated afterwards, you wont be able
			//to use this more than once though.
			using(DarkRiftReader reader = (DarkRiftReader)data)
			{
				//Then read!
				transform.position = new Vector3(
					reader.ReadSingle(),
					reader.ReadSingle(),
					reader.ReadSingle()
				);

				transform.rotation = new Quaternion(
					reader.ReadSingle(),
					reader.ReadSingle(),
					reader.ReadSingle(),
					reader.ReadSingle()
				);
			}
		}
		else
		{
			Debug.LogError("Should have recieved a DarkRiftReciever but didn't! (Got: " + data.GetType() + ")");
			transform.position = transform.position;
			transform.rotation = transform.rotation;
		}
	}

}
