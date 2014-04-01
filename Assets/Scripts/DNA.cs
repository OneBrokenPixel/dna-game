using UnityEngine;
using System;
using System.Collections;

public class DNA : MonoBehaviour {

    public enum GATC
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
            }
        }

    }


    public DNA_Data dna;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
