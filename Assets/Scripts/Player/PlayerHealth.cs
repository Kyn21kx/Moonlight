using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Auxiliars;
using System.Linq;

public class PlayerHealth : MonoBehaviour {

	#region Variables
	public int Health { get; private set; }
	public int Armor { get; private set; }
	public bool Dead => Health <= 0 && Armor <= 0;
	private int CurrHeartIndex => SpartanMath.RoundToInt((this.Health / 2f), RoundingMode.UP, 0f) - 1;

	[SerializeField]
	private GameObject heartImgParent;
	[SerializeField]
	private Sprite fullHeartSprite;
	[SerializeField]
	private Sprite brokenHeartSprite;
	private Image[] hearts = new Image[MAX_HEALTH / 2];
	#endregion

	#region Constants
	public const int MAX_HEALTH = 6;
	private const string HEART_CHILD_NAME_IDENTIFIER = "Heart";
	#endregion

	private void Start() {
		this.Health = MAX_HEALTH;
		hearts = heartImgParent.GetComponentsInChildren<Image>()
			.Where((child) => child.name.StartsWith(HEART_CHILD_NAME_IDENTIFIER))
			.ToArray();
		//Populate heart array with the images
		for (int i = 0; i < hearts.Length; i++) {
			this.hearts[i].sprite = this.fullHeartSprite;
		}
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			this.Damage();
		}
	}

	public bool Damage() {
		//Handle parry or any other damage avoiding methods here
		//This is if we do take damage:
		//If the health has been damaged before, take it out
		if (this.hearts[this.CurrHeartIndex].sprite == this.brokenHeartSprite) {
			this.hearts[this.CurrHeartIndex].gameObject.SetActive(false);
		}
		this.Health--;
		//If the health is odd, the heart should be a half
		if (!SpartanMath.IsEven(this.Health)) {
			this.hearts[this.CurrHeartIndex].sprite = this.brokenHeartSprite;
		}

		if (this.Dead) {
			Destroy(this.gameObject);
		}
		return true;
	}

	public void Heal(int amount) {
		throw new System.NotImplementedException();
	}

}
