using System;
using Xunit;
using zCompany.Utilities;

namespace zCompany.TaskAide.Tests
{
    public class TaskPersistenceTests : IClassFixture<EmptyDatabaseFixture>
    {
        // Fields
        private Database db;
        private SystemTimeDev systemTime;
        private TimerDev timer;

        // Constructor
        public TaskPersistenceTests(EmptyDatabaseFixture fixture)
        {
            this.db = fixture.Db;
            this.systemTime = fixture.SystemTime;
            this.timer = fixture.Timer;
        }

        // Destructors
        ~TaskPersistenceTests()
        {

        }

        // Tests
        [Fact]
        public void Add()
        {
            ITask task;
            string name = "TP_AddTask";

            this.AddTask(name, out task);
            this.AddTaskConfirm(task, name);
        }

        [Fact]
        public void Remove()
        {
            ITask task;
            string name = "TP_RemoveTask";

            this.AddTask(name, out task);
            this.RemoveTask(task);
            this.RemoveTaskConfirm(task);
        }

        [Fact]
        public void Rename()
        {
            ITask task;
            string oldName = "TP_RenameTask";
            string newName = "TP_RenameTaskNewName";

            this.AddTask(oldName, out task);
            this.RenameTask(task, newName);
            this.RenameTaskConfirm(task, newName);
        }

        // Helpers
        private void AddTask(string name, out ITask addedTask)
        {
            // Configure

            // Setup
            var controller = new TaskAide(db, this.systemTime, this.timer);

            // Test
            addedTask = controller.AddTask(name);
            Assert.Equal(name, addedTask.Name);

            // Confirm
            var task = controller.GetTask(addedTask.TID);
            Assert.NotNull(task);
            Assert.Equal(name, task.Name);
        }

        private void AddTaskConfirm(ITask addedTask, string name)
        {
            // Configure

            // Setup
            var controller = new TaskAide(db, this.systemTime, this.timer);

            // Confirm (previous test persistence)
            var task = controller.GetTask(addedTask.TID);
            Assert.NotNull(task);
            Assert.Equal(name, task.Name);
        }

        private void RemoveTask(ITask removeTask)
        {
            // Configure

            // Setup
            var controller = new TaskAide(db, this.systemTime, this.timer);

            // Test
            controller.RemoveTask(removeTask);

            // Confirm
            var task = controller.GetTask(removeTask.TID);
            Assert.Null(task);
        }

        private void RemoveTaskConfirm(ITask removeTask)
        {
            // Configure

            // Setup
            var controller = new TaskAide(db, this.systemTime, this.timer);

            // Confirm (previous test persistence)
            var task = controller.GetTask(removeTask.TID);
            Assert.Null(task);
        }

        private void RenameTask(ITask renameTask, string name)
        {
            // Configure

            // Setup
            var controller = new TaskAide(db, this.systemTime, this.timer);

            // Test
            controller.RenameTask(renameTask, name);

            // Confirm
            var task = controller.GetTask(renameTask.TID);
            Assert.NotNull(task);
            Assert.Equal(name, task.Name);
        }

        private void RenameTaskConfirm(ITask renameTask, string name)
        {
            // Configure

            // Setup
            var controller = new TaskAide(db, this.systemTime, this.timer);

            // Confirm (previous test persistence)
            var task = controller.GetTask(renameTask.TID);
            Assert.NotNull(task);
            Assert.Equal(name, task.Name);
        }
    }
}
