using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {
	
	private int _playerScore;
	private int _gameLevel;
	private int _playerLives;
	private HUD _hud;
	private ButtonManager _buttonManager;
	private Game _game;
	
	void Start() {
		// Get the reference to the HUD script
		_hud = (HUD)GameObject.FindObjectOfType(typeof(HUD));
		if (_hud == null){
			Debug.LogError("ScoreManager: Start: Can't find the HUD script!");
			return;
		}
		
		// Get the reference to the button manager script
		_buttonManager = (ButtonManager)GameObject.FindObjectOfType(typeof(ButtonManager));
		if (_buttonManager == null){
			Debug.LogError("ScoreManager: Start: Can't find the ButtonManager script!");
			return;
		}
		
		// Get the reference to the game script
		_game = (Game)GameObject.FindObjectOfType(typeof(Game));
		if (_game == null){
			Debug.LogError("ButtonManager: Start: Can't find the Game script!");
			return;
		}
	}
	
	public void ResetGame() {
		PlayerScore = 0;
		GameLevel = 1;
		PlayerLives = 3;
	}
	
	#region Accessors
	public int PlayerScore {
		get { return _playerScore; }
		set 
		{
			if (value >= 0) {
				_playerScore = value;
				_hud.OnScoreChanged();
			}
		}
	}
	
	public int GameLevel {
		get { return _gameLevel; }
		set 
		{
			if (value >= 0) {
				_gameLevel = value;
				_hud.OnLevelChanged();
			}
		}
	}
	
	public int PlayerLives {
		get { return _playerLives; }
		set 
		{
			if (value >= 0) {
				_playerLives = value;
				_hud.OnLivesChanged();
				if(_playerLives == 0) {
					_game.StopGame();
					_hud.OnGameEnded();
					_buttonManager.OnGameEnded();
				}
			}
		}
	}
	#endregion
}
