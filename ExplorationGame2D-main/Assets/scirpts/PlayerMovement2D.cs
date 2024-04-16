using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{

    public Animator animator;
    bool FacingLeft = true;

    public GameObject WalkSound;
 


    [Tooltip("How fast it accelerates")]
    public float movementForce = 1f;

    [Tooltip("Limit the velocity")]
    public Vector2 maxVelocity = new Vector2(10, 10);

    [Tooltip("Make it into a side scroller")]
    public bool twoDirection = false;

    [Tooltip("Only for two direction: jump force on the Y axis (0 for no jump)")]
    public float jumpForce = 0;

    [Tooltip("Set true to reach full speed instantly")]
    public bool analogSpeed = true;

    [Tooltip("Zero the velocity when direction is unpressed")]
    public bool noInertia = false;

    [Tooltip("The component that manages the 2D physics")]
    public Rigidbody2D rb;

    

    [Tooltip("Series of settings to determine if the collision is ground")]
    public ContactFilter2D GroundFilter;
    //*Ground contact is filtered based on the normal (if the ground is below around 90 degrees corner)

    [Tooltip("For side view check if hitting collider below the sprite")]
    public bool isGrounded = false;

    public Vector2 movementInput;

    [Tooltip("If set to true prevents any movements")]
    public bool frozen = false;

    public float jumpWait = 0.5f;
    private float jumpTimer = 0;

    [Tooltip("Gravity scale when moving up")]
    public float jumpGravity = 0;
    [Tooltip("Gravity scale when falling (tune for less floaty movement)")]
    public float fallGravity = 0;

    // Start is called before the first frame update
    void Start()
    {
        //add a reference to the controller component at the beginning
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

        if (!frozen)
        {


            float targetForce = movementForce;


            //create a 2D vector with the movement input (analog stick, arrows, or WASD) 
            movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            //if not analog speed overrides unity's axis smoothing (emulating analog stick) by reading the raw input
            if (!analogSpeed)
            {
                //both movement components can only be 0 or 1
                movementInput = Vector2.zero;
                //Debug.Log("movementInput.x=" + Input.GetAxisRaw("Horizontal"));
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    movementInput.x = 1;
                    WalkSound.SetActive(true);
                    //Debug.Log("Facing left when moving to right"+FacingLeft);
                    if (FacingLeft)
                    {
                      
                        Flip();
                    }
                }

                if (Input.GetAxisRaw("Horizontal") < 0) { 
                    movementInput.x = -1;
                    WalkSound.SetActive(true);
                    if (!FacingLeft)
                    {
                       
                        Flip();
                    }
                }
                   

                if (Input.GetAxisRaw("Vertical") > 0)
                    movementInput.y = 1;

                if (Input.GetAxisRaw("Vertical") < 0)
                    movementInput.y = -1;
                
            }

            //two direction zero the vertical component
            if (twoDirection)
            {
                movementInput = new Vector2(movementInput.x, 0);
            }
            

            //jump logic only if two direction and jump is set
            if (twoDirection && jumpForce > 0)
            {
                jumpTimer -= Time.deltaTime;

                //is touching ground?
                isGrounded = rb.IsTouching(GroundFilter);


                //jump if active
                if ((Input.GetButtonDown("Fire2") || Input.GetAxisRaw("Vertical") > 0) && isGrounded && jumpTimer < 0)
                {
                    jumpTimer = jumpWait;

                    rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                }

                if (rb.velocity.y > 0)
                    rb.gravityScale = jumpGravity;
                else
                    rb.gravityScale = fallGravity;

                //zero the y
                movementInput = new Vector2(movementInput.x, 0);
            }

            //force zero velocity to stop immediately
            if (noInertia)
            {
                Vector2 newVelocity = rb.velocity;

                //left right not pressed zero the horizontal velocity
                if (Input.GetAxisRaw("Horizontal") == 0)
                    newVelocity.x = 0;
                    animator.SetFloat("PlayerSpeed", 0);
                    WalkSound.SetActive(false);
                //WalkSound.enabled = false;


                //up down not pressed zero the vertical velocity (unless two direction)
                if (Input.GetAxisRaw("Vertical") == 0 && !twoDirection)
                    newVelocity.y = 0;

                rb.velocity = newVelocity;
            }

            //combining the left stick input and the vertical velocity
            //absolute coordinates movement: up means +z in the world, left means -x
            Vector2 movement = new Vector2(movementInput.x * targetForce, movementInput.y * targetForce);

            //add movement as force to the rigidbody
            //since it's continuous I have to multiply by delta time to make it frame independent
            rb.AddForce(movement * Time.deltaTime * 1000);

            //limit the velocity in both components separately
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxVelocity.x, maxVelocity.x), Mathf.Clamp(rb.velocity.y, -maxVelocity.y, maxVelocity.y));

            //little vector trick to prevent diagonal movement from going faster
            if (maxVelocity.x == maxVelocity.y)
                rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity.x);

            if (rb.velocity.x != 0)
                
                animator.SetFloat("PlayerSpeed", 1);
                //WalkSound.enabled = true;
                

        }
        
    }


    //these functions can be called externally to block the controls
    public void Freeze()
    {
        frozen = true;
        rb.velocity = Vector2.zero;
    }

    public void UnFreeze()
    {
        frozen = false;
    }

    void Flip()
    {
        //Debug.Log("Should flip now");
        Vector3 currentscale = gameObject.transform.localScale;
        currentscale.x = -currentscale.x;
        gameObject.transform.localScale = currentscale;

        FacingLeft = !FacingLeft;
        
    }

    

    //example of custom function to toggle the two direction movement on and off
    //it can be called externally from a TriggerInteraction2D event
    public void LadderMode(bool ladderOn)
    {
        if (ladderOn)
        {
            twoDirection = false;
            rb.gravityScale = 0;
        }
        else
        {
            twoDirection = true;

            //prevent forward momentum
            if (!isGrounded)
            {
                rb.velocity = Vector2.zero;
            }

        }

    }
}
