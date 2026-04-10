using UnityEngine;
using System.Collections.Generic;
public class MobRushManager : MonoBehaviour
{
    #region Variables
    [Header("Control Variables")]
    [SerializeField] private int numStages = 3;
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
    [SerializeField] private BoxCollider2D leftBound;
    [SerializeField] private BoxCollider2D rightBound;
    private GameManager gameManager;
    private List<DogStateMachine> dogs;
    private int numDogs;
    private int numCrows;
    #endregion

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        dogs = new();
        numDogs = 0;
        numCrows = 0;
    }

    public void BeginStageOne()
    {
        Debug.Log("starting stage one");
        leftBound.enabled = true;
        rightBound.enabled = true;
        for (int i = 0; i < maxDogs; i++)
        {
            SpawnDog();
        }
    }
    public void FinishFight()
    {
        Debug.Log("ending");
        leftBound.enabled = false;
        rightBound.enabled = false;
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

    public void OnDogDeath(DogStateMachine dogInstance)
    {
        numDogs += 1;
        dogs.Remove(dogInstance);
        if (numDogs <= maxDogs)
        {
            StartCoroutine(Cooldown());
        } else if (numCrows > maxCrows)
        {
            FinishFight();
        }
    }

    System.Collections.IEnumerator Cooldown()
    {
        yield return new WaitForSecondsRealtime(cooldown);
        SpawnDog();
    }

}