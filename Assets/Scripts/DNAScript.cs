using UnityEngine;
using System.Collections;

// Attached to: DNA game object.
// Needs: 1 gene prefab.

public class DNAScript : MonoBehaviour
{
    // What the dna is made of. Prefab.
    public GameObject Gene;

    public int length { get; set; }
    public GeneScript[] topStrand;
    public GeneScript[] bottomStrand;

    public int Length
    {
        get { return Mathf.Min(topStrand.Length, bottomStrand.Length); }
    }

    public static Sprite[] sprites;

    // Use this for initialization
    public void Start()
    {
        length = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void createDNA(string top, string bottom)
    {
        if (top.Length != bottom.Length)
        {
            // TODO: ideally throw an error
            print("top and bottom strands need to be the same length");
        }

        length = top.Length;
        topStrand = new GeneScript[length];
        bottomStrand = new GeneScript[length];

        float margin = 0.3f;    // padding between each gene
        Vector3 pos = new Vector3(transform.position.x, transform.position.y);
        
        /*
        for (int i = 0; i < length; i++)
        {
            topStrand[i] = createGene(top[i], pos, Quaternion.identity);

            bottomStrand[i] = createGene(bottom[i], pos,Quaternion.Euler(new Vector3(0, 0, 180)));

            pos.x += margin;
        }
        */

        // trying something different - easier than positioning in main
        int mid = Mathf.CeilToInt(length/2);
        Vector3 leftPos = new Vector3(pos.x - margin, pos.y);
        Vector3 rightPos = new Vector3(pos.x, pos.y);

        for (int i = 0; i < mid; i++)
        {
            int left = mid - i - 1;
            int right = mid + i;
            
            topStrand[left] = createGene(top[left], leftPos, Quaternion.identity);
            topStrand[right] = createGene(top[right], rightPos, Quaternion.identity);

            bottomStrand[left] = createGene(bottom[left], leftPos, Quaternion.Euler(new Vector3(0, 0, 180)));
            bottomStrand[right] = createGene(bottom[right], rightPos, Quaternion.Euler(new Vector3(0, 0, 180)));

            leftPos.x -= margin;
            rightPos.x += margin;
        }
        

    }

    public GeneScript createGene(char type, Vector3 pos, Quaternion rot)
    {
        GameObject gene;
        SpriteRenderer sr;

        gene = Instantiate(Gene,pos, rot) as GameObject;
        gene.transform.parent = transform;
        sr = gene.GetComponent<SpriteRenderer>();

        switch (type)
        {
            case 'R':
                sr.sprite = sprites[2];
                break;
            case 'r':
                sr.sprite = sprites[8];
                break;
            case 'B':
                sr.sprite = sprites[1];
                break;
            case 'b':
                sr.sprite = sprites[5];
                break;
            case 'G':
                sr.sprite = sprites[0];
                break;
            case 'g':
                sr.sprite = sprites[6];
                break;
            case 'Y':
                sr.sprite = sprites[4];
                break;
            case 'y':
                sr.sprite = sprites[7];
                break;
        }

        //gene.AddComponent("BoxCollider2D");
        return gene.GetComponent<GeneScript>();
    }


    // flips genes in a box area - temporary, will be changed later
    public void flipGenesInArea(Vector3 bottomLeft, Vector3 topRight)
    {
        for (int i = 0; i < topStrand.Length; i++)
        {
            if (topStrand[i].transform.position.x > bottomLeft.x && topStrand[i].transform.position.x < topRight.x)
            {
                topStrand[i].GetComponent<Animator>().SetTrigger("playFlip");
            }
            if (bottomStrand[i].transform.position.x > bottomLeft.x && bottomStrand[i].transform.position.x < topRight.x)
            {
                bottomStrand[i].GetComponent<Animator>().SetTrigger("playFlip");
            }
        }
    }

}
