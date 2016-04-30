using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BossSphinx : Boss
{

	public int countdownSecs = 5;
	public float wrongAnswerPenalty = .05f;
	public float questionDelay = 2f;

	private Image panel;
	private Text question;
	private Text countdown;
	private Text answer1;
	private Text answer2;
	private Text answer3;
	private Text answer4;
	private Text[] answerSlots;
	private int currCountdown;
	private HashSet<int> fakeAnswers;

	protected override void Start ()
	{
		base.Start ();

		answerSlots = new Text[4];
		fakeAnswers = new HashSet<int> ();
		Transform canvas = GameObject.Find ("HPBars").transform.Find ("SphinxCanvas");
		panel = canvas.Find ("Panel").GetComponent<Image> ();
		question = panel.GetComponentInChildren<Text> ();
		countdown = canvas.Find ("Countdown").GetComponent<Text> ();
		answer1 = canvas.Find ("Answer1").GetComponent<Text> ();
		answer2 = canvas.Find ("Answer2").GetComponent<Text> ();
		answer3 = canvas.Find ("Answer3").GetComponent<Text> ();
		answer4 = canvas.Find ("Answer4").GetComponent<Text> ();
		answerSlots [0] = answer1;
		answerSlots [1] = answer2;
		answerSlots [2] = answer3;
		answerSlots [3] = answer4;

		AskQuestion ();
	}

	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}

	private void ShowUIObjects (bool show)
	{
		panel.enabled = show;
		question.enabled = show;
		countdown.enabled = show;
		answer1.enabled = show;
		answer2.enabled = show;
		answer3.enabled = show;
		answer4.enabled = show;
	}

	private void AskQuestion ()
	{
		ShowUIObjects (true);
		fakeAnswers.Clear ();

		switch (currentPhaseIndex) {
		case 0:
			int num1 = UnityEngine.Random.Range (0, 100);
			int num2 = UnityEngine.Random.Range (0, 100);

			int answerSlot = UnityEngine.Random.Range (0, 4);
			Text answerText = answerSlots [answerSlot];

			int answer = num1 + num2;
			fakeAnswers.Add (answer);
			question.text = num1 + " + " + num2;
			answerText.text = answer + "";

			for (int i = 0; i < 4; i++) {
				if (i == answerSlot) {
					continue;
				}
				int fakeAnswer = answer + UnityEngine.Random.Range (-5, 5);
				while (fakeAnswers.Contains (fakeAnswer)) {
					fakeAnswer = answer + UnityEngine.Random.Range (-5, 5);
				}
				answerSlots [i].text = fakeAnswer + "";
			}

			break;
		}

		currCountdown = countdownSecs;
		DoCountdown ();
	}

	private void DoCountdown ()
	{
		countdown.text = currCountdown + "";
		currCountdown--;
		if (currCountdown < 0) {
			CheckAnswer ();
		} else {
			Invoke ("DoCountdown", 1f);
		}
	}

	private void CheckAnswer ()
	{
		ShowUIObjects (false);

		Invoke ("AskQuestion", questionDelay);
	}
}
