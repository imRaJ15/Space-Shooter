using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject _preFabCoOpPlayers;

    [SerializeField]
    GameObject _puaseMenuPanel;

    public GameObject player;
    public bool GameOver = false;
    public bool isCoOpModeActive = false;
    SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if  (GameOver == true)
        {
            if (isCoOpModeActive == false)
            { Instantiate(player, Vector3.zero, Quaternion.identity); }
            else {  Instantiate(_preFabCoOpPlayers, new Vector3(-13, 6, 0), Quaternion.identity); }
            GameOver = false;
            _spawnManager.StartSpawning();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            _puaseMenuPanel.SetActive(true);
            Time.timeScale = 0;
        }

    }

    public void ResumeGame()
    {
        _puaseMenuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void GotoMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
