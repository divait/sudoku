using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SudokuGenerator))]
public class GameControl : MonoBehaviour {

	private int number = -1;
	private SudokuGenerator sudoku;
	private bool inGame;

	public Animator anim;
	public GameObject restartBtn;
	public int life = 10;
	public Text lifeView;
	public Image[] numbersBTn;
	public Color active;

	void Start()
	{
		sudoku = GetComponent<SudokuGenerator> ();
		inGame = true;
	}

	void Update() {
		lifeView.text = life + "";
	}

	public void setNumber(int number) {
		this.number = number;

		foreach(Image img in numbersBTn) 
			img.color = Color.white;
		numbersBTn [number - 1].color = active;
	}

	public void setNumberInBoard (string xy) {
		if (inGame) {
			string[] split = xy.Split (',');

			if (number > 0) {
				bool check = sudoku.checkedNumber (number, int.Parse (xy [0].ToString ()), int.Parse (xy [2].ToString ()));

				if (!check) {
					
					life--;
					if (life == 0)
						lose ();
					else
						anim.SetTrigger ("mistake");
				} else {
					print ("win: " + sudoku.answers);
					if (sudoku.answers == 0)
						win ();
				}
			} else {
				anim.SetTrigger ("mistake");
			}
		}
	}

	private void lose() {
		anim.SetTrigger ("lose");
		inGame = false;
		restartBtn.SetActive (true);
	}

	private void win() {
		anim.SetTrigger ("win");
		inGame = false;
		restartBtn.SetActive (true);
	}

	public void restart() {
		SceneManager.LoadScene ("Menu");
	}
}
