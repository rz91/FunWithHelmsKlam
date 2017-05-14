using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    private MeshFilter directionPlane;
    private GameObject player;

    private Animator animator;

    private bool inRange = false;

    private int closedStateHash = Animator.StringToHash("Base Layer.SmallDoorClosed");
    private int openedToOutsideStateHash = Animator.StringToHash("Base Layer.SmallDoorOpenedToOutside");
    private int openedToInsideStateHash = Animator.StringToHash("Base Layer.SmallDsoorOpenedToInside");

    private AudioSource[] audiosources;

    void Start () {
        directionPlane = GetComponent<MeshFilter>();
        player = GameObject.Find("FPSController");
        animator = GetComponent<Animator>();
        audiosources = GetComponents<AudioSource>();
    }
	
	void Update () {

        if(!inRange)
        {
            return;
        }

        if (Input.GetKey(KeyCode.E))
        {
            Vector3 direction = player.transform.position - directionPlane.transform.position;

            Vector3 normal = directionPlane.transform.TransformDirection(directionPlane.mesh.normals[0]);

            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if(stateInfo.fullPathHash == closedStateHash)
            {
                if (Vector3.Angle(direction, normal) < 90)
                {
                    animator.SetTrigger("openToInside");
                }
                else
                {
                    animator.SetTrigger("openToOutside");
                }

                audiosources[0].Play();
            } else if (stateInfo.fullPathHash == openedToOutsideStateHash || stateInfo.fullPathHash == openedToInsideStateHash)
            {
                animator.SetTrigger("close");
                audiosources[1].Play();
            }
        }
    }
    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "FPSController")
        {
            inRange = true;
        }
    }


    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "FPSController")
        {
            inRange = false;
        }
    }
}