using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    public Animator anim;
    public AudioSource aud;
    float hoverTime = 0;
    public void Click()
    {
        if (anim)
        {
            anim.Play("Click");
        }
        if (aud)
        {
            aud.Play();
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

    void Update()
    {
        if (hoverTime + 0.2f < Time.time)
        {
            anim.SetBool("Hover", false);
        }
    }
    public void Hover()
    {
        anim.SetBool("Hover", true);
        hoverTime = Time.time;
    }
}
