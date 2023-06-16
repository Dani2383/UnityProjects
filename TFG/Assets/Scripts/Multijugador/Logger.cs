using System.Linq;
using TMPro;
using System;

public class Logger : Singleton<Logger>
{
    public TextMeshProUGUI debugAreaText = null;

    public bool enableDebug = false;

    public int maxLines = 15;

    private void Awake()
    {
        if (debugAreaText == null)
        {
            debugAreaText = GetComponent<TextMeshProUGUI>();
        }
        debugAreaText.text = string.Empty;
    }

    private void OnEnable()
    {
        debugAreaText.enabled = enableDebug;
        enabled = enableDebug;

        if (enabled)
        {
            debugAreaText.text += $"<color=\"white\">{DateTime.Now:HH:mm:ss.fff} {this.GetType().Name} enabled</color>\n";
        }
    }

    public void LogInfo(string message)
    {
        ClearLines();

        debugAreaText.text += $"<color=\"green\">{DateTime.Now:HH:mm:ss.fff} {message}</color>\n";
    }

    public void LogError(string message)
    {
        ClearLines();
        debugAreaText.text += $"<color=\"red\">{DateTime.Now:HH:mm:ss.fff} {message}</color>\n";
    }

    public void LogWarning(string message)
    {
        ClearLines();
        debugAreaText.text += $"<color=\"yellow\">{DateTime.Now:HH:mm:ss.fff} {message}</color>\n";
    }

    private void ClearLines()
    {
        if (debugAreaText.text.Split('\n').Count() >= maxLines)
        {
            debugAreaText.text = string.Empty;
        }
    }
}