using UnityEngine;
using System;
using System.Collections;
using Darkhexxa.SimplePool;

[ExecuteInEditMode]
public class DNA : MonoBehaviour {

    
    private static GameObject gene = null;

    private static Sprite redGene = null;
    private static Sprite blueGene = null;
    private static Sprite greenGene = null;
    private static Sprite yellowGene = null;


    public DNA_Data dna;

    static DNA()
    {
        gene = Resources.Load("Gene") as GameObject;

        Sprite[] geneSprites = Resources.LoadAll<Sprite>("dna");

        redGene = geneSprites[0];
        greenGene = geneSprites[1];
        yellowGene = geneSprites[2];
        blueGene = geneSprites[3];

        Debug.Log(geneSprites.Length);
        Debug.Log(redGene);
        Debug.Log(blueGene);
        Debug.Log(greenGene);
        Debug.Log(yellowGene);

    }

    public enum GATC
    {
        Red, Blue, Yellow, Green
    }

    public class Gene {

        
        private void SetObjSprite()
        {
            if (_obj != null)
            {
                switch (_type)
                {
                    case GATC.Red:
                        _obj.sprite = redGene;
                        break;
                    case GATC.Blue:
                        _obj.sprite = blueGene;
                        break;
                    case GATC.Green:
                        _obj.sprite = greenGene;
                        break;
                    case GATC.Yellow:
                        _obj.sprite = yellowGene;
                        break;
                    default:
                        _obj.sprite = null;
                        break;
                }
            }
        }

        private GATC _type;

        public GATC type
        {
            get { return _type; }
            set
            {
                _type = value;
                SetObjSprite();
                
            }
        }

        private SpriteRenderer _obj = null;
        public SpriteRenderer obj
        {
            get { return _obj; }
            set
            {
                _obj = value;
                SetObjSprite();
            }
        }


    }

    [Serializable]
    public class DNA_Data
    {
        private Gene[] _top;
        private Gene[] _bottom;

        private int _length;

        private Transform _parent;

        public Transform parent
        {
            get { return _parent; }
            set { _parent = value; }
        }


        private bool _isDirty;
        public bool isDirty {
            get
            {
                return _isDirty;
            }
            set
            {
                _isDirty = value;
            }
        }

        public Gene[] top
        {
            get
            {
                return _top;
            }
        }

        public Gene[] bottom
        {
            get
            {
                return _bottom;
            }
        }

        public int length
        {
            get
            {
                return _length;
            }

            set
            {
                _length = value;
                if (_length == 0)
                {
                    _top = null;
                    _bottom = null;
                }
                else
                {
                    if (_top == null)
                        _top = new Gene[_length];

                    if (_bottom == null)
                        _bottom = new Gene[_length];

                    int min = Mathf.Min(_top.Length,_bottom.Length);

                    for (int i = _length; i < min; i++)
                    {
                        if (_top[i] != null && _top[i].obj != null)
                            if (Application.isEditor)
                                DestroyImmediate(_top[i].obj);
                            else
                                Destroy(_top[i].obj);
                        if (_bottom[i] != null && _bottom[i].obj != null)
                            if (Application.isEditor)
                                DestroyImmediate(_bottom[i].obj);
                            else
                                Destroy(_bottom[i].obj);
                    }

                    Array.Resize<Gene>(ref _top, _length);
                    Array.Resize<Gene>(ref _bottom, _length);

                    for (int i = 0; i < _length; i++)
                    {
                        if (_top[i] == null)
                        {
                            _top[i] = new Gene();
                            GameObject obj = Instantiate(gene) as GameObject;
                            obj.transform.parent = _parent;
                            _top[i].obj = obj.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
                        }
                        if (_bottom[i] == null)
                        {
                            _bottom[i] = new Gene();
                            GameObject obj = Instantiate(gene) as GameObject;
                            obj.transform.parent = _parent;
                            _bottom[i].obj = obj.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
                        }
                    }
                }
            }

        }
    }


	// Use this for initialization
    void Start()
    {
        dna.parent = transform;
    }

	// Update is called once per frame
	void Update () {

	}
}
