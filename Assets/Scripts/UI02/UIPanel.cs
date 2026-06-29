using UnityEngine;

public class UIPanel : MonoBehaviour //UI Base Panel (ON / OFF °ü¸®)
{
    public bool IsOpen { get; private set; }

    protected virtual void Awake()
    {
        Close();
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
        IsOpen = true;
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
        IsOpen = false;
    }
}

