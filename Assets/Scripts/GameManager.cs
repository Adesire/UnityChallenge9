using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }

    public bool IsDead { set; get; }
    private bool _isGameStarted = false;
    private PlayerMotor _motor;

    //UI and UI fields
    public Animator gameCanvas, menuAnim, diamondAnim;
    public Text scoreText, coinText, modifierText;
    public float score, coinScore, modifierScore;
    private const int COIN_SCORE_AMOUNT = 5;
    private int lastScore;
    
    //Death menu
    public Animator deathMenuAnimator;
    public Text deathScoreText, deathCoinText;

    private void Awake()
    {
        Instance = this;
        modifierScore = 1;
        _motor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMotor>();
        
        scoreText.text = score.ToString("0");
        coinText.text = coinScore.ToString("0");
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }

    // Update is called once per frame
    void Update()
    {
        if (MobileInput.Instance.Tap && !_isGameStarted)
        {
            _isGameStarted = true;
            _motor.StartRunning();
            FindObjectOfType<GlacierSpawner>().IsScrolling = true;
            FindObjectOfType<CameraMotor>().IsMoving = true;
            gameCanvas.SetTrigger("Show");
            menuAnim.SetTrigger("Hide");
        }

        if (_isGameStarted && !IsDead)
        {
            //Bump score up
            score += (Time.deltaTime * modifierScore);
            if(lastScore != (int)score)
            {
                lastScore = (int) score;
                scoreText.text = score.ToString("0");
            }
        }
    }

    public void GetCoin()
    {
        diamondAnim.SetTrigger("Collect");
        coinScore++;
        coinText.text = coinScore.ToString("0");
        score += COIN_SCORE_AMOUNT;
        scoreText.text = score.ToString("0");
    }

    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }

    public void OnPlayButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void OnDeath()
    {
        IsDead = true;
        FindObjectOfType<GlacierSpawner>().IsScrolling = false;
        deathScoreText.text = score.ToString("0");
        deathCoinText.text = coinScore.ToString("0");
        deathMenuAnimator.SetTrigger("Dead");
    }
}