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

	void Start()
	{
		stateMachine.BossDialogue += PrintText;
	}
	// void Update() {
	// 	// Detect stun
	// 	if (stateMachine.IsStunned && !prevStunned) {
	// 		stunCount++;
	// 		PrintText(true);
	// 	}
	// 	prevStunned = stateMachine.IsStunned;

	// 	// Detect attack finished
	// 	if (stateMachine.InIdle && Random.Range(0f, 1f) < dialogueChance) {
	// 		PrintText(false);
	// 	}
	// 	prevAttackFinished = stateMachine.AttackFinished;
	// }

		void CloseText() {
			text.SetActive(false);
		}

		void PrintText(int stage) {
			string a = "p";
			int dialougeChoice = Random.Range(0,5);
			if (stage == 1)
			{
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
			}
			else if (stage == 2)
			{
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
			} 
			else if (stage == 3)
			{
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
			} 
			else
			{
				//ultimate dialogue
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
			}
			
			textField.text = a;
			text.SetActive(true);
			CancelInvoke("CloseText");
			Invoke("CloseText", 2f);
		}
}
