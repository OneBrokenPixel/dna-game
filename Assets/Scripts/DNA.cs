using UnityEngine;
using System;
using System.Collections;

public enum GATC
{
    Red, Blue, Yellow, Green
}

[Serializable]
public struct SDNA
{
    private GATC[] _top;
    private GATC[] _bottom;

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
            if (_length == 0)
            {
                _top = null;
                _bottom = null;
            }
            else
            {
                Array.Resize<GATC>(ref _top, _length);
                Array.Resize<GATC>(ref _bottom, _length);
            }
        }
    }

    public SDNA(int length)
    {
        _top = null;
        _bottom = null;
        _length = 0;

        this.length = length;
    }

}

public class DNA : MonoBehaviour {

    public SDNA dna = new SDNA();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
