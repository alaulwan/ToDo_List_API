using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ToDo_List_API.Controllers;
using ToDo_List_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using ToDo_List_API.Models;

namespace ToDo_List_API.Controllers.Tests
{
    [TestClass()]
    public class PersonControllerTests
    {
        public DbSet<Person> persons { get; set; }

        [TestMethod()]
        public void PersonControllerTest()
        {
            var DataContext = new DataContext ();
            PersonController controller = new PersonController(DataContext);
            DataContext.Persons.Add(null);
            Assert.AreEqual(1, DataContext.Persons.Count());
        }

        [TestMethod()]
        public void GetTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetPersonTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetToDoForPersonTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AddPersonTest()
        {
            Assert.Fail();
        }
    }
}