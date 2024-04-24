using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[System.Serializable]
public class LevelHandler : MonoBehaviour
{
    [SerializeField]
    private LevelData LvlData;

    private bool isGameOver;
    public bool IsGameOver => isGameOver;

    public UnityEvent<bool> OnGameOver;

    [Header("In Game")]

    //[SerializeField]
    private List<EnemyChar> enemies = new List<EnemyChar>();

    //[SerializeField]
    private PlayerChar player;
    public PlayerChar Player { get => player; }

    public List<BaseChar> AllChar
    {
        get
        {
            List<BaseChar> all = new List<BaseChar>
            {
                player
            };
            all.AddRange(enemies);

            return all;
        }
    }

    private bool allEnemiesDie
    {
        get
        {
            foreach (BaseChar item in enemies)
            {
                if (!item.IsDie) { return false; }
            }
            return true;
        }
    }

    private EnemyChar enemyBoss;

    [Header("Prefabs")]

    [SerializeField]
    private CharacterData playerCharData;

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject enemyBossPrefab;

    [Header("Components")]
    [SerializeField]
    private Transform spawnParent;

    [SerializeField]
    private Transform pointPlayer;

    [SerializeField]
    private Transform pointEnemy;

    public void SetLevelData(LevelData _data)
    {
        LvlData = _data;
    }

    public IEnumerator Dialog()
    {
        if (LvlData.dialog != null)
        {
            DialogManager.Instance.StartDialog(LvlData.dialog);
            yield return DialogManager.Instance.Dialoging();
        }
        else
        {
            Debug.Log("Dialog Null");
        }
    }

    #region Win Lose

    public void LoseCheck()
    {
        if (!player.IsDie) { return; }

        Debug.Log("LOSE");
        gameOver(false);
    }

    public void WinCheck()
    {
        if (!allEnemiesDie) { return; }

        Debug.Log("WIN");
        ShopManager.Instance.ProcessReward(LvlData.Rewards);
        gameOver(true);
    }

    private void gameOver(bool isWin)
    {
        isGameOver = true;
        OnGameOver?.Invoke(isWin);
    }

    #endregion Win Lose

    #region Spawn

    public IEnumerator spawnEnemy()
    {
        for (int i = 0; i < LvlData.enemies.Length; i++)
        {
            CharacterData enemyData = LvlData.enemies[i];

            GameObject prefab = enemyData.isBoss ? enemyBossPrefab : enemyPrefab;
            EnemyChar _enemy = Instantiate(prefab, enemyPosGenerated(i), Quaternion.identity, spawnParent).GetComponent<EnemyChar>();

            enemies.Add(_enemy);
            _enemy.SetData(enemyData); // set data

            if (enemyData.isBoss)
            {
                if (enemyBoss == null)
                {
                    enemyBoss = _enemy;
                }
            }
        }

        yield return null;
    }

    private Vector3 enemyPosGenerated(int i)
    {
        Vector3 pos = pointEnemy.position;
        pos.z -= i * 2;
        pos.x -= (i % 2 == 0 ? -1 : 1) * 2;
        return pos;
    }

    public IEnumerator spawnPlayer()
    {
        PlayerChar _player = Instantiate(playerPrefab, pointPlayer.position, Quaternion.identity, spawnParent).GetComponent<PlayerChar>();
        player = _player;

        player.SetData(playerCharData);

        isGameOver = false;

        yield return null;
    }

    public void ClearSpawn()
    {
        foreach (BaseChar baseChar in enemies)
        {
            Destroy(baseChar.gameObject);
        }
        enemies.Clear();

        Destroy(player.gameObject);
        player = null;
    }

    #endregion Spawn

    public List<BaseChar> GetEnemiesInRange(BaseChar target, int range)
    {
        List<BaseChar> x = new List<BaseChar>();

        if (range <= 0)
        {
            x.Add(target);
        }
        else if (range >= 2)
        {
            x.AddRange(enemies);
        }
        else
        {
            x.Add(target);
            BaseChar randomTarget = enemies[Random.Range(0, enemies.Count)];
            x.Add(randomTarget);
            //// cleave
            //int targetIndex = enemies.IndexOf((EnemyChar)target);

            //for (int i = targetIndex - 1; i < targetIndex + 2; i++)
            //{
            //    if (i < 0 || i >= enemies.Count) { continue; }
            //    x.Add(enemies[i]);
            //}
        }

        return x;
    }
}