using UnityEngine;

public class UIPanel : MonoBehaviour
{
    public bool IsOpen => gameObject.activeSelf;

    protected virtual void Awake()
    {
        Close();
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}

