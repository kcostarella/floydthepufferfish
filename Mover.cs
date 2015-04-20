using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	private Rigidbody2D body;
	public float speed;
	// Use this for initialization
	void Start () {
		body = gameObject.GetComponent<Rigidbody2D> ();
		if (transform.position.y < 0) {
			speed = speed * -1;
		}
		body.velocity = new Vector2 (0.0f, -1.0f) * speed;
	}


	// Update is called once per frame
	void Update () {
	
	}
}
