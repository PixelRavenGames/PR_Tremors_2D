using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectEffect : HumanEffect {

	public GameObject objectPrefab;

	public Vector2 offsetFromTarget = Vector2.zero;

	private GameObject spawnedObject = null;

	public override void OnStartEffect(Human human) {
		if (!spawnedObject) spawnedObject = Instantiate(objectPrefab, (Vector2)human.transform.position + offsetFromTarget, Quaternion.identity);
	}

	public override void OnEndEffect(Human human) {
		if (spawnedObject) Destroy(spawnedObject);
		spawnedObject = null;
	}

}
