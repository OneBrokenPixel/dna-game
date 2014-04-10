using UnityEngine;
using System.Collections;

public class GeneScript : MonoBehaviour {

    Animator animator;
    //private bool selected = false;

	// Use this for initialization
	void Start () {
         animator = GetComponent<Animator>() as Animator;
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void flip()
    {
        animator.SetTrigger("playFlip");
    }

    public void flipFrom(Vector3 pos)
    {
        transform.position = pos;
        transform.rotation = Quaternion.Euler(0, 0, 180);
        flip();
    }
}
