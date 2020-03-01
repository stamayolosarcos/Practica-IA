using FSM;
using UnityEngine;
using Steerings;


namespace FSM
{
    [RequireComponent(typeof(LION_Blackboard))]
    [RequireComponent(typeof(WanderAroundPlusAvoid))]
    [RequireComponent(typeof(Seek))]
    public class FSM_LION : FiniteStateMachine
    {

        public enum State { INITIAL, WANDERING, SCARING };

        public State currentState { get; set; } = State.INITIAL;

        private KinematicState kinematic;
        private LION_Blackboard blackboard;

        private WanderAroundPlusAvoid wander;
        private Seek seek;
        private GameObject hyena;

        void Start()
        {
            wander = GetComponent<WanderAroundPlusAvoid>();
            seek = GetComponent<Seek>();

            kinematic = GetComponent<KinematicState>();
            blackboard = GetComponent<LION_Blackboard>();

            wander.enabled = false;
            seek.enabled = false;
        }


        public override void Exit()
        {
            wander.enabled = false;
            seek.enabled = false;
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
                    hyena = SensingUtils.FindInstanceWithinRadius(gameObject, "HYENA", blackboard.hyenaDetectableRadius);
                    if (hyena != null)
                    {
                        ChangeState(State.SCARING);
                        break;
                    }
                    break;
                case State.SCARING:
                    if (SensingUtils.DistanceToTarget(gameObject, hyena) > blackboard.hyenaFarEnoughRadius || hyena.gameObject.tag != "HYENA")
                    {
                        ChangeState(State.WANDERING);
                        break;
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
                case State.SCARING:
                    seek.enabled = false;
                    seek.target = null;
                    hyena = null;
                    break;
            }

            switch (newState)
            {
                case State.WANDERING:
                    wander.enabled = true;
                    kinematic.maxSpeed = blackboard.wanderSpeed;
                    break;
                case State.SCARING:
                    seek.enabled = true;
                    seek.target = hyena;
                    kinematic.maxSpeed = blackboard.scareSpeed;
                    break;
            }
            currentState = newState;
        }
    }
}
