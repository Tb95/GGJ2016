using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpiderCombo {

	public List<ButtonsManager.Button> buttonsList;
	public Spider spider;

	public SpiderCombo(List<ButtonsManager.Button> bl, Spider sp) {
		buttonsList = bl;
		spider = sp;
	}

}