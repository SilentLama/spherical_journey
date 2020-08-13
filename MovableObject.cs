using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{


    [SerializeField] bool isPlayer = false;
    [SerializeField] Material playerMaterial = null;
    [SerializeField] AudioClip ballCollisionSound = null;
    [SerializeField] AudioClip wallCollisionSound = null;


    [SerializeField] GameObject directionArrow = null;
    [SerializeField] GameObject accelerationArrow = null;
    public float arrowRotationSpeed = 5f;
    public bool timeIsFrozen = true;
    
    [Header("Kinematik")]

    public float startingVelocityX = 0f;
    public float startingVelocityZ = 0f;
    public bool isKinematic = false;
    public bool affectedByGravity = false;


    [Header("Dynamik")]
    public float mass = 0f;
    public float drag = 0f;


    private float accelerationX = 0f;
    private float accelerationZ = 0f;
    private float accelerationTimer = 0f;

    private float velocityX;
    private float velocityZ;
    private float handOverVelocityX;
    private float handOverVelocityZ;

    private MeshRenderer meshRenderer;

    private Material normalMaterial;

    private Quaternion lookRotation;
    private bool velocityWasChanged;
    private AudioSource audioSource;

    private Vector3 originalPosition;
    private Vector3 originalScale;

    private void Start()
    {
        velocityX = startingVelocityX;
        velocityZ = startingVelocityZ;
        meshRenderer = GetComponent<MeshRenderer>();
        if (isPlayer)
        {
            meshRenderer.material = playerMaterial;
        }

        normalMaterial = meshRenderer.material;
        audioSource = GetComponent<AudioSource>();
        accelerationArrow.SetActive(false);

        originalPosition = transform.position;
        originalScale = transform.localScale;
    }
    private void Update()
    {
        HandleVelocity();
        HandleAcceleration();
        //HandleGravity();
        UpdateDirectionArrow();
        UpdateAccelerationArrow();

        DebugUpdateStuff();

    }

    private void HandleVelocity()
    {
        if (!timeIsFrozen)
        {
            transform.Translate(new Vector3(velocityX, 0f, velocityZ) * Time.deltaTime); 
        }
    }

    private void HandleAcceleration()
    {
        if (!timeIsFrozen)
        {
            if (accelerationTimer > 0)
            {
                velocityX += accelerationX * Time.deltaTime;
                velocityZ += accelerationZ * Time.deltaTime;
            }
            else
            {
                accelerationX = 0f;
                accelerationZ = 0f;
            }
            accelerationTimer -= Time.deltaTime;
            accelerationTimer = Mathf.Clamp(accelerationTimer, 0, Mathf.Infinity);
        }
        
        
    }

    public void SetAccelerationParameters(float _accelerationTime, float _accelerationX, float _accelerationZ)
    {
        accelerationTimer += _accelerationTime;
        accelerationX += _accelerationX;
        accelerationZ += _accelerationZ;
    }

    public float[] GetObjectParameters()
    {
        float[] objectParameters = { velocityX, velocityZ, accelerationX, accelerationZ, accelerationTimer };
        return objectParameters;
    }

    public void SetTimeFreeze()
    {
        if (timeIsFrozen)
        {
            timeIsFrozen = false;
        }
        else
        {
            timeIsFrozen = true;
        }
    }

    public void DeselectObject()
    {
        GetComponent<MeshRenderer>().material = normalMaterial;
    }

    private Vector3 UpdateDirectionVector()
    {
        Vector3 direction = new Vector3(-velocityZ, 0f, velocityX).normalized; // X und Y(Z) sind irgendwie vertauscht und Z muss negativ
        return direction;
    }

    private void UpdateDirectionArrow()
    {
        lookRotation = Quaternion.LookRotation(UpdateDirectionVector());
        directionArrow.transform.rotation = Quaternion.Slerp(directionArrow.transform.rotation, lookRotation, Time.deltaTime * arrowRotationSpeed);
        if (velocityX < 0.1f && velocityZ < 0.1f && velocityX > -0.1f && velocityZ > -0.1f)
        {
            directionArrow.gameObject.SetActive(false);
        }
        else
        {
            directionArrow.gameObject.SetActive(true);
        }
    }

    private void UpdateAccelerationArrow()
    {
        if (accelerationX > 0f && accelerationTimer > 0f || accelerationX < 0f && accelerationTimer > 0f || accelerationZ > 0f && accelerationTimer > 0f || accelerationZ < 0f && accelerationTimer > 0f)//meshRenderer.material != normalMaterial && meshRenderer.material != playerMaterial)
        {
            accelerationArrow.SetActive(true);
            Quaternion velocityDirection = Quaternion.LookRotation(new Vector3(-accelerationZ, 0f, accelerationX).normalized);
            accelerationArrow.transform.rotation = Quaternion.Slerp(accelerationArrow.transform.rotation, velocityDirection, Time.deltaTime * arrowRotationSpeed);
        }
        else
        {
            accelerationArrow.SetActive(false);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        velocityWasChanged = false;
        Debug.Log("Collision!");

        if (other.gameObject.GetComponent<MovableObject>() != null && !other.gameObject.GetComponent<MovableObject>().CheckIfVelocityWasChanged())
        {
            velocityWasChanged = true;

            MovableObject movableCollider = other.gameObject.GetComponent<MovableObject>();

            handOverVelocityX = movableCollider.GetVelocityX();
            handOverVelocityZ = movableCollider.GetVelocityZ();

            movableCollider.GetComponent<MovableObject>().SetVelocityX(velocityX); //velocityX
            movableCollider.GetComponent<MovableObject>().SetVelocityZ(velocityZ); //velocityZ

            velocityX = handOverVelocityX;
            velocityZ = handOverVelocityZ;

            audioSource.clip = ballCollisionSound;
            audioSource.Play();
            //TODO: SFX
        }
        else if (other.gameObject.GetComponent<UnmovableObject>())  //might disturb since it doesnt get checked if the first one is true
        {
            Vector3 currentVelocity = new Vector3(velocityX, 0f, velocityZ);

            RaycastHit hit;
            Ray ray = new Ray(transform.position, currentVelocity);
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 newVelocity = Vector3.Reflect(currentVelocity, hit.normal);
                Debug.DrawRay(ray.origin, ray.direction, Color.green, 3f);

                SetVelocityX(newVelocity.x);
                SetVelocityZ(newVelocity.z);

                audioSource.clip = wallCollisionSound;
                audioSource.Play();
            }
        }
    }





    public bool CheckIfVelocityWasChanged()
    {
        return velocityWasChanged;
    }

    public float GetVelocityX()    // wird vom anderen Objekt gecalled
    {
        return velocityX;
    }
    public float GetVelocityZ()    // wird vom anderen Objekt gecalled
    {
        return velocityZ;
    }
    public void SetVelocityX(float _velocityX)
    {
    		velocityX = _velocityX;
    }
    public void  SetVelocityZ(float _velocityZ)
    {
        velocityZ = _velocityZ;
    } 

    public bool IsPlayerObject()
    {
        return isPlayer;
    }

    public void ResetPosition()
    {
        transform.position = originalPosition;
    }

    public void ResetScale()
    {
        transform.localScale = originalScale;
    }



    private void DebugUpdateStuff()
    {
        Debug.DrawRay(transform.position, new Vector3(velocityX, 0f, velocityZ) * 5f, Color.blue, .1f);
    }
}
