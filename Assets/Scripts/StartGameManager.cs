using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameManager : MonoBehaviour
{
    public Button startButton;
    public TextMeshProUGUI highScoreText;
    public GameObject Moth;

    void Start()
    {
        startButton.onClick.AddListener(StartGame);

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = highScore.ToString();
    }

    private void Update()
    {
        // Moth.transform.Translate(Vector3.up * 5 * Time.deltaTime);
    }

    void StartGame()
    {
        Debug.Log("Start!");
        SceneManager.LoadScene("MainScene");
    }
}
