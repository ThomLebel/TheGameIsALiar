using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	public bool canPlay = true;
	public LayerMask blockingLayer;

	private Vector2 input;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		

		if (input.x == 0 && input.y == 0)
		{
			canPlay = true;
		}
		else
		{
			AttemptMove(input);
		}
	}

	void AttemptMove(Vector2 input)
	{
		if (!canPlay)
		{
			return;
		}

		RaycastHit2D hit;
		Vector2 start = transform.position;
		Vector2 end = start + input;

		transform.GetComponent<BoxCollider2D>().enabled = false;

		hit = Physics2D.Linecast(start, end, blockingLayer);

		transform.GetComponent<BoxCollider2D>().enabled = true;

		if (hit.transform == null)
		{
			transform.position = new Vector3(transform.position.x + input.x, transform.position.y + input.y, transform.position.z);
		}
		
		canPlay = false;
	}
}
