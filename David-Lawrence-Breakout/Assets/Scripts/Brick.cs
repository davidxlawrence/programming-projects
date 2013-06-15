using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {
	
	public enum BrickState { UNTOUCHED, WEAKENED }
	
	public float damageFlashDuration;
	public float damageFlashInterval;
	public Color damageFlashColor;
	
	private BrickState _currentBrickState;
	private ScoreManager _scoreManager;
	private int _scoreValue = 0;
	private int _row;
	private Color _originalColor;
	private float _damageFlashTimer = 0.0f;
	private float _currentFlashInterval = 0.0f;
	
	private Game _gameBoard;

	void Start () {
		_currentBrickState = BrickState.UNTOUCHED;
		_originalColor = this.renderer.material.color;
		
		// Get the reference to the score manager script
		_scoreManager = (ScoreManager)GameObject.FindObjectOfType(typeof(ScoreManager));
		if (_scoreManager == null){
			Debug.LogError("Brick: Start: Can't find the score manager!");
			return;
		}
		
		// Get the reference to the game script
		_gameBoard = (Game)GameObject.FindObjectOfType(typeof(Game));
		if (_gameBoard == null){
			Debug.LogError("Brick: Start: Can't find the game board script!");
			return;
		}
	}
	
	void Update () {
		if(_currentBrickState == BrickState.WEAKENED && _damageFlashTimer < damageFlashDuration) {
			_damageFlashTimer += Time.deltaTime;
			_currentFlashInterval += Time.deltaTime;
			
			// Swap the colors every flash interval and reset the timer
			if(_currentFlashInterval > damageFlashInterval) {
				_currentFlashInterval = 0.0f;
				this.renderer.material.color = (this.renderer.material.color == _originalColor) ? damageFlashColor : _originalColor;
			}
			
			// Make sure the brick returns to the original color but add a tint
			if(_damageFlashTimer > damageFlashDuration) {
				this.renderer.material.color = (_originalColor + damageFlashColor) / 5;
			}
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		if(collision.collider.gameObject.tag == "Ball") {
			if(_currentBrickState == BrickState.UNTOUCHED) {
				_currentBrickState = BrickState.WEAKENED;
			} else {
				_scoreManager.PlayerScore += _scoreValue;
				_gameBoard.OnBrickDestroyed(this);
				GameObject.Destroy(this.gameObject);
			}
		}
	}
	
	// How much this brick is worth when destroyed
	public int ScoreValue {
		get { return _scoreValue; }
		set 
		{
			if (value > 0) _scoreValue = value;
		}
	}
	
	// Which row this brick is in
	public int Row {
		get { return _row; }
		set 
		{
			if (value > 0) _row = value;
		}
	}
}
