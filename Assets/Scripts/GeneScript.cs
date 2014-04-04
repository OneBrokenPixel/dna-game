using UnityEngine;
using System.Collections;

public class GeneScript : MonoBehaviour {

    //SpriteCollection sprites;
    Animator animator;
    //private bool selected = false;

	// Use this for initialization
	void Start () {
        //sprites = new SpriteCollection("Spritesheet");
         animator = GetComponent<Animator>() as Animator;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("playFlip");
        }
	}

    
    void OnMouseDown()
    {
        //animator.SetBool("selected", true);
        //animator.SetTrigger("playFlip");
    }
    

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

}
