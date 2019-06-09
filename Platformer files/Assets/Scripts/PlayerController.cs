using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float jumpForce;

    public GameObject groundDetector;
    public PhysicsMaterial2D crateMaterial;

    private Rigidbody2D rb;
    private Vector3 startingScale;
    private Animator animator;
    private GameManager gameManager;

    private bool isJumping = false;
    private bool facingRight = true;
    private bool holdingCrate = false;
    private float cooldown = 0.05f;

	// Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody2D>();
        startingScale = transform.localScale;
        animator = gameObject.GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

    // Update is called once per frame

    private float timePassed = 0;
	void FixedUpdate () {

        timePassed += Time.fixedDeltaTime;

        float x = Input.GetAxisRaw("Horizontal");

        if (x != 0 && !isJumping)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);

        if (Input.GetKey(KeyCode.UpArrow) && !isJumping) {
            isJumping = true;
            //rb.AddForce(new Vector2(0, jumpForce));
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("isJumping", true);
        }

        if (x < 0) {
            transform.localScale = new Vector3(-startingScale.x, startingScale.y, startingScale.z);
            facingRight = false;
        }
        if (x > 0) {
            transform.localScale = startingScale;
            facingRight = true;
        }
        rb.velocity = new Vector2(x * speed, rb.velocity.y);
        if (rb.velocity.y < 0)
            isJumping = true;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(groundDetector.transform.position, new Vector2(0.66f, 0.02f), 0);
        for (int i=0; i<colliders.Length; i++) {
            string name = colliders[i].gameObject.tag;
            if (name.Equals("Floor") || name.Equals("Crate")) {
                isJumping = false;
                animator.SetBool("isJumping", false);
                break;
            }
        }

        if (timePassed >= cooldown && Input.GetKeyDown(KeyCode.X)) {
            if (!holdingCrate) {
                Collider2D collider = Physics2D.OverlapBox(gameObject.transform.GetChild(0).position, new Vector2(0.8f, 0.8f), 0);//0.75
                if (collider == null) {
                    PickupCrate();
                    //timePassed = 0;
                }
            }
            else if (holdingCrate) {
                ThrowCrate();
                //timePassed = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            gameManager.RestartLevel();
        }
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Goal")) {
            gameManager.LoadNextLevel();
        }
        if (collision.gameObject.CompareTag("Hazard") || collision.gameObject.CompareTag("Overlap")) {
            Death();
        }
        if (collision.gameObject.name.Equals("Key")) {
            Destroy(GameObject.Find("Lock"));
            Destroy(collision.gameObject);
        }
    }

    private void Death() {
        gameManager.RestartLevel();
    }

    private void PickupCrate() {
        RaycastHit2D[] hits;
        if (facingRight) {
            hits = Physics2D.RaycastAll(transform.position, Vector2.right, 0.7f);
        }
        else {
            hits = Physics2D.RaycastAll(transform.position, Vector2.left, 0.7f);
        }
        for (int i=0; i<hits.Length; i++) {
            RaycastHit2D hit = hits[i];
            if (hit.collider != null && hit.collider.CompareTag("Crate")) {
                GameObject crate = hit.collider.gameObject;
                Destroy(crate.GetComponent<Rigidbody2D>());
                crate.transform.parent = gameObject.transform.GetChild(0);
                holdingCrate = true;
                crate.transform.localPosition = new Vector3(0, 0, 0);
                crate.GetComponent<CrateScript>().beingCarried = true;
                timePassed = 0;
            }
        }
    }

    private void ThrowCrate() {
        GameObject crate = gameObject.transform.GetChild(0).GetChild(0).gameObject;
        crate.transform.parent = null;
        crate.GetComponent<CrateScript>().beingCarried = false;
        crate.AddComponent<Rigidbody2D>();
        Rigidbody2D crateRB = crate.GetComponent<Rigidbody2D>();
        crateRB.sharedMaterial = crateMaterial;
        crateRB.mass = 3;
        crateRB.gravityScale = 2.5f;
        crateRB.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        crateRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (facingRight)
            crateRB.velocity = new Vector2(7 + rb.velocity.x, rb.velocity.y);
        else
            crateRB.velocity = new Vector2(-7 + rb.velocity.x, rb.velocity.y);
        holdingCrate = false;
        timePassed = 0;
    }
}
