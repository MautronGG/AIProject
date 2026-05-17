using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavedEditorLevels : MonoBehaviour
{
    [Header("Data")]
    [Tooltip("Contains the file paths of all saved levels.")]
    public List<string> savedLevelPaths = new List<string>();

    [Header("UI Pages")]
    [Tooltip("Assign the 5 GameObjects that CONTAIN your 12 buttons (e.g. your Canvases or Grid Layout Groups).")]
    public List<GameObject> pageContainers = new List<GameObject>();

    [Header("Navigation")]
    [Tooltip("Assign the 'Previous' buttons for each page container (must match the order of pageContainers).")]
    public List<Button> nextButtons = new List<Button>();
    [Tooltip("Assign the 'Next' buttons for each page container (must match the order of pageContainers).")]
    public List<Button> prevButtons = new List<Button>();

    /// <summary>
    /// Searches for all JSON files in the persistent data path, populates the level buttons,
    /// and sets up pagination.
    /// </summary>
    public void RefreshLevels()
    {
        Time.timeScale = 1f;

        MenuCamera menuCamera = FindObjectOfType<MenuCamera>();

        if (menuCamera != null)
        {
            // Automatically hook up the Next and Previous buttons to the MenuCamera
            //for (int i = 0; i < prevButtons.Count; i++)
            //{
            //    if (prevButtons[i] != null)
            //    {
            //        prevButtons[i].onClick.RemoveAllListeners();
            //        // Swapped to match your Inspector setup
            //        prevButtons[i].onClick.AddListener(() =>
            //        {
            //            menuCamera.PreviousPage();
            //        });
            //    }
            //}
            //
            //for (int i = 0; i < nextButtons.Count; i++)
            //{
            //    if (nextButtons[i] != null)
            //    {
            //        nextButtons[i].onClick.RemoveAllListeners();
            //        // Swapped to match your Inspector setup
            //        nextButtons[i].onClick.AddListener(() =>
            //        {
            //            menuCamera.NextPage();
            //        });
            //    }
            //}

        }
        else
        {
            Debug.LogWarning("MenuCamera not found! Cannot hook up Next/Previous buttons.");
        }
        savedLevelPaths.Clear();

        // 1. Get the path where levels are saved
        string saveDirectory = Application.persistentDataPath;

        // 2. Find all json files in that directory and add them to the list
        if (Directory.Exists(saveDirectory))
        {
            string[] files = Directory.GetFiles(saveDirectory, "*.json");
            savedLevelPaths.AddRange(files);
        }

        int totalLevels = savedLevelPaths.Count;

        // Calculate how many pages are actually needed (minimum 1)

        int totalAvailablePages = Mathf.CeilToInt((float)totalLevels / 12f);
        if (totalAvailablePages == 0) totalAvailablePages = 1;

        // Cap totalAvailablePages to the number of containers we actually have

        totalAvailablePages = Mathf.Min(totalAvailablePages, pageContainers.Count);

        int currentLevelIndex = 0;

        // 3. Loop through all canvases/containers to populate them
        for (int i = 0; i < pageContainers.Count; i++)
        {
            GameObject containerObj = pageContainers[i];
            if (containerObj == null) continue;

            // Disable containers that are beyond the available pages
            if (i >= totalAvailablePages)
            {
                containerObj.SetActive(false);
                continue;
            }
            else
            {
                containerObj.SetActive(true);
            }

            // Automatically find all Button components inside this canvas/container (even if they are currently turned off)
            Button[] buttonsInContainer = containerObj.GetComponentsInChildren<Button>(true);

            int buttonsProcessed = 0;

            // Configure each button we found in this container
            for (int b = 0; b < buttonsInContainer.Length; b++)
            {
                Button btnComponent = buttonsInContainer[b];

                // Skip navigation buttons by name so they don't get overwritten
                string btnName = btnComponent.gameObject.name.ToLower();
                if (btnName.Contains("next") || btnName.Contains("prev") || btnName.Contains("arrow"))
                {
                    continue;
                }

                // Stop after we've configured 12 level buttons for this specific page
                if (buttonsProcessed >= 12) break;

                GameObject btnObj = btnComponent.gameObject;

                // If we still have saved levels to display
                if (currentLevelIndex < totalLevels)
                {
                    btnObj.SetActive(true);

                    // Read the JSON file to get the level name
                    string jsonContent = File.ReadAllText(savedLevelPaths[currentLevelIndex]);
                    string levelName = "Unknown Level";

                    try
                    {
                        LevelData data = JsonUtility.FromJson<LevelData>(jsonContent);
                        if (data != null && !string.IsNullOrEmpty(data.levelName))
                        {
                            levelName = data.levelName;
                        }
                        else
                        {
                            levelName = Path.GetFileNameWithoutExtension(savedLevelPaths[currentLevelIndex]);
                        }
                    }
                    catch
                    {
                        levelName = Path.GetFileNameWithoutExtension(savedLevelPaths[currentLevelIndex]);
                    }

                    // Update TextMeshPro or standard Text
                    TextMeshProUGUI tmpText = btnObj.GetComponentInChildren<TextMeshProUGUI>();
                    if (tmpText != null)
                    {
                        tmpText.text = levelName;
                    }
                    else
                    {
                        Text legacyText = btnObj.GetComponentInChildren<Text>();
                        if (legacyText != null)
                        {
                            legacyText.text = levelName;
                        }
                    }
                    btnObj.GetComponent<LoadLevelInEditor>().jsonFileNameWithoutExt = levelName;
                    currentLevelIndex++;
                }
                else
                {
                    // No more saved levels to display, turn OFF this button
                    btnObj.SetActive(false);
                }

                buttonsProcessed++;
            }
        }

        // Disable buttons if there's only 1 page
        for (int i = 0; i < pageContainers.Count; i++)
        {
            if (i < prevButtons.Count && prevButtons[i] != null)
                prevButtons[i].interactable = (totalAvailablePages > 1);

            if (i < nextButtons.Count && nextButtons[i] != null)
                nextButtons[i].interactable = (totalAvailablePages > 1);
        }

        // Update MenuCamera with only the active pages to prevent scrolling to empty pages
        MenuCamera cam = FindObjectOfType<MenuCamera>();
        if (cam != null)
        {
            List<GameObject> activePages = new List<GameObject>();
            for (int i = 0; i < totalAvailablePages; i++)
            {
                activePages.Add(pageContainers[i]);
            }
            cam.SetActiveLevelEditorPages(activePages);
        }

        Debug.Log($"Found {totalLevels} saved levels. Configured buttons across {pageContainers.Count} pages. Available pages: {totalAvailablePages}");
    }
}
