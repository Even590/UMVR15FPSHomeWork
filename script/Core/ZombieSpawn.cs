using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ZombieSpawn : MonoBehaviour
{
    public GameObject ZombiePrefab;
    public int maxZombiePerWave = 30;
    public float spawnInterval = 0.5f;
    public float waveInterval = 5f;
    public Transform[] spawnPoints;

    public Text ZombieWave;

    private int currentWave = 0;
    private int zombiesLeft = 0; // 確保這裡初始化為 0
    private bool isSpawning = false;

    void Start()
    {
        StartCoroutine(StartZombieWave());
    }

    private void Update() 
    {
        UpdateWave();
    }

    private IEnumerator StartZombieWave()
    {
        while (true)
        {
            // 如果還有殭屍活著或正在生成，等待
            if (zombiesLeft > 0 || isSpawning)
            {
                yield return null;
                continue;
            }

            // 開始新的一波
            currentWave++;
            Debug.Log($"開始第 {currentWave} 波！");
            yield return new WaitForSeconds(waveInterval); // 等待波次間隔時間

            StartCoroutine(SpawnZombies(maxZombiePerWave));
        }
    }

    private IEnumerator SpawnZombies(int count)
    {
        isSpawning = true;
        zombiesLeft = count;

        for (int i = 0; i < count; i++)
        {
            SpawnZombie();
            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
    }

    private void SpawnZombie()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.Log("沒有設置生成點！");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        NavMeshHit hit;
        if (NavMesh.SamplePosition(spawnPoint.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            GameObject zombie = Instantiate(ZombiePrefab, hit.position, Quaternion.identity);

            AIController zombieScript = zombie.GetComponent<AIController>();
            if (zombieScript != null)
            {
                zombieScript.zombieSpawn = this;
            }
        }
        else
        {
            Debug.Log("生成點不在NavMesh上，無法生成殭屍！");
        }
    }

    public void OnZombieKilled()
    {
        zombiesLeft--; // 每當殭屍被殺死時減少數量
        if (zombiesLeft < 0) zombiesLeft = 0;

        Debug.Log($"剩餘殭屍：{zombiesLeft}");
    }

    private void UpdateWave()
    {
        if(ZombieWave != null)
        {
            ZombieWave.text = $"{currentWave}";
        }
    }
}
