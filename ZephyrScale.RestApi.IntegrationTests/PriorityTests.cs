using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZephyrScale.RestApi.IntegrationTests
{
    public class PriorityTests : TestCaseBase
    {
        [TestCase]
        public void Should_Return_FullPriorities()
        {
            var priorities = _zephyrService.PrioritiesGetFull(_jiraProjectKey);
            Assert.IsNotNull(priorities);
            Assert.IsTrue(priorities.Count > 1);
        }

        [TestCase("Low")]
        [TestCase("Normal")]
        [TestCase("High")]
        public void Should_Return_SearchResult(string name)
        {
            var priority = _zephyrService.PrioritiesGetFull(_jiraProjectKey, p => p.Name.Equals(name), breakSearchOnFirstConditionValid: true);
            Assert.IsNotNull(priority);
            Assert.IsTrue(priority.Count == 1);
            Assert.IsTrue(priority.First().Name == name);
        }

        [TestCase]
        public void Should_Return_Priority()
        {
            var priorities = _zephyrService.PrioritiesGetFull(_jiraProjectKey);
            if (priorities?.Count > 0)
            {
                var priority = _zephyrService.PriorityGet(priorities.First().Id.Value);
                Assert.IsNotNull(priority);
                Assert.IsNotNull(priority.Id == priorities.First().Id.Value);
                Assert.IsNotNull(priority.Name == priorities.First().Name);
            }
        }
    }
}
