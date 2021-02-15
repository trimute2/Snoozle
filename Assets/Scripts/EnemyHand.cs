using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Kyle Weekley
// Governs movement and attack behavior for hand enemies

public class EnemyHand : MonoBehaviour
{
    public enum HandState
    {
        Move,
        HorizontalSwipe,
        VerticalSlam
    }

    public HandState currentState;

    public GameObject player;
    public GameObject movementZone;
    public GameObject ground;
    public GameObject mainCamera;
    private BoxCollider2D movementZoneBox;
    private BoxCollider2D groundBox;
    private BoxCollider2D handBox;
    private BattleManager battleManager;

    private float movementSpeedHorizontal = 15f;
    private float movementSpeedVertical = 18f;
    [SerializeField] private bool movingRight;
    [SerializeField] private bool movingUp;
    private bool atHorizontalEdge;

    private float verticalSlamRange = 0.25f;
    private float verticalSlamSpeed = 30f;

    private float horizontalSwipeYSpeed = 20f;
    private float horizontalSwipeXSpeed = 30f;

    [SerializeField] private float actionTimer = 0f;
    private float minimumMoveDuration = 1f;
    private float verticalSlamDuration = 1.5f;

    System.Random rnd;

    //Percent chance of attack occuring under proper conditions
    private float horizontalSwipeChance = 50f;
    private float verticalSlamChance = 75f;

    private float timeMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        movementZoneBox = movementZone.GetComponent<BoxCollider2D>();
        groundBox = ground.GetComponent<BoxCollider2D>();
        handBox = GetComponent<BoxCollider2D>();
        battleManager = mainCamera.GetComponent<BattleManager>();
        rnd = new System.Random();
        atHorizontalEdge = false;
        timeMultiplier = battleManager.timeMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        timeMultiplier = battleManager.timeMultiplier;

        // Generate a number from 0-99 for use in attack chance rolls
        int randomAction = rnd.Next(0, 100);

        //When the hand is moving, check for valid attack conditions
        if (currentState == HandState.Move)
        {
            //Only perform an attack if enough time has passed since the last attack attempt
            if (actionTimer >= minimumMoveDuration) 
            {       
                if (atHorizontalEdge)
                {
                    actionTimer = 0f;

                    //Execute the attack a specified percentage of the time
                    if (randomAction < horizontalSwipeChance)
                        HorizontalSwipe();
                }
                else if (DetectPlayerBelow())
                {
                    actionTimer = 0f;

                    //Execute the attack a specified percentage of the time
                    if (randomAction < verticalSlamChance)
                    VerticalSlam();
                }
                else
                {
                    Move();
                }
            }
            else
            {
                Move();
            }
        }
        //If currently in an attacking state, continue that attack
        else if (currentState == HandState.VerticalSlam)
        {
            VerticalSlam();
        }
        else if (currentState == HandState.HorizontalSwipe)
        {
            HorizontalSwipe();
        }
        
    }

    //Move back and forth in a zigzag pattern
    public void Move()
    {
        atHorizontalEdge = false;
        actionTimer += Time.deltaTime * timeMultiplier;
        
        //Horizontal movement section
        if (movingRight)
        {
            transform.position += new Vector3(movementSpeedHorizontal * Time.deltaTime * timeMultiplier, 0, 0);

            if (handBox.bounds.max.x > movementZoneBox.bounds.max.x)
            {
                movingRight = false;
                atHorizontalEdge = true;
            }
        }
        else
        {
            transform.position -= new Vector3(movementSpeedHorizontal * Time.deltaTime * timeMultiplier, 0, 0);

            if (handBox.bounds.min.x < movementZoneBox.bounds.min.x)
            {
                movingRight = true;
                atHorizontalEdge = true;
            }
        }

        //Vertical movement section
        if (movingUp)
        {
            transform.position += new Vector3(0, movementSpeedVertical * Time.deltaTime * timeMultiplier, 0);

            if (handBox.bounds.max.y > movementZoneBox.bounds.max.y)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.position -= new Vector3(0, movementSpeedVertical * Time.deltaTime * timeMultiplier, 0);

            if (handBox.bounds.min.y < movementZoneBox.bounds.min.y)
            {
                movingUp = true;
            }
        }
    }

    //Return true if the player is located below this hand
    public bool DetectPlayerBelow()
    {
        if (handBox.bounds.min.y > player.transform.position.y && Mathf.Abs(transform.position.x - player.transform.position.x) <= verticalSlamRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Execute horizontal swipe attack
    public void HorizontalSwipe()
    {
        currentState = HandState.HorizontalSwipe;

        //Step 1: The hand lowers itself to just above the ground
        if (handBox.bounds.min.y > groundBox.bounds.max.y)
        {
            transform.position -= new Vector3 (0, horizontalSwipeYSpeed * Time.deltaTime * timeMultiplier, 0);
        }

        //Step 2: The hand moves quickly along the floor until it reaches the other side of the arena
        else if (movingRight)
        {
            transform.position += new Vector3 (horizontalSwipeXSpeed * Time.deltaTime * timeMultiplier, 0, 0);

            //Once the hand reaches the other side of the arena, return to moving state
            if (handBox.bounds.max.x >= movementZoneBox.bounds.max.x)
            {
                currentState = HandState.Move;
                actionTimer = 0f;
            }
        }
        else if (!movingRight)
        {
            transform.position -= new Vector3 (horizontalSwipeXSpeed * Time.deltaTime * timeMultiplier, 0, 0);

            //Once the hand reaches the other side of the arena, return to moving state
            if (handBox.bounds.min.x <= movementZoneBox.bounds.min.x)
            {
                currentState = HandState.Move;
                actionTimer = 0f;
            }
        }
    }

    //Execute vertical slam attack
    public void VerticalSlam()
    {
        currentState = HandState.VerticalSlam;

        //Step 3: The hand rests on the ground for a moment, allowing the player to counterattack
        actionTimer += Time.deltaTime * timeMultiplier;
        if (handBox.bounds.min.y <= groundBox.bounds.max.y)
        {
            //Animate, probably?
        }

        //Step 1: The hand rears up, rising a little
        else if (actionTimer <= verticalSlamDuration * 0.15f)
        {
            transform.position += new Vector3(0, verticalSlamSpeed / 4 * Time.deltaTime * timeMultiplier, 0);
        }

        //Step 2: The hand quickly slams down until it strikes the ground
        else
        {
            transform.position -= new Vector3(0, verticalSlamSpeed * Time.deltaTime * timeMultiplier, 0);
        }

        //Once the attck's duration has been reached, return to moving state
        if (actionTimer >= verticalSlamDuration)
        {
            currentState = HandState.Move;
            actionTimer = 0f;
        }
    }
}
