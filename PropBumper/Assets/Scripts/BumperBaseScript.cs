using UnityEngine;
using System.Collections;

public class BumperBaseScript : MonoBehaviour {

    public bool floored = false;
    public GameObject playerPointer;

	// Use this for initialization
	void Start () {
        OnEnable();
	}

   public  void OnEnable ()
        {
        floored = false;
        GetComponent<Rigidbody>().isKinematic = false;
        }

    public void OnTriggerEnter (Collider other)
        {
        if (other.tag == "Platform" && !floored)
            {
            floored = true;
          //  GetComponent<Rigidbody>().isKinematic = true;
            }
        }


	// Update is called once per frame
	void Update () {
	
	}
}
