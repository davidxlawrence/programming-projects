using UnityEngine;
using System.Collections;

public class BoundaryManager : MonoBehaviour {
	
	private Vector3 _min;
	private Vector3 _max;

	void Awake () {
		// Use the camera to determine the screen boundaries
		float z = -Camera.main.transform.position.z + transform.position.z;
    	_min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, z));
    	_max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, z));
	}
	
	// Restrict the movement of the object based on camera boundaries
	public void ClampObjectPosition(GameObject obj) {
		// Get the padding required for the object
		float xpadding = obj.renderer.bounds.size.x / 2;
		float ypadding = obj.renderer.bounds.size.y / 2;
		
		// Clamp the movement using the padding
		Vector3 pos = obj.transform.position;
    	pos.x = Mathf.Clamp(pos.x, _min.x + xpadding, _max.x - xpadding);
		pos.y = Mathf.Clamp(pos.y, _min.y + ypadding, _max.y - ypadding);
    	obj.transform.position = pos;
	}
	
	// X value is the left wall while Y value is the bottom wall
	public Vector3 BoundaryMin {
		get { return _min; }	
	}
	
	// X value is the right wall while Y value is the top wall
	public Vector3 BoundaryMax {
		get { return _max; }	
	}
}
