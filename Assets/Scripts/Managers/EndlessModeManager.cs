using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
public class EndlessModeManager : MonoBehaviour
{
    #region Variables
    [Header("Control Variables")]
    [SerializeField] private int maxCrowsAtOnce = 2;
    [SerializeField] private int maxDogsAtOnce = 2;
    [SerializeField] private float cooldown = 5f;

    [Header("Object References")]
    [SerializeField] private GameObject crow;
    [SerializeField] private GameObject dog;
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private Transform dogSpawnPointOne;
    [SerializeField] private Transform dogSpawnPointTwo;
    [SerializeField] private Transform crowSpawnPointOne;
    [SerializeField] private Transform crowSpawnPointTwo;

    private GameManager gameManager;
    private CutsceneManager cutsceneManager;
    private PlayerStateMachine player;
    private TestEvaStateMachine eva;

    private List<DogStateMachine> dogs;
    private List<CrowStateMachine> crows;
    private int numDogs;
    private int numCrows;

    private float timePassed;
    private bool started;
    public Action<StateMachine> AddedEnemy;
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cutsceneManager = GameObject.Find("CutsceneManager").GetComponent<CutsceneManager>();
        player = GameObject.Find("Player").GetComponent<PlayerStateMachine>();
        player.SetHealth(player.MaxHealth);
        player.Energy = player.MaxEnergy;
        eva = GameObject.Find("Eva_Separate").GetComponent<TestEvaStateMachine>();
        eva.EvaDeath += OnEvaDeath;
        dogs = new();
        crows = new();
        numDogs = 0;
        numCrows = 0;
        timePassed = 0f;
        started = false;
    }

    void Update()
    {
        if (started && !gameManager.GameOver)
        {
            timePassed += Time.deltaTime;
            int minutes = Mathf.FloorToInt(timePassed / 60f);
            int seconds = Mathf.FloorToInt(timePassed % 60f);
            timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void OnEvaDeath(TestEvaStateMachine tesm)
    {
        gameManager.GameOver = true;
        cutsceneManager.PlayCutScene(0);
    }

    public void BeginMobRush()
    {
        started = true;
        for (int i = 0; i < maxDogsAtOnce; i++)
        {
            SpawnDog();
        }
        for (int i = 0; i < maxCrowsAtOnce; i++)
        {
            SpawnCrow();
        }
    }

    public void SpawnDog()
    {
        float randomChance = UnityEngine.Random.Range(0f, 1f);
        GameObject dogInstance;
        if (randomChance <= 0.5f)
        {
            dogInstance = Instantiate(dog, dogSpawnPointOne.position, Quaternion.identity);
        } else
        {
            dogInstance = Instantiate(dog, dogSpawnPointTwo.position, Quaternion.identity);
        }
        dogs.Add(dogInstance.GetComponent<DogStateMachine>());
        AddedEnemy?.Invoke(dogs[dogs.Count - 1]);
        dogs[dogs.Count - 1].DogDeath += OnDogDeath;
        dogs[dogs.Count - 1].Attack();
        
    }

    public void SpawnCrow()
    {
        float randomChance = UnityEngine.Random.Range(0f, 1f);
        GameObject crowIsntance;
        if (randomChance <= 0.5f)
        {
            crowIsntance = Instantiate(crow, crowSpawnPointOne.position, Quaternion.identity);
        } else
        {
            crowIsntance = Instantiate(crow, crowSpawnPointTwo.position, Quaternion.identity);
        }
        crows.Add(crowIsntance.GetComponent<CrowStateMachine>());
        AddedEnemy?.Invoke(crows[crows.Count - 1]);
        crows[crows.Count - 1].CrowDeath += OnCrowDeath;
        crows[crows.Count - 1].Attack();
        
    }

    public void OnDogDeath(DogStateMachine dogInstance)
    {
        numDogs += 1;
        dogs.Remove(dogInstance);
        StartCoroutine(CooldownDog());
    }

    public void OnCrowDeath(CrowStateMachine crowInstance)
    {
        numCrows += 1;
        crows.Remove(crowInstance);
        StartCoroutine(CooldownCrow());
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