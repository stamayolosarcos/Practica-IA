using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using Steerings;


[RequireComponent(typeof(Arrive))]
[RequireComponent(typeof(TANUKI_BLACKBOARD))]
[RequireComponent(typeof(WanderAround))]

public class FSM_TANUKI_EATS : FiniteStateMachine
{

    public enum State { INITIAL, DANCING, GOTO_FOOD, EAT}
    public State currentState = State.INITIAL;

    private WanderAround wanderAround;
    private Arrive arrive;
    private TANUKI_BLACKBOARD blackboard;

    private GameObject fish;
    private float elapsedTime;


    // Start is called before the first frame update
    void Start()
    {

        blackboard = GetComponent<TANUKI_BLACKBOARD>();
        arrive = GetComponent<Arrive>();
        wanderAround = GetComponent<WanderAround>();

        wanderAround.seekWeight = 0.2f;
        wanderAround.attractor = blackboard.orbitTarget;

        arrive.enabled = false;
        wanderAround.enabled = false;

        
    }

    public override void Exit() 
    {
        fish = null;
        arrive.enabled = false;

        base.Exit();
    }

    public override void ReEnter()
    {
        currentState = State.INITIAL;
        base.ReEnter();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState) 
        {
            case State.INITIAL:
                ChangeState(State.DANCING);
                break;

            case State.DANCING:

                fish = SensingUtils.FindInstanceWithinRadius(gameObject, "FOOD", blackboard.foodDetectableRadius);
                if(fish!= null) 
                {
                    ChangeState(State.GOTO_FOOD);
                    break;
                }
                break;

            case State.GOTO_FOOD:
                if(fish == null) 
                {
                    ChangeState(State.DANCING);
                    break;
                }
                else if (SensingUtils.DistanceToTarget(gameObject, fish)<= blackboard.foodReachedRadius) 
                {
                    ChangeState(State.EAT);
                    break;
                }

                break;
                

            case State.EAT:
                if(fish == null || fish.Equals(null)) 
                {
                    ChangeState(State.DANCING);
                    break;
                }
                if(elapsedTime <= blackboard.timeToEat) 
                {
                    elapsedTime += Time.deltaTime;
                }
                else 
                {
                    ChangeState(State.DANCING);
                    break;
                }
                break;
        
        
        }
    }

    void ChangeState(State newState) 
    {
        switch (currentState) 
        {
            case State.DANCING:
                //Steering de bailar .enabled = false//
                wanderAround.enabled = false;
                break;

            case State.GOTO_FOOD:
                arrive.enabled = false;
                break;
            

            case State.EAT:
                if(fish != null) 
                {
                    Destroy(fish);
                }
                break;
        
        }

        switch (newState) 
        {
            case State.DANCING:
                //Steering de bailar .enabled = false//
                wanderAround.enabled = true;
                break;

            case State.GOTO_FOOD:
                arrive.target = fish;
                arrive.enabled = true;
                break;

            case State.EAT:
                elapsedTime = 0f;
                break;
        }

        currentState = newState;

    }
}
