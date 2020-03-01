using FSM;
using Steerings;
using UnityEngine;

[RequireComponent(typeof(WanderAround))]
[RequireComponent(typeof(Arrive))]
[RequireComponent(typeof(MOUSE_BLACKBOARD))]
[RequireComponent(typeof(AudioSource))]
public class FSM_MOUSE : FiniteStateMachine
{
    public enum State {INITIAL, WANDER, GOTO_FOOD, EAT};
    public State currentState = State.INITIAL;

    private WanderAround wanderAround;
    private Arrive arrive;
    private MOUSE_BLACKBOARD blackboard;
    private AudioSource audioSource;

    private GameObject food;
    private float elapsedTime;

    void Start()
    {
        blackboard = GetComponent<MOUSE_BLACKBOARD>();
        wanderAround = GetComponent<WanderAround>();
        arrive = GetComponent<Arrive>();
        audioSource = GetComponent<AudioSource>();

        wanderAround.attractor = blackboard.attractor;

        wanderAround.enabled = false;
        arrive.enabled = false;
    }

    public override void Exit()
    {
        elapsedTime = 0;
        wanderAround.enabled = false;
        arrive.enabled = false;
        base.Exit();
    }

    public override void ReEnter()
    {
        currentState = State.INITIAL;
        base.ReEnter();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.INITIAL:
                ChangeState(State.WANDER);
                break;
            case State.WANDER:
                food = SensingUtils.FindInstanceWithinRadius(this.gameObject, "FOOD", blackboard.foodDetectableRadius);
                if (food != null)
                {
                    ChangeState(State.GOTO_FOOD);
                }
                break;
            case State.GOTO_FOOD:
                if (food == null) ChangeState(State.WANDER);
                else if (blackboard.foodReachedRadius > SensingUtils.DistanceToTarget(this.gameObject, food))
                {
                    ChangeState(State.EAT);
                }
                break;
            case State.EAT:
                if (food == null) ChangeState(State.WANDER);
                elapsedTime += Time.deltaTime;
                if (elapsedTime > blackboard.timeToEat) ChangeState(State.WANDER);
                break;
        }
    }

    void ChangeState (State newState)
    {
        switch (currentState)
        {
            case State.WANDER:
                wanderAround.enabled = false;
                break;
            case State.GOTO_FOOD:
                arrive.enabled = false;
                break;
            case State.EAT:
                if (food != null) Destroy(food.gameObject);
                break;
        }

        switch (newState)
        {
            case State.WANDER:
                wanderAround.enabled = true;
                break;
            case State.GOTO_FOOD:
                arrive.enabled = true;
                arrive.target = food;
                break;
            case State.EAT:
                elapsedTime = 0;
                break;
        }
        currentState = newState;
    }
}
