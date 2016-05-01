using UnityEngine;
using System.Collections;

public class AnswerBlock : MonoBehaviour {

	private BossSphinx sphinx;
	private bool enabled;

	// Use this for initialization
	void Start () {
		sphinx = transform.parent.GetComponent<BossSphinx> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (!enabled)
			return;
		Player player = col.gameObject.transform.GetComponentInParent<Player>();
		if (player != null) {
			sphinx.TogglePlayerCorrect (player.playerNum, true);
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (!enabled)
			return;
		Player player = col.gameObject.transform.GetComponentInParent<Player>();
		if (player != null) {
			sphinx.TogglePlayerCorrect (player.playerNum, false);
		}
	}

	public void ToggleEnable(bool toggle)
	{
		enabled = toggle;
	}

}