using ToDoApi.Services.Interfaces;

namespace ToDOApi.Tests
{
    [TestFixture]
    public class TaskServiceTests
    {
        private readonly ITaskService _taskService;
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}