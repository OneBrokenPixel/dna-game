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

    public static Sprite[] sprites;

    public static Sprite getSprite(char colour)
    {
        switch (colour)
        {
            case 'R':
                return sprites[2];
            case 'r':
                return sprites[8];
            case 'B':
                return sprites[1];
            case 'b':
                return sprites[5];
            case 'G':
                return sprites[0];
            case 'g':
                return sprites[6];
            case 'Y':
                return sprites[4];
            case 'y':
                return sprites[7];
            default:
                return null;
        }
    }

    // Use this for initialization
    public void Start()
    {
        sprites = Resources.LoadAll<Sprite>("dna");
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
        
        // position the genes starting from the middle of the array
        // this keeps the dna centred
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
        GeneScript script;

        gene = Instantiate(Gene,pos, rot) as GameObject;
        gene.transform.parent = transform;
        script = gene.GetComponent<GeneScript>();
        changeColour(script, type);

        return script;
    }

    public void clearGenes()
    {
        for( int i = 0; i <transform.childCount; i++ )
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        topStrand = null;
        bottomStrand = null;
        length = 0;
    }

    public void changeColour(GeneScript script, char colour)
    {
        //Sprite sprite = getSprite(colour);
        script.changeType(colour);
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
