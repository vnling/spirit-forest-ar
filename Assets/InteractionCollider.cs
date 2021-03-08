using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionCollider : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public GameObject domeSphere;
    private bool played;

     private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Rock"))
        {

            Debug.Log("rock entered zone");
            if (!played)
            {
                audioSource.PlayOneShot(audioClip);
                domeSphere.GetComponent<Renderer>().material.color =  Color.white;
                played = true;
            }
        }
    }
}
