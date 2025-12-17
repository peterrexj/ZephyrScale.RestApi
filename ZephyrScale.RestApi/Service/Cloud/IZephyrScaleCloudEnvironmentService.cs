using System;
using System.Collections.Generic;
using ZephyrScale.RestApi.Dtos.Cloud;
using Environment = ZephyrScale.RestApi.Dtos.Cloud.Environment;

namespace ZephyrScale.RestApi.Service.Cloud;

public interface IZephyrScaleCloudEnvironmentService
{
    /// <summary>
    /// Creates an environment. All required environment custom fields should be present in the request.
    /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/createEnvironment
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="Exception"></exception>
    Environment EnvironmentCreate(EnvironmentCreateRequest request);

    /// <summary>
    /// Returns an environment for the given ID.
    /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getEnvironment
    /// </summary>
    /// <param name="environmentId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    Environment EnvironmentGet(string environmentId);

    /// <summary>
    /// Updates an existing environment.
    /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/updateEnvironment
    /// </summary>
    /// <param name="environment"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="Exception"></exception>
    Environment EnvironmentUpdate(Environment environment);


    /// <summary>
    /// Returns the list of environment based on the search request
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Pagination<Environment> EnvironmentsGet(SearchRequestBase request);

    /// <summary>
    /// Returns all environments.
    /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/listEnvironments
    /// Use predicate to streamline your result, for examplev: (t) => t.Name.EqualsIgnoreCase("test")
    /// Use [breakSearchOnFirstConditionValid] if you want break the search once the match is found. This can improve performance when searching for specific item
    /// </summary>
    /// <param name="projectKey"></param>
    /// <param name="predicate"></param>
    /// <param name="breakSearchOnFirstConditionValid"></param>
    /// <returns></returns>
    List<Environment> EnvironmentsGetFull(string projectKey,
        Func<Environment, bool> predicate = null,
        bool breakSearchOnFirstConditionValid = true);
}