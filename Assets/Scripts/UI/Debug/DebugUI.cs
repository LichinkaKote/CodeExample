using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private TMP_Text fpsText;
    private float fpsTimer;
    private float[] fpsCounts = new float[20];
    private byte index;

    public static DebugUI Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        CalcFPS();
    }
    private void CalcFPS()
    {
        fpsTimer -= Time.unscaledDeltaTime;
        if (fpsTimer <= 0f)
        {
            fpsTimer = 0.05f;

            fpsCounts[index] = 1f / Time.unscaledDeltaTime;
            float fps = 0f;
            for (int i = 0; i < fpsCounts.Length; i++)
            {
                fps += fpsCounts[i];
            }
            fps /= fpsCounts.Length;

            fpsText.text = "FPS:" + Mathf.RoundToInt(fps).ToString();
            index++;
            if (index >= fpsCounts.Length)
                index = 0;
        }

    }

}
