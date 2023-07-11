using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Auxiliars;

public class PlayerHealth : MonoBehaviour {

    #region Variables
    public int Health { get; private set; }
    public int Armor { get; private set; }
    private int CurrHeartIndex => SpartanMath.RoundToInt((this.Health / 2f), RoundingMode.UP, 0f) - 1;

    [SerializeField]
    private Image heartImage;
    private Image[] hearts = new Image[MAX_HEALTH / 2];
    #endregion

    #region Constants
    public const int MAX_HEALTH = 6;
    #endregion

    private void Start() {
        this.Health = MAX_HEALTH;
        //Populate heart array with the images
        for (int i = 0; i < hearts.Length; i++) {
            this.hearts[i] = this.heartImage;
        }
    }

    public bool Damage() {
        //Handle parry or any other damage avoiding methods here
        this.Health--;
        //If the health is odd, the heart should be a half
        if (!SpartanMath.IsEven(this.Health)) {
            this.hearts[this.CurrHeartIndex].fillAmount = 0.5f;
        }
        //Make sure to disable all other hearts here (we can afford a loop)
        return true;
    }

}
