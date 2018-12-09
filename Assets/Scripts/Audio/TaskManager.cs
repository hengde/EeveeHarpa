using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager {

	// *************************** EXPLANATION *************************** 
	// How to reference:
	// This is a definition of a task manager - you can make instances
	// of a task manager in your own code, but you probably don't need to
	// change anything in here.
	// ******************************************************************* 
	
	private readonly List<Task> _tasks = new List<Task>();  

	// Add a task
	public void Do(Task task) 
	{     
		Debug.Assert(task != null);      
		Debug.Assert(!task.IsAttached);
		_tasks.Add(task);     
		task.SetStatus(Task.TaskStatus.Pending); 
	}

	public void Update() 
	{     
	// iterate through all the tasks
		for (int i = _tasks.Count - 1; i >= 0; --i)     
		{         
			Task task = _tasks[i];         
			// Initialize tasks that have just been added                 
			if (task.IsPending)         
			{             
				task.SetStatus(Task.TaskStatus.Working);        
			}          

			// A task can finish during initialization               
			// so you need to check before the update 
			if (task.IsFinished)         
			{             
				HandleCompletion(task, i);         
			}         
			else         
			{             
				task.Update();             
			}
		}
	}

	private void HandleCompletion(Task task, int taskIndex) 
	{     
	// If the finished task has a "next" task
	// queue it up - but only if the original task was     
	// successful     
		if (task.NextTask != null && task.IsSuccessful)     
		{         
			Do(task.NextTask);     
		}     
		// clear the task from the manager and let it know     
		// it's no longer being managed     
		_tasks.RemoveAt(taskIndex);     
		task.SetStatus(Task.TaskStatus.Detached); 
	}

	public bool HasTasks() {
		if (_tasks.Count == 0)
			return false;
		return true;
	}
}

public class Task {

	// *************************** EXPLANATION *************************** 
	// How to reference:
	// This contains the definitions for the various tasks for the game.
	// You shouldn't be making changes in here.
	// ******************************************************************* 
	
	public enum TaskStatus : byte 
	{     
		Detached, // Task has not been attached to a TaskManager     
		Pending, // Task has not been initialized     
		Working, // Task has been initialized     
		Success, // Task completed successfully     
		Fail, // Task completed unsuccessfully     
		Aborted // Task was aborted 
	}  

	public TaskStatus Status { get; private set; }  

	public bool IsDetached { get { return Status == TaskStatus.Detached;}}
	public bool IsAttached { get { return Status != TaskStatus.Detached;}}
	public bool IsPending { get { return Status == TaskStatus.Pending; } }
	public bool IsWorking { get { return Status == TaskStatus.Working; } }
	public bool IsSuccessful {get{ return Status == TaskStatus.Success;}} 
	public bool IsFailed { get { return Status == TaskStatus.Fail; } }
	public bool IsAborted { get { return Status == TaskStatus.Aborted; } } 
	public bool IsFinished { get { return (Status == TaskStatus.Fail || Status == TaskStatus.Success || Status == TaskStatus.Aborted); } }  

	public void Abort() 
	{     
		SetStatus(TaskStatus.Aborted); 
	}

	internal void SetStatus(TaskStatus newStatus) 
	{     
		if (Status == newStatus) return;      
		Status = newStatus;      
		switch (newStatus)     
		{
			case TaskStatus.Working:             
				Initialize();             
				break;
			case TaskStatus.Success:
				OnSuccess();             
				CleanUp();             
				break;          
			case TaskStatus.Aborted:             
				OnAbort();             
				CleanUp();             
				break;
			case TaskStatus.Fail:             
				OnFail();             
				CleanUp();             
				break;          
			// These are "internal" states that are relevant for         
			// the task manager         
			case TaskStatus.Detached:
			case TaskStatus.Pending:
				break;
			default:
				throw new System.ArgumentOutOfRangeException(newStatus.ToString(), newStatus, null);     
		} 
	}

	protected virtual void OnAbort() {}
	protected virtual void OnSuccess() {}  
	protected virtual void OnFail() {}

	// Override this to handle initialization of the task. 
	// This is called when the task enters the Working state 

	protected virtual void Initialize() { }  

	// Called whenever the TaskManager updates. Your tasks' work 
	// generally goes here 

	internal virtual void Update() { }  

	// This is called when the tasks completes (i.e. is aborted, 
	// fails, or succeeds). It is called after the status change 
	// handlers are called 

	protected virtual void CleanUp() { }

	// Assign a task to be run if this task runs successfully 
	public Task NextTask { get; private set; }  

	// Sets a task to be automatically attached when this one completes successfully 
	// NOTE: if a task is aborted or fails, its next task will not be queued 
	// NOTE: **DO NOT** assign attached tasks with this method. 
	public Task Then(Task task) 
	{     
		Debug.Assert(!task.IsAttached);     
		NextTask = task;     
		return task; 
	}

}

public class ActionTask : Task {
	private readonly System.Action _action;

	public ActionTask(System.Action action)     
	{         
		_action = action;     
	}      

	protected override void Initialize()     
	{         
		SetStatus(TaskStatus.Success);       
		_action();     
	} 
}

public class WaitTask : Task 
{     
	// Get the timestamp in floating point milliseconds from the Unix epoch     

	private static readonly System.DateTime UnixEpoch = new System.DateTime(1970, 1, 1);     

	private static double GetTimestamp()     
	{         
		return (System.DateTime.UtcNow - UnixEpoch).TotalMilliseconds;     
	}      

	private readonly double _duration;     
	private double _startTime;

	public WaitTask(double duration)
	{
		_duration = duration * 1000f;
	}

	public WaitTask (float duration)
	{
		_duration = duration * 1000f;
	}

	public WaitTask(int duration)     
	{         
		_duration = duration * 1000f;
	}     

	protected override void Initialize()     
	{         
		_startTime = GetTimestamp();     
	}      

	internal override void Update()     
	{         
		var now = GetTimestamp();
		var durationElapsed = (now - _startTime) > _duration;         

		if (durationElapsed)         
		{             
			SetStatus(TaskStatus.Success);         
		}     
	} 
}

public class OnGoingTask : Task {
	// Get the timestamp in floating point milliseconds from the Unix epoch     

	private static readonly System.DateTime UnixEpoch = new System.DateTime(1970, 1, 1);   

	private static double GetTimestamp()     
	{         
		return (System.DateTime.UtcNow - UnixEpoch).TotalMilliseconds;     
	}      

	private readonly System.Action _action;
	private readonly double _duration;     
	private double _startTime;      

	public OnGoingTask(double duration, System.Action action)     
	{         
		this._duration = duration;
		this._action = action;
	}

	public OnGoingTask (float duration, System.Action action)
	{
		this._duration = duration*1000;
		this._action = action;
	} 

	public OnGoingTask (int duration, System.Action action)
	{
		this._duration = duration*1000;
		this._action = action;
	}  

	protected override void Initialize()     
	{         
		_startTime = GetTimestamp();     
	}      

	internal override void Update()     
	{         
		var now = GetTimestamp();
		var durationElapsed = (now - _startTime) > _duration;         

		if (durationElapsed)         
		{             
			SetStatus(TaskStatus.Success);         
		}
		_action();
	} 
}
