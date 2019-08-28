using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public WeaponInfo weapon;

	public virtual void Attack(CharacterInformation characterInfo, string targetTag)
	{
		if (characterInfo.target != null)
		{
			if (characterInfo.target.tag == targetTag)
			{
				CharacterScript targetScript = characterInfo.target.GetComponent<CharacterScript>();

				targetScript.TakeDamage(weapon.damage);
			}
		}
	}
}
