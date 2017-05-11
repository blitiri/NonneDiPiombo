using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IComparerClass : IComparer<GameObject> {

    public int Compare(GameObject x, GameObject y)
    {
        if (x.layer < y.layer)
        {
            return -1;
        }
        else if (x.layer > y.layer)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
