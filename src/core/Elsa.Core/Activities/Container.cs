using System.Collections.ObjectModel;
using Elsa.Attributes;
using Elsa.Behaviors;
using Elsa.Contracts;
using Elsa.Models;

namespace Elsa.Activities;

/// <summary>
/// A base class for activities that control a collection of activities.
/// </summary>
public abstract class Container : Activity, IContainer
{
    protected Container()
    {
        Behaviors.Remove<AutoCompleteBehavior>();
    }

    protected Container(params IActivity[] activities) : this()
    {
        Activities = activities;
    }

    protected Container(ICollection<Variable> variables, params IActivity[] activities) : this(activities)
    {
        Variables = variables;
    }

    [Outbound] public ICollection<IActivity> Activities { get; set; } = new List<IActivity>();
    public ICollection<Variable> Variables { get; set; } = new Collection<Variable>();

    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        // Register variables.
        context.ExpressionExecutionContext.Register.Declare(Variables);

        // Schedule children.
        await ScheduleChildrenAsync(context);
    }

    protected virtual ValueTask ScheduleChildrenAsync(ActivityExecutionContext context)
    {
        ScheduleChildren(context);
        return ValueTask.CompletedTask;
    }

    protected virtual void ScheduleChildren(ActivityExecutionContext context)
    {
    }
}