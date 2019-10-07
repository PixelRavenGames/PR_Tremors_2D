using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HumanEffect : MonoBehaviour {

	public string id = "NULL";

	public virtual void OnStartEffect(Human human) {/*MT*/}

	public virtual void OnEndEffect(Human human) {/*MT*/}

	public virtual void OnDamageTaken(Human human, float damage) {/*MT*/}
	
}
