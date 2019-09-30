using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrigger : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D collision) {
		Human human = collision.GetComponent<Human>();
		if (human) human.Kill();
	}

}
