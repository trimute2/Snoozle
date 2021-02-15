using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class abilities : MonoBehaviour
{
    public GameObject mainCamera;
    private BattleManager battleManager;
    public Image abilityImage1;
    public float cooldown = 20;
    public float enemySlowTime = 10;
    bool isCooldown = false;
    public KeyCode ability1;
    public GameObject timeFog;

    // Start is called before the first frame update
    void Start()
    {
        abilityImage1.fillAmount = 0;
        battleManager = mainCamera.GetComponent<BattleManager>();
        timeFog.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Ability1();
    }

    void Ability1()
    {
        if(Input.GetKey(ability1) && isCooldown == false)
        {
            isCooldown = true;
            abilityImage1.fillAmount = 1;
            battleManager.timeMultiplier = 0.2f;
            timeFog.SetActive(true);
        }

        if (isCooldown)
        {
            abilityImage1.fillAmount -= 1 / cooldown * Time.deltaTime;

            if (abilityImage1.fillAmount <= 0.65)
            {
                battleManager.timeMultiplier = 1.0f;
                timeFog.SetActive(false);

            }

            if (abilityImage1.fillAmount <= 0)
            {
                abilityImage1.fillAmount = 0;
                isCooldown = false;
            }
        }
    }
}
