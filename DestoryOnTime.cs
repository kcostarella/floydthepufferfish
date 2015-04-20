using UnityEngine;
using System.Collections;

public class DestoryOnTime : MonoBehaviour {
	public float seconds;
	// Use this for initialization
	void Start () {
		StartCoroutine (ImpendingDoom());
	}
	
	IEnumerator ImpendingDoom () {
		yield return new WaitForSeconds(seconds);
		Destroy (gameObject);
	}
}