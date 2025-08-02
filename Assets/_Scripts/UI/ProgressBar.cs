using UnityEngine;
using UnityEngine.UI;

public partial class ProgressBar : MonoBehaviour
{
    [SerializeField]
    private Image FillImage = null;

    public void OnEnable()
    {
        Debug.Log("ProgressBar enabled");
        if (FillImage != null)
        {
            Debug.Log("FillImage is not null, setting fill amount to 0");
            FillImage.fillAmount = 0f;
        }
    }

    public void SetRatio(float current, float max)
    {
        float pct = (max > 0f)
            ? Mathf.Clamp01(current / max)
            : 0f;

        if (FillImage != null)
        {
            FillImage.fillAmount = pct;
        }
    }
}