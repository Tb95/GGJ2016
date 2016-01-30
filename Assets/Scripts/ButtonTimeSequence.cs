﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonTimeSequence {

	public struct ButtonTime {
		public ButtonsManager.Button button;
		public float timeFromStart;
	}
	List<ButtonTime> buttonTimeList = new List<ButtonTime>();
	public int maxListLength = 10;

	public void prepend(ButtonTime bt) {
		buttonTimeList.Insert (0, bt);
		if (buttonTimeList.Count > maxListLength) {
			buttonTimeList.RemoveAt (buttonTimeList.Count - 1);
		}
	}

	public bool isSequenceOK(SpiderCombo spiderCombo, float minDeltaT) {
		// CONTROLLI INIZIALI
		if (buttonTimeList.Count < spiderCombo.buttonsList.Count) // Se non ho fatto abbastanza mosse
			return false;
		if (!spiderCombo.spider.isDown) // Se il ragno non è girato
			return false;
		if (Vector3.Distance (spiderCombo.spider.player.transform.position, spiderCombo.spider.transform.position) > spiderCombo.spider.radiusForAttack) // Se non sono vicino al ragno
			return false;
		
		bool isOK = true;
		for (int i = 0; i < spiderCombo.buttonsList.Count; i++) {
			if (buttonTimeList [i].button != spiderCombo.buttonsList [spiderCombo.buttonsList.Count - 1 - i]) {
				isOK = false;
			}
		}
			
		if ((buttonTimeList [0].timeFromStart - buttonTimeList [spiderCombo.buttonsList.Count - 1].timeFromStart) > minDeltaT) {
			isOK = false;
		}

		return isOK;
	}

	public void resetSequence() {
		buttonTimeList = new List<ButtonTime>();
	}
}