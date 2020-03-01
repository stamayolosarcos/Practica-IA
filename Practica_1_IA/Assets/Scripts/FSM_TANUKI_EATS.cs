using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using Steerings;


[RequireComponent(typeof(Arrive))]
[RequireComponent(typeof(TANUKI_BLACKBOARD))]

[RequireComponent(typeof(Orbit_SB))]

public class FSM_TANUKI_EATS : FiniteStateMachine
{

    public enum State { INITIAL, DANCING, GOTO_SHARPIE }
    public State currentState = State.INITIAL;


    private Arrive arrive;
    private TANUKI_BLACKBOARD blackboard;
    private Orbit_SB orbit;








    private bool tooFarFromSharpie = false; //constraint 


    // Start is called before the first frame update
    void Start()
    {
        blackboard = GetComponent<TANUKI_BLACKBOARD>();
        arrive = GetComponent<Arrive>();

        orbit = GetComponent<Orbit_SB>();
        arrive.enabled = false;
        orbit.enabled = false;
    }

    public override void Exit()
    {
        orbit.enabled = false;
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

                if (SensingUtils.DistanceToTarget(this.gameObject, blackboard.orbitTarget) > blackboard.tooFarFromSharpie)
                {

                    ChangeState(State.GOTO_SHARPIE);
                    break;
                }


                break;

            case State.GOTO_SHARPIE:
                if (SensingUtils.DistanceToTarget(gameObject, blackboard.orbitTarget) <= blackboard.tooFarFromSharpie)
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

                orbit.target = null;
                orbit.enabled = false;
                
                
                break;

            case State.GOTO_SHARPIE:
                arrive.enabled = false;
                arrive.target = null;
                tooFarFromSharpie = false;
                break;
        }

        switch (newState)
        {
            case State.DANCING:

                orbit.enabled = true;
                orbit.SetAngularSpeed(20);
                orbit.target = blackboard.orbitTarget;

                break;


            case State.GOTO_SHARPIE:
                tooFarFromSharpie = true;
                arrive.enabled = true;
                arrive.target = blackboard.orbitTarget;
                
                break;
        }

        currentState = newState;

    }
}
