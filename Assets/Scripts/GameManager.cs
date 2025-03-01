using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject mothPrefab;

    [Header("UI")]
    public RawImage[] lifeIcons;
    public TextMeshProUGUI scoreText;
    public GameObject endGamePanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;
    public Button restartButton;
    public Button mainMenuButton;

    private GameObject moth;
    private mothController mothControl;
    //private cameraFollow mothCamera;
    //private roadController roadControl;

    private int score = 0;
    private int remainingLives = 3;

    void Start()
    {
        // reset
        remainingLives = 3;
        score = 0;
        endGamePanel.SetActive(false);

        // Instantiate moth
        moth = Instantiate(mothPrefab, new Vector2(0, 0), Quaternion.identity);

        // get mothControl component
        mothControl = moth.GetComponent<mothController>();

        // set relative references to moth
        cameraFollow mothCamera = Camera.main.GetComponent<cameraFollow>();
        mothCamera.moth = moth;

        roadController roadControl = GetComponent<roadController>();
        roadControl.moth = moth;

        ObstacleGeneration obstacleGenerator = GetComponent<ObstacleGeneration>();
        obstacleGenerator.moth = moth;

        // set button
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    void Update()
    {
        score = Mathf.FloorToInt(moth.transform.position.y * 10.0f);
        scoreText.text = score.ToString();
    }

    public void TakeDamage()
    {
        remainingLives -= 1;
        UpdateLifeUI();

        Debug.Log("Remaining Lives: " + remainingLives);

        if (remainingLives > 0)
        {
            Debug.Log("Moth get dizzy!");
            StartCoroutine(mothControl.Dizzy());
        }
        else
        {
            Debug.Log("Moth is dead!");
            mothControl.Die();
            GameEnd();
        }
    }

    private void UpdateLifeUI()
    {
        if (remainingLives >= 0 && remainingLives < lifeIcons.Length)
        {
            lifeIcons[remainingLives].color = new Color(1, 1, 1, 0.1f);  // transparent
        }
    }

    private void GameEnd()
    {
        // Game End Panel
        endGamePanel.SetActive(true);

        // write final score and 
        finalScoreText.text = score.ToString();

        // update High score and display
        UpdateAndDisplayHighScore();

        // move endGamePanel to top, then use a Coroutine to slowly move it down to the middle
        RectTransform panelRectTransform = endGamePanel.GetComponent<RectTransform>();
        panelRectTransform.anchoredPosition = new Vector2(0, 1200);
        StartCoroutine(SmoothMovePanel(panelRectTransform));
    }

    private IEnumerator SmoothMovePanel(RectTransform panelRectTransform)
    {
        Vector2 targetPosition = new Vector2(0, 0);
        while (Vector2.Distance(panelRectTransform.anchoredPosition, targetPosition) > 1f)
        {
            panelRectTransform.anchoredPosition = Vector2.Lerp(panelRectTransform.anchoredPosition, targetPosition, Time.deltaTime * 2.0f);
            yield return null;
        }
        panelRectTransform.anchoredPosition = targetPosition;
    }

    private void UpdateAndDisplayHighScore()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);  // save new high score
            PlayerPrefs.Save();
        }
        highScoreText.text = highScore.ToString();
    }

    private void RestartGame()
    {
        Debug.Log("Restart");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ReturnToMainMenu()
    {
        Debug.Log("Main Menu");
        SceneManager.LoadScene("MainMenu");
    }
}

