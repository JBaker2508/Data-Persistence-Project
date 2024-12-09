using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Brick : MonoBehaviour
{
    public UnityEvent<int> onDestroyed;
    public AudioClip collisionSound;
    private AudioSource audioSource;

    public int PointValue;
    private int timesHit = 0;

    void Start()
    {
        // Get the AudioSource component on the object
        audioSource = GetComponent<AudioSource>();
        
        var renderer = GetComponentInChildren<Renderer>();

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        switch (PointValue)
        {
            case 1 :
                block.SetColor("_BaseColor", Color.magenta);
                break;
            case 2:
                block.SetColor("_BaseColor", Color.blue);
                break;
            case 3:
                block.SetColor("_BaseColor", Color.cyan);
                break;
            case 4:
                block.SetColor("_BaseColor", Color.green);
                break;
            case 5:
                block.SetColor("_BaseColor", Color.yellow);
                break;
            case 6:
                block.SetColor("_BaseColor", new Color(1f, 0.5f, 0f)); //Orange
                break;
            case 7:
                block.SetColor("_BaseColor", Color.red);
                break;
            case 8:
                block.SetColor("_BaseColor", new Color(1f, 0.75f, 0.8f)); //Pink
                break;
            case 9:
                block.SetColor("_BaseColor", Color.white);
                break;
            default:
                block.SetColor("_BaseColor", Color.gray);
                break;
        }
        renderer.SetPropertyBlock(block);
    }

    private void OnCollisionEnter(Collision other)
    {
        // Play the collision sound
        if (collisionSound != null && audioSource != null)
        {
            // Map PointValue (1-9) to a pitch range (e.g., 0.8 to 1.2)
            float pitch = Mathf.Lerp(0.3f, 0.8f, (PointValue - 1) / 8f);

            // Set the pitch
            audioSource.pitch = pitch;

            // Adjust the volume
            audioSource.volume = 0.5f;

            // Play the sound
            audioSource.PlayOneShot(collisionSound); 
        }

        if (timesHit == PointValue)
        {
            onDestroyed.Invoke(PointValue);

            //slight delay to be sure the ball have time to bounce
            Destroy(gameObject, 0.2f);
        }
        else
        { timesHit++; }
    }
}