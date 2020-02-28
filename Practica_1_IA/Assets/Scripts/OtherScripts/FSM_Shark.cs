using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using Steerings;

[RequireComponent(typeof(WanderAroundPlusAvoid))]
[RequireComponent(typeof(ArrivePlusAvoid))]
[RequireComponent(typeof(SharkBlackboard))]
public class FSM_Shark : FiniteStateMachine
{
    public enum State {INITIAL, WANDER, EAT_FOOD, GOTO_FOOD};
    public State currentState = State.INITIAL;

    private WanderAroundPlusAvoid wanderAroundPlusAvoid;
    private ArrivePlusAvoid arrivePlusAvoid;
    private SharkBlackboard sharkBlackboard;

    private GameObject food;
    private float elapsedTime;
    // Start is called before the first frame update
    void Start()
    {
        wanderAroundPlusAvoid = GetComponent<WanderAroundPlusAvoid>();
        arrivePlusAvoid = GetComponent<ArrivePlusAvoid>();
        sharkBlackboard = GetComponent<SharkBlackboard>();

        wanderAroundPlusAvoid.enabled = false;
        arrivePlusAvoid.enabled = false;
    }

    public override void Exit()
    {
        wanderAroundPlusAvoid.enabled = false;
        arrivePlusAvoid.enabled = false;
    }

    public override void ReEnter()
    {
        currentState = State.INITIAL;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.INITIAL:
                ChangeState(State.WANDER);
                break;

            case State.WANDER:
                food = SensingUtils.FindInstanceWithinRadius(gameObject, "SHARK_FOOD", sharkBlackboard.foodDectableRadius);
                if(food != null)
                {
                    ChangeState(State.GOTO_FOOD);
                    break;
                }
                break;

            case State.GOTO_FOOD:
                if (SensingUtils.DistanceToTarget(gameObject, food) <= sharkBlackboard.foodReachedRadius)
                {
                    ChangeState(State.EAT_FOOD);
                    break;
                }
                break;

            case State.EAT_FOOD:
                if (elapsedTime < sharkBlackboard.timeToEatFood)
                {
                    elapsedTime = elapsedTime + Time.deltaTime;
                }
                else
                {
                    ChangeState(State.WANDER);
                    break;
                }
                break;
        }
    }

    void ChangeState(State newState)
    {
        //enter exit
        switch (currentState)
        {
            case State.WANDER:
                wanderAroundPlusAvoid.enabled = false;
                break;

            case State.GOTO_FOOD:
                arrivePlusAvoid.enabled = false;
                break;

            case State.EAT_FOOD:
                if(food != null)
                {
                    Destroy(food);
                }
                break;
        }

        //enter logic
        switch (newState)
        {
            case State.WANDER:
                wanderAroundPlusAvoid.enabled = true;
                break;

            case State.GOTO_FOOD:
                arrivePlusAvoid.target = food;
                arrivePlusAvoid.enabled = true;
                break;

            case State.EAT_FOOD:
                elapsedTime = 0f;
                break;
        }

        currentState = newState;
    }
}
