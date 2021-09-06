using System;
using System.Threading.Tasks;

public class ActionTrigger
{
    public Task Trigger;
    public Func<Task> Action;
}
