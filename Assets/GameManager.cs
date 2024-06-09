using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private Dictionary<string, int> counters = new Dictionary<string, int>
    {
        {"Eggs", 0},
        {"Chick", 0},
        {"Hen", 0},
        {"Rooster", 0},
        {"TotalChick", 0},
        {"TotalHen", 0},
        {"TotalRooster", 0}
    };

    public TextMeshProUGUI eggsText;
    public TextMeshProUGUI chicksText;
    public TextMeshProUGUI hensText;
    public TextMeshProUGUI roostersText;
    public TextMeshProUGUI totalChicksText;
    public TextMeshProUGUI totalHensText;
    public TextMeshProUGUI totalRoostersText;

    public GameObject eggPrefab;
    public GameObject chickPrefab;
    public GameObject henPrefab;
    public GameObject roosterPrefab;

    private bool isFirstEgg = true;

    void Start()
    {
        // Start the game with one egg
        StartCoroutine(HandleEggHatching());
    }

    IEnumerator HandleEggHatching()
    {
        counters["Eggs"] += 1;
        UpdateUI();
        GameObject egg = Instantiate(eggPrefab, GetRandomPositionAboveGround(), Quaternion.identity);
        yield return new WaitForSeconds(10);

        counters["Eggs"] -= 1;
        counters["Chick"] += 1;
        counters["TotalChick"] += 1;
        UpdateUI();
        Destroy(egg);
        GameObject chick = Instantiate(chickPrefab, GetRandomPositionAboveGround(), Quaternion.identity);
        chick.tag = "Chick";

        // The first chick matures into a hen
        yield return new WaitForSeconds(10);

        counters["Chick"] -= 1;
        if (isFirstEgg)
        {
            isFirstEgg = false;
            counters["Hen"] += 1;
            counters["TotalHen"] += 1;
            UpdateUI();
            Destroy(chick);
            GameObject hen = Instantiate(henPrefab, GetRandomPositionAboveGround(), Quaternion.identity);
            hen.tag = "Hen";

            // Start the lifecycle of the hen
            StartCoroutine(HandleHenLifecycle(hen));
        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                counters["Hen"] += 1;
                counters["TotalHen"] += 1;
                UpdateUI();
                Destroy(chick);
                GameObject hen = Instantiate(henPrefab, GetRandomPositionAboveGround(), Quaternion.identity);
                hen.tag = "Hen";

                // Start the lifecycle of the hen
                StartCoroutine(HandleHenLifecycle(hen));
            }
            else
            {
                counters["Rooster"] += 1;
                counters["TotalRooster"] += 1;
                UpdateUI();
                Destroy(chick);
                GameObject rooster = Instantiate(roosterPrefab, GetRandomPositionAboveGround(), Quaternion.identity);
                rooster.tag = "Rooster";

                // Start the lifecycle of the rooster
                StartCoroutine(HandleRoosterLifecycle(rooster));
            }
        }
    }

    IEnumerator HandleHenLifecycle(GameObject hen)
    {
        // Hen lays eggs after 30 seconds
        yield return new WaitForSeconds(30);

        int eggsLaid = Random.Range(2, 11);
        counters["Eggs"] += eggsLaid;
        UpdateUI();

        for (int i = 0; i < eggsLaid; i++)
        {
            GameObject egg = Instantiate(eggPrefab, GetRandomPositionAboveGround(), Quaternion.identity);
            StartCoroutine(HandleChickLifecycle(egg));
        }

        // Hen perishes immediately after laying eggs
        counters["Hen"] -= 1;
        UpdateUI();
        Destroy(hen);
    }

    IEnumerator HandleChickLifecycle(GameObject egg)
    {
        yield return new WaitForSeconds(10);

        counters["Eggs"] -= 1;
        counters["Chick"] += 1;
        counters["TotalChick"] += 1;
        UpdateUI();
        Destroy(egg);
        GameObject chick = Instantiate(chickPrefab, GetRandomPositionAboveGround(), Quaternion.identity);
        chick.tag = "Chick";

        yield return new WaitForSeconds(10);

        counters["Chick"] -= 1;
        if (Random.Range(0, 2) == 0)
        {
            counters["Hen"] += 1;
            counters["TotalHen"] += 1;
            UpdateUI();
            Destroy(chick);
            GameObject hen = Instantiate(henPrefab, GetRandomPositionAboveGround(), Quaternion.identity);
            hen.tag = "Hen";
            StartCoroutine(HandleHenLifecycle(hen));
        }
        else
        {
            counters["Rooster"] += 1;
            counters["TotalRooster"] += 1;
            UpdateUI();
            Destroy(chick);
            GameObject rooster = Instantiate(roosterPrefab, GetRandomPositionAboveGround(), Quaternion.identity);
            rooster.tag = "Rooster";
            StartCoroutine(HandleRoosterLifecycle(rooster));
        }
    }

    IEnumerator HandleRoosterLifecycle(GameObject rooster)
    {
        yield return new WaitForSeconds(40);
        counters["Rooster"] -= 1;
        UpdateUI();
        Destroy(rooster);
    }

    void UpdateUI()
    {
        eggsText.text = "Eggs: " + counters["Eggs"];
        chicksText.text = "Chicks: " + counters["Chick"];
        hensText.text = "Hens: " + counters["Hen"];
        roostersText.text = "Roosters: " + counters["Rooster"];
        totalChicksText.text = "Total Chicks: " + counters["TotalChick"];
        totalHensText.text = "Total Hens: " + counters["TotalHen"];
        totalRoostersText.text = "Total Roosters: " + counters["TotalRooster"];
    }

    Vector3 GetRandomPositionAboveGround()
    {
        return new Vector3(Random.Range(-4.5f, 4.5f), 0.5f, Random.Range(-4.5f, 4.5f));
    }
}
