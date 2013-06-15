using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour {
	
	public float paddleSpeedMultiplier;
	public float paddleSizeModifier;
	
	private BoundaryManager _boundaryManager;
	private Vector3 _originalPosition;
	private Vector3 _paddleDisplacement;
	private bool _canMovePaddle;
	private Vector3 _originalSize;

	void Start () {
		// Get the reference to the boundary manager script
		_boundaryManager = (BoundaryManager)GameObject.FindObjectOfType(typeof(BoundaryManager));
		if (_boundaryManager == null){
			Debug.LogError("Paddle: Start: Can't find the boundary manager!");
			return;
		}
		
		_originalPosition = this.transform.position;
		_originalSize = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
		_canMovePaddle = false;
	}
	
	void Update () {
		// Move the paddle left and right
		if(_canMovePaddle) {
			this.transform.Translate(Input.GetAxis("Horizontal") * paddleSpeedMultiplier * Time.deltaTime, 0, 0);
		}
		
		_boundaryManager.ClampObjectPosition(this.gameObject);
	}
	
	// Decrease the paddle's size. Expects a float between 0-1
	public void DecreasePaddleSize() {
		if(paddleSizeModifier < 0 || paddleSizeModifier > 1) {
			Debug.LogError("Paddle: DecreasePaddleSize: This function expects a float between 0 and 1");
			return;
		}
		
		// Modify the paddle's original size
		this.transform.localScale = new Vector3(this.transform.localScale.x * paddleSizeModifier,
												this.transform.localScale.y,
												this.transform.localScale.z);
	}
	
	public void EnablePaddle() {
		_canMovePaddle = true;
	}
	
	// The game is over so reset the paddle and stop player movement
	public void DisablePaddle() {
		_canMovePaddle = false;
		this.transform.position = _originalPosition;
		this.transform.localScale = _originalSize;
	}
}
