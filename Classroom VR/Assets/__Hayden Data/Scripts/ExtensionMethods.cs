using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods 
{
    public static void Reset(this LineRenderer lr)
    {
        lr.positionCount = 0;
    }
}
