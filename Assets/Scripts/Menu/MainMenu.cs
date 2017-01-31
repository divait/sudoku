using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public GameObject menu, levels, credits, ranking; // Menus


	/**
	 * Open game Scene
	 * 
	 * */ 
	public void playGame() {
		SceneManager.LoadScene ("Game");
	}


	/**
	 * Show Leveles Menu
	 * 
	 * */
	public void showLevels() {
		menu.SetActive (false);
		credits.SetActive (false);
		ranking.SetActive (false);
		levels.SetActive (true);
	}

	/**
	 * Show Credits Menu
	 * 
	 * */
	public void showCredits() {
		menu.SetActive (false);
		credits.SetActive (true);
		ranking.SetActive (false);
		levels.SetActive (false);
	}

	/**
	 * Show Ranking Menu
	 * 
	 * */
	public void showRanking() {
		menu.SetActive (false);
		credits.SetActive (false);
		ranking.SetActive (true);
		levels.SetActive (false);
	}

	/**
	 * Show Main Menu
	 * 
	 * */
	public void showMain() {
		menu.SetActive (true);
		credits.SetActive (false);
		ranking.SetActive (false);
		levels.SetActive (false);
	}

	/**
	 * Save Level
	 * 
	 * **/
	public void setLevel(string level) {
		PlayerPrefs.SetString("Level", level);
	}
}
