using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Transform PlayerCamera;
    [Header("MaxDistance you can open or close the door.")]
    public float MaxDistance = 5;

    private bool opened = false;
    private Animator anim;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Pressed();
            Debug.Log("You Press F");
        }
    }

    void Pressed()
    {



        if (transform.tag == "Door")
        {

            anim = transform.GetComponentInParent<Animator>();

            opened = !opened;

            anim.SetBool("Opened", !opened);
        }
    }
}