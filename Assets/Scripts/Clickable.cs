using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    public void Click()
    {
        if (name == "Hotter")
        {
            Manager.instance.RaiseHeat();
        }
        else
        {
            Manager.instance.LowerHeat();
        }
    }
}
