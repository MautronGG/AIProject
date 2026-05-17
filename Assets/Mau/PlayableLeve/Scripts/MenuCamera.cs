using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuType
{
    LevelSelector,
    LevelEditor,
    Customization
}

public class MenuCamera : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] RectTransform mainMenu;
    [SerializeField] float menuExitDistance = 600f;
    [SerializeField] float menuExitDuration = 0.4f;

    [Header("Global")]
    [SerializeField] List<GameObject> pagesList;
    [SerializeField] GameObject menuRoot;
    [SerializeField] Transform pagesRoot;
    [SerializeField] float pageSpacing = 600f;
    [SerializeField] int currentPageIndex = 0;


    [Header("Level Selector")]
    [SerializeField] List<GameObject> LS_pagesList;
    [SerializeField] GameObject LS_levelSelectorRoot;
    [SerializeField] Transform LS_pagesRoot;
    [SerializeField] float LS_pageSpacing = 600f;
    [SerializeField] int LS_currentPageIndex = 0;
    [SerializeField] public Vector3 LS_offscreenOffset = new Vector3(0, 1500f, 0);

    [Header("Level Editor")]
    [SerializeField] SavedEditorLevels savedEditorLevels;
    [SerializeField] List<GameObject> LE_pagesList;
    [SerializeField] GameObject LE_levelEditorRoot;
    [SerializeField] Transform LE_pagesRoot;
    [SerializeField] float LE_pageSpacing = 600f;
    [SerializeField] int LE_currentPageIndex = 0;
    [SerializeField] public Vector3 LE_offscreenOffset = new Vector3(1920f, 0, 0);

    [Header("Background")]
    [SerializeField] Transform background;
    [SerializeField] float backgroundParallax = 0.3f;
    SpriteRenderer bgSprite;
    float loopWidth;

    [Header("Movement")]
    [SerializeField] float pageMoveDuration = 0.35f;
    [SerializeField]
    AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    const float OFFSCREEN_Y = -5000f;

    Transform pageLeft;
    Transform pageCenter;
    Transform pageRight;

    bool isMoving;
    bool selectorActive;

    MenuType activeMenuType;
    Vector3 activeOffscreenOffset;

    // =====================================================
    // INITIAL SETUP
    // =====================================================
    void Awake()
    {
        savedEditorLevels.RefreshLevels();

        // Setup Level Selector

        if (LS_levelSelectorRoot != null)
        {
            LS_levelSelectorRoot.SetActive(true);
            LS_levelSelectorRoot.transform.localPosition = LS_offscreenOffset;
            InitPages(LS_pagesList, LS_pagesRoot, LS_pageSpacing);
            //LS_levelSelectorRoot.SetActive(false);
        }

        // Setup Level Editor
        if (LE_levelEditorRoot != null)
        {
            LE_levelEditorRoot.SetActive(true);
            LE_levelEditorRoot.transform.localPosition = LE_offscreenOffset;
            InitPages(LE_pagesList, LE_pagesRoot, LE_pageSpacing);
            //LE_levelEditorRoot.SetActive(false);
        }

        // Customization not implemented yet

        bgSprite = background.GetComponent<SpriteRenderer>();
        if (bgSprite != null)
            loopWidth = bgSprite.bounds.size.x;

        Canvas.ForceUpdateCanvases();
    }

    void InitPages(List<GameObject> list, Transform root, float spacing)
    {
        if (list == null || list.Count == 0) return;

        // Move ALL pages far away first
        for (int i = 0; i < list.Count; i++)
        {
            list[i].transform.SetParent(root, false);
            list[i].transform.localPosition = new Vector3(0, OFFSCREEN_Y, 0);
        }

        // Place ONLY the visible center page for index 0
        Transform c = list[0].transform;
        c.localPosition = Vector3.zero;
    }

    void LoadMenuData(MenuType type)
    {
        activeMenuType = type;
        if (type == MenuType.LevelSelector)
        {
            pagesList = LS_pagesList;
            menuRoot = LS_levelSelectorRoot;
            pagesRoot = LS_pagesRoot;
            pageSpacing = LS_pageSpacing;
            currentPageIndex = LS_currentPageIndex;
            activeOffscreenOffset = LS_offscreenOffset;
        }
        else if (type == MenuType.LevelEditor)
        {
            pagesList = LE_pagesList;
            menuRoot = LE_levelEditorRoot;
            pagesRoot = LE_pagesRoot;
            pageSpacing = LE_pageSpacing;
            currentPageIndex = LE_currentPageIndex;
            activeOffscreenOffset = LE_offscreenOffset;
        }
        // Customization would go here


        RefreshPages();
    }

    void SaveMenuData()
    {
        if (activeMenuType == MenuType.LevelSelector)
        {
            LS_currentPageIndex = currentPageIndex;
        }
        else if (activeMenuType == MenuType.LevelEditor)
        {
            LE_currentPageIndex = currentPageIndex;
        }
    }

    public void SetActiveLevelEditorPages(List<GameObject> activePages)
    {
        LE_pagesList = activePages;

        // If we are currently in the LevelEditor menu, refresh the active pages

        if (activeMenuType == MenuType.LevelEditor)
        {
            pagesList = LE_pagesList;

            // Ensure currentPageIndex is valid

            if (pagesList != null && pagesList.Count > 0)
            {
                if (currentPageIndex >= pagesList.Count)
                {
                    currentPageIndex = pagesList.Count - 1;
                }


                RefreshPages();
            }
        }
    }

    int WrapIndex(int index, int count)
    {
        if (count == 0) return 0;
        if (index < 0) return count - 1;
        if (index >= count) return 0;
        return index;
    }

    // Default wrapper for current pagesList
    int WrapIndex(int index)
    {
        if (pagesList == null || pagesList.Count == 0) return 0;
        return WrapIndex(index, pagesList.Count);
    }

    // =====================================================
    // WRAPPERS FOR UNITY BUTTON EVENTS
    // =====================================================
    public void StartLevelSelector()

    {
        Debug.Log("StartLevelSelector button clicked!");
        StartPagesScrolling(MenuType.LevelSelector);
    }


    public void StartLevelEditor()

    {
        Debug.Log("StartLevelEditor button clicked!");
        StartPagesScrolling(MenuType.LevelEditor);
    }


    public void StartCustomization()

    {
        Debug.Log("StartCustomization button clicked!");
        StartPagesScrolling(MenuType.Customization);
    }

    // =====================================================
    // MAIN MENU → PAGES SCROLLING
    // =====================================================
    public void StartPagesScrolling(MenuType menuType)
    {
        if (selectorActive)

        {
            Debug.LogWarning("StartPagesScrolling ignored because selectorActive is already true.");
            return;
        }


        LoadMenuData(menuType);

        if (menuRoot == null)

        {
            Debug.LogError($"menuRoot is NULL for {menuType}! Did you assign it in the inspector?");
            return;
        }

        if (menuRoot != null)
        {
            menuRoot.SetActive(true);
        }

        StartCoroutine(StartScrollingRoutine(menuType));
    }

    IEnumerator StartScrollingRoutine(MenuType menuType)
    {
        selectorActive = true;

        Vector2 menuStart = mainMenu.anchoredPosition;
        Vector2 menuEnd = menuStart;

        Vector3 selectorStart = menuRoot.transform.localPosition;
        Vector3 selectorEnd = selectorStart;

        Vector3 bgStart = background.localPosition;
        Vector3 bgEnd = bgStart;

        Vector3 xyOffset = new Vector3(activeOffscreenOffset.x, activeOffscreenOffset.y, 0f);

        // Move the main menu away by the offset, and bring the selector to 0.
        menuEnd -= (Vector2)activeOffscreenOffset;
        selectorEnd -= xyOffset;
        bgEnd -= xyOffset * backgroundParallax;

        float t = 0f;

        while (t < menuExitDuration)
        {
            t += Time.deltaTime;
            float eased = moveCurve.Evaluate(t / menuExitDuration);

            mainMenu.anchoredPosition = Vector2.LerpUnclamped(menuStart, menuEnd, eased);
            menuRoot.transform.localPosition = Vector3.LerpUnclamped(selectorStart, selectorEnd, eased);
            background.localPosition = Vector3.LerpUnclamped(bgStart, bgEnd, eased);

            yield return null;
        }

        mainMenu.gameObject.SetActive(false);
    }

    // =====================================================
    // PAGES SCROLLING → MAIN MENU
    // =====================================================
    public void StopPagesScrolling()
    {
        if (!selectorActive || isMoving) return;
        StartCoroutine(StopScrollingRoutine());
    }

    IEnumerator StopScrollingRoutine()
    {
        mainMenu.gameObject.SetActive(true);

        Vector2 menuStart = mainMenu.anchoredPosition;
        Vector2 menuEnd = menuStart;

        Vector3 selectorStart = menuRoot.transform.localPosition;
        Vector3 selectorEnd = selectorStart;

        Vector3 bgStart = background.localPosition;
        Vector3 bgEnd = bgStart;

        Vector3 xyOffset = new Vector3(activeOffscreenOffset.x, activeOffscreenOffset.y, 0f);

        // Move the main menu back to 0, and push the selector away by the offset.
        menuEnd += (Vector2)activeOffscreenOffset;
        selectorEnd += xyOffset;
        bgEnd += xyOffset * backgroundParallax;

        float t = 0f;

        while (t < menuExitDuration)
        {
            t += Time.deltaTime;
            float eased = moveCurve.Evaluate(t / menuExitDuration);

            mainMenu.anchoredPosition = Vector2.LerpUnclamped(menuStart, menuEnd, eased);
            menuRoot.transform.localPosition = Vector3.LerpUnclamped(selectorStart, selectorEnd, eased);
            background.localPosition = Vector3.LerpUnclamped(bgStart, bgEnd, eased);

            yield return null;
        }

        // Reset carousel to start state
        currentPageIndex = 0;
        RefreshPages();
        SaveMenuData();

        // Reset background parallax horizontal position

        background.localPosition = new Vector3(0, background.localPosition.y, background.localPosition.z);

        selectorActive = false;

        if (menuRoot != null)
        {
            menuRoot.SetActive(false);
        }
    }


    // =====================================================
    // CAROUSEL CONTROLS
    // =====================================================
    public void NextPage()
    {
        if (!selectorActive || isMoving || pagesList == null || pagesList.Count <= 1) return;
        StartCoroutine(MoveCarousel(-1));
    }

    public void PreviousPage()
    {
        if (!selectorActive || isMoving || pagesList == null || pagesList.Count <= 1) return;
        StartCoroutine(MoveCarousel(+1));
    }

    IEnumerator MoveCarousel(int direction)
    {
        isMoving = true;

        // Teleport the TARGET page to the correct side BEFORE sliding
        if (pagesList != null && pagesList.Count > 1)
        {
            int otherIndex = WrapIndex(currentPageIndex - direction);
            pagesList[otherIndex].transform.localPosition = Vector3.right * (-direction) * pageSpacing;
        }

        Vector3 pagesStart = pagesRoot.localPosition;
        Vector3 pagesEnd = pagesStart + Vector3.right * direction * pageSpacing;

        Vector3 bgStart = background.localPosition;
        Vector3 bgEnd = bgStart + Vector3.right * direction * pageSpacing * backgroundParallax;

        float t = 0f;

        while (t < pageMoveDuration)
        {
            t += Time.deltaTime;
            float eased = moveCurve.Evaluate(t / pageMoveDuration);

            pagesRoot.localPosition = Vector3.LerpUnclamped(pagesStart, pagesEnd, eased);
            background.localPosition = Vector3.LerpUnclamped(bgStart, bgEnd, eased);

            yield return null;
        }

        // Logical page switch AFTER movement
        currentPageIndex = WrapIndex(currentPageIndex - direction);
        RefreshPages();
        SaveMenuData();

        WrapBackground();

        // Snap back (illusion reset)
        pagesRoot.localPosition = pagesStart;
        //background.localPosition = bgStart;

        isMoving = false;
    }

    // =====================================================
    // PAGE REFRESH (CORE LOOPING LOGIC)
    // =====================================================
    void RefreshPages()
    {
        if (pagesList == null || pagesList.Count == 0) return;

        pageCenter = pagesList[currentPageIndex].transform;
        pageLeft = pagesList[WrapIndex(currentPageIndex - 1)].transform;
        pageRight = pagesList[WrapIndex(currentPageIndex + 1)].transform;

        pageCenter.SetParent(pagesRoot, false);
        pageLeft.SetParent(pagesRoot, false);
        pageRight.SetParent(pagesRoot, false);

        PositionPages();
    }

    void PositionPages()
    {
        if (pagesList == null || pagesList.Count == 0) return;

        // Move ALL pages far away first
        for (int i = 0; i < pagesList.Count; i++)
        {
            pagesList[i].transform.localPosition = new Vector3(0, OFFSCREEN_Y, 0);
        }

        // Now place ONLY the visible center page
        // All other pages remain safely hidden at OFFSCREEN_Y
        pageCenter.localPosition = Vector3.zero;
    }

    void WrapBackground()
    {
        if (loopWidth <= 0) return;

        Vector3 pos = background.localPosition;

        if (pos.x <= -loopWidth)
            pos.x += loopWidth;
        else if (pos.x >= loopWidth)
            pos.x -= loopWidth;

        background.localPosition = pos;
    }
}
