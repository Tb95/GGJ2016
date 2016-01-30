using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ButtonsManager {

	public enum Button {leftButton, rightButton, upButton, downButton, redButton, blueButton, yellowButton, greenButton};

	public List<Button> getRandomCombo(int comboLength, InputManager.Side side) {
		List<Button> aList = new List<Button> ();
		for (int i = 0; i < comboLength; i++) {
			aList.Add (getRandomButton (side));
		}
		return aList;
	}

	Button getRandomButton(InputManager.Side side) {
		int a = Random.Range (0, 4);
		if (a == 0) {
			if (side == InputManager.Side.Left) {
				return Button.upButton;
			} else {
				return Button.yellowButton;
			}
		} else if (a == 1) {
			if (side == InputManager.Side.Left) {
				return Button.leftButton;
			} else {
				return Button.redButton;
			}
		} else if (a == 2) {
			if (side == InputManager.Side.Left) {
				return Button.downButton;
			} else {
				return Button.blueButton;
			}
		} else {
			if (side == InputManager.Side.Left) {
				return Button.rightButton;
			} else {
				return Button.greenButton;
			}
		}
	}
}
