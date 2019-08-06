using System;
using System.Collections.Generic;
using Xunit;
using zCompany.Utilities;

namespace zCompany.TaskAide.Tests
{
    public class SystemTests : IClassFixture<AppSimulationDatabaseFixture>
    {
        // Fields
        private Database db;
        private AppSimulationDatabaseFixture dbMeta;
        private SystemTimeDev systemTime;
        private List<Task> tasks;
        private TimerDev timer;

        // Constructor
        public SystemTests(AppSimulationDatabaseFixture fixture)
        {
            this.db = fixture.Db;
            this.dbMeta = fixture;
            this.systemTime = fixture.SystemTime;
            this.tasks = fixture.Tasks;
            this.timer = fixture.Timer;

            this.dbMeta.PopulateTasks("Tasks");
        }

        // Destructors
        ~SystemTests()
        {
            this.dbMeta.DestroyTable("Tasks");
        }

        // Tests
        [Fact]
        public void AddGetRemoveTasks()
        {
            // Configure
            var newTaskName = "Omega";

            // Setup
            var controller = new TaskAide(db, this.systemTime, this.timer);

            // Test: Add
            var taskAdded = controller.AddTask(newTaskName);
            Assert.Equal(newTaskName, taskAdded.Name);

            // Test: Get
            var taskFound = controller.GetTask(taskAdded.TID);
            Assert.Equal(taskAdded.Name, taskFound.Name);

            // Test: Remove
            controller.RemoveTask(taskAdded);
            taskFound = controller.GetTask(taskAdded.TID);
            Assert.Null(taskFound);
        }

        [Fact]
        public void GetTaskList()
        {
            // Configure
            
            // Setup
            var controller = new TaskAide(this.db, this.systemTime, this.timer);

            // Test
            var model = controller.TaskList;
            Assert.NotNull(model);
            Assert.Equal(this.tasks.Count, model.Tasks.Count);

            // Confirm
            foreach (Task task in model.Tasks)
            {
                var foundTask = this.tasks.Find((t) => t.TID == task.TID);
                Assert.NotNull(foundTask);
                Assert.Equal(task.Name, foundTask.Name);
            }
        }

        [Fact]
        public void GetTaskList_Empty()
        {
            // Configure
            var db = new Database("Filename=GetTaskList_Empty_Test.db");

            // Setup
            var controller = new TaskAide(db, this.systemTime, this.timer);

            // Test
            var model = controller.TaskList;
            Assert.NotNull(model);
            Assert.Empty(model.Tasks);
        }

        [Fact]
        public void Rename()
        {
            // Configure
            var refTask = this.tasks[1];
            var newName = "newName";

            // Setup
            var controller = new TaskAide(db, this.systemTime, this.timer);
            var model = controller.TaskList;
            var task = model.Tasks[model.Tasks.IndexOf(refTask)];
            Assert.Equal(refTask.Name, task.Name);

            // Test
            controller.RenameTask(task, newName);

            // Confirm
            task = model.Tasks[model.Tasks.IndexOf(refTask)];
            Assert.Equal(newName, task.Name);

            // Restore
            controller.RenameTask(task, refTask.Name);
        }

        [Fact]
        public void StartWorking()
        {
            // Configure
            var refTask = this.tasks[1];
            int timeProgression_mins = 20;

            // Setup
            var controller = new TaskAide(db, this.systemTime, this.timer);
            var session = controller.ActiveSession;
            Assert.Equal(this.systemTime.UtcNow.UtcTicks, session.DateTimeStart.UtcTicks);

            // Test
            controller.SwitchTasks(refTask);
            Assert.NotNull(session.ActiveInterval);
            Assert.Equal(refTask, controller.ActiveTask);

            // Test: time passage
            this.systemTime.FastForward(TimeSpan.FromMinutes(timeProgression_mins));
            Assert.Equal(timeProgression_mins, Math.Ceiling(session.ActiveInterval.Span.TotalMinutes));
        }
    }
}
