#region Using directives
using UAManagedCore;
using FTOptix.CoreBase;
using FTOptix.NetLogic;
#endregion

public class DelayedActionLogic : BaseNetLogic
{
    public override void Start()
    {
        duration = LogicObject.GetVariable("Period");
        if (duration == null)
            throw new CoreConfigurationException("Unable to find Period variable");

        enabledVariable = LogicObject.GetVariable("Enabled");
        if (enabledVariable == null)
            throw new CoreConfigurationException("Unable to find Enabled variable");

        actionMethodInvocation = LogicObject.Get("Action") as MethodInvocation;
        if (actionMethodInvocation == null)
            throw new CoreConfigurationException("Unable to find Action method invocation object");

        enabledVariable.VariableChange += EnabledVariable_VariableChange;
        actionTask = new DelayedTask(DelayedMethodInvocation, duration.Value, LogicObject);

        if (enabledVariable.Value)
            actionTask.Start();
    }

    public override void Stop()
    {
        enabledVariable.VariableChange -= EnabledVariable_VariableChange;
    }

    private void EnabledVariable_VariableChange(object sender, VariableChangeEventArgs e)
    {
        if (!e.NewValue)
            StopDelayedTask();
        else
            RestartDelayedTask();
    }

    private void StopDelayedTask()
    {
        actionTask?.Dispose();
        actionTask = null;
    }

    private void RestartDelayedTask()
    {
        StopDelayedTask();
        actionTask = new DelayedTask(DelayedMethodInvocation, duration.Value, LogicObject);
        actionTask.Start();
    }

    private void DelayedMethodInvocation()
    {
        actionMethodInvocation.Invoke();
    }

    private DelayedTask actionTask;
    private IUAVariable duration;
    private IUAVariable enabledVariable;
    private MethodInvocation actionMethodInvocation;
}
