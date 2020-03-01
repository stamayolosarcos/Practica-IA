using FSM;
using UnityEngine;
using Steerings;


namespace FSM
{
    [RequireComponent(typeof(HYENA_Blackboard))]
    [RequireComponent(typeof(Arrive))]
    [RequireComponent(typeof(WanderAroundPlusAvoid))]
    public class FSM_HYENA_FINDHIDESPOT : FiniteStateMachine
    {

        public enum State { INITIAL, WANDERING, HIDING};

        public State currentState { get; set; } = State.INITIAL;

        private HYENA_Blackboard blackboard;

        private Arrive arrive;
        private WanderAroundPlusAvoid wander;

        void Start()
        {
            arrive = GetComponent<Arrive>();
            wander = GetComponent<WanderAroundPlusAvoid>();

            blackboard = GetComponent<HYENA_Blackboard>();

            arrive.enabled = false;
            wander.enabled = false;
        }


        public override void Exit()
        {
            arrive.enabled = false;
            wander.enabled = false;
            gameObject.tag = "HYENA";
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
                    ChangeState(State.WANDERING);
                    break;
                case State.WANDERING:
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.hidespot) < blackboard.hidespotDetectableRadius)
                    {
                        ChangeState(State.HIDING);
                        break;
                    }
                    break;
                case State.HIDING:
                    if (SensingUtils.DistanceToTarget(gameObject, blackboard.hidespot) < blackboard.hidespotReachedRadius)
                    {
                        arrive.enabled = false;
                    }
                    break;
            }
        }



        private void ChangeState(State newState)
        {
            switch (this.currentState)
            {
                case State.WANDERING:
                    wander.enabled = false;
                    break;
                case State.HIDING:
                    arrive.enabled = false;
                    arrive.target = null;
                    gameObject.tag = "HYENA";
                    break;
            }

            switch (newState)
            {
                case State.WANDERING:
                    wander.enabled = true;
                    break;
                case State.HIDING:
                    arrive.enabled = true;
                    arrive.target = blackboard.hidespot;
                    gameObject.tag = "HYENA_HIDE";
                    break;
            }
            currentState = newState;
        }
    }
}
