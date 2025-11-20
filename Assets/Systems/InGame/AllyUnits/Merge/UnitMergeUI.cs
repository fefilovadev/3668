using UnityEngine;
using UnityEngine.UI;

public class UnitMergeUI : MonoBehaviour
{
    [Header("Low Level Stars (1-3)")]
    [SerializeField] private GameObject starContainerLow;
    [SerializeField] private Image lowStarsImage;

    [Header("High Level Stars (4-6)")]
    [SerializeField] private GameObject starContainerHigh;
    [SerializeField] private Image highStarsImage;

    public void UpdateUI(int level)
    {
        starContainerLow.SetActive(false);
        starContainerHigh.SetActive(false);

        if (level >= 1 && level <= 3)
        {
            starContainerLow.SetActive(true);
            lowStarsImage.fillAmount = level / 3f;
        }
        else if (level >= 4 && level <= 6)
        {
            starContainerHigh.SetActive(true);
            highStarsImage.fillAmount = (level - 3) / 3f;
        }
    }
}
