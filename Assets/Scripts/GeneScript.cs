using UnityEngine;
using System.Collections;
using System;

public class GeneScript : MonoBehaviour {

    Animator animator;
    internal SpriteRenderer _r;
    SpriteRenderer spriteRenderer
    {
        get
        {
            if(_r == null)
                _r = GetComponent<SpriteRenderer>();
            return _r;
        }

    }

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
        //transform.rotation *= Quaternion.Euler(0, 0, 180);
        flip();
        return 0;
        //return animator.playbackTime;
    }

    internal bool compare(GeneScript geneScript)
    {
        return colour == geneScript.colour;
    }

    public void changeType(char colour)
    {

        // test the char to see that it matches the case
        // if not change it!
        if (Char.IsLower(this.colour) && Char.IsUpper(colour))
        {
            colour = Char.ToLower(colour);
        }
        else if (Char.IsUpper(this.colour) && Char.IsLower(colour))
        {
            colour = Char.ToUpper(colour);
        }
        
        this.colour = colour;
        spriteRenderer.sprite = DNAScript.getSprite(colour);
    }
}
