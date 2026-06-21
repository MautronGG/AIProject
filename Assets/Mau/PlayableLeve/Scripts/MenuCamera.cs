using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuCamera : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] RectTransform mainMenu;
    [SerializeField] float menuExitDistance = 600f;
    [SerializeField] float menuExitDuration = 0.4f;

    [Header("Level Selector")]
    [SerializeField] List<GameObject> pagesList;
    [SerializeField] GameObject levelSelectorRoot;
    [SerializeField] Transform pagesRoot;
    [SerializeField] float pageSpacing = 600f;
    [SerializeField] int currentPageIndex = 0;

    [Header("Background")]
    [SerializeField] Transform background;
    [SerializeField] float backgroundParallax = 0.3f;
    SpriteRenderer bgSprite;
    float loopWidth;

    [Header("Movement")]
    [SerializeField] float pageMoveDuration = 0.35f;
    [SerializeField]
    AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    const float OFFSCREEN_Y = -50f;


    Transform pageLeft;
    Transform pageCenter;
    Transform pageRight;

    bool isMoving;
    bool selectorActive;

    // =====================================================
    // INITIAL SETUP
    // =====================================================
    void Awake()
    {
        // Selector is active, but positioned ABOVE view
        levelSelectorRoot.SetActive(true);

        // Setup pages
        pageCenter = pagesList[currentPageIndex].transform;
        pageLeft = pagesList[WrapIndex(currentPageIndex - 1)].transform;
        pageRight = pagesList[WrapIndex(currentPageIndex + 1)].transform;

        pageCenter.SetParent(pagesRoot, false);
        pageLeft.SetParent(pagesRoot, false);
        pageRight.SetParent(pagesRoot, false);

        PositionPages();
        Canvas.ForceUpdateCanvases();

        // Move selector ABOVE menu
        levelSelectorRoot.transform.localPosition +=
            Vector3.up * menuExitDistance;

        bgSprite = background.GetComponent<SpriteRenderer>();
        loopWidth = bgSprite.bounds.size.x;
    }



    void PositionPages()
    {
        // Move ALL pages far away first
        for (int i = 0; i < pagesList.Count; i++)
        {
            pagesList[i].transform.localPosition =
                new Vector3(0, OFFSCREEN_Y, 0);
        }

        // Now place ONLY the visible 3
        pageCenter.localPosition = Vector3.zero;
        pageLeft.localPosition = Vector3.left * pageSpacing;
        pageRight.localPosition = Vector3.right * pageSpacing;
    }


    int WrapIndex(int index)
    {
        if (index < 0) return pagesList.Count - 1;
        if (index >= pagesList.Count) return 0;
        return index;
    }

    // =====================================================
    // MAIN MENU → LEVEL SELECTOR
    // =====================================================
    public void StartLevelSelector()
    {
        if (selectorActive) return;
        StartCoroutine(StartSelectorRoutine());
    }

    IEnumerator StartSelectorRoutine()
    {
        selectorActive = true;

        Vector2 menuStart = mainMenu.anchoredPosition;
        Vector2 menuEnd = menuStart + Vector2.down * menuExitDistance;

        Vector3 selectorStart = levelSelectorRoot.transform.localPosition;
        Vector3 selectorEnd = selectorStart + Vector3.down * menuExitDistance;

        Vector3 bgStart = background.localPosition;
        Vector3 bgEnd = bgStart + Vector3.down * menuExitDistance * backgroundParallax;

        float t = 0f;

        while (t < menuExitDuration)
        {
            t += Time.deltaTime;
            float eased = moveCurve.Evaluate(t / menuExitDuration);

            mainMenu.anchoredPosition =
                Vector2.LerpUnclamped(menuStart, menuEnd, eased);

            levelSelectorRoot.transform.localPosition =
                Vector3.LerpUnclamped(selectorStart, selectorEnd, eased);

            background.localPosition =
                Vector3.LerpUnclamped(bgStart, bgEnd, eased);

            yield return null;
        }

        mainMenu.gameObject.SetActive(false);
    }



    // =====================================================
    // CAROUSEL CONTROLS
    // =====================================================
    public void NextPage()
    {
        if (!selectorActive || isMoving) return;
        StartCoroutine(MoveCarousel(-1));
    }

    public void PreviousPage()
    {
        if (!selectorActive || isMoving) return;
        StartCoroutine(MoveCarousel(+1));
    }

    IEnumerator MoveCarousel(int direction)
    {
        isMoving = true;

        Vector3 pagesStart = pagesRoot.localPosition;
        Vector3 pagesEnd =
            pagesStart + Vector3.right * direction * pageSpacing;

        Vector3 bgStart = background.localPosition;
        Vector3 bgEnd =
            bgStart + Vector3.right * direction * pageSpacing * backgroundParallax;

        float t = 0f;

        while (t < pageMoveDuration)
        {
            t += Time.deltaTime;
            float eased = moveCurve.Evaluate(t / pageMoveDuration);

            pagesRoot.localPosition =
                Vector3.LerpUnclamped(pagesStart, pagesEnd, eased);

            background.localPosition =
                Vector3.LerpUnclamped(bgStart, bgEnd, eased);

            yield return null;
        }

        // Logical page switch AFTER movement
        currentPageIndex = WrapIndex(currentPageIndex - direction);
        RefreshPages();

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
        pageCenter = pagesList[currentPageIndex].transform;
        pageLeft = pagesList[WrapIndex(currentPageIndex - 1)].transform;
        pageRight = pagesList[WrapIndex(currentPageIndex + 1)].transform;

        pageCenter.SetParent(pagesRoot, false);
        pageLeft.SetParent(pagesRoot, false);
        pageRight.SetParent(pagesRoot, false);

        PositionPages();
    }

    void WrapBackground()
    {
        Vector3 pos = background.localPosition;

        if (pos.x <= -loopWidth)
            pos.x += loopWidth;
        else if (pos.x >= loopWidth)
            pos.x -= loopWidth;

        background.localPosition = pos;
    }

}
