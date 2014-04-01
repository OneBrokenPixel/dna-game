using UnityEngine;
using System.Collections;


public class Gene : MonoBehaviour {

    public static Sprite[] Genes;

    public enum GATC : int
    {
        Red, Blue, Yellow, Green
    }

    private GATC _type;
    public GATC type
    {
        get
        {
            return _type;
        }
        set
        {
            _type = value;
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = Genes[(int)_type];
            }
        }
    }
    public SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
        if (Genes == null)
        {
            Genes = Resources.LoadAll<Sprite>("dna");
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
