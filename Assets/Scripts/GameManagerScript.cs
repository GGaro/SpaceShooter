using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] GameObject PlayerPrefab;
    [SerializeField] float TimeForEnemySpawn = 1.0f;
    [HideInInspector]
    public int EnemyNum = 0;
    public int enemySpeed = 6;
    [SerializeField]
    int EnemyLimit = 1;
    int ScoreNum;
    bool GameOverbool = false;

    [SerializeField]
    float limitX = 8.4f;
    [SerializeField]
    float limitY = 5f;
    Quaternion rot;
    public AudioSource audioSource;

    [SerializeField] TextMeshProUGUI GameOverText;
    [SerializeField] TextMeshProUGUI ContinueText;
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] Image[] PlayerHealthImage;

    [SerializeField] TextMeshProUGUI TutorialText;

    private void Start()
    {
        ResetUI();
        StartCoroutine(TutorialOver());
        rot = EnemyPrefab.transform.rotation;
    }

    private void Update()
    {
        if (GameOverbool)
        {

            if (Input.anyKey)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void SpawnMore()
    {
        EnemyLimit++;
        enemySpeed += 2;
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator TutorialOver()
    {
        yield return new WaitForSeconds(1f);
        DisableTutorialUI();
        Instantiate(PlayerPrefab);
        StartCoroutine(SpawnEnemy());

    }

    private void DisableTutorialUI()
    {
        TutorialText.enabled = false;
    }

    private IEnumerator SpawnEnemy()
    {
        float randX = Random.Range(-limitX - 1f, limitX + 1f) * (Random.Range(0, 2) * 2 - 1);

        float randY = limitY + 1 * (Random.Range(0, 2) * 2 - 1);

        Debug.Log(randX + ", " + randY);
        yield return new WaitForSeconds(TimeForEnemySpawn);

        Instantiate(EnemyPrefab, new Vector3(randX, randY), rot);

        EnemyNum++;
        if (EnemyNum < EnemyLimit)
        {
            StartCoroutine(SpawnEnemy());
        }
    }

    public void Loop(Transform obj, float offsetX, float offsetY)
    {
        var x = obj.position.x;
        var y = obj.position.y;
        if (x > limitX + offsetX)
        {
            obj.position = new Vector3(-limitX - offsetX, obj.position.y, obj.position.z);
        }
        else if (x < -limitX + -offsetX)
        {
            obj.position = new Vector3(limitX + offsetX, obj.position.y, obj.position.z);

        }

        if (y > limitY + offsetY)
        {
            obj.position = new Vector3(obj.position.x, -limitY - offsetY, obj.position.z);

        }
        else if (y < -limitY + -offsetY)
        {
            obj.position = new Vector3(obj.position.x, limitY + offsetY, obj.position.z);

        }
    }
    public void GameOver()
    {
        GameOverText.enabled = true;
        ContinueText.enabled = true;
        StartCoroutine(Retry());
    }
    private IEnumerator Retry()
    {
        yield return new WaitForSeconds(2f);
        GameOverbool = true;
    }

    public void AddScore()
    {
        ScoreNum++;
        ScoreText.text = "SCORE:" + ScoreNum.ToString();
    }

    public void DamagePlayer(int pos)
    {
        PlayerHealthImage[pos].enabled = false;
    }

    private void ResetUI()
    {
        GameOverbool = false;
        GameOverText.enabled = false;
        ContinueText.enabled = false;
        ScoreNum = 0;
        ScoreText.text = "SCORE:" + ScoreNum.ToString();
        foreach (var item in PlayerHealthImage)
        {
            item.enabled = true;
        }
    }


}
