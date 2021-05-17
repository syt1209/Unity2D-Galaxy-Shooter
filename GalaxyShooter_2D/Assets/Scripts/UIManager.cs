using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // cached reference
    [SerializeField] private Text _scoreText, _gameoverText, _restartText, _levelText, _waveText;
    [SerializeField] private Image _lifeImage, _thrusterFillImage, _ammoImage;
    [SerializeField] private GameManager _gameManager;

    // config variables
    [SerializeField] private Sprite[] _lifeSprite, _ammoSprite;
    private WaitForSeconds _flickerWait = new WaitForSeconds(0.5f);
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameoverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
    }

    public void UpdateScoreText(int score)
    {
        _scoreText.text = "Score: " + score.ToString();
    }
    public void UpdateLevelText(int level)
    {
        _levelText.text = "Level: " + level.ToString();
    }
    public void UpdateWaveText(int wave)
    {
        _waveText.text = "Wave: " + wave.ToString();
    }

    public void UpdateLifeImage(int life)
    {
        _lifeImage.sprite = _lifeSprite[life];

        if (life == 0)
        {
            GameOverSequence();
        }
    }

    public void UpdateAmmoImage(int ammo)
    {
        _ammoImage.sprite = _ammoSprite[ammo];
    }

    public void UpdateThrusterHUD(float fillAmount)
    {
        _thrusterFillImage.fillAmount = fillAmount;
    }

    private void GameOverSequence()
    {
        _gameoverText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickrRoutine());

        _restartText.gameObject.SetActive(true);
        _gameManager.GameOver();
    }

    private IEnumerator GameOverFlickrRoutine()
    {
        while (true)
        {
            yield return _flickerWait;
            _gameoverText.text = "";
            yield return _flickerWait;
            _gameoverText.text = "Game Over";
        }
    }
}
