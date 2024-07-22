using UnityEngine;
using UnityEngine.UI;

public class BtnManager : MonoBehaviour
{
    public Button nextButton;
    public Button previousButton;

    private void Start()
    {
        nextButton.onClick.AddListener(() => SkinManager.Instance.NextSkin());
        previousButton.onClick.AddListener(() => SkinManager.Instance.PreviousSkin());
    }
}
