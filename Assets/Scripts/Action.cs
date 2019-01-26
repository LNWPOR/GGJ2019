using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ActionEndEvent();
public interface IAction
{
  void Start();
  void AddEndEvent(ActionEndEvent endEventListener);
}

public abstract class ActionBased : IAction
{
  private ActionEndEvent listener;
  public void AddEndEvent(ActionEndEvent endEventListener)
  {
    listener += endEventListener;
  }

  public void InvokeEndEvent()
  {
    listener?.Invoke();
  }

  abstract public void Start();
}

public class GroupAction : ActionBased
{
  private readonly IAction[] actionGroup;
  private int CountCallEnd = 0;

  public GroupAction(IAction[] actionGroup)
  {
    this.actionGroup = actionGroup;
    for (int i = 0; i < actionGroup.Length; ++i)
    {
      actionGroup[i].AddEndEvent(NotifyEnd);
    }
  }

  private void NotifyEnd()
  {
    ++CountCallEnd;
    if (CountCallEnd == actionGroup.Length) InvokeEndEvent();
  }

  public override void Start()
  {
    for (int i = 0; i < actionGroup.Length; ++i)
    {
      actionGroup[i].Start();
    }
  }
}

public class QueueAction : ActionBased
{
  private readonly Queue<IAction> queue = new Queue<IAction>();

  private void RunNextAction()
  {
    if (queue.Count > 0)
    {
      var action = queue.Dequeue();
      action.Start();
    }
    else
    {
      InvokeEndEvent();
    }
  }

  public void AddAction(IAction action)
  {
    action.AddEndEvent(RunNextAction);
    queue.Enqueue(action);
  }

  public override void Start()
  {
    RunNextAction();
  }
}

public class AutoQueue
{
  private Queue<IAction> actionQueue = new Queue<IAction>();

  private void RunNextAction()
  {
    if (actionQueue.Count > 0)
    {
      var action = actionQueue.Peek();
      action.Start();
    }
  }

  private void OnFinish()
  {
    actionQueue.Dequeue();
    RunNextAction();
  }

  public void AddAction(IAction action)
  {
    action.AddEndEvent(OnFinish);
    if (actionQueue.Count == 0)
    {
      actionQueue.Enqueue(action);
      RunNextAction();
    }
    else
    {
      actionQueue.Enqueue(action);
    }
  }
}
