using UnityEngine;
using TMPro;
using System.Collections;

public class BossDialogue : MonoBehaviour
{

	public BossStateMachine stateMachine;
	public GameObject text;
	public TextMeshProUGUI textField;

	int stunCount = 0; 

	// Update is called once per frame
	bool prevStunned = false;
	int prevAttackFinished = 0;
	void Update() {
		// Detect stun
		if (stateMachine.IsStunned && !prevStunned) {
			stunCount++;
			PrintText(true);
		}
		prevStunned = stateMachine.IsStunned;

		// Detect attack finished
		if (stateMachine.AttackFinished == 1 && prevAttackFinished == 0) {
			PrintText(false);
		}
		prevAttackFinished = stateMachine.AttackFinished;
	}

		void CloseText() {
			text.SetActive(false);
		}

		void PrintText(bool stunned) {
			string a = "p";
			if (stunned) {
				if (stunCount == 1) {
					a = "Ugh!";
				} else if (stunCount == 2) {
					a = "Impressive...";
				} else if (stunCount == 3) {
					a = "Ngh!";
				}
				textField.text = a;
				text.SetActive(true);
				Invoke("CloseText", 2f);
			} else {
					int dialougeChoice = Random.Range(0,5);
					if (dialougeChoice == 0) {
						a = "\"This must be done...\"";
					} else if (dialougeChoice == 1) {
						a = "\"You should have surrendered, ONE.\"";
					} else if (dialougeChoice == 2) {
						a = "\"Duty outweighs desire.\"";
					} else if (dialougeChoice == 3) {
						a = "\"There is no other way.\"";
					} else if (dialougeChoice == 4) {
						a = "\"Things could have been different...\"";
					}
					textField.text = a;
					text.SetActive(true);
					CancelInvoke("CloseText");
					Invoke("CloseText", 2f);
			}
		}
}
