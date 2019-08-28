using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	public float health = 10f;
	public float damage = 5f;
	public Weapon currentWeapon;
	
	public float shakeDuration = 0.15f;
	public float shakeMagnitude = .2f;
	//public bool canPlay = true;
	public LayerMask blockingLayer;
	
	private CharacterInformation characterInfo;
	private Vector2 input;
	[SerializeField]
	private Vector2 currentDir = new Vector2(0f,-1f);

	private void Awake()
	{
		characterInfo.ownCollider = GetComponent<BoxCollider2D>();
	}

	// Start is called before the first frame update
	void Start()
    {
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
			currentDir = input;
			AttemptMove(input);
		}

		//Player's action
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Action();
		}
	}

	void AttemptMove(Vector2 input)
	{
		Vector3 originalPos = transform.position;
		Transform targetCell = CheckNextCell(currentDir, blockingLayer);

		if (targetCell == null)
		{
			transform.position = new Vector3(originalPos.x + input.x, originalPos.y + input.y, originalPos.z);
		}
		else
		{
			if (targetCell.tag == GameConstants.TAG_Enemy)
			{
				Attack(targetCell);
			}
		}

		GameMaster.Instance.playerTurn = false;
	}

	void Action()
	{
		characterInfo.origin = transform;
		characterInfo.direction = currentDir;
		characterInfo.target = CheckNextCell(currentDir, blockingLayer);

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
		if (attackedCell != null)
		{
			if (attackedCell.tag == GameConstants.TAG_Enemy)
			{
				Debug.Log("We attack "+attackedCell.name);
				EnemyScript enemyScript = attackedCell.GetComponent<EnemyScript>();

				enemyScript.Hit(damage);
			}
		}

		StartCoroutine(CameraShake.Instance.Shake(shakeDuration, shakeMagnitude));
		GameMaster.Instance.ActivateEnemies();
		GameMaster.Instance.playerTurn = false;
	}

	void Interact(Transform interactiveCell)
	{
		Debug.Log("We interact with "+interactiveCell.name);

		GameMaster.Instance.playerTurn = false;
	}

	public void Hit(float damageTaken)
	{
		health -= damageTaken;
		StartCoroutine(CameraShake.Instance.Shake(shakeDuration, shakeMagnitude));
		if (health <= 0)
		{
			health = 0;
		}
	}

	private Transform CheckNextCell(Vector2 direction, LayerMask mask)
	{
		RaycastHit2D hit;
		Vector2 start = transform.position;
		Vector2 end = start + direction;

		characterInfo.ownCollider.enabled = false;

		hit = Physics2D.Linecast(start, end, mask);

		characterInfo.ownCollider.enabled = true;

		return hit.transform;
	}

	//public struct CharInfo
	//{
	//	public Transform origin;
	//	public Vector2 direction;
	//	public Collider2D ownCollider;
	//	public Transform target;
	//}
}
