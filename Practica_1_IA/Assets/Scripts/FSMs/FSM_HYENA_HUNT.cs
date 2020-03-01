using FSM;
using UnityEngine;
using Steerings;


namespace FSM
{
	[RequireComponent(typeof(HYENA_Blackboard))]
	[RequireComponent(typeof(Arrive))]
	//[RequireComponent(typeof(Pursue))]
    [RequireComponent(typeof(FSM_HYENA_FINDHIDESPOT))]
    public class FSM_HYENA_HUNT : FiniteStateMachine
	{
		public enum State {INITIAL, HIDING, ATTACKING, EATING};

		public State currentState { get; set; } = State.INITIAL; 

		private HYENA_Blackboard blackboard;

		private GameObject mouse;
		private Arrive arrive; 
		//private Pursue pursue;
        private FSM_HYENA_FINDHIDESPOT findHidespot;

        private float eatingTime;

        void Start () {
			arrive = GetComponent<Arrive>();
			//pursue = GetComponent<Pursue>();
            findHidespot = GetComponent<FSM_HYENA_FINDHIDESPOT>();
	
			blackboard = GetComponent<HYENA_Blackboard>();

			arrive.enabled = false;
			//pursue.enabled = false;
            findHidespot.enabled = false;
		}


		public override void Exit () {
			arrive.enabled = false;
			//pursue.enabled = false;
            findHidespot.enabled = false;
            mouse = null;
            base.Exit ();
		}

		public override void ReEnter() {
			currentState = State.INITIAL;
			base.ReEnter ();
		}

		void Update ()
		{
			switch (currentState) {

			case State.INITIAL:
				ChangeState(State.HIDING);
				break;

			case State.HIDING:
				mouse = SensingUtils.FindInstanceWithinRadius (gameObject, "MOUSE", blackboard.mouseDetectableRadius); 
				if (mouse != null)
                {
					ChangeState(State.ATTACKING);
					break;
				}
				break;

			case State.ATTACKING:
				if (SensingUtils.DistanceToTarget (gameObject, mouse) <= blackboard.mouseReachedRadius)
                {
                    mouse.GetComponent<FSM_MOUSE>().Exit();
                    mouse.gameObject.tag = "MOUSE_DEAD";
					ChangeState(State.EATING);
					break;
				}

				GameObject otherMouse = SensingUtils.FindInstanceWithinRadius (gameObject, "MOUSE", blackboard.mouseDetectableRadius);
				if (otherMouse!=null && otherMouse != mouse && SensingUtils.DistanceToTarget (gameObject, otherMouse) < SensingUtils.DistanceToTarget (gameObject, mouse))
                {
					mouse = otherMouse;
					break;
				}

				if (mouse == null)
                { 
					ChangeState(State.HIDING);
					break;
				}
				break;
                case State.EATING:
                    eatingTime += Time.deltaTime;
                    if (eatingTime > blackboard.eatingTime)
                    {
                        ChangeState(State.HIDING);
                        break;
                    }
                    break;
            } 

		}



		private void ChangeState (State newState)
        {
			switch (this.currentState)
            {
			    case State.HIDING:
                    findHidespot.Exit();
				    break;
			    case State.ATTACKING:
				    arrive.enabled = false;
				    arrive.target = null;
				    break;
                case State.EATING:
                    eatingTime = 0;
                    break;
			}

			switch (newState)
            {
			    case State.HIDING:
                    findHidespot.ReEnter();
                    break;
			    case State.ATTACKING:
				    arrive.target = mouse;
				    arrive.enabled = true;
				    break;
                case State.EATING:
                    eatingTime = 0;
                    break;
            } 
			currentState = newState;
		}
    }
}