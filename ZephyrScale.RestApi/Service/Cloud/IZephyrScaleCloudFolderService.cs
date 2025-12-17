using System;
using System.Collections.Generic;
using ZephyrScale.RestApi.Dtos.Cloud;

namespace ZephyrScale.RestApi.Service.Cloud;

public interface IZephyrScaleCloudFolderService
{
  /// <summary>
        /// Returns a folder for the given ID
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/getFolder
        /// </summary>
        /// <param name="folderId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Folder FolderGetById(string folderId);

        /// <summary>
        /// Creates a folder
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/createFolder
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Folder FolderCreate(FolderCreateRequest request);

        /// <summary>
        /// Creates the full folder structure
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/createFolder
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        Folder FolderCreateRecursive(FolderCreateRequest request);

        /// <summary>
        /// Returns the list of the folders based on the search request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Pagination<Folder> FoldersGet(FolderSearchRequest request);

        /// <summary>
        /// Returns all folder.
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/listFolders
        /// Use predicate to streamline your result, for examplev: (t) => t.Name.EqualsIgnoreCase("test") 
        /// Use [breakSearchOnFirstConditionValid] if you want break the search once the match is found. This can improve performance when searching for specific item
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="folderType"></param>
        /// <param name="predicate"></param>
        /// <param name="breakSearchOnFirstConditionValid"></param>
        /// <returns></returns>
        List<Folder> FoldersGetFull(string projectKey, FolderType folderType, Func<Folder, bool> predicate = null, bool breakSearchOnFirstConditionValid = true);

        /// <summary>
        /// Returns all folders and build the full path
        /// https://support.smartbear.com/zephyr-scale-cloud/api-docs/#operation/listFolders
        /// </summary>
        /// <param name="folders"></param>
        /// <returns></returns>
        List<Folder> FolderWithFullPath(List<Folder> folders);

}