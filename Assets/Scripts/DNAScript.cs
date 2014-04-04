using UnityEngine;
using System.Collections;

public class DNAScript : MonoBehaviour
{

    public GameObject Gene;

    int length;
    public GameObject[] topStrand;
    public GameObject[] bottomStrand;

    Sprite[] sprites;

    // Use this for initialization
    public void Start()
    {
        sprites = Resources.LoadAll<Sprite>("dna");
        print(sprites.Length);
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
            // ideally throw an error
            print("top and bottom strands need to be the same length");
        }

        length = top.Length;
        topStrand = new GameObject[length];
        bottomStrand = new GameObject[length];

        GameObject gene;

        float margin = 0.3f;
        for (int i = 0; i < length; i++)
        {
            gene = createGene(top[i]);
            gene.transform.position = new Vector3(i * margin, 0, 0);

            gene = createGene(bottom[i]);
            gene.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            gene.transform.position = new Vector3(i * margin, 0, 0);
        }

    }

    public GameObject createGene(char type)
    {
        GameObject gene;
        SpriteRenderer sr;

        gene = Instantiate(Gene) as GameObject;
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
        return gene;
    }
}
