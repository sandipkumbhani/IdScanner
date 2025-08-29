using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class GoogleDriveService
{
    private DriveService _driveService;

    private readonly string[] _scopes;
    private readonly string _applicationName;
    private readonly string _credentialPath;

    public GoogleDriveService(string[] scopes = null, string applicationName = "Broadsys ID Scanner", string credentialPath = "GoogleDriveKeys/client_secret.json")
    {
        _scopes = scopes ?? new[] { DriveService.Scope.Drive };
        _applicationName = applicationName;
        _credentialPath = credentialPath;
    }
    private async Task InitializeDriveServiceAsync()
    {
        UserCredential credential;
        using (var stream = new FileStream(_credentialPath, FileMode.Open, FileAccess.Read))
        {
            string tokenFolder = "token.json";

            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                _scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(tokenFolder, true));
        }

        _driveService = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = _applicationName,
        });
    }
    public async Task<string> CreateFolderAsync(string folderName)
    {
        await InitializeDriveServiceAsync();
        var fileMetadata = new Google.Apis.Drive.v3.Data.File()
        {
            Name = folderName,
            MimeType = "application/vnd.google-apps.folder"
        };
        var request = _driveService.Files.Create(fileMetadata);
        request.Fields = "id";
        var folder = await request.ExecuteAsync();
        return folder.Id;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string mimeType, string folderId)
    {
        await InitializeDriveServiceAsync(); 

        var fileMetadata = new Google.Apis.Drive.v3.Data.File
        {
            Name = fileName,
            Parents = new List<string> { folderId }
        };

        var request = _driveService.Files.Create(fileMetadata, fileStream, mimeType);
        request.Fields = "id";
        var result = await request.UploadAsync();

        if (result.Status == UploadStatus.Completed)
        {
            return fileName;
        }

        throw new Exception("Upload failed: " + result.Exception?.Message);
    }

    public async Task<string> GetFileIdByNameAsync(string folderId, string fileName)
    {
        await InitializeDriveServiceAsync();
        try
        {
            string FileId = "";
            FilesResource.ListRequest listRequest = _driveService.Files.List();
            listRequest.Fields = "files(id, name)";
            var files = await listRequest.ExecuteAsync();
            var file = files.Files.FirstOrDefault(x => x.Name == fileName);
            FileId = file != null ? file.Id : "";
            return FileId;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            throw;
        }
    }
    public async Task<string> UpdateFileAsync(string fileId, Stream fileStream, string mimeType)
    {
        await InitializeDriveServiceAsync();

        var fileMetadata = new Google.Apis.Drive.v3.Data.File();
        var updateRequest = _driveService.Files.Update(fileMetadata, fileId, fileStream, mimeType);
        updateRequest.Fields = "id, webViewLink";

        var result = await updateRequest.UploadAsync();
        if (result.Status == Google.Apis.Upload.UploadStatus.Completed)
        {
            var updatedFile = await _driveService.Files.Get(fileId).ExecuteAsync();
            return updatedFile.Name ?? throw new Exception("File updated but Name is null.");
        }

        throw new Exception($"File update failed: {result.Exception?.Message}");
    }

}
