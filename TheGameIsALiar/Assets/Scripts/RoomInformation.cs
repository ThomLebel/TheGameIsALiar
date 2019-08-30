using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RoomInformation")]
public class RoomInformation : ScriptableObject
{
	public string roomName;
	public Transform room;
	public List<RoomInformation> exits;
	[TextArea(3,10)]
	public string description;

}
