using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;

public class LevelUnitUI : MonoBehaviour
{
    [SerializeField] private Image levelFrame;
    [SerializeField] private Image levelImage;
    [SerializeField] private Image starsImage;
    [SerializeField] private Image lockImage;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Material closedStateMaterial;

    private Color topColor;
    private Color bottomColor;
    public void InitLevelUI(Sprite _levelImage, int _levelId, ELevelStates leveState)
    {
        levelImage.sprite = _levelImage;
        levelText.text = "LEVEL " + _levelId;
        topColor = levelText.colorGradient.topLeft;
        bottomColor = levelText.colorGradient.bottomLeft;
        UpdateLevelUI(leveState);
    }
    public void UpdateLevelUI(ELevelStates state)
    {
        switch (state)
        {
            case ELevelStates.Closed:
                SetUI(0, true);
                break;
            case ELevelStates.Opened:
                SetUI(0, false);
                break;
            case ELevelStates.OneStar:
                SetUI(1, false);
                break;
            case ELevelStates.TwoStars:
                SetUI(2, false);
                break;
            case ELevelStates.ThreeStars:
                SetUI(3, false);
                break;
        }
    }

    private void SetUI(int starsCount, bool isLocked)
    {
        if (isLocked)
        {
            lockImage.gameObject.SetActive(true);
            SetMaterials(closedStateMaterial);
            starsImage.gameObject.SetActive(false);

            VertexGradient newVG = new VertexGradient(Color.grey, Color.grey, Color.grey, Color.grey);
            levelText.colorGradient = newVG;
            return;
        }
        else
        {
            lockImage.gameObject.SetActive(false);
            VertexGradient newVG = new VertexGradient(topColor, topColor, bottomColor, bottomColor);
            levelText.colorGradient = newVG;
            SetMaterials(null);
        }
        if (starsCount == 0) starsImage.gameObject.SetActive(false);
        else if (starsCount > 0)
        {
            starsImage.gameObject.SetActive(true);
            float fill = starsCount / 3f;
            starsImage.fillAmount = fill;
        }
    }

    private void SetMaterials(Material material)
    {
        levelFrame.material = material;
        levelImage.material = material;
    }
}
