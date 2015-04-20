using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary {
	public float xMin, xMax, yMin, yMax;
}

public class PlayerController : MonoBehaviour {
	public float epsilon;
	public float velocity;
	public Boundary boundary;
	public GameObject splat;
	public GameObject pop;
	public GameObject[] squishSounds;
	public GameObject[] damageSounds;
	public GameObject[] bubbleSounds;
	public GameObject[] heartSounds;
	public GameObject popSound;
	public GameObject suckIn;
	public GameObject blowOut;

	
	private Animator anim;
	private float probabiltyNotBlink;
	private Rigidbody2D body;
	private BoxCollider2D floydcollider;
	private CircleCollider2D puffcollider;
	private GameController gameController;
	private float health;
	private float breath;
	private bool alive;
	private bool invinsible;

	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		body = gameObject.GetComponent<Rigidbody2D> ();
		floydcollider = gameObject.GetComponent<BoxCollider2D> ();
		puffcollider = gameObject.GetComponent<CircleCollider2D> ();
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		gameController = gameControllerObject.GetComponent<GameController>();
		health = 1.0f;
		breath = 0.0f;
		puffcollider.enabled = false;
		probabiltyNotBlink = 1.0f;
		alive = true;
	}

	IEnumerator damageBlink () {
		int i = 5;
		while (i > 0) {
			invinsible = true;
			gameObject.GetComponent<SpriteRenderer>().enabled = false;
			yield return new WaitForSeconds(0.1f);
			gameObject.GetComponent<SpriteRenderer>().enabled = true;
			yield return new WaitForSeconds(0.1f);
			i = i - 1;
		}
		invinsible = false;
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag != "Background") {
			if (other.tag == "Enemy") {
				if (isPuffed()) {
					Instantiate(splat, other.gameObject.transform.position, Quaternion.identity);
					int rand = Random.Range(0,squishSounds.Length);
					squishSounds[rand].GetComponent<AudioSource>().Play();
					gameController.UpdateScore();
				} else if (invinsible == false){
					health -= 0.22f;
					StartCoroutine(damageBlink());
					int rand = Random.Range(0,damageSounds.Length);
					damageSounds[rand].GetComponent<AudioSource>().Play();
				}
			}
			if (other.tag == "Bubble") {
				if (!isPuffed ()) {
					breath += 0.25f;
					int rand = Random.Range(0,bubbleSounds.Length);
					bubbleSounds[rand].GetComponent<AudioSource>().Play();
				} else {
					popSound.GetComponent<AudioSource>().Play ();
					Instantiate(pop, other.gameObject.transform.position,Quaternion.identity);
				}
			}

			if (other.tag == "Heart") {
				health += 0.22f;
				int rand = Random.Range(0,heartSounds.Length);
				heartSounds[rand].GetComponent<AudioSource>().Play();

			}

			Destroy (other.gameObject);
		}
	}
	
	private bool isBlinking() {
		return anim.GetBool ("blink");
	}
	
	public bool isPuffed() {
		return anim.GetBool ("puff");
	}
	
	void checkToBlink() {
		float rand = Random.Range (0.0f, 1.0f);
		
		if (isBlinking () == false) {
			
			if (rand > probabiltyNotBlink) {
				anim.SetBool ("blink", true);
			} else {
				probabiltyNotBlink = probabiltyNotBlink * (1 - epsilon);
			}
		} else {
			anim.SetBool ("blink",false);
			probabiltyNotBlink = 1.0f;
		}
	}
	
	void Move()
	{
		float xcoordinate = Input.GetAxis ("Horizontal");
		float ycoordinate = Input.GetAxis ("Vertical");
		body.velocity = new Vector2 (xcoordinate, ycoordinate) * velocity;
		if (xcoordinate > 0.0f) {
			gameObject.transform.rotation = Quaternion.Euler(0,180,0);
		} else if (xcoordinate < 0.0f) {
			gameObject.transform.rotation =  Quaternion.Euler(0,0,0);
		}
		body.position = new Vector2 (
			Mathf.Clamp (body.position.x, boundary.xMin, boundary.xMax),
			Mathf.Clamp (body.position.y, boundary.yMin, boundary.yMax));
	}
	
	void Puff() 
	{
		if (Input.GetButtonDown ("Fire1") && Mathf.Approximately(breath, 1.0f)) {
			floydcollider.enabled = false;
			anim.SetBool ("puff", true);
			puffcollider.enabled = true;
			suckIn.GetComponent<AudioSource>().Play();
		}
	}
	
	// Update is called once per frame
	void Update () {
		checkToBlink ();
		Puff ();
		if (isPuffed ()) {
			breath -= 0.2f * Time.deltaTime;
		}
		breath = Mathf.Clamp(breath, 0.0f, 1.0f);
		health = Mathf.Clamp(health,0.0f,1.0f);
		gameController.setHeath (health);
		gameController.setBreath (breath);
		if (puffcollider.enabled && Mathf.Approximately (breath, 0.0f)) {
			floydcollider.enabled = true;
			anim.SetBool("puff",false);
			puffcollider.enabled = false;
			blowOut.GetComponent<AudioSource>().Play ();
		}
		if (health <= 0) {
			anim.SetBool("dead",true);
			gameObject.transform.rotation = Quaternion.Euler(0,0,180);
			gameController.GameOver();
			alive = false;
		}
	}
	
	void FixedUpdate() {
		if (alive) {
			Move ();
		} else {
			body.velocity = new Vector2(0.0f,1.0f);
		}
	}
}
