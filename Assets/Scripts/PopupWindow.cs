using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PopupWindow : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private float _animationDuration = 0.15f;

    private bool _closeButtonPressed;
    
    public async Task Open()
    {
        gameObject.SetActive(true);
        _closeButtonPressed = false;
        
        await PlayShowAnimation();
        
        do
        {
            var allActions = GetTriggers().ToList();
            if (_closeButton)
            {
                allActions.Add(new ActionTrigger
                {
                    Trigger = WaitButtonPress(_closeButton),
                    Action = OnClosePressed
                });
            }
            
            var selectedTriggerAction = await SelectTrigger(allActions);
            await selectedTriggerAction();
            
        } while (!_closeButtonPressed);
        
        await PlayHideAnimation();
        
        gameObject.SetActive(false);
    }

    protected static async Task WaitButton(Button button)
    {
        var pressed = false;
        button.onClick.AddListener(() => pressed = true);
        
        while (!pressed)
            await Task.Yield();
    }
    
    protected virtual IEnumerable<ActionTrigger> GetTriggers() => new List<ActionTrigger>();

    private async Task OnClosePressed()
    {
        _closeButtonPressed = true;
    }

    private static async Task<Func<Task>> WaitTrigger(ActionTrigger trigger)
    {
        await trigger.Trigger;
        return trigger.Action;
    }

    private static async Task<Func<Task>> SelectTrigger(IEnumerable<ActionTrigger> trigger)
    {
        var task = await Task.WhenAny(trigger.Select(WaitTrigger));
        return task.Result;
    }

    private static async Task<Button> WaitButtonPress(Button button)
    {
        var pressed = false;
        button.onClick.AddListener(() => pressed = true);
        
        while (!pressed)
            await Task.Yield();

        return button;
    }

    private async Task PlayShowAnimation()
    {
        var time = 0f;
        var scale = Vector3.one * 0.6f;
        var targetScale = Vector3.one;
        while (time < _animationDuration)
        {
            transform.localScale = Vector3.Lerp(scale, targetScale, time / _animationDuration);
            time += Time.deltaTime;
            await Task.Yield();
        }

        transform.localScale = targetScale;
    }

    private async Task PlayHideAnimation()
    {
        var time = 0f;
        var scale = Vector3.one;
        var targetScale = Vector3.one * 0.6f;
        while (time < _animationDuration)
        {
            transform.localScale = Vector3.Lerp(scale, targetScale, time / _animationDuration);
            time += Time.deltaTime;
            await Task.Yield();
        }

        transform.localScale = targetScale;
    }
}
