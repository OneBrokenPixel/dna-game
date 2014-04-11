using UnityEngine;
using System.Collections;

public class GeneScript : MonoBehaviour {

    Animator animator;
    //SpriteRenderer spriteRenderer;
    [HideInInspector]
    public char colour { get; private set; }


    //private bool selected = false;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>() as Animator;
        //spriteRenderer = GetComponent<SpriteRenderer>();
           
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void flip()
    {
        animator.SetTrigger("playFlip");
    }

    public float flipFrom(Vector3 pos)
    {
        transform.position = pos;
        //transform.rotation = Quaternion.Euler(0, 0, 180);
        flip();
        return animator.playbackTime;

    }

    public void changeType(char colour)
    {
        this.colour = colour;
        GetComponent<SpriteRenderer>().sprite = DNAScript.getSprite(colour);
    }
}
