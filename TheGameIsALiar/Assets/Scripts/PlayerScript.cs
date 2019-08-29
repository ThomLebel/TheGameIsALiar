using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : CharacterScript
{
	public Text manaBarText;
	public Image manaBar;
	public Text expBarText;
	public Image expBar;

	private Vector2 input;

	// Start is called before the first frame update
	public override void Start()
    {
		base.Start();
		currentWeapon.weapon.owner = GameConstants.TAG_Player;
		characterInfo.direction = new Vector2(0f, -1f);
		UpdateHealthBar();
	}

    // Update is called once per frame
    void Update()
    {
		if (!GameMaster.Instance.playerTurn) return;

		//Player's movement
		input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		if (input.x != 0)
		{
			input.y = 0;
		}

		if (input.x != 0 || input.y != 0)
		{
			characterInfo.direction = input;
			AttemptMove(input);
		}

		//Player's action
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Action();
		}
	}

	public override void AttemptMove(Vector2 input)
	{
		base.AttemptMove(input);

		GameMaster.Instance.playerTurn = false;
	}

	public override void CantMove(Transform target)
	{
		characterInfo.origin = transform;
		characterInfo.target = target;

		if (target.tag == GameConstants.TAG_Enemy)
		{
			Attack(target);
		}
	}

	void Action()
	{
		characterInfo.origin = transform;
		characterInfo.target = CheckNextCell(characterInfo.direction, blockingLayer);

		if (characterInfo.target == null)
		{
			Attack(characterInfo.target);
		}
		else if (characterInfo.target.tag == GameConstants.TAG_Interactive)
		{
			Interact(characterInfo.target);
		}
		else
		{
			Attack(characterInfo.target);
		}
	}

	void Attack(Transform attackedCell)
	{
		currentWeapon.Attack(characterInfo, GameConstants.TAG_Enemy);

		StartCoroutine(CameraShake.Instance.Shake());
		GameMaster.Instance.ActivateEnemies();
		GameMaster.Instance.playerTurn = false;
	}

	void Interact(Transform interactiveCell)
	{
		Debug.Log("We interact with "+interactiveCell.name);

		GameMaster.Instance.playerTurn = false;
	}

	public override void TakeDamage(float damageTaken)
	{
		base.TakeDamage(damageTaken);
		UpdateHealthBar();
		StartCoroutine(CameraShake.Instance.Shake());
	}

	public override void UpdateHealthBar()
	{
		base.UpdateHealthBar();
		healthBarText.text = (health + "/" + maxHealth).ToString();
	}
}
