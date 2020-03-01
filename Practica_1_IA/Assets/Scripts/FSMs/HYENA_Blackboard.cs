using UnityEngine;

public class HYENA_Blackboard : MonoBehaviour {

	// CONSTANTS
	public float mouseDetectableRadius = 150f;
    public float hidespotDetectableRadius = 500f;
    public float lionDetectableRadius = 50f;
    public float lionFarEnoughRadius = 120;
	public float eatingTime = 5f;
	public float mouseReachedRadius = 10f;
	public float hidespotReachedRadius = 15;

	public GameObject hidespot;

	void Start () {

		if (hidespot == null) {
            hidespot = GameObject.Find ("HIDESPOT");
		}
	}

}
