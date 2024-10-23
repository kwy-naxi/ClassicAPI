using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BaseAPI
{
    public class AppSettings
    {
        public Dictionary<string, string> ConnectionStrings { get; set; }
        public Dictionary<string, string> ConnectionStringsDev { get; set; }
        public Dictionary<string, string> Authentication { get; set; }
        public Dictionary<string, string> MongoDbEntitiesStrings { get; set; }
        public Dictionary<string, string> SocialLogin { get; set; }
        public Dictionary<string, string> DistributedCache { get; set; }
        public Dictionary<string, string> DBBackup { get; set; }
        public Dictionary<string, string> ProjectData { get; set; }
        public Dictionary<string, string> SystemManagement { get; set; }
        public List<Authorization> Authorizations { get; set; }

        public string DBProdYn { get; set; } = "Y";
        public string UploadDir { get; set; } = "";
        public string DownloadUrl { get; set; } = "";
        public string MongoDbUrl { get; set; } = "";
        public string MongoDbImageUrl { get; set; } = "";
        public string MongoDbImageDBName { get; set; } = "";
        public string MongoDbFeedUrl { get; set; } = "";
        public string MongoDbFeedDBName { get; set; } = "";
        public string MongoDbEventUrl { get; set; } = "";
        public string MongoDbEventDbName { get; set; } = "";
        public string MongoDbHubUrl { get; set; } = "";
        public string MongoDbHubDbName { get; set; } = "";
        public string MongoDbSearchUrl { get; set; } = "";
        public string DomainUrl { get; set; } = "";
        public string WebsiteUrl { get; set; } = "";
        public string MainsiteUrl { get; set; } = "";
        public string WebsiteDescription { get; set; } = "";
        public string DomainPcHomeUrl { get; set; } = "";
        public string DomainMobileHomeUrl { get; set; } = "";
        public string SideMenuUseYn { get; set; } = "";
        public string CacheTypeIdx { get; set; } = "";
        public string HttpsUseYn { get; set; } = "";
        public string EmailRepeatSendYn { get; set; } = "";
        public string EmailHtmlSaveYn { get; set; } = "";
        public string NoticeYn { get; set; } = "";
        public string PushNotificationYn { get; set; } = "";
        public string ServiceCategory { get; set; } = "";
        public string RedisUseYn { get; set; } = "";
        public string MenuAuthCheckUseYn { get; set; } = "";
        public string RedisMenuKey { get; set; } = "";
        public string BannerUrl { get; set; } = "";
        public string PathForEMailFiles { get; set; } = "";
        public string IamportDomainUrl { get; set; } = "";
        public string IamportId { get; set; } = "";
        public string IamportRestApiKey { get; set; } = "";
        public string IamportRestApiSecret { get; set; } = "";
        public string IamportMID { get; set; } = "";
        public string IamportCID { get; set; } = "";
        public string PGProvider { get; set; } = "";
        public string PGProviders { get; set; } = "";
        public string PGMultipleUseYn { get; set; } = "";
        public string PGEscrowYn { get; set; } = "";
        public string JsVersion { get; set; } = "";
        public string CssVersion { get; set; } = "";
        public string RedisDataVersion { get; set; } = "";
        public string SmtpHost { get; set; } = "";
        public string SmtpPort { get; set; } = "";
        public string SmtpId { get; set; } = "";
        public string SmtpPassword { get; set; } = "";
        public string SmtpFrom { get; set; } = "";
        public string EmailSendType { get; set; } = "SMTP";
        public string MessageFollow { get; set; } = "";
        public string MessageBookmark { get; set; } = "";
        public string MessageLike { get; set; } = "";
        public string AppStoreUrl { get; set; } = "";
        public string GooglePlayStorUrl { get; set; } = "";
        public string MenuTypeIdx { get; set; } = "";
        public string PostAllUserPublishYn { get; set; } = "";
        public string BlockUseYn { get; set; } = "";
        public string RabbitMq_Que_Name { get; set; } = "";
        public string RabbitMq_Que_Host { get; set; } = "";
        public string RabbitMq_Que_Id { get; set; } = "";
        public string RabbitMq_Que_Password { get; set; } = "";
        public string AdminSellerIdx { get; set; } = "";
        public string JwtSecret { get; set; } = "";
        public string JwtUseTypeIdx { get; set; } = ""; // 1:Cookie 2: JWT 3: Cookie + JWT
        public string LoginCookieName { get; set; } = ".login_session";
        public string ExpireMinutes { get; set; } = "";
        public string ServiceName { get; set; } = "";
        public string ServiceShortName { get; set; } = "";
        public string ServiceEngName { get; set; } = "";
        public int RedenominationRate { get; set; }
        public string WebPushSubject { get; set; } = "";
        public string WebPushPublicKey { get; set; } = "";
        public string WebPushPrivateKey { get; set; } = "";
        public string BlankImageUrl { get; set; } = "";
        public string FcmPushUseYn { get; set; } = "";
        public string FcmPushGoogleCredentialPath { get; set; } = "";
        public string FcmPushServerKey { get; set; } = "";
        public string WebPushUseYn { get; set; } = "";
        public string KeyRingUseYn { get; set; } = "";
        public string KeyRingLocation { get; set; } = "";
        public string HubsWithAuthentication { get; set; } = "";
        public int LoginSessionDefaultForSecond { get; set; }
        public int LoginSessionExtensionForSecond { get; set; }
        public string MvcApplicationAssemblyName { get; set; } = "";
        public string OneSignalApiUrl { get; set; } = "";
        public string OneSignalAppId { get; set; } = "";
        public string OneSignalRestApiKey { get; set; } = "";
        public string AppScheme { get; set; } = "";
        public string TempFilesPath { get; set; } = "";
        public string WebAppYn { get; set; } = "";
        public string SiteInfoUseYn { get; set; } = "";
        public string SiteInfoDbType { get; set; } = "";
        public string JoinForSellerYn { get; set; } = "";
        public string SmsYn { get; set; } = "";
        public string SmsType { get; set; } = "";
        public string Cafe24Info { get; set; } = "";
        public string DeveloperEmail { get; set; } = "";
        public string DeveloperPhone { get; set; } = "";
        public string ManagerEmail { get; set; } = "";
        public string SocialLoginUseYn { get; set; } = "";
        public string CognitiveServicesSpeechKey { get; set; } = "";
        public string MultiLanguageDataVersion { get; set; } = "";
        public string MultiLanguageUseYn { get; set; } = "N";
        public string DefaultLanguage { get; set; } = "";
        public string SiteMapUrls { get; set; } = "";
        public string ServiceWebSiteLocalPath { get; set; } = "";
        public string CellPhoneLoginUseYn { get; set; } = "N";
        public string EncryptionKey { get; set; } = "";
        public string AllowCorsOrigins { get; set; } = "";
        public string DevUserIdxs { get; set; } = "";
        public string BitlyAccessToken { get; set; } = "";
        public string CuttlyAccessToken { get; set; } = "";
        public string ProductItemName { get; set; } = "";
        public string DevYn { get; set; } = "N";
        public string DeepLinkSchema { get; set; }
        public string FileRootDirectory { get; set; }
        public string Favicon { get; set; }
    }

    public class Authorization
    {
        public string Path { get; set; }
        public bool Allow { get; set; }
        public IList<string> Roles { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class Authentication
    {
        public string LoginUrl { get; set; }
        public IList<Authorization> Authorizations { get; set; }
    }

    public static class ServiceAuthenticationSchemes
    {
        public const string Api = "API";
        public const string Moblie = "Moblie";
        public const string JwtOnly = JwtBearerDefaults.AuthenticationScheme;
        public const string CookieOnly = CookieAuthenticationDefaults.AuthenticationScheme;
        public const string TwoWayAuthentication = CookieAuthenticationDefaults.AuthenticationScheme + "," + JwtBearerDefaults.AuthenticationScheme;
    }

    public static class AreaSchemes
    {
        public const string Api = "api";
        public const string Market = "market";
        public const string Chat = "chat";
        public const string Test = "test";
        public const string Service = "service";
        public const string Budlish = "buds";
    }
}
