using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	
	public UILabel levelLabel;
	public UILabel scoreLabel;
	public UILabel livesLabel;
	public UILabel levelPromptLabel;
	public UILabel finalScoreLabel;
	public UILabel instructionsLabel;
	public float levelPromptDuration;
	
	private ScoreManager _scoreManager;
	private bool _levelChanged;
	private float _levelPromptTimer = 0.0f;
	
	void Start() {
		// Get the reference to the score manager script
		_scoreManager = (ScoreManager)GameObject.FindObjectOfType(typeof(ScoreManager));
		if (_scoreManager == null){
			Debug.LogError("Game: Start: Can't find the score manager!");
			return;
		}
		
		_levelChanged = false;
		finalScoreLabel.enabled = false;
		levelLabel.enabled = false;
		scoreLabel.enabled = false;
		livesLabel.enabled = false;
		levelPromptLabel.enabled = false;
	}
	
	void Update () {
		// Show the player that it is the start of the next level
		if(_levelChanged) {
			levelPromptLabel.enabled = true;
			levelPromptLabel.text = "Level " + _scoreManager.GameLevel;
			_levelPromptTimer += Time.deltaTime;
			
			if(_levelPromptTimer >= levelPromptDuration) {
				levelPromptLabel.enabled = false;
				_levelChanged = false;
				_levelPromptTimer = 0.0f;
			}
		}
	}
	
	// Model will notify the view when the level has changed
	public void OnLevelChanged() {
		levelLabel.text = "Level: " + _scoreManager.GameLevel;
		_levelChanged = true;
	}
	
	// Model will notify the view when the score has changed
	public void OnScoreChanged() {
		scoreLabel.text = "Score: " + _scoreManager.PlayerScore;
	}
	
	// Model will notify the view when the number of lives has changed
	public void OnLivesChanged() {
		livesLabel.text = "Lives: " + _scoreManager.PlayerLives;
	}
	
	// Model will notify the view when the game has ended
	public void OnGameEnded() {
		levelLabel.enabled = false;
		scoreLabel.enabled = false;
		livesLabel.enabled = false;
		finalScoreLabel.enabled = true;
		finalScoreLabel.text = "Game Over\nFinal Score: " + _scoreManager.PlayerScore;
	}
	
	// Controller will notify the view when the player clicked the button to play
	public void OnGameStarted() {
		levelLabel.enabled = true;
		scoreLabel.enabled = true;
		livesLabel.enabled = true;
		finalScoreLabel.enabled = false;
		instructionsLabel.enabled = false;
	}
}
