namespace ZephyrScale.RestApi.Service.Cloud;

public interface IZephyrScaleCloudService :
    IZephyrScaleCloudTestCaseService,
    IZephyrScaleCloudTestCycleService,
    IZephyrScaleCloudTestExecutionService,
    IZephyrScaleCloudTestPlanService,
    IZephyrScaleCloudFolderService,
    IZephyrScaleCloudStatusService,
    IZephyrScaleCloudEnvironmentService,
    IZephyrScaleCloudProjectService,
    IZephyrScaleCloudIssueLinkService,
    IZephyrScaleCloudPriorityService
{
    string ZeypherUrl { get; set; }
    string ZephyrApiVersion { get; set; }
    string JiraApiVersion { get; set; }
    string FolderSeparator { get; set; }
    bool CanConnect { get; }
}
