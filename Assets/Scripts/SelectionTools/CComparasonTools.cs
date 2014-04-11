using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

class CComparasonTools
{
    public static DNAScript[] s_input; // is reference to input array
    public static DNAScript s_goal;

    public static IEnumerable<float> compare()
    {
        foreach( DNAScript dna in s_input)
        {
            int correct = 0;

            for (int i = 0; i < dna.length; i++)
            {
                correct += (dna.topStrand[i].compare(s_goal.topStrand[i])) ? 1 : 0;
                correct += (dna.bottomStrand[i].compare(s_goal.bottomStrand[i])) ? 1 : 0;
            }
            yield return ((float)correct)/(dna.length*2.0f);
        }
    }

}
