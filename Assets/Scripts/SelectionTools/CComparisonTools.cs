using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

class CComparisonTools
{
    public static DNAScript[] s_input; // is reference to input array
    public static DNAScript s_goal;

    public static float compare( int index )
    {
        if (index >= 0 && index < s_input.Length)
            return s_input[index].compare(s_goal);
        else
            return -1f;
    }


    public static IEnumerable<float> compare()
    {
        foreach( DNAScript dna in s_input)
        {
            yield return dna.compare(s_goal); ;
        }
    }

    public static void UpdateCompleteness()
    {
        foreach (DNAScript dna in s_input)
        {
            dna.compare(s_goal); ;
        }
    }

}
