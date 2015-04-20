using UnityEngine;
using System.Collections;

public class heatSeeker : MonoBehaviour {
	private Rigidbody2D body;
	public float speed;
	private GameObject player;
	// Use this for initialization
	void Start () {
		body = gameObject.GetComponent<Rigidbody2D> ();
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	// Update is called once per frame
	void FixedUpdate () {
		if (player != null) {
			Vector3 direction = (gameObject.transform.position - player.transform.position).normalized;
			body.velocity = new Vector2 (-direction.x, -direction.y) * speed;
		}
	}
}
