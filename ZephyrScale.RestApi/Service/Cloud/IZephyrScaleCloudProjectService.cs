using System;
using System.Collections.Generic;
using ZephyrScale.RestApi.Dtos.Cloud;

namespace ZephyrScale.RestApi.Service.Cloud;

public interface IZephyrScaleCloudProjectService
{
    /// <summary>
    /// Returns a project for the given ID or key.
    /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getProject
    /// </summary>
    /// <param name="projectIdOrKey"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    Project ProjectGet(string projectIdOrKey);

    /// <summary>
    /// Returns the project based on the search query
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Pagination<Project> ProjectsGet(SearchRequestBase request = null);

    /// <summary>
    /// Returns all projects.
    /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/listProjects
    /// Use predicate to streamline your result, for examplev: (t) => t.Name.EqualsIgnoreCase("test") 
    /// Use [breakSearchOnFirstConditionValid] if you want break the search once the match is found. This can improve performance when searching for specific item
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="breakSearchOnFirstConditionValid"></param>
    /// <returns></returns>
    List<Project> ProjectsGetFull(Func<Project, bool> predicate = null,
        bool breakSearchOnFirstConditionValid = true);
}