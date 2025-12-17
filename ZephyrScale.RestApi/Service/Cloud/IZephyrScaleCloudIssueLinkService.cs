using System.Collections.Generic;
using ZephyrScale.RestApi.Dtos.Cloud;

namespace ZephyrScale.RestApi.Service.Cloud;

public interface IZephyrScaleCloudIssueLinkService
{
    /// <summary>
    /// Get test cycle IDs linked to the given Jira issue.
    /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#tag/Issue-Links/operation/getIssueLinkTestCycles
    /// </summary>
    /// <param name="issueKey"></param>
    /// <returns></returns>
    List<TestCycle> IssueLinksTestCycles(string issueKey);

    /// <summary>
    /// Get test case keys and versions linked to the given Jira issue.
    /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#tag/Issue-Links/operation/getIssueLinkTestCases
    /// </summary>
    /// <param name="issueKey"></param>
    /// <returns></returns>
    List<TestCase> IssueLinksTestCases(string issueKey);

    /// <summary>
    /// Returns links for a test execution with specified key.
    /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getTestExecutionLinks
    /// </summary>
    /// <param name="testExecutionIdOrKey"></param>
    /// <returns></returns>
    Links TestExecutionLinksGet(string testExecutionIdOrKey);

    /// <summary>
    /// Creates a link between a test execution and a Jira issue
    /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/createTestExecutionIssueLink
    /// </summary>
    /// <param name="testExecutionIdOrKey"></param>
    /// <param name="issueId"></param>
    /// <exception cref="Exception"></exception>
    void TestExecutionLinkCreate(string testExecutionIdOrKey, long issueId);
    
}