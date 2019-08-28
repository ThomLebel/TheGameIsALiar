using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	WeaponInfo weapon;

	public virtual void Attack(CharacterInformation characterInfo, string targetTag)
	{
		if (characterInfo.target != null)
		{
			if (characterInfo.target.tag == targetTag)
			{
				Debug.Log("We attack " + characterInfo.target.name);
				//EnemyScript enemyScript = attackedCell.GetComponent<EnemyScript>();

				//enemyScript.Hit(damage);
			}
		}
	}
}
