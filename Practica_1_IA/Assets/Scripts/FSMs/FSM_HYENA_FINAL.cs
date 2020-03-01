using FSM;
using UnityEngine;
using Steerings;


namespace FSM
{
    [RequireComponent(typeof(HYENA_Blackboard))]
    [RequireComponent(typeof(FSM_HYENA_HUNT))]
    [RequireComponent(typeof(FleePlusAvoid))]
    public class FSM_HYENA_FINAL : FiniteStateMachine
    {

        public enum State { INITIAL, HUNTING, FLEEING };

        public State currentState { get; set; } = State.INITIAL;

        private HYENA_Blackboard blackboard;

        private FSM_HYENA_HUNT hunt;
        private FleePlusAvoid flee;
        public GameObject lion;

        void Start()
        {
            hunt = GetComponent<FSM_HYENA_HUNT>();
            flee = GetComponent<FleePlusAvoid>();

            blackboard = GetComponent<HYENA_Blackboard>();

            hunt.enabled = false;
            flee.enabled = false;

            lion = GameObject.Find("LION");
            flee.target = lion;
        }


        public override void Exit()
        {
            hunt.enabled = false;
            flee.enabled = false;
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
                    ChangeState(State.HUNTING);
                    break;
                case State.HUNTING:
                    if (SensingUtils.DistanceToTarget(gameObject, lion) < blackboard.lionDetectableRadius && hunt.currentState != FSM_HYENA_HUNT.State.HIDING)
                    {
                        ChangeState(State.FLEEING);
                        break;
                    }
                    break;
                case State.FLEEING:
                    if (SensingUtils.DistanceToTarget(gameObject, lion) > blackboard.lionFarEnoughRadius)
                    {
                        ChangeState(State.HUNTING);
                        break;
                    }
                    break;
            }
        }



        private void ChangeState(State newState)
        {
            switch (this.currentState)
            {
                case State.HUNTING:
                    hunt.Exit();
                    break;
                case State.FLEEING:
                    flee.enabled = false;
                    break;
            }

            switch (newState)
            {
                case State.HUNTING:
                    hunt.ReEnter();
                    break;
                case State.FLEEING:
                    flee.enabled = true;
                    break;
            }
            currentState = newState;
        }
    }
}
