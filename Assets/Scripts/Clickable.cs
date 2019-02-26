using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    public Animator anim;
    public void Click()
    {
        if (anim)
        {
            anim.Play("Click");
        }
        switch (name)
        {
            case "Hotter":
                Manager.instance.RaiseHeat();
                break;
            case "Cooler":
                Manager.instance.LowerHeat();
                break;
            case "Start":
                Manager.instance.StartGame();
                break;
            case "Next":
                Manager.instance.Next();
                break;
            case "Back":
                Manager.instance.Back();
                break;
            default:
                break;
        }
    }
}
