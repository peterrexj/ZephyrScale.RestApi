using System;
using System.Collections.Generic;
using ZephyrScale.RestApi.Dtos.Cloud;

namespace ZephyrScale.RestApi.Service.Cloud;

public interface IZephyrScaleCloudStatusService
{
    /// <summary>
    /// Returns a status for the given ID.
    /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getStatus
    /// </summary>
    /// <param name="statusId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    Status StatusGet(string statusId);

    /// <summary>
    /// Returns all statuses.
    /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/listStatuses
    /// Use predicate to streamline your result, for examplev: (t) => t.Name.EqualsIgnoreCase("test")
    /// Use [breakSearchOnFirstConditionValid] if you want break the search once the match is found. This can improve performance when searching for specific item
    /// </summary>
    /// <param name="projectKey"></param>
    /// <param name="statusType"></param>
    /// <param name="predicate"></param>
    /// <param name="breakSearchOnFirstConditionValid"></param>
    /// <returns></returns>
    List<Status> StatusesGetFull(string projectKey, string statusType = null, Func<Status, bool> predicate = null, bool breakSearchOnFirstConditionValid = true);

    /// <summary>
    /// Returns the list of statuses based on the search request
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Pagination<Status> StatusesGet(StatusSearchRequest request);
}