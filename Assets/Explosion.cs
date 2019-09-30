using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Explosion : MonoBehaviour
{
    public void Explode(float radius)
    {
        transform.localScale = new Vector3(radius * 4, radius * 4, 1);
        GetComponent<Animator>().SetTrigger("Explode");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
