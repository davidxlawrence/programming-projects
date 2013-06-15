using UnityEngine;
using System.Collections;

public class ButtonManager : MonoBehaviour {
	
	public UIButton playButton;
	public UIButton tryAgainButton;
	
	private HUD _hud;
	private Game _game;

	void Start () {
		// Get the reference to the HUD script
		_hud = (HUD)GameObject.FindObjectOfType(typeof(HUD));
		if (_hud == null){
			Debug.LogError("ButtonManager: Start: Can't find the HUD script!");
			return;
		}
		
		// Get the reference to the game script
		_game = (Game)GameObject.FindObjectOfType(typeof(Game));
		if (_game == null){
			Debug.LogError("ButtonManager: Start: Can't find the Game script!");
			return;
		}
		
		NGUITools.SetActive(tryAgainButton.gameObject, false);
	}
	
	// Start the game when the player presses the play button or try again button
	public void OnStartGameButtonClicked() {
		NGUITools.SetActive(playButton.gameObject, false);
		NGUITools.SetActive(tryAgainButton.gameObject, false);
		_game.StartGame();
		_hud.OnGameStarted();
	}
	
	// Model will let the controller know when the game has ended
	public void OnGameEnded() {
		NGUITools.SetActive(tryAgainButton.gameObject, true);
	}
}
