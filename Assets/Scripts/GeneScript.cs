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
}
