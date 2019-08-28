using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
	public override void Attack(CharacterInformation characterInfo, string targetTag)
	{
		if (weapon.ammo <= 0)
		{
			base.Attack(characterInfo, targetTag);
			return;
		}

		GameObject shot = Instantiate(weapon.munition, characterInfo.origin.position, characterInfo.origin.rotation);
		shot.GetComponent<Arrow>().direction = characterInfo.direction;
		shot.GetComponent<Arrow>().owner = weapon.owner;
		GameMaster.Instance.ActivateEnemies();

		weapon.ammo--;
	}
}
