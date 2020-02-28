using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;
using FSM;

[RequireComponent(typeof(FlockingAroundPlusAvoid))]
[RequireComponent(typeof(FleePlusAvoid))]
[RequireComponent(typeof(FishBlackboard))]

public class FSM_Fish : FiniteStateMachine
{
    public enum State {INITIAL, FLOCKING, FLEE};
    public State currentState = State.INITIAL;

    private FlockingAroundPlusAvoid flockingAroundPlusAvoid;
    private FleePlusAvoid fleePlusAvoid;
    private FishBlackboard fishBlackboard;

    private GameObject shark;

    // Start is called before the first frame update
    void Start()
    {
        fishBlackboard = GetComponent<FishBlackboard>();
        flockingAroundPlusAvoid = GetComponent<FlockingAroundPlusAvoid>();
        fleePlusAvoid = GetComponent<FleePlusAvoid>();

        flockingAroundPlusAvoid.attractor = fishBlackboard.attractor;

        shark = fishBlackboard.shark;

        flockingAroundPlusAvoid.enabled = false;
        fleePlusAvoid.enabled = false;
    }

    public override void Exit()
    {
        flockingAroundPlusAvoid.enabled = false;
        fleePlusAvoid.enabled = false;
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
                ChangeState(State.FLOCKING);
                break;

            case State.FLOCKING:
                if(SensingUtils.DistanceToTarget(gameObject, shark) <= fishBlackboard.sharkDetectableRadius)
                {
                    ChangeState(State.FLEE);
                    break;
                }
                break;

            case State.FLEE:
                if (SensingUtils.DistanceToTarget(gameObject, shark) >= fishBlackboard.sharkFleedRadius)
                {
                    ChangeState(State.FLOCKING);
                    break;
                }
                break;
        }
    }

    void ChangeState(State newState)
    {
        //exit logic
        switch (currentState)
        {
            case State.FLOCKING:
                flockingAroundPlusAvoid.enabled = false;
                break;

            case State.FLEE:
                fleePlusAvoid.enabled = false;
                break;
        }

        //enterlogic
        switch (newState)
        {
            case State.FLOCKING:
                flockingAroundPlusAvoid.enabled = true;
                break;

            case State.FLEE:
                fleePlusAvoid.enabled = true;
                fleePlusAvoid.target = shark;
                break;
        }
        currentState = newState;
    }
}
