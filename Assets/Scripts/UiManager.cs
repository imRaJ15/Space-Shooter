using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    #region Serialize Fields

    [SerializeField]
    Text _scoreText;

    [SerializeField]
    Text _highScore;

    [SerializeField]
    Image _liveImage;

    [SerializeField]
    Text _gameOverText;

    [SerializeField]
    Text _restartGameText;

    [SerializeField]
    Sprite[] _liveSprites;

    #endregion 

    GameManager _gameManager;
    int highScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score : " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartGameText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    }

    public void UpdateScore(int scorepoint)
    {
        _scoreText.text = "Score : " + scorepoint.ToString();
    }

    public void UpdateHighScorew(int highScore)
    {
        _highScore.text = "Highest Score : " + highScore.ToString();
    }


    public void UpdateLive(int currentLive)
    {
        _liveImage.sprite = _liveSprites[currentLive];

        if (currentLive == 0)
        {
            GameOverSenerioes();
        }
    }

    void GameOverSenerioes()
    {
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
        _restartGameText.gameObject.SetActive(true);
        _restartGameText.text = "Press... 'M' key goto Main Menu";
        _gameManager.GameOver = true;
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "Game Over";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
