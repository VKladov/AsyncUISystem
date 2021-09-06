using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupWithSubwindow : PopupWindow
{
    [SerializeField] private Button _button;
    [SerializeField] private PopupWindow _subwindow;

    protected override IEnumerable<ActionTrigger> GetTriggers()
    {
        return new List<ActionTrigger>
        {
            new ActionTrigger()
            {
                Trigger = WaitButton(_button),
                Action = _subwindow.Open
            }
        };
    }
}
