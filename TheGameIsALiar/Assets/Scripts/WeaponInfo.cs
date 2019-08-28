using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponInfo")]
public class WeaponInfo : ScriptableObject
{
	public string weaponName;
	public string owner;
	public int damage;
	[Tooltip("The type of munition this weapon uses, arrow for bow and nothing for sword")]
	public GameObject munition;
	[Tooltip("The number of munitions this weapon has")]
	public int ammo = 0;
}
