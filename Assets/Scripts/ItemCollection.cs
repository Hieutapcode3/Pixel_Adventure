using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollection : MonoBehaviour
{
    [SerializeField] private AudioSource itemCollect;
    [SerializeField] private Text cherriesText;
    [SerializeField] private Text strawberriesText;
    [SerializeField] private Text bananasText;
    [SerializeField] private Text melonsText;

    [SerializeField] private int totalCherries;
    [SerializeField] private int totalStrawberries;
    [SerializeField] private int totalBananas;
    [SerializeField] private int totalMelons;

    private int cherriesLeft = 0;
    private int strawberriesLeft = 0;
    private int bananasLeft = 0;
    private int melonsLeft = 0;

    private void Start()
    {
        UpdateUI();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            itemCollect.Play();
            collision.GetComponent<Animator>().SetTrigger("Collision");
           CollectItem(collision.gameObject);
        }
    }

    private void CollectItem(GameObject item)
    {
        string itemName = item.name.ToLower();

        if (itemName.Contains("cherry"))
        {
            cherriesLeft++;
        }
        else if (itemName.Contains("strawberry"))
        {
            strawberriesLeft++;
        }
        else if (itemName.Contains("banana"))
        {
            bananasLeft++;
        }
        else if (itemName.Contains("melon"))
        {
            melonsLeft++;
        }

        Destroy(item);
        UpdateUI();
    }

    private void UpdateUI()
    {
        cherriesText.text =  ":" + cherriesLeft + "/" + totalCherries;
        strawberriesText.text = ":" + strawberriesLeft + "/" + totalStrawberries;
        bananasText.text = ":" + bananasLeft + "/" + totalBananas;
        melonsText.text = ":" + melonsLeft + "/" + totalMelons;
    }
}
