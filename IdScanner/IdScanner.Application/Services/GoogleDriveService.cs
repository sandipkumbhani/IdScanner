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
    private readonly string[] Scopes = { DriveService.Scope.DriveFile };
    private readonly string ApplicationName = "Broadsys ID Scanner";
    private readonly string CredentialPath = "GoogleDriveKeys/client_secret_450198704638-s68uhit4jk57hqpdj7tgpq7jhrq75qca.apps.googleusercontent.com.json";
    public GoogleDriveService(string[] scopes, string applicationName, string credentialPath)
    {
        Scopes = scopes;
        ApplicationName = applicationName;
        CredentialPath = credentialPath;
    }

    private async Task<DriveService> GetDriveServiceAsync()
    {
        UserCredential credential;

        using (var stream = new FileStream(CredentialPath, FileMode.Open, FileAccess.Read))
        {
            string tokenPath = "token.json";
            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(tokenPath, true));
        }

        return new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string mimeType)
    {
        var driveService = await GetDriveServiceAsync();

        var fileMetadata = new Google.Apis.Drive.v3.Data.File
        {
            Name = fileName
        };

        var request = driveService.Files.Create(fileMetadata, fileStream, mimeType);
        request.Fields = "id";

        var uploadProgress = await request.UploadAsync();

        if (uploadProgress.Status == UploadStatus.Completed)
        {
            var file = request.ResponseBody;
            return $"https://drive.google.com/file/d/{file.Id}/view";
        }
        else
        {
            throw new Exception("File upload failed: " + uploadProgress.Exception?.Message);
        }
    }
}
