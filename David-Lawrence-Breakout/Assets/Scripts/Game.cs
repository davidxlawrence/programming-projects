using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
	
	public GameObject brick;
	public GameObject wall;
	
	public int numRows;
	public int numBricksPerRow;
	public Color[] rowColors;
	public int[] brickValuesPerColor;
	public int rowsPerColor;
	
	private BoundaryManager _boundaryManager;
	private ScoreManager _scoreManager;
	private Ball _ball;
	private Paddle _paddle;
	private bool _brokeTopRowThisLevel;
	
	private int[] _numDestroyedBricksInRows;

	void Start () {
		_numDestroyedBricksInRows = new int[numRows];
		
		// Get the reference to the boundary manager script
		_boundaryManager = (BoundaryManager)GameObject.FindObjectOfType(typeof(BoundaryManager));
		if (_boundaryManager == null){
			Debug.LogError("Game: Start: Can't find the boundary manager!");
			return;
		}
		
		// Get the reference to the score manager script
		_scoreManager = (ScoreManager)GameObject.FindObjectOfType(typeof(ScoreManager));
		if (_scoreManager == null){
			Debug.LogError("Game: Start: Can't find the score manager!");
			return;
		}
		
		// Get the reference to the ball script
		_ball = (Ball)GameObject.FindObjectOfType(typeof(Ball));
		if (_ball == null){
			Debug.LogError("Game: Start: Can't find the ball!");
			return;
		}
		
		// Get the reference to the paddle script
		_paddle = (Paddle)GameObject.FindObjectOfType(typeof(Paddle));
		if (_paddle == null){
			Debug.LogError("Game: Start: Can't find the paddle!");
			return;
		}
		
		// Set the starting position of the game board based on the boundaries
		float gameBoardLength = brick.renderer.bounds.size.x * numBricksPerRow;
		float screenLength = _boundaryManager.BoundaryMax.x - _boundaryManager.BoundaryMin.x;
		float gameBoardXPadding = (screenLength - gameBoardLength) / 2;
		Vector3 gameBoardStartingPosition = new Vector3(_boundaryManager.BoundaryMin.x + gameBoardXPadding + 2.1f, 
											   			_boundaryManager.BoundaryMax.y - 10.0f, 
											   			0);
		this.transform.position = gameBoardStartingPosition;
		
		InitializeBoundaries();
		InitializeGameBoard();
	}
	
	// Add walls to the play area to prevent the ball from leaving the screen
	private void InitializeBoundaries() {
		float wallHeight;
		float wallWidth;
		Vector3 wallStartingPosition;
		GameObject wallClone;
		
		// Left wall
		wallClone = Instantiate(wall) as GameObject;
		wallClone.transform.localScale = new Vector3(wallClone.transform.localScale.x, 
													 _boundaryManager.BoundaryMax.y - _boundaryManager.BoundaryMin.y,
													 wallClone.transform.localScale.z);
		wallHeight = wallClone.renderer.bounds.size.y;
		wallWidth = wallClone.renderer.bounds.size.x;
		wallStartingPosition = new Vector3(_boundaryManager.BoundaryMin.x - (wallWidth / 2), 
										   _boundaryManager.BoundaryMax.y - (wallHeight / 2), 
										   0);
		wallClone.transform.position = wallStartingPosition;
		wallClone.renderer.enabled = false;
		
		// Top wall
		wallClone = Instantiate(wall) as GameObject;
		wallClone.transform.localScale = new Vector3(_boundaryManager.BoundaryMax.x - _boundaryManager.BoundaryMin.x, 
													 wallClone.transform.localScale.y,
													 wallClone.transform.localScale.z);
		wallHeight = wallClone.renderer.bounds.size.y;
		wallWidth = wallClone.renderer.bounds.size.x;
		wallStartingPosition = new Vector3(_boundaryManager.BoundaryMin.x + (wallWidth / 2), 
										   _boundaryManager.BoundaryMax.y + (wallHeight / 2), 
										   0);
		wallClone.transform.position = wallStartingPosition;
		wallClone.renderer.enabled = false;
		
		// Right wall
		wallClone = Instantiate(wall) as GameObject;
		wallClone.transform.localScale = new Vector3(wallClone.transform.localScale.x, 
													 _boundaryManager.BoundaryMax.y - _boundaryManager.BoundaryMin.y,
													 wallClone.transform.localScale.z);
		wallHeight = wallClone.renderer.bounds.size.y;
		wallWidth = wallClone.renderer.bounds.size.x;
		wallStartingPosition = new Vector3(_boundaryManager.BoundaryMax.x + (wallWidth / 2), 
										   _boundaryManager.BoundaryMax.y - (wallHeight / 2), 
										   0);
		wallClone.transform.position = wallStartingPosition;
		wallClone.renderer.enabled = false;
		
		// Bottom wall
		wallClone = Instantiate(wall) as GameObject;
		wallClone.transform.localScale = new Vector3(_boundaryManager.BoundaryMax.x - _boundaryManager.BoundaryMin.x, 
													 wallClone.transform.localScale.y,
													 wallClone.transform.localScale.z);
		wallHeight = wallClone.renderer.bounds.size.y;
		wallWidth = wallClone.renderer.bounds.size.x;
		wallStartingPosition = new Vector3(_boundaryManager.BoundaryMin.x + (wallWidth / 2), 
										   _boundaryManager.BoundaryMin.y - (wallHeight / 2), 
										   0);
		wallClone.transform.position = wallStartingPosition;
		wallClone.renderer.enabled = false;
		wallClone.gameObject.name = "BottomWall";
	}
	
	// Populate the game board with bricks
	private void InitializeGameBoard() {
		_brokeTopRowThisLevel = false;
		
		float brickLength = brick.renderer.bounds.size.x;
		float brickWidth = brick.renderer.bounds.size.y;
		Vector3 initialPosition = this.transform.position;
		Vector3 currentPosition = initialPosition;
		int currentColorIndex = 0;
		
		// Add bricks to the game board
		for(int rowIndex = 0; rowIndex < numRows; rowIndex++) {
			// Reset how many bricks are destroyed in each row
			_numDestroyedBricksInRows[rowIndex] = 0;
			
			// Create individual bricks and set their position and values
			for(int brickIndex = 0; brickIndex < numBricksPerRow; brickIndex++) {
				GameObject brickClone = Instantiate(brick, currentPosition, this.transform.rotation) as GameObject;
				brickClone.transform.parent = this.transform;
				brickClone.renderer.material.color = rowColors[currentColorIndex];
				Brick brickScript = brickClone.GetComponent<Brick>();
				brickScript.ScoreValue = brickValuesPerColor[currentColorIndex];
				brickScript.Row = rowIndex;
				
				currentPosition.x += brickLength;
			}
			
			// Move onto the next color brick
			if((rowIndex + 1) % rowsPerColor == 0) {
				currentColorIndex++;
			}
			
			currentPosition.x = initialPosition.x;
			currentPosition.y -= brickWidth;
		}
	}
	
	public void OnBrickDestroyed(Brick destroyedBrick) {
		// Check the row that the destroyed brick was in to see if all bricks in that row were destroyed
		_numDestroyedBricksInRows[destroyedBrick.Row]++;
		if(_numDestroyedBricksInRows[destroyedBrick.Row] == numBricksPerRow) {
			_ball.IncrementBallSpeed();
		}
		
		// Check if the player broke the top row brick
		if(!_brokeTopRowThisLevel && destroyedBrick.Row == 0) {
			_brokeTopRowThisLevel = true;
			_paddle.DecreasePaddleSize();
		}
		
		// Count the remaining number of bricks to see if the level is done
		GameObject[] bricks = GameObject.FindGameObjectsWithTag("Brick");
		if(bricks.Length - 1 == 0) {
			_scoreManager.GameLevel++;
			_ball.ResetBallPosition();
			InitializeGameBoard();
		}
	}
	
	// Let the player move the paddle and ball
	public void StartGame() {
		_scoreManager.ResetGame();
		_paddle.EnablePaddle();
		_ball.LaunchBall();
	}
	
	// Prevent the player from playing the game
	public void StopGame() {
		_paddle.DisablePaddle();
		_ball.StopBall();
		
		// Destroy the remaining bricks
		Brick[] bricks = FindObjectsOfType(typeof(Brick)) as Brick[];
		foreach(Brick brick in bricks)
		{
		    Destroy(brick.gameObject);
		}
		
		// Rebuild the game
		InitializeGameBoard();
	}
}
