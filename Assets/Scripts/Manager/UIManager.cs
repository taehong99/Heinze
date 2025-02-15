using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] Canvas popUpCanvas;
    [SerializeField] Canvas windowCanvas;
    [SerializeField] Canvas inGameCanvas;
    [SerializeField] Canvas fadeCanvas;

    [SerializeField] Image popUpBlocker;
    [SerializeField] Button inGameBlocker;
    [SerializeField] ScreenFader fader;
    [SerializeField] CardSelectUI cardSelectUI;
    public ScreenFader Fader => fader;
    public CardSelectUI CardSelectUI => cardSelectUI;

    private Dictionary<string, BaseUI> dictionary = new Dictionary<string, BaseUI>();

    private Stack<PopUpUI> popUpStack = new Stack<PopUpUI>();
    private float prevTimeScale;
    private InGameUI curInGameUI;

    private void Start()
    {
        EnsureEventSystem();
        Manager.Player.PlayerDied += ShowGameOverUI;
    }

    private void OnDestroy()
    {
        Manager.Player.PlayerDied -= ShowGameOverUI;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (popUpStack.Count == 0)
            {
                Manager.Sound.PlaySFX(Manager.Sound.AudioClips.tabOpenSFX);
                ShowPopUpUI<PlayerStatsUI>();
            }
            else if (popUpStack.Peek().name == "PlayerStatsUI(Clone)")
            {
                Manager.Sound.PlaySFX(Manager.Sound.AudioClips.tabCloseSFX);
                ClosePopUpUI();
            }
            else
            {
                return;
            }
        }
    }

    private void ShowGameOverUI()
    {
        Manager.Sound.PlaySFX(Manager.Sound.AudioClips.gameOverSFX);
        ShowPopUpUI<GameOverUI>();
    }

    public void EnsureEventSystem()
    {
        if (EventSystem.current != null)
            return;

        EventSystem eventSystem = Resources.Load<EventSystem>("UI/EventSystem");
        Instantiate(eventSystem);
    }

    public T ShowPopUpUI<T>(T popUpUI) where T : PopUpUI
    {
        if (popUpStack.Count > 0)
        {
            PopUpUI topUI = popUpStack.Peek();
            topUI.gameObject.SetActive(false);
        }
        else
        {
            popUpBlocker.gameObject.SetActive(true);
            prevTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }

        T ui = Instantiate(popUpUI, popUpCanvas.transform);
        popUpStack.Push(ui);
        return ui;
    }

    public T ShowPopUpUI<T>() where T : PopUpUI
    {
        T resource = Load<T>($"UI/PopUp/{typeof(T).Name}");
        return ShowPopUpUI(resource);
    }

    public void ClosePopUpUI()
    {
        PopUpUI ui = popUpStack.Pop();
        Destroy(ui.gameObject);

        if (popUpStack.Count > 0)
        {
            PopUpUI topUI = popUpStack.Peek();
            topUI.gameObject.SetActive(true);
        }
        else
        {
            popUpBlocker.gameObject.SetActive(false);
            Time.timeScale = prevTimeScale;
        }
    }

    public void ClearPopUpUI()
    {
        while (popUpStack.Count > 0)
        {
            ClosePopUpUI();
        }
    }

    public T ShowWindowUI<T>(T windowUI) where T : WindowUI
    {
        return Instantiate(windowUI, windowCanvas.transform);
    }

    public void SelectWindowUI(WindowUI windowUI)
    {
        windowUI.transform.SetAsLastSibling();
    }

    public void CloseWindowUI(WindowUI windowUI)
    {
        Destroy(windowUI.gameObject);
    }

    public void ClearWindowUI()
    {
        for (int i = 0; i < windowCanvas.transform.childCount; i++)
        {
            Destroy(windowCanvas.transform.GetChild(i).gameObject);
        }
    }

    public T ShowInGameUI<T>(T inGameUI) where T : InGameUI
    {
        if (curInGameUI != null)
        {
            Destroy(curInGameUI.gameObject);
        }

        T ui = Instantiate(inGameUI, inGameCanvas.transform);
        curInGameUI = ui;
        inGameBlocker.gameObject.SetActive(true);
        return ui;
    }

    public void CloseInGameUI()
    {
        if (curInGameUI == null)
            return;

        inGameBlocker.gameObject.SetActive(false);
        Destroy(curInGameUI.gameObject);
        curInGameUI = null;
    }

    private T Load<T>(string path) where T : BaseUI
    {
        if (dictionary.TryGetValue(path, out BaseUI ui))
        {
            return ui as T;
        }
        else
        {
            T resource = Resources.Load<T>(path);
            dictionary.Add(path, resource);
            return resource;
        }
    }
}
