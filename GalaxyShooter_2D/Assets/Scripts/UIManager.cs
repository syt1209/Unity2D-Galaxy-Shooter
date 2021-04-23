using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // cached reference
    [SerializeField] private Text _scoreText, _gameoverText;
    [SerializeField] private Image _lifeImage;

    // config variables
    [SerializeField] private Sprite[] _lifeSprite;
    private WaitForSeconds _flickerWait = new WaitForSeconds(0.5f);
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _gameoverText.gameObject.SetActive(false);
    }

    public void UpdateScoreText(int score)
    {
        _scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateLifeImage(int life)
    {
        _lifeImage.sprite = _lifeSprite[life];

        if (life == 0)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        _gameoverText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickrRoutine());
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
