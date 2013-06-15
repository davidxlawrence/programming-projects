using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	
	public GameObject ballSpawnPosition;
	public float ballMovementSpeed;
	public float ballSpeedIncrement;
	
	private ScoreManager _scoreManager;
	private float _originalSpeed;

	void Start () {
		ResetBallPosition();
		_originalSpeed = ballMovementSpeed;
		
		// Get the reference to the score manager script
		_scoreManager = (ScoreManager)GameObject.FindObjectOfType(typeof(ScoreManager));
		if (_scoreManager == null){
			Debug.LogError("Ball: Start: Can't find the score manager!");
			return;
		}
	}
	
	void FixedUpdate () {
		// Ensure the speed remains constant
		rigidbody.velocity = rigidbody.velocity.normalized * ballMovementSpeed;
	}
	
	void OnCollisionEnter(Collision collision) {
		// Angle the ball depending on where it hits the paddle
		if(collision.collider.gameObject.tag == "Paddle" )
		{
			float hitDistance = this.transform.position.x - collision.collider.transform.position.x;
       		float hitDistancePercent = hitDistance / (collision.collider.renderer.bounds.size.x);
			float xVelocity = hitDistancePercent * ballMovementSpeed;
			float yVelocity = ballMovementSpeed - Mathf.Abs(xVelocity);
			this.rigidbody.velocity = new Vector3(xVelocity, yVelocity, 0);
		}
		
		if(collision.collider.gameObject.name == "BottomWall") {
			_scoreManager.PlayerLives -= 1;
			ResetBallPosition();
		}
	}
	
	public void LaunchBall() {
		this.rigidbody.AddForce(new Vector3(Random.Range(-ballMovementSpeed, ballMovementSpeed), ballMovementSpeed, 0));	
	}
	
	// The game is over so reset the ball
	public void StopBall() {
		this.rigidbody.velocity = Vector3.zero;
		ballMovementSpeed = _originalSpeed;
	}
	
	public void IncrementBallSpeed() {
		ballMovementSpeed += ballSpeedIncrement;
	}
	
	public void ResetBallPosition() {
		this.transform.position = ballSpawnPosition.transform.position;	
	}
}
