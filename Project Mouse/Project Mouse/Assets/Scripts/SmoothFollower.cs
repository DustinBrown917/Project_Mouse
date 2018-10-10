using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollower : MonoBehaviour {

    [SerializeField] private Transform target;
    private Vector3 newPosition = new Vector3();
    private Vector2 currentVelocity = new Vector2();
    [SerializeField] private float smoothDampTime = 0.5f;

    [Space]
    [Header("Clamp Position:")]
    [SerializeField] private float minY = 0;
    [SerializeField] private float maxY = 999;
    [SerializeField] private float minX = 0;
    [SerializeField] private float maxX = 999;
    private float zClamp;

    private void Awake()
    {
        zClamp = transform.position.z;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        newPosition = Vector2.SmoothDamp(transform.position, target.position, ref currentVelocity, smoothDampTime);
        newPosition.z = zClamp;
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        transform.position = newPosition;
	}
}
