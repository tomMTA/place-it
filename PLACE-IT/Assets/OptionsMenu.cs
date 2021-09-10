using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
	Resolution[] resolutions;
	public TMP_Dropdown resolutionsDropdown;
	public Toggle FullscreenToggle;

	private void Start()
	{
		FullscreenToggle.isOn = Screen.fullScreen;
		//FullscreenToggle.RefreshShownValue();
		resolutions = Screen.resolutions;
		resolutionsDropdown.ClearOptions();

		List<string> newResolutions = new List<string>();
		int currentResolutionIndex = 0;
		for(int i = 0; i < resolutions.Length; i++)
		{
			newResolutions.Add(resolutions[i].width + " x " + resolutions[i].height);
			if(resolutions[i].width == Screen.currentResolution.width &&
				resolutions[i].height== Screen.currentResolution.height)
			{
				currentResolutionIndex = i;
			}
		}
		resolutionsDropdown.AddOptions(newResolutions);
		resolutionsDropdown.value = currentResolutionIndex;
		resolutionsDropdown.RefreshShownValue();
	}

	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	} 

	// Start is called before the first frame update
	public void SetQuality(int qualityIndex)
	{
		QualitySettings.SetQualityLevel(qualityIndex);
	}

	public void SetFullscreen(bool isFullscreen)
	{
		Screen.fullScreen = isFullscreen;
	}
}
