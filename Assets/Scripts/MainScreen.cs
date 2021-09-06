using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainScreen : PopupWindow
{
    [SerializeField] private Button _button1;
    [SerializeField] private Button _button2;
    [SerializeField] private PopupWindow _window1;
    [SerializeField] private PopupWindow _window2;
    [SerializeField] private PopupWindow _autoWindow;

    private Task _autoWindowDelay;
    private bool _autoWindowDisplayed;
    
    private async void Start()
    {
        _autoWindowDelay = Task.Delay(Random.Range(10, 20) * 1000);
        await Open();
    }

    protected override IEnumerable<ActionTrigger> GetTriggers()
    {
        var triggers = new List<ActionTrigger>
        {
            new ActionTrigger
            {
                Trigger = WaitButton(_button1),
                Action = _window1.Open
            },
            new ActionTrigger
            {
                Trigger = WaitButton(_button2),
                Action = _window2.Open
            }
        };

        if (!_autoWindowDisplayed)
        {
            triggers.Add(new ActionTrigger
            {
                Trigger = _autoWindowDelay,
                Action = OpenAutoWindow
            });
        }

        return triggers;
    }

    private Task OpenAutoWindow()
    {
        _autoWindowDisplayed = true;
        return _autoWindow.Open();
    }
}
