using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;

public class ScoreGameObject : MonoBehaviour
{
    public static ScoreGameObject instance;
    private Text _text;
    [SerializeField]
    private Text _winText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            _text = GetComponent<Text>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetText(int value)
    {
        _text.text = value.ToString();
        if(value > 11)
        {
            _winText.gameObject.SetActive(true);
        }
    }
}