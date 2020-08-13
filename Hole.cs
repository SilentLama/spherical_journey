using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    MovableObject movableObject = null;
    [SerializeField] float suckingTime = 2f; //must be transfered in a local variable
    [SerializeField] AudioClip suckingSound = null;
    [SerializeField] AudioClip teleportingSound = null;

    private float newSuckingTime;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
       if (movableObject && newSuckingTime > 0)
        {
            movableObject.transform.localScale = Vector3.Lerp(movableObject.transform.localScale, movableObject.transform.localScale * 0.0001f, Time.deltaTime * newSuckingTime);
            newSuckingTime -= Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        movableObject = other.GetComponent<MovableObject>();

        if (movableObject != null)
        {
            float objectVelocityX = movableObject.GetVelocityX();
            float objectVelocityZ = movableObject.GetVelocityZ();

            movableObject.SetAccelerationParameters(1f, -objectVelocityX, -objectVelocityZ);
            newSuckingTime = suckingTime;

            audioSource.clip = suckingSound;
            audioSource.Play();
            StartCoroutine(WaitUntilDestroy());
        }
    }

    public IEnumerator WaitUntilDestroy()
    {
        yield return new WaitForSeconds(suckingTime);
        if (movableObject.IsPlayerObject())
        {
            audioSource.clip = teleportingSound;
            audioSource.Play();
            movableObject.ResetPosition();
            movableObject.ResetScale();
        }
        else
        {
            Destroy(movableObject.gameObject);
            Debug.Log("Destroy! " + movableObject.gameObject.name);
        }
    }
}
