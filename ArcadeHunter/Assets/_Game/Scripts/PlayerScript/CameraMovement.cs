using UnityEngine;

public class CameraMovement : MonoBehaviour {
    Transform playerPos;
    //Transform mainCamera;
    Vector3 offset;

	// Use this for initialization
	void Start () {
        playerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();
        //mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
        offset = playerPos.position - transform.position;
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = playerPos.position - offset;// * Time.deltaTime;
		
	}
}
