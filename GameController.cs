using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public GameObject[] enemy;
	public GameObject[] bubbles;
	public GameObject heart;
	public Counter one;
	public Vector3 spawnValues;
	public float startWait;
	//Enemy Spawn Values
	public int enemyCount;
	public float enemySpawnWait;
	public float enemyWaveWait;
	//Bubble spawn Values
	public int bubbleCount;
	public float bubbleSpawnWait;
	public float bubbleWaveWait;
	public GameObject gameOverText;
	public GameObject restartText;
	//int to keep track of waves
	private int level;
	
	public Vector2 pos;
	public Vector2 size;
	public float offset;
	public Texture2D emptyTex;
	public Texture2D healthTex;
	public Texture2D breathTex;

	private float healthDisplay;
	private float breathDisplay;
	private bool restart;
	private bool gameOver;
	private GUIStyle style;
	private int score;


	void Start ()
	{
		level = 1;
		restart = false;
		gameOver = false;
		style = new GUIStyle ();
		style.fixedHeight = 0;
		style.fixedWidth = 0;
		style.stretchWidth = true;
		style.stretchHeight = true;
		StartCoroutine (SpawnEnemies ());
		StartCoroutine (SpawnBubbles ());
		healthDisplay = 0;
		breathDisplay = 0;

		pos = new Vector2 (Screen.width / 60.0f, (Screen.height * 8.0f / 9.0f));
		size = new Vector2 (Screen.width / 2.0f, Screen.height / 9.0f);
		offset = -(Screen.height / 18.0f);
	}
	

	void Update() {
		pos = new Vector2 (Screen.width / 60.0f, (Screen.height * 8.0f / 9.0f));
		size = new Vector2 (Screen.width / 2.0f, Screen.height / 9.0f);
		offset = -(Screen.height / 18.0f);
		if (restart) {
			if (Input.GetKeyDown (KeyCode.R)) {
				Application.LoadLevel (Application.loadedLevel);
			}
		}

		if (gameOver) {
			foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
				Destroy (enemy);
			}
			foreach (GameObject bubble in GameObject.FindGameObjectsWithTag("Bubble")) {
				Destroy (bubble);
			}
			foreach (GameObject heart in GameObject.FindGameObjectsWithTag("Heart")) {
				Destroy (heart);
			}

		}
	}

	IEnumerator SpawnEnemies ()
	{
		yield return new WaitForSeconds (startWait);
		while (true) {
			for (int i =0; i < enemyCount; i++) {
				int direction = RandomNegation();
				int rand = Random.Range (0,enemy.Length) % level;
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), direction * spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (enemy[rand], spawnPosition, spawnRotation);
				yield return new WaitForSeconds (enemySpawnWait);		
			}
			yield return new WaitForSeconds(enemyWaveWait);
			//change level aka difficulty
			level  += 1;
			enemyCount+= 2;
			enemySpawnWait -= (0.25f * 1/level);
			enemySpawnWait = Mathf.Clamp(enemySpawnWait,0.65f,3.0f);
			enemyCount = Mathf.Clamp (enemyCount,0,20);
			if (gameOver) {
				restart = true;
				restartText.SetActive(true);
				break;
			}
		}
	}

	IEnumerator SpawnBubbles ()
	{
		yield return new WaitForSeconds (startWait);
		while (true) {
			for (int i =0; i < bubbleCount; i++) {
				int direction = RandomNegation();
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), direction * spawnValues.y, spawnValues.z);
				int rand = Random.Range (0,bubbles.Length);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (bubbles[rand], spawnPosition, spawnRotation);
				yield return new WaitForSeconds (bubbleSpawnWait);		
			}
			if (level > 3) {
				int rand = Random.Range (0,2);
				if (level > 8) {
					rand = Random.Range(0,3);
				}
				if (rand >= 1) {
					int direction = RandomNegation();
					Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), direction * spawnValues.y, spawnValues.z);
					Quaternion spawnRotation = Quaternion.identity;
					Instantiate(heart,spawnPosition, spawnRotation);
				}
			}
			yield return new WaitForSeconds(bubbleWaveWait);
			if (gameOver) {
				restart = true;
				break;
			}
		}
	}
	
	void OnGUI() {
		//draws the health bar
		GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
		GUI.Box(new Rect(0,0, size.x, size.y), emptyTex, style);
		//draws the amount of health
		GUI.BeginGroup(new Rect(0,0, size.x * healthDisplay, size.y));
		GUI.Box(new Rect(0,0, size.x, size.y), healthTex, style);
		GUI.EndGroup();
		GUI.EndGroup();

		//draws the breath collected
		GUI.BeginGroup(new Rect(pos.x, pos.y - offset, size.x, size.y));
		GUI.Box(new Rect(0,0, size.x, size.y), emptyTex, style);
		//draw the amount of breath left
		GUI.BeginGroup(new Rect(0,0, size.x * breathDisplay, size.y));
		GUI.Box(new Rect(0,0, size.x, size.y), breathTex, style);
		GUI.EndGroup();
		GUI.EndGroup();
		}
	
	

	public void UpdateScore() { 
		one.Increment ();
	}
	public void GameOver() {
		gameOver = true;
		gameOverText.SetActive (true);
	}

	public int RandomNegation() {
		int rand = Random.Range (0, 2);
		if (rand == 1) {
			return 1;
		}
		return -1;
	}

	public void setHeath(float amount) {
		healthDisplay = amount;
	}
	public void setBreath(float amount) {
		breathDisplay = amount;
	}

}