using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button startButton;
    public Button resumeButton;
    public Button exitButton;
    public Button pauseButton;
    public Button restartButton; 

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public GameObject menuPanel;
    private int score = 0;

    private void OnEnable()
    {
        Food.OnFoodCollected += UpdateScore;
        GameManager.OnGameOver += ShowGameOverMenu;
    }

    void Awake()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Food.OnFoodCollected -= UpdateScore;
        GameManager.OnGameOver -= ShowGameOverMenu;
    }

    private void Start()
    {
        scoreText.text = "Score: 0";
        gameOverText.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);

        menuPanel.SetActive(true);
        Time.timeScale = 0;

        startButton.onClick.AddListener(HandleStart);
        resumeButton.onClick.AddListener(HandleResume);
        exitButton.onClick.AddListener(HandleExit);
        pauseButton.onClick.AddListener(HandlePause);
        restartButton.onClick.AddListener(HandleRestart);

    }

    private void HandlePause()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0;
        startButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true); 
        resumeButton.gameObject.SetActive(true); 
    }

    private void UpdateScore()
    {
        score++;
        scoreText.text = $"Score: {score}";
    }

    private void ShowGameOverMenu()
    {
        menuPanel.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        gameOverText.text = $"Game Over! Final Score: {score}";
        Time.timeScale = 0;
        resumeButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);
        GameManager.Isgridinitialized = false;
    }

    private void HandleStart()
    {
        score = 0;
        scoreText.text = $"Score: {score}";
        gameOverText.gameObject.SetActive(false);
        Time.timeScale = 1;
        menuPanel.SetActive(false);
        resumeButton.gameObject.SetActive(true);
    }

    private void HandleResume()
    {
        Time.timeScale = 1;
        menuPanel.SetActive(false);
    }

    private void HandleExit()
    {
        Application.Quit();
    }

    private void HandleRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
