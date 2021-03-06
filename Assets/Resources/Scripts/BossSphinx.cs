﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BossSphinx : Boss
{

	public int countdownSecs = 5;
	public float wrongAnswerPenalty = .05f;
	public float currentShotDelay = 1f;
	public float questionDelay = 2f;

	private Image panel;
	private Text question;
	private Text countdown;
	private Text answer1;
	private Text answer2;
	private Text answer3;
	private Text answer4;
	private AnswerBlock answerBlock1;
	private AnswerBlock answerBlock2;
	private AnswerBlock answerBlock3;
	private AnswerBlock answerBlock4;
	private Text[] answerSlots;
	private AnswerBlock[] answerBlocks;
	private int currCountdown;
	private HashSet<int> fakeAnswers;
	private HashSet<int> correctPlayers;
	private GameManager gameMgr;

	protected override void Start ()
	{
		base.Start ();

		answerSlots = new Text[4];
		answerBlocks = new AnswerBlock[4];
		fakeAnswers = new HashSet<int> ();
		correctPlayers = new HashSet<int> ();
		gameMgr = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		Transform canvas = GameObject.Find ("HPBars").transform.Find ("SphinxCanvas");
		panel = canvas.Find ("Panel").GetComponent<Image> ();
		question = panel.GetComponentInChildren<Text> ();
		countdown = canvas.Find ("Countdown").GetComponent<Text> ();
		answer1 = canvas.Find ("Answer1").GetComponent<Text> ();
		answer2 = canvas.Find ("Answer2").GetComponent<Text> ();
		answer3 = canvas.Find ("Answer3").GetComponent<Text> ();
		answer4 = canvas.Find ("Answer4").GetComponent<Text> ();
		answerBlock1 = transform.FindChild ("answerBlock1").GetComponent<AnswerBlock> ();
		answerBlock2 = transform.FindChild ("answerBlock2").GetComponent<AnswerBlock> ();
		answerBlock3 = transform.FindChild ("answerBlock3").GetComponent<AnswerBlock> ();
		answerBlock4 = transform.FindChild ("answerBlock4").GetComponent<AnswerBlock> ();
		answerSlots [0] = answer1;
		answerSlots [1] = answer2;
		answerSlots [2] = answer3;
		answerSlots [3] = answer4;
		answerBlocks [0] = answerBlock1;
		answerBlocks [1] = answerBlock2;
		answerBlocks [2] = answerBlock3;
		answerBlocks [3] = answerBlock4;

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
		answerBlock1.GetComponent<SpriteRenderer>().enabled = show;
		answerBlock2.GetComponent<SpriteRenderer>().enabled = show;
		answerBlock3.GetComponent<SpriteRenderer>().enabled = show;
		answerBlock4.GetComponent<SpriteRenderer>().enabled = show;
	}

	private void AskQuestion ()
	{
		Barrel[] barrels = GetComponentsInChildren<Barrel> ();
		foreach (Barrel barrel in barrels) {
			barrel.shotDelay = currentShotDelay;
		}
		ShowUIObjects (true);
		correctPlayers.Clear ();
		fakeAnswers.Clear ();
		for (int i = 0; i < 4; i++) {
			answerBlocks [i].GetComponent<AnswerBlock> ().ToggleEnable (false);
		}

		int answerSlot = UnityEngine.Random.Range (0, 4);
		Text answerText = answerSlots [answerSlot];
		answerBlocks [answerSlot].GetComponent<AnswerBlock> ().ToggleEnable (true);

		switch (currentPhaseIndex) {
		case 0:
			{
				int num1 = UnityEngine.Random.Range (0, 100);
				int num2 = UnityEngine.Random.Range (0, 100);

				int answer = num1 + num2;
				fakeAnswers.Add (answer);
				question.text = num1 + " + " + num2;
				answerText.text = answer + "";

				for (int i = 0; i < 4; i++) {
					if (i == answerSlot) {
						continue;
					}
					int fakeAnswer = answer + UnityEngine.Random.Range (-10, 11);
					while (fakeAnswers.Contains (fakeAnswer)) {
						fakeAnswer = answer + UnityEngine.Random.Range (-10, 11);
					}
					fakeAnswers.Add (fakeAnswer);
					answerSlots [i].text = fakeAnswer + "";
				}
			}
			break;
		case 1:
			{
				int num1 = UnityEngine.Random.Range (2, 10);
				int num2 = UnityEngine.Random.Range (2, 10);
				int num3 = UnityEngine.Random.Range (1, 10);

				int answer = num1 * num2 + num3;
				fakeAnswers.Add (answer);
				question.text = num1 + " x " + num2 + " + " + num3;
				answerText.text = answer + "";

				for (int i = 0; i < 4; i++) {
					if (i == answerSlot) {
						continue;
					}
					int fakeAnswer = answer + UnityEngine.Random.Range (-10, 11);
					while (fakeAnswers.Contains (fakeAnswer)) {
						fakeAnswer = answer + UnityEngine.Random.Range (-10, 11);
					}
					fakeAnswers.Add (fakeAnswer);
					answerSlots [i].text = fakeAnswer + "";
				}
			}
			break;
		case 2:
			{
				int num1 = UnityEngine.Random.Range (2, 10);
				int num2 = UnityEngine.Random.Range (2, 10);
				int num3 = UnityEngine.Random.Range (1, 10);

				int answer = num1 * num2 - num3;
				fakeAnswers.Add (answer);
				question.text = num1 + " x " + num2 + " - " + num3;
				answerText.text = answer + "";

				for (int i = 0; i < 4; i++) {
					if (i == answerSlot) {
						continue;
					}
					int fakeAnswer = answer + UnityEngine.Random.Range (-10, 11);
					while (fakeAnswers.Contains (fakeAnswer)) {
						fakeAnswer = answer + UnityEngine.Random.Range (-10, 11);
					}
					fakeAnswers.Add (fakeAnswer);
					answerSlots [i].text = fakeAnswer + "";
				}
			}
			break;
		case 3:
			{
				int num1 = UnityEngine.Random.Range (2, 10);
				int num2 = UnityEngine.Random.Range (2, 10);
				int num3 = UnityEngine.Random.Range (2, 10);

				int answer = num1 * num2 * num3;
				fakeAnswers.Add (answer);
				question.text = num1 + " x " + num2 + " x " + num3;
				answerText.text = answer + "";

				for (int i = 0; i < 4; i++) {
					if (i == answerSlot) {
						continue;
					}
					int fakeAnswer = answer + 10 * UnityEngine.Random.Range (-4, 5);
					while (fakeAnswers.Contains (fakeAnswer)) {
						fakeAnswer = answer + 10 * UnityEngine.Random.Range (-4, 5);
					}
					fakeAnswers.Add (fakeAnswer);
					answerSlots [i].text = fakeAnswer + "";
				}
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

		List<Player> playerList = gameMgr.playerObjList;
		int deaths = 0;
		foreach (Player player in playerList) {
			if (!correctPlayers.Contains (player.playerNum)) {
				player.GetComponentInChildren<Head>().Die ();
				deaths++;
			}
		}
		currentShotDelay = Mathf.Clamp (currentShotDelay - deaths * wrongAnswerPenalty, 0.05f, 1f);

		Invoke ("AskQuestion", questionDelay);
	}

	public void TogglePlayerCorrect (int playerNum, bool toggle)
	{
		if (toggle) {
			correctPlayers.Add (playerNum);
		} else {
			correctPlayers.Remove (playerNum);
		}
	}
}
