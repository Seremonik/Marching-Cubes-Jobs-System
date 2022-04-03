using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CoherentNoise.Generation.Fractal;
using CoherentNoise.Texturing;
using CoherentNoise;

public class NoiseVisualizer : MonoBehaviour
{
    public static NoiseVisualizer Instance;

    public static int Octaves => Instance.octaves;
    public static float Frequency => Instance.frequency;
    public static float Bias => Instance.bias;
    public static float Gain => Instance.gain;
    public static FractalNoiseBase Noise => Instance.noise;

    public RawImage image;
    public UISeed ui_seed;

    private int octaves = 4;
    private float frequency = 0.2f;
    private float bias;
    private float gain;
    private float lacunarity;
    private float persistance;
    private FractalNoiseBase noise;

    [Space(10)]
    [Header("UI")]
    
    [SerializeField]
    private Text frequencyText;

    [SerializeField]
    private Text gainText;

    [SerializeField]
    private Text biasText;

    [SerializeField]
    private Text lacunarityText;

    [SerializeField]
    private Text persistanceText;

    void Awake()
    {
        Instance = this;
        ui_seed.OnChange += Recalculate;
    }

    public void Recalculate()
    {
        noise = new PinkNoise(UIManager.Instance.GetSeed());
        noise.Frequency = frequency * 16f;
        noise.Lacunarity = lacunarity;
        ((PinkNoise) noise).Persistence = persistance;
        noise.OctaveCount = octaves;

        image.texture = TextureMaker.Make(64, 64, (x, y) =>
            new Color((noise.Bias(bias).Gain(gain).GetValue(x, y, 0) + 1) / 2,
                (noise.Bias(bias).Gain(gain).GetValue(x, y, 0) + 1) / 2,
                (noise.Bias(bias).Gain(gain).GetValue(x, y, 0) + 1) / 2));

        noise.Frequency = frequency;
    }

    #region UI

    public void OnOctaveChange(string value)
    {
        int.TryParse(value, out octaves);
        Recalculate();
    }

    public void OnFrequencyChange(float value)
    {
        frequency = value;
        frequencyText.text = ("Frequency: " + ((int) (frequency * 100)) / 100f);
        Recalculate();
    }

    public void OnGainChange(float value)
    {
        gain = value;
        gainText.text = "Gain: " + ((int) (value * 100)) / 100f;
        Recalculate();
    }

    public void OnBiasChange(float value)
    {
        bias = value;
        biasText.text = "Bias: " + ((int) (value * 100)) / 100f;
        Recalculate();
    }

    public void OnPersistanceChange(float value)
    {
        persistance = value;
        persistanceText.text = "Persistance: " + ((int) (value * 100)) / 100f;
        Recalculate();
    }

    public void OnLacunarityChange(float value)
    {
        lacunarity = value;
        lacunarityText.text = "Lacunarity: " + ((int) (value * 100)) / 100f;
        Recalculate();
    }

    #endregion
}