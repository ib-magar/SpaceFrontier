
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager _instance;   
    [SerializeField] GameObject _GameplayUI;
    [SerializeField] GameObject _MenuUI;
    [SerializeField] GameObject _deathMenu;
    [SerializeField] Transform _headTransform;
    [SerializeField] float _scoreMultiplier;
    [SerializeField] TMP_Text _scoreText;

    private int _highScore;
    private void Awake()
    {
            _instance = this;
    }
    private void Start()
    {
        setUI(false);
        _deathMenu.SetActive(false);
    }
    public void setUI(bool _isGameplayUIActive)
    {
        _GameplayUI.SetActive( _isGameplayUIActive);
        _MenuUI.SetActive(!_isGameplayUIActive);
    }
    public void DeathMenu()
    {
        _deathMenu.SetActive(true);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void FixedUpdate()
    {
        int value =(int) (_headTransform.position.y * _scoreMultiplier);
        if (value > _highScore)
        {
            _highScore = value;
            _scoreText.text = _highScore.ToString("0");
        }
    }
}
