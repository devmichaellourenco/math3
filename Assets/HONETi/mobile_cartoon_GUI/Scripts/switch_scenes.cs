using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class switch_scenes : MonoBehaviour {

	public string sceneName = "";

	void Start ()
	{
		Button b = GetComponent<Button> ();
		if (b != null && sceneName != "") {
			b.onClick.AddListener (() => SceneManager.LoadScene (sceneName));
		}
	}
}
