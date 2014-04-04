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

    
    void OnMouseDown()
    {
        //animator.SetBool("selected", true);
        //animator.SetTrigger("playFlip");
    }

    public void flip()
    {
        animator.SetTrigger("playFlip");
    }

    /*
    void OnTriggerEnter2D(Collider2D col)
    {
        print("in");
        animator.SetBool("selected", true);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        print("out");
        animator.SetBool("selected", false);
    }
    */
}
