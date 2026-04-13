using UnityEngine;
using System.Collections.Generic;
public class MobRushManager : MonoBehaviour
{
    #region Variables
    [Header("Control Variables")]
    [SerializeField] private int maxCrows = 10;
    [SerializeField] private int maxDogs = 10;
    [SerializeField] private int maxCrowsAtOnce = 2;
    [SerializeField] private int maxDogsAtOnce = 2;
    [SerializeField] private float cooldown = 5f;

    [Header("Object References")]
    [SerializeField] private GameObject crow;
    [SerializeField] private GameObject dog;
    [SerializeField] private Transform dogSpawnPointOne;
    [SerializeField] private Transform dogSpawnPointTwo;
    [SerializeField] private Transform crowSpawnPointOne;
    [SerializeField] private Transform crowSpawnPointTwo;
    [SerializeField] private GameObject blaster;

    private GameManager gameManager;
    private BossStateMachine boss;
    private PlayerStateMachine player;

    private List<DogStateMachine> dogs;
    private List<CrowStateMachine> crows;
    private int numDogs;
    private int numCrows;
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        boss = GameObject.Find("HUE").GetComponent<BossStateMachine>();
        boss.gameObject.SetActive(false);
        player = GameObject.Find("Player").GetComponent<PlayerStateMachine>();
        boss.BossDeath += OnBossDeath;
        dogs = new();
        crows = new();
        numDogs = 0;
        numCrows = 0;
    }

    public void BeginMobRush()
    {
        gameManager.FightStarted = false;
        boss.gameObject.SetActive(false);
        for (int i = 0; i < maxDogsAtOnce; i++)
        {
            SpawnDog();
        }
        for (int i = 0; i < maxCrowsAtOnce; i++)
        {
            SpawnCrow();
        }
    }

    public void TriggerHUE()
    {
        boss.gameObject.SetActive(true);
        gameManager.FightStarted = true;
    }
    public void FinishFight()
    {
        Debug.Log("ending");
        gameManager.FightStarted = false;
        boss.JumpToState(new BossStartState(boss));
    }

    public void SpawnDog()
    {
        float randomChance = Random.Range(0f, 1f);
        GameObject dogInstance;
        if (randomChance <= 0.5f)
        {
            dogInstance = Instantiate(dog, dogSpawnPointOne.position, Quaternion.identity);
        } else
        {
            dogInstance = Instantiate(dog, dogSpawnPointTwo.position, Quaternion.identity);
        }
        dogs.Add(dogInstance.GetComponent<DogStateMachine>());
        dogs[dogs.Count - 1].DogDeath += OnDogDeath;
        dogs[dogs.Count - 1].Attack();
    }

    public void SpawnCrow()
    {
        float randomChance = Random.Range(0f, 1f);
        GameObject crowIsntance;
        if (randomChance <= 0.5f)
        {
            crowIsntance = Instantiate(crow, crowSpawnPointOne.position, Quaternion.identity);
        } else
        {
            crowIsntance = Instantiate(crow, crowSpawnPointTwo.position, Quaternion.identity);
        }
        crows.Add(crowIsntance.GetComponent<CrowStateMachine>());
        crows[crows.Count - 1].CrowDeath += OnCrowDeath;
        crows[crows.Count - 1].Attack();
    }

    public void OnDogDeath(DogStateMachine dogInstance)
    {
        numDogs += 1;
        dogs.Remove(dogInstance);
        if (numDogs < maxDogs)
        {
            StartCoroutine(CooldownDog());
        } else if (numCrows >= maxCrows)
        {
            TriggerHUE();
        }
    }

    public void OnCrowDeath(CrowStateMachine crowInstance)
    {
        numCrows += 1;
        crows.Remove(crowInstance);
        if (numCrows < maxCrows)
        {
            StartCoroutine(CooldownCrow());
        } else if (numDogs >= maxDogs)
        {
            TriggerHUE();
        }
    }

    public void OnBossDeath()
    {
        FinishFight();
    }

    public void KillHUE()
    {
        boss.gameObject.SetActive(false);
        player.OnEnable();
        blaster.SetActive(true);
    }

    System.Collections.IEnumerator CooldownDog()
    {
        yield return new WaitForSecondsRealtime(cooldown);
        SpawnDog();
    }

    System.Collections.IEnumerator CooldownCrow()
    {
        yield return new WaitForSecondsRealtime(cooldown);
        SpawnCrow();
    }

}