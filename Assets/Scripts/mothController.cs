using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class mothController : MonoBehaviour
{
    [Header("Speed Settings")]
    public float rotationSpeed = 100.0f;
    public float initSpeed = 5f;
    public float speedIncreaseAmount = 0.1f;

    [Header("Control Settings")]
    public float leftBoundary = -4.4f;  // left boundary
    public float rightBoundary = 4.4f;  // right boundary
    public float dizzyTime = 2.0f;
    public float invisibleTime = 1.0f;

    [Header("Audio Setting")]
    public AudioClip moveSound;
    public AudioClip hitSound;
    public AudioClip dizzySound;
    // public AudioClip dieSound;

    private Animator animator;
    private GameManager gameManager;
    private SpriteRenderer sr;
    private AudioSource audioSource;

    private bool canMove = true; // whether can move
    private bool isInvisible = false; // control invisible status


    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        gameManager = FindFirstObjectByType<GameManager>();
        audioSource = GetComponent<AudioSource>();

        InvokeRepeating("IncreaseSpeed", 5f, 5f);
    }

    void Update()
    {
        if (canMove)
        {
            moveForward();
            rotationUpdate();
            if (!audioSource.isPlaying)  // make sure audio is not playing
            {
                audioSource.PlayOneShot(moveSound);
            }
        }
    }

    public void setMovable(bool IsMovable)
    {
        canMove = IsMovable;
    }

    private void moveForward()
    {
        transform.Translate(Vector3.up * initSpeed * Time.deltaTime);       // move towards z direction
        float clampedX = Mathf.Clamp(transform.position.x, leftBoundary, rightBoundary);    // clamp x to avoid leave frame
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    private void rotationUpdate()
    {
        /*float horizontalInput = Input.GetAxis("Horizontal");
        float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;

        transform.Rotate(0, 0, -rotationAmount);

        float currentZRotation = transform.eulerAngles.z;
        if (currentZRotation > 180f)
        {
            currentZRotation -= 360f;
        }
        currentZRotation = Mathf.Clamp(currentZRotation, -89f, 89f);
        transform.rotation = Quaternion.Euler(0, 0, currentZRotation);*/

        float horizontalInput = Input.GetAxis("Horizontal");
        
        float mappedInput = Mathf.Tan(horizontalInput * Mathf.PI / 4);
        // Debug.Log(horizontalInput + " " + mappedInput);

        float targetZRotation = mappedInput * 80;

        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetZRotation);
        transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void IncreaseSpeed()
    {
        if (canMove)
        {
            initSpeed += speedIncreaseAmount;
            Debug.Log("Speed increased! New speed: " + initSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isInvisible) { return; }

        Debug.Log("HiT!");
        if (collision.CompareTag("obstacle"))
        {
            // TakeDamage();
            audioSource.Stop();
            audioSource.PlayOneShot(hitSound);
            gameManager.TakeDamage();
        }
    }


    //public void TakeDamage()
    //{
    //    remainingLives -= 1;
    //    Debug.Log("Remaining Lives: " + remainingLives);

    //    UpdateLifeUI();

    //    if (remainingLives > 0)
    //    {
    //        StartCoroutine(Dizzy()); 
    //    }
    //    else
    //    {
    //        Die();
    //    }
    //}

    public IEnumerator Dizzy()
    {
        isInvisible = true;
        canMove = false;

        audioSource.PlayOneShot(dizzySound);

        animator.SetBool("hit", true);
        yield return new WaitForSeconds(dizzyTime); // wait for 2s
        animator.SetBool("hit", false);         // return to flying animation state
        yield return new WaitForSeconds(0.3f);   // adjust for animation delay

        canMove = true;     // resume movement
        sr.color = new Color(1, 1, 1, 0.2f);   // transparent Invisible time
        yield return new WaitForSeconds(invisibleTime); // 1s invisible time
        sr.color = Color.white;         // change color back
        isInvisible = false;
    }

    public void Die()
    {
        canMove = false;
        isInvisible = true;

        // audioSource.PlayOneShot(dieSound);

        animator.SetTrigger("death"); 
    }

    
}