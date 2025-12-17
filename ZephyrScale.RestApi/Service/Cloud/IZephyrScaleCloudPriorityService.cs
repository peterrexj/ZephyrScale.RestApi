using System;
using System.Collections.Generic;
using ZephyrScale.RestApi.Dtos.Cloud;

namespace ZephyrScale.RestApi.Service.Cloud;

public interface IZephyrScaleCloudPriorityService
{
    /// <summary>
    /// Returns a priority for the given ID.
    /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#tag/Priorities/operation/getPriority
    /// </summary>
    /// <param name="priorityId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    Priority PriorityGet(long priorityId);

    /// <summary>
    /// Returns all priorities.
    /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#tag/Priorities/operation/listPriorities
    /// Use predicate to streamline your result, for examplev: (t) => t.Name.EqualsIgnoreCase("test")
    /// Use [breakSearchOnFirstConditionValid] if you want break the search once the match is found. This can improve performance when searching for specific item
    /// </summary>
    /// <param name="projectKey"></param>
    /// <param name="predicate"></param>
    /// <param name="breakSearchOnFirstConditionValid"></param>
    /// <returns></returns>
    List<Priority> PrioritiesGetFull(string projectKey, Func<Priority, bool> predicate = null, bool breakSearchOnFirstConditionValid = true);

    /// <summary>
    /// Returns the list of Priorities based on the search request
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Pagination<Priority> PrioritiesGet(SearchRequestBase request);
}