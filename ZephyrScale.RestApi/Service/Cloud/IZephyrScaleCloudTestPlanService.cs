using System;
using System.Collections.Generic;
using ZephyrScale.RestApi.Dtos.Cloud;

namespace ZephyrScale.RestApi.Service.Cloud;

public interface IZephyrScaleCloudTestPlanService
{
    /// <summary>
    /// Creates a test plan. All required test plan custom fields should be present in the request.
    /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/createTestPlan
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="Exception"></exception>
    TestPlan TestPlanCreate(TestPlanCreateRequest request);

    /// <summary>
    /// Returns a test plan for the given key.
    /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getTestPlan
    /// </summary>
    /// <param name="testPlanIdOrKey"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    TestPlan TestPlanGet(string testPlanIdOrKey);

    /// <summary>
    /// Returns the list of test plans based on the search request
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Pagination<TestPlan> TestPlansGet(SearchRequestBase request);

    /// <summary>
    /// Retrieves all test plans. Query parameters can be used to filter the results.
    /// Use predicate to streamline your result, for example: (t) => t.Name.EqualsIgnoreCase("test") 
    /// Use [breakSearchOnFirstConditionValid] if you want break the search once the match is found. This can improve performance when searching for specific item
    /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/listTestPlans
    /// </summary>
    /// <param name="projectKey">Jira project key filter</param>
    /// <param name="folderId"></param>
    /// <param name="predicate"></param>
    /// <param name="breakSearchOnFirstConditionValid"></param>
    /// <returns></returns>
    List<TestPlan> TestPlansGetFull(string projectKey, long? folderId = null,
        Func<TestPlan, bool> predicate = null, bool breakSearchOnFirstConditionValid = true);

    /// <summary>
    /// Returns the total number of test plans available in the project
    /// </summary>
    /// <param name="projectKey"></param>
    /// <returns></returns>
    long TestPlanCountGet(string projectKey);

}