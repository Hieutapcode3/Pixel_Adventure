using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollection : MonoBehaviour
{
    private int cherries = 0;
    [SerializeField] private Text cherriesText;
    [SerializeField] private AudioSource itemCollect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            itemCollect.Play();
            collision.GetComponent<Animator>().SetTrigger("Collision");
            StartCoroutine(CollectItem(collision.gameObject));
        }
    }

    private IEnumerator CollectItem(GameObject item)
    {
        yield return new WaitForSeconds(0.5f); 
        Destroy(item);
        cherries++;
        cherriesText.text = "cherries: " + cherries;
    }
}
