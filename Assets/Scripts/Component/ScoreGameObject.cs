using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gameシーン上のスコアUI
/// </summary>
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

    /// <summary>
    /// 引数の値にUIテキストを変更
    /// </summary>
    /// <param name="value">引数にとる数値</param>
    public void SetText(int value)
    {
        _text.text = value.ToString();
        if(value > 11)
        {
            _winText.gameObject.SetActive(true);
        }
    }
}