using UnityEngine;
using System;
using System.Collections;

public class DNA : MonoBehaviour
{

    static public GameObject Gene;
    static public Sprite[] Genes;

    public enum GATC : int
    {
        Red, Blue, Yellow, Green
    }

    [Serializable]
    public class DNA_Data
    {
        [SerializeField]
        private GATC[] _top;
        [SerializeField]
        private GATC[] _bottom;
        [SerializeField]
        private int _length;

        [SerializeField]
        public Transform parent;

        [SerializeField]
        public float stepSize = 0.5f;

        [SerializeField]
        private SpriteRenderer[] _topSprites;
        [SerializeField]
        private SpriteRenderer[] _bottomSprites;

        public GATC[] top
        {
            get
            {
                return _top;
            }
        }

        public GATC[] bottom
        {
            get
            {
                return _bottom;
            }
        }

        public SpriteRenderer[] topSprites
        {
            get
            {
                return _topSprites;
            }
        }

        public SpriteRenderer[] bottomSprites
        {
            get
            {
                return _bottomSprites;
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
                Array.Resize<GATC>(ref _top, _length);
                Array.Resize<GATC>(ref _bottom, _length);

                float step = parent.position.x;
                int len = 0;
                if (_topSprites != null)
                {
                    len = _topSprites.Length;
                    if (len != 0)
                        step = _topSprites[len - 1].transform.position.x + stepSize;
                }

                for (int i = _length; i < len; i++)
                {
                    if (Application.isEditor)
                    {
                        DestroyImmediate(_topSprites[i].gameObject, false);
                        DestroyImmediate(_bottomSprites[i].gameObject, false);
                    }
                    else
                    {
                        Destroy(_topSprites[i].gameObject);
                        Destroy(_bottomSprites[i].gameObject);
                    }
                }

                Array.Resize<SpriteRenderer>(ref _topSprites, _length);
                Array.Resize<SpriteRenderer>(ref _bottomSprites, _length);

                Vector3 pos = new Vector3(step, parent.position.y, parent.position.z);

                for (int i = len; i < _length; i++)
                {
                    GameObject obj = Instantiate(Gene, pos, Quaternion.identity) as GameObject;
                    obj.transform.parent = parent;
                    _topSprites[i] = obj.GetComponent<SpriteRenderer>();

                    obj = Instantiate(Gene, pos, Quaternion.Euler(0, 0, 180)) as GameObject;
                    obj.transform.parent = parent;
                    _bottomSprites[i] = obj.GetComponent<SpriteRenderer>();

                    pos.x += stepSize;
                }
            }
        }

    }


    public DNA_Data dna;

    // Use this for initialization
    void Start()
    {
        if (Gene == null)
        {
            Gene = Resources.Load<GameObject>("Gene");
        }
        if (Genes == null)
        {
            Genes = Resources.LoadAll<Sprite>("dna");
        }

        dna.parent = transform;
    }

    // Update is called once per frame
    void Update()
    {

    }
}