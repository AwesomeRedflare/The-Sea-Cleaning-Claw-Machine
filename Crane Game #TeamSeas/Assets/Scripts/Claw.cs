using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour
{
    private Rigidbody2D rb;

    float input;
    public float speed;
    public float clawSpeed;
    public float upSpeed;
    public float downSpeed;

    public float waitTime;
    public float rayLenght;

    [HideInInspector]
    public bool canMove = true;
    private bool movingDown = false;

    public Transform leftClaw;
    public Transform rightClaw;
    public float targetAngle;

    public LayerMask clawCheckLayer;

    public Transform boat;
    private LineRenderer rope;

    public GameObject attractor;

    private void Start()
    {
        rope = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        input = Input.GetAxisRaw("Horizontal");

        if(input == 0)
        {
            rb.velocity = Vector2.zero;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canMove == true)
        {
            RaycastHit2D dropCheck = Physics2D.Raycast(transform.position, Vector2.down, rayLenght, clawCheckLayer);

            if(dropCheck.collider != null && dropCheck.collider.CompareTag("trash"))
            {
                StartCoroutine("DropTrash");
            }
            else
            {
                StartCoroutine("DropClaw");
            }
        }

        boat.position = new Vector2(transform.position.x, boat.position.y);

        rope.SetPosition(0, new Vector2(boat.position.x, boat.position.y - .25f));
        rope.SetPosition(1, transform.position);

    }

    private void FixedUpdate()
    {
        if (input != 0 && canMove == true)
        {
            rb.velocity = new Vector2(speed * input, rb.velocity.y);
        }
    }

    IEnumerator DropClaw()
    {
        canMove = false;
        attractor.SetActive(false);
        movingDown = true;
        rb.velocity = Vector2.zero;

        bool playSound = true;

        float startY = transform.position.y;

        //OpenClaw
        while(rightClaw.eulerAngles.z < targetAngle)
        {
            if(playSound == true)
            {
                FindObjectOfType<AudioManager>().Play("Claw");
                playSound = false;
            }
            rightClaw.rotation = Quaternion.RotateTowards(rightClaw.rotation, Quaternion.Euler(0, 0, targetAngle), clawSpeed * Time.deltaTime);
            leftClaw.rotation = Quaternion.RotateTowards(leftClaw.rotation, Quaternion.Euler(0, 0, -targetAngle), clawSpeed * Time.deltaTime);
            yield return null;
        }

        FindObjectOfType<AudioManager>().Stop("Claw");
        playSound = true;

        StartCoroutine("ReturnClaw");

        //Moves Down
        while (movingDown == true)
        {
            RaycastHit2D dropCheck = Physics2D.Raycast(transform.position, Vector2.down, rayLenght, clawCheckLayer);

            rb.velocity = Vector2.down * downSpeed;

            if (dropCheck == true)
            {
                StopCoroutine("ReturnClaw");
                FindObjectOfType<AudioManager>().Stop("Down");
                movingDown = false;

                yield return new WaitForSeconds(waitTime);
            }

            if (playSound == true)
            {
                FindObjectOfType<AudioManager>().Play("Down");
                playSound = false;
            }

            yield return null;
        }

        FindObjectOfType<AudioManager>().Stop("Down");
        playSound = true;
        attractor.SetActive(true);

        //CloseClaw
        while (rightClaw.eulerAngles.z > 0)
        {
            if (playSound == true)
            {
                FindObjectOfType<AudioManager>().Play("Claw");
                playSound = false;
            }
            rightClaw.rotation = Quaternion.RotateTowards(rightClaw.rotation, Quaternion.Euler(0, 0, 0f), clawSpeed * Time.deltaTime);
            leftClaw.rotation = Quaternion.RotateTowards(leftClaw.rotation, Quaternion.Euler(0, 0, 0f), clawSpeed * Time.deltaTime);
            yield return null;
        }

        FindObjectOfType<AudioManager>().Stop("Claw");
        playSound = true;

        //Moves Up
        while (movingDown == false && canMove == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, startY), upSpeed * Time.deltaTime);

            if(transform.position.y == startY)
            {
                canMove = true;
            }

            if (playSound == true)
            {
                FindObjectOfType<AudioManager>().Play("Up");
                playSound = false;
            }

            yield return null;
        }

        FindObjectOfType<AudioManager>().Stop("Up");
    }

    IEnumerator DropTrash()
    {
        canMove = false;
        attractor.SetActive(false);
        rb.velocity = Vector2.zero;

        bool playSound = true;

        //Opens Claw
        while (rightClaw.eulerAngles.z < targetAngle)
        {
            if (playSound == true)
            {
                FindObjectOfType<AudioManager>().Play("Claw");
                playSound = false;
            }
            rightClaw.rotation = Quaternion.RotateTowards(rightClaw.rotation, Quaternion.Euler(0, 0, targetAngle), clawSpeed * Time.deltaTime);
            leftClaw.rotation = Quaternion.RotateTowards(leftClaw.rotation, Quaternion.Euler(0, 0, -targetAngle), clawSpeed * Time.deltaTime);
            yield return null;
        }

        FindObjectOfType<AudioManager>().Stop("Claw");
        playSound = true;

        yield return new WaitForSeconds(waitTime);

        //Closes Claw
        while (rightClaw.eulerAngles.z > 0)
        {
            if (playSound == true)
            {
                FindObjectOfType<AudioManager>().Play("Claw");
                playSound = false;
            }
            rightClaw.rotation = Quaternion.RotateTowards(rightClaw.rotation, Quaternion.Euler(0, 0, 0f), clawSpeed * Time.deltaTime);
            leftClaw.rotation = Quaternion.RotateTowards(leftClaw.rotation, Quaternion.Euler(0, 0, 0f), clawSpeed * Time.deltaTime);
            yield return null;
        }

        FindObjectOfType<AudioManager>().Stop("Claw");

        canMove = true;
    }

    IEnumerator ReturnClaw()
    {
        yield return new WaitForSeconds(3f);

        if (movingDown == true)
        {
            movingDown = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - rayLenght));
    }
}
