
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsManager : MonoBehaviour
{
    [SerializeField] GameObject[] FirstPanel;
    [SerializeField] GameObject[] SecondPanel;

    public void SetOn()
    {
        foreach (GameObject panel in FirstPanel)
            panel.SetActive(false);
        foreach (GameObject panel in SecondPanel)
            panel.SetActive(true);
    }

    public void SetOff()
    {
        foreach (GameObject panel in FirstPanel)
            panel.SetActive(true);
        foreach (GameObject panel in SecondPanel)
            panel.SetActive(false);
    }
}
