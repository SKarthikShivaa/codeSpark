using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    public static Transform playerLocation;
    [Range(0,10)]public float speed;
    [Range(0,1)]public float acceleratingRate;  //The rate of acceleration
    public Rigidbody bumperRigidBody;
    public bool isPlayer;
    bool accelerating, contact = false;
    float acceleration;             //factor of acceleration between 0f to 1f

	// Use this for initialization
	void Start () {

        if (isPlayer)
            {
                 
            playerLocation = this.transform;
            }
        else            ///AI
            {
            gameObject.GetComponent<BumperBaseScript>().playerPointer.SetActive(false);
            
            }
     
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void OnTriggerEnter ( Collider other )
        {
        if (other.tag == "Player" && !contact && !isPlayer)  //If AI collides with player, focus on running away
            {
          
          
            StartCoroutine(RunFromPlayer());
            }
        }

    /// <summary>
    /// Accelerates the variable from 0 to 1
    /// </summary>
    /// <returns></returns>
   IEnumerator Acceleration ()
        {
        accelerating = true;
        while (accelerating)
            {
            if(acceleration<2f)
            acceleration += 0.05f;
            yield return new WaitForSeconds(acceleratingRate);
            }
        }

    IEnumerator RunFromPlayer ()
        {
        contact = true;

        // NavMeshHit hit;
        //  bumperNavAgent.enabled = true;
        // NavMesh.SamplePosition(transform.position - transform.forward * 50,out hit,100,1);
        // bumperNavAgent.destination = transform.forward -  new Vector3(Random.Range(0f,10f),0,Random.Range(0f,10f));
        //  Debug.Log(bumperNavAgent.destination + gameObject.ToString());

        Vector3 target = transform.position - transform.forward * Random.Range(0f,4f);
        Vector3 direction = Vector3.zero;
        ;
        while (contact)
            {
            target = new Vector3(target.x,transform.position.y,target.z);
            if (!accelerating)
                StartCoroutine(Acceleration());
            direction = target - transform.position;
            transform.forward = Vector3.Lerp(transform.forward,(direction),speed * Time.deltaTime);
            transform.Translate(0f,0f,acceleration * speed * Time.deltaTime);
            yield return new WaitForFixedUpdate();

            if (direction.magnitude < 0.5f)
                {
           //     bumperNavAgent.enabled = false;
                contact = false;

                }
            }
        }

    void FixedUpdate ()
        {
        if (isPlayer)
            {
            if (CnControls.CnInputManager.GetAxis("Horizontal") == 0f && CnControls.CnInputManager.GetAxis("Vertical") == 0f && GetComponent<BumperBaseScript>().floored)
                {
               // bumperRigidBody.isKinematic = true;         //This is to stop object from sliding without friction
                accelerating = false;
                acceleration = 0f;
                bumperRigidBody.velocity = Vector3.zero;
                }
            else if (GetComponent<BumperBaseScript>().floored)
                {
                if (!accelerating)
                    StartCoroutine(Acceleration());
                bumperRigidBody.isKinematic = false;
                transform.forward = Vector3.Lerp(transform.forward, new Vector3(CnControls.CnInputManager.GetAxis("Horizontal"),0,CnControls.CnInputManager.GetAxis("Vertical")),2 * speed * Time.deltaTime);
                transform.Translate(0f,0f,acceleration * speed * Time.deltaTime);
                }
            }
        else
            {
            if (contact || GameManager.instance.game != GameManager.GameState.GameRunning)    //Had already touched the player, so focus on running away
                {
                 return;
                }
           // transform.rotation = new Quaternion(0f,transform.rotation.y,0,transform.rotation.z);
            if (!accelerating)
                StartCoroutine(Acceleration());
            transform.forward = Vector3.Lerp(transform.forward,(Movement.playerLocation.position - transform.position),speed * Time.deltaTime);
            transform.Translate(0f,0f,acceleration * speed * Time.deltaTime);
            }
        }

}
