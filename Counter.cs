using UnityEngine;
using System.Collections;

public class Counter : MonoBehaviour {
	public Counter next;
	private Animator anim;
	int value;
	int prev;
	private string[] nameDict;
	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator> ();
		value = 0;
		nameDict = new string[10] {"zero","one","two","three","four","five","six","seven","eight","nine"};
	}
	
	public void Increment() {
		value = value + 1;
	}

	void Update() {
		if (value >= 10) {
			next.Increment();
			value = value % 10;
		}
		foreach (string name in nameDict) {
			if (nameDict[value] != name) {
				anim.SetBool (name, false);
			}
		}
		anim.SetBool (nameDict[value], true);
	}
}
