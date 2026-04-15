using UnityEngine;
using UnityEngine.UI;
using System.Collections;
    using TMPro;
public class AbilityUIManager : MonoBehaviour
{


    [SerializeField] private GameObject abilityPickup;
    public Image iconUI;

    public TMP_Text abilityName;
    public TMP_Text abilityDescription;
    private Vector3 originalScale;

    //sprites to drag in
    public GameObject dashAnimation;
    public GameObject shootAnimation;
    public Sprite dashIconSprite;
    public Sprite shootIconSprite;
    public TMP_Text abilityUnlockPopup;
    public CanvasGroup mainCanvas;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {



    }

    public void PlayAbilityUnlock(int abilityNum)
    {
        string name = "";
        string description = "";
        Sprite iconSprite = null;
        GameObject abilityAnimation = null;

        if (abilityNum == 3)
        {
            name = "Glitch Dash";
            description = "Press SHIFT to phase through objects and opponents, destroying everything in your path";
            iconSprite = dashIconSprite;
            abilityAnimation = dashAnimation;
        }
        else if (abilityNum == 2)
        {
            name = "Arm Cannon";
            description = "Point with your mouse and click LMB to shoot an explosive projectile!";
            iconSprite = shootIconSprite;
            abilityAnimation = shootAnimation;
        }

        iconUI.sprite = iconSprite;

        StartCoroutine(PlayUnlockRoutine(name, description, abilityAnimation , iconUI));

    }
    private IEnumerator PlayUnlockRoutine(string name, string description, GameObject anim, Image icon)
    {
        mainCanvas.alpha = 1f;
        mainCanvas.blocksRaycasts = true;
        mainCanvas.interactable = true;
        Time.timeScale = 0f;


        anim.SetActive(true);

        CanvasGroup cg = anim.GetComponent<CanvasGroup>();
        if (cg== null) cg = anim.AddComponent<CanvasGroup>();

        abilityName.text = name;
        abilityDescription.text = description;

        abilityUnlockPopup.gameObject.SetActive(true);
        Color popupColor = abilityUnlockPopup.color;
        popupColor.a = 1f;
        abilityUnlockPopup.color = popupColor;


        yield return new WaitForSecondsRealtime(2.0f);


        float fadeDuration = 0.5f;
        float currentTime = 0f;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.unscaledDeltaTime;
            float t = currentTime / fadeDuration;
            cg.alpha = Mathf.Lerp(1f, 0f, t);
            Color c = abilityUnlockPopup.color;
            c.a = Mathf.Lerp(1f, 0f, t);
            abilityUnlockPopup.color = c;

            yield return null;
        }
        anim.SetActive(false);
        abilityUnlockPopup.gameObject.SetActive(false);


        yield return new WaitUntil(() => Input.anyKeyDown);
        abilityPickup.SetActive(false);
        mainCanvas.alpha = 0;
        Time.timeScale = 1f;


    }
    // Update is called once per frame
    void Update()
    {

    }
}
