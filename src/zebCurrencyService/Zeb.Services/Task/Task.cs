﻿using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeb.Core.Configuration;
using Zeb.Core.Infrastructure;
using Zeb.Data;
using Zeb.Services.Infrastructure;

namespace Zeb.Services.Task
{
    /// <summary>
    /// Task
    /// </summary>
    public partial class Task
    {
        #region Ctor

        /// <summary>
        /// Ctor for Task
        /// </summary>
        private Task()
        {
            this.Enabled = true;
        }

        /// <summary>
        /// Ctor for Task
        /// </summary>
        /// <param name="task">Task </param>
        public Task(ScheduleTask task)
        {
            this.Type = task.Type;
            this.Enabled = task.Enabled;
            this.Name = task.Name;
        }

        #endregion

        #region Utilities

        private ITask CreateTask(ILifetimeScope scope)
        {
            ITask task = null;
            if (this.Enabled)
            {

                var type2 = System.Type.GetType(this.Type);
                if (type2 != null)
                {
                    object instance;
                    if (!EngineContext.Current.ContainerManager.TryResolve(type2, scope, out instance))
                    {
                        //not resolved
                        instance = EngineContext.Current.ContainerManager.ResolveUnregistered(type2, scope);
                    }
                    task = instance as ITask;
                }
            }
            return task;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes the task
        /// </summary>
        /// <param name="throwException">A value indicating whether exception should be thrown if some error happens</param>
        /// <param name="dispose">A value indicating whether all instances should be disposed after task run</param>
        /// <param name="ensureRunOnOneWebFarmInstance">A value indicating whether we should ensure this task is run on one farm node at a time</param>
        public void Execute(bool throwException = false, bool dispose = true, bool ensureRunOnOneWebFarmInstance = true)
        {
            //background tasks has an issue with Autofac
            //because scope is generated each time it's requested
            //that's why we get one single scope here
            //this way we can also dispose resources once a task is completed
            var scope = EngineContext.Current.ContainerManager.Scope();
            var scheduleTaskService = EngineContext.Current.ContainerManager.Resolve<IScheduleTaskService>("", scope);
            var scheduleTask = scheduleTaskService.GetTaskByType(this.Type);

            try
            {
                //task is run on one farm node at a time?
                if (ensureRunOnOneWebFarmInstance)
                {
                    //is web farm enabled (multiple instances)?
                    var zebConfig = EngineContext.Current.ContainerManager.Resolve<ZebConfig>("", scope);
                    if (zebConfig.MultipleInstancesEnabled)
                    {
                        var machineNameProvider = EngineContext.Current.ContainerManager.Resolve<IMachineNameProvider>("", scope);
                        var machineName = machineNameProvider.GetMachineName();
                        if (String.IsNullOrEmpty(machineName))
                        {
                            throw new Exception("Machine name cannot be detected. You cannot run in web farm.");
                            //actually in this case we can generate some unique string (e.g. Guid) and store it in some "static" (!!!) variable
                            //then it can be used as a machine name
                        }

                       

                       
                        scheduleTaskService.UpdateTask(scheduleTask);
                    }
                }

                //initialize and execute
                var task = this.CreateTask(scope);
                if (task != null)
                {
                    this.LastStartUtc = DateTime.UtcNow;
                    if (scheduleTask != null)
                    {
                        //update appropriate datetime properties
                        scheduleTask.LastStartUtc = this.LastStartUtc;
                        scheduleTaskService.UpdateTask(scheduleTask);
                    }
                    task.Execute();
                    this.LastEndUtc = this.LastSuccessUtc = DateTime.UtcNow;
                }
            }
            catch (Exception exc)
            {
                this.Enabled = !this.StopOnError;
                this.LastEndUtc = DateTime.UtcNow;
                   //DUE TO LOGGER IS NOT INPLEMENTED I JUST THROW.
                    throw exc;
            }

            if (scheduleTask != null)
            {
                //update appropriate datetime properties
                scheduleTask.LastEndUtc = this.LastEndUtc;
                scheduleTask.LastSuccessUtc = this.LastSuccessUtc;
                scheduleTaskService.UpdateTask(scheduleTask);
            }

            //dispose all resources
            if (dispose)
            {
                scope.Dispose();
            }
        }

        #endregion
        #region Properties

        /// <summary>
        /// Datetime of the last start
        /// </summary>
        public DateTime? LastStartUtc { get; private set; }

        /// <summary>
        /// Datetime of the last end
        /// </summary>
        public DateTime? LastEndUtc { get; private set; }

        /// <summary>
        /// Datetime of the last success
        /// </summary>
        public DateTime? LastSuccessUtc { get; private set; }

        /// <summary>
        /// A value indicating type of the task
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// A value indicating whether to stop task on error
        /// </summary>
        public bool StopOnError { get; private set; }

        /// <summary>
        /// Get the task name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// A value indicating whether the task is enabled
        /// </summary>
        public bool Enabled { get; set; }

        #endregion


    }
}
