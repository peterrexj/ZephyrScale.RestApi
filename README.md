ZeyphrScale.Rest.Sdk
========

Open SDK to connect to the Zeyphr Scale application using Rest endpoints. Manage your communication and easily retrieve and publish test cases, test cycle and execution results to Zephyr Scale. You can integrate with you existing automation solution or process that will manage

Support both Server and Cloud based application.

The request and response objects are having proper DTOS (data transfer or model objects) defined within this package.

## How to Use

### Connect to service

```C#
     //Connect to cloud hosted Zeyphr Scale service
     var zService = new ZepyhrScaleCloudService("app url", "user api token");

     //Connect to server hosted Zeyphr Scale service
     var zService = new ZepyhrScaleCloudService("app url", "username", "password");

     //Get a test case by Key
     var testCase = zService.TestCasesGet("POC-T1");

     //Create a new Test case
     var newTestCase = ZeyphrOnPremService.TestCaseCreate(new TestCaseCreateRequest
                        {
                            ProjectKey = "PCO",
                            Folder = "/Automation",
                            Name = "Verify the integration between app and zyphr",
                            IssueLinks = new[] { "PCO-1432", "PCO-23" }.ToList(),
                            Owner = "peterjoseph",
                            Labels = new[] { "Automation", "Integration", "Api" }.ToList(),
                            Status = "Approved"
                        });
```

### Features Cloud

     - TestCaseCreate
     - TestCaseUpdate
     - TestCaseGetById
     - TestCasesGet
     - TestCasesGetFull = with options to search by any test case's field property
     - TestCaseLinksGet
     - TestCaseLinkCreate
     - TestCaseCustomFieldNames
     - TestCaseCustomFieldName
     - FolderGetById
     - FolderCreate
     - FolderCreateRecursive = create's the full folder path based on folder not available
     - FoldersGet
     - FoldersGetFull = with options to search by any folder's field property
     - FolderWithFullPath = builds the full path for each folder
     - TestCycleCreate
     - TestCycleGet
     - TestCyclesGet
     - TestCyclesGetFull = with option to search by any test cycle's field property
     - TestCycleLinksGet
     - TestCycleLinkCreate
     - TestCycleCustomFieldNames
     - TestExecutionCreate
     - TestExecutionGet
     - TestExecutionsGetFull = with options to search by any test execution result's field property value
     - StatusGet
     - StatusesGet
     - StatusesGetFull = with options to search by any status's field property value
     - EnvironmentGet
     - EnvironmentsGet
     - EnvironmentsGetFull = with options to search by any environment's field property value
     - ProjectGet
     - ProjectsGet
     - ProjectsGetFull = with options to search by any project's field property value
     - LinkDelete

### Features Server

     - TestCaseById
     - TestCasesByNames
     - TestCasesByNamesInFolder
     - TestCaseByIdsInFolder
     - TestCaseByIds
     - TestCaseByTestCaseName
     - TestCaseGetTopNInFolder
     - TestCaseGetTopN
     - TestCaseByJql
     - TestCaseCreate
     - TestCaseUpdate
     - TestCaseGetCustomMetadata
     - TestCycleGetById
     - TestCycleGetByKey
     - TestCycleGetByKey
     - TestCycleGetByName
     - TestCycleGetByName
     - TestCycleGetByProjectInFolder
     - TestCycleGetByFolder
     - TestCycleGetByProjectKeys
     - TestCycleGetByJql
     - TestCycleCreate
     - TestExecutionResultCreateByTestCaseInTestCycle
     - TestExecutionResultUpdateByTestCaseInTestCycle
     - TestExecutionResultCreateByTestCycle
     - TestExecutionResultGetByTestCycle
     - FolderCreate
     - ProjectsInTestGet
     - ProjectsInJiraGet

### Custom Control on the Service - Cloud / Server

There are some level of custom customization available on the service that can be passed on via the constuctor.

1. restApiVersion - Rest API version for the Zeyphr Service (default value: 'v2')
2. folderSeparator - Folder separator string (default value: '/')
3. logPrefix - Prefix text that will be added to all the logs generated from this service (default value: 'Zeyphr: ')
4. pageSizeSearchResult - Page size for search request (default value: '50')
5. requestRetryTimes - Number of time to retry when there is a network failure (default value: '1'). You can increase the number of times to retry based on your infrastructure if there are chance for a request to fail randomly
6. timeToSleepBetweenRetryInMilliseconds - Time to sleep in milliseconds between each time a call is retry (default value: '1000'). Applied only when requestRetryTimes is more than 1
7. assertResponseStatusOk - True/False whether the response code status from the server needs to be asserted for OK (default value 'true')
8. listOfResponseCodeOnFailureToRetry - Any of these status code matched from response will then use for retry the request. For example Proxy Authentication randomly failing can be then used to retry (default value 'null' which means it is not checking any response code for fail retry)

A scenario where you have network issues and you want to retry operation, then try this
```C#
     //Connect to cloud hosted Zeyphr Scale service
     var zService = new ZepyhrScaleCloudService("app url", "user api token",
          requestRetryTimes: 10,
          timeToSleepBetweenRetryInMilliseconds: 1500,
          assertResponseStatusOk: true,
          listOfResponseCodeOnFailureToRetry: new System.Net.HttpStatusCode []  { System.Net.HttpStatusCode.ProxyAuthenticationRequired  });
```
The above will apply an automatic retry of a maximum 20 times, giving itself a break of 1500 milliseconds between each request made to Zeyhpr that fails with a response code of HttpStatusCode.ProxyAuthenticationRequired

## Interesting Use Cases

### Folder Recursive Create - Cloud Version

With the cloud version there are some limitations on the folder, where the service by default 

- does not return the full path
- does not build the parent tree structure
- does not create the full tree structure in a single command
- does not fail when the folder already exists, instead create a new folder

FolderCreateRecursive - this option will create the full path and also checks if the folder already exists on any level of the path. For example, '/Automation/A/B/C', this method will create the full path in a single request and also if any of the folder available will be skipped and only required will be created

FolderWithFullPath - this option will return you the list of folder but along with that, one of the additional property on the model is "FullPath", that build the full path of each folder in the list

### Tips

1. [Server Mode] When updating test case, try to provide only the required property or provide only those properties that require update, for example, CustomFields or Labels or Properties
```C#
     var testcase = zService.TestCaseById(testCaseId).ToTestCaseUpdate();
     testcase.CustomFields.YouCustomFieldName = "YourCustomFieldValue";
     zService.TestCaseUpdate(testCaseId, new TestCaseCreateRequest { CustomFields = testcase.CustomFields });
```

## Upcoming Features

1. Zeyphr - Specflow integration module (automatically push results from Specflow to Zeyphr)
2. Zeyphr - nUnit integration module (automatically push results from nUnit tests to Zeyphr)
3. Zeyphr - Postman integration module (automatically push results from Postman tests to Zeyphr)

These features will help to track the automated test execution back to Zeyphr and you will have more control on each of the test cases from
- Control on what you to push - whether you want to push everything to Zeyphr or only for those test cases you are interested from a list of test you have
- Control on where you push - for a single test in nUnit or Specflow you can push into one or more different test cases in Zeyphr or even to multiple projects
- Control on how your push
     * based on the executing environment, for example for a test case, if run against Test Environment, then TestCase1, or if running against Stag, then TestCase2
     * based on the data, for example you can have a single test case running against several test scenarios (may be different products you are testing), you can now push into different test cases based on the data from these scenarios or more simpler like different products to different test cases
     * all of the above can be combined
4. End to End business logic implementation - Create Folder, Test cases, Test Cycle and Test Execution