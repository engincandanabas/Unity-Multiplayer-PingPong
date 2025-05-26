using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public static SettingsUI Instance { get; private set; }

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Button exitButton;
    [SerializeField] private CanvasGroup[] canvasGroups;
    [SerializeField] private TMP_Dropdown resDropdown;

    [Header("Animation Settings")]
    [SerializeField] private Ease ease;
    [SerializeField] private float animDuration;
    [SerializeField] private Transform childObject;

    Resolution[] resolutions;
    private List<Resolution> filteredResolutions;
    private void Awake()
    {
        Instance = this;

        SetResolution();

        HideWithoutAnim();
        exitButton.onClick.AddListener(Hide);
    }
    private void SetResolution()
    {
        resolutions = Screen.resolutions;
        filteredResolutions = resolutions
            .Where(r => Mathf.Approximately((float)r.width / r.height, 16f / 9f))
            .GroupBy(r => new Vector2Int(r.width, r.height)) // Ayný çözünürlükte olanlarý grupla
            .Select(g => g.OrderByDescending(r => r.refreshRate).First()) // En yüksek Hz olaný seç
            .OrderByDescending(r => r.width * r.height) // Opsiyonel: çözünürlükleri büyükten küçüðe sýrala
            .ToList();

        foreach (var res in filteredResolutions)
        {
            Debug.Log($"Resolution: {res.width}x{res.height} @ {res.refreshRate}Hz");
        }
        resDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResIndex = 0;

        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            
            string option = filteredResolutions[i].width+" x " + filteredResolutions[i].height;
            options.Add(option);

            if (filteredResolutions[i].width == Screen.currentResolution.width &&
                filteredResolutions[i].height== Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resDropdown.AddOptions(options);
        resDropdown.value = currentResIndex;
        resDropdown.RefreshShownValue();
        SetResolution(currentResIndex);
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,resolution.height,Screen.fullScreen);
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;   
    }

    public void Hide()
    {
        childObject.transform.DOScale(Vector3.zero, 0.2f).SetEase(ease).OnComplete(() =>
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
            foreach (var group in canvasGroups)
            {
                group.alpha = 1f;
            }
        });

    }
    public void HideWithoutAnim()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
    public void Show()
    {
        foreach (var group in canvasGroups)
        {
            group.alpha = 0f;
        }
        if (!this.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            childObject.transform.localScale = Vector3.zero;
            this.transform.GetChild(0).gameObject.SetActive(true);
            childObject.transform.DOScale(Vector3.one, 0.2f).SetEase(ease).SetDelay(0.25f);
        }
        else
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
        }

    }
}
