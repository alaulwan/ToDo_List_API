using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ToDo_List_API.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ToDo_List_API.Models;
using ToDo_List_API.Repositories;

namespace ToDo_List_API.Controllers.Tests
{
    [TestClass()]
    public class PersonControllerTests
    {
        [TestMethod()]
        public async Task PersonControllerTest()
        {
            var repoMock = new Mock<IPersonRepo>();
            Person[] persons = { new Person { Id=1, Name="Alaa" }, new Person { Id = 2, Name = "Omar" } };
            repoMock.Setup(r => r.Get()).Returns(Task.FromResult(persons));
            PersonController controller = new PersonController(new DataContext ());
            controller.repo = repoMock.Object;
            var all = await controller.Get();
            var result = (Person[])((ObjectResult)all.Result).Value;
            Assert.AreEqual(persons, result);
        }

        [TestMethod()]
        public async Task GetPersonTest()
        {
            var repoMock = new Mock<IPersonRepo>();
            Person[] persons = { new Person { Id = 1, Name = "Alaa" }, new Person { Id = 2, Name = "Omar", tickets = new HashSet<ToDo> { new ToDo { Id=1, Task="Task 1", Status=0 } } } };
            repoMock.Setup(r => r.GetPerson(2)).Returns(persons[1]);
            PersonController controller = new PersonController(new DataContext());
            controller.repo = repoMock.Object;
            var personResponse = await controller.GetPerson(2);
            var expected = new PersonResponse(persons[1], 1, 1);
            var result = (PersonResponse)((ObjectResult)personResponse.Result).Value;
            Assert.AreEqual(expected.Id, result.Id);
            Assert.AreEqual(expected.Name, result.Name);
            Assert.AreEqual(expected.tasksCount, result.tasksCount);
            Assert.AreEqual(expected.openTasksCount, result.openTasksCount);
        }

        [TestMethod()]
        public async Task GetToDoForPersonTest()
        {
            var repoMock = new Mock<IPersonRepo>();
            Person[] persons = { new Person { Id = 1, Name = "Alaa" }, new Person { Id = 2, Name = "Omar", tickets = new HashSet<ToDo> { new ToDo { Id = 1, Task = "Task 1", Status = 0 } } } };
            var toDoList = persons[1].tickets.ToList<ToDo>();
            repoMock.Setup(r => r.GetToDoForPerson(2)).Returns(toDoList);
            PersonController controller = new PersonController(new DataContext());
            controller.repo = repoMock.Object;
            var resultResponse = await controller.GetToDoForPerson(2, true);
            var result = (List<ToDo>)((ObjectResult)resultResponse.Result).Value;
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(toDoList[0].Id, result[0].Id);
        }

        [TestMethod()]
        public async Task AddPersonTest()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "ToDoDatabase")
            .Options;

            var newPerson = new Person { Name = "Tim" };
            PersonController controller = new PersonController(new DataContext(options));
            var resultResponse = await controller.AddPerson(newPerson);
            var result = (Person)((ObjectResult)resultResponse.Result).Value;
            Assert.AreEqual(newPerson, result);

            resultResponse = await controller.GetPerson(1);
            result = (Person)((ObjectResult)resultResponse.Result).Value;
            Assert.AreEqual(newPerson.Id, result.Id);
            Assert.AreEqual(newPerson.Name, result.Name);
        }
    }
}