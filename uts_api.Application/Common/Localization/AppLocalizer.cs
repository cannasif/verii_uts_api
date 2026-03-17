using System.Globalization;

namespace uts_api.Application.Common.Localization;

public static class AppLocalizer
{
    private static readonly IReadOnlyDictionary<string, string> En = new Dictionary<string, string>
    {
        [LocalizationKeys.RequiredField] = "This field is required.",
        [LocalizationKeys.InvalidEmailAddress] = "Please enter a valid email address.",
        [LocalizationKeys.MaximumLengthExceeded] = "Maximum allowed length is {0} characters.",
        [LocalizationKeys.MinimumLengthRequired] = "Minimum required length is {0} characters.",
        [LocalizationKeys.InvalidRange] = "Value must be between {0} and {1}.",
        [LocalizationKeys.InvalidRequest] = "The request payload is invalid.",
        [LocalizationKeys.PasswordResetEmailSubject] = "Password Reset",
        [LocalizationKeys.PasswordResetEmailGreeting] = "Hello {0},",
        [LocalizationKeys.PasswordResetEmailBody] = "Your password reset request has been received.",
        [LocalizationKeys.PasswordResetEmailToken] = "Reset token: {0}",
        [LocalizationKeys.PasswordResetEmailExpiry] = "This token is valid for 2 hours.",
        [LocalizationKeys.Forbidden] = "You do not have permission to access this resource.",
        [LocalizationKeys.PermissionGroupInUse] = "This permission group is assigned to one or more users and cannot be deleted.",
        [LocalizationKeys.SystemPermissionGroupCannotBeDeleted] = "System permission groups cannot be deleted.",
        [LocalizationKeys.StockSyncStarted] = "RII_STOCK sync job started at {0}.",
        [LocalizationKeys.StockSyncAborted] = "RII_STOCK sync aborted because RII_FN_STOK call failed.",
        [LocalizationKeys.StockSyncSkipped] = "RII_STOCK sync skipped because RII_FN_STOK returned no rows.",
        [LocalizationKeys.StockSyncCompleted] = "RII_STOCK sync completed. created={0}, updated={1}, failed={2}, skipped={3}, duplicatePayload={4}.",
        [LocalizationKeys.StockSyncRecordFailed] = "RII_STOCK sync record failed. StockCode: {0}",
        [LocalizationKeys.StockSyncFailureLogWriteFailed] = "RII_STOCK sync failure could not be written to RII_HANGFIRE_JOB_LOGS. StockCode: {0}",
        [LocalizationKeys.CustomerSyncStarted] = "RII_CUSTOMERS sync job started at {0}.",
        [LocalizationKeys.CustomerSyncAborted] = "RII_CUSTOMERS sync aborted because RII_FN_CARI call failed.",
        [LocalizationKeys.CustomerSyncSkipped] = "RII_CUSTOMERS sync skipped because RII_FN_CARI returned no rows.",
        [LocalizationKeys.CustomerSyncCompleted] = "RII_CUSTOMERS sync completed. created={0}, updated={1}, reactivated={2}, failed={3}, skipped={4}, duplicatePayload={5}.",
        [LocalizationKeys.CustomerSyncRecordFailed] = "RII_CUSTOMERS sync record failed. CustomerCode: {0}",
        [LocalizationKeys.CustomerSyncFailureLogWriteFailed] = "RII_CUSTOMERS sync failure could not be written to RII_HANGFIRE_JOB_LOGS. CustomerCode: {0}",
        [LocalizationKeys.HangfireJobFailed] = "Hangfire job failed. JobId: {0}, Job: {1}, RetryCount: {2}, Reason: {3}",
        [LocalizationKeys.HangfireJobSucceeded] = "Hangfire job succeeded. JobId: {0}, Job: {1}, Latency: {2}, Duration: {3}",
        [LocalizationKeys.HangfireMovedToDeadLetter] = "Moved to dead-letter queue.",
        [LocalizationKeys.HangfireSqlLogFailed] = "Hangfire job log SQL write failed. JobId: {0}",
        [LocalizationKeys.HangfireDeadLetterCaptured] = "Dead-letter Hangfire job captured. JobId: {0}, JobName: {1}, Queue: {2}, RetryCount: {3}, Reason: {4}, Exception: {5} - {6}",
        [LocalizationKeys.HangfireHeartbeatExecuted] = "UTS Hangfire heartbeat job executed at {0}.",
        [LocalizationKeys.UtsLogNotFound] = "UTS log record not found.",
        [LocalizationKeys.UretimTarihiNotFound] = "Production date record not found."
    };

    private static readonly IReadOnlyDictionary<string, string> Tr = new Dictionary<string, string>
    {
        [LocalizationKeys.RequiredField] = "Bu alan zorunludur.",
        [LocalizationKeys.InvalidEmailAddress] = "Lutfen gecerli bir e-posta adresi girin.",
        [LocalizationKeys.MaximumLengthExceeded] = "Izin verilen en fazla uzunluk {0} karakterdir.",
        [LocalizationKeys.MinimumLengthRequired] = "Gerekli en az uzunluk {0} karakterdir.",
        [LocalizationKeys.InvalidRange] = "Deger {0} ile {1} arasinda olmalidir.",
        [LocalizationKeys.InvalidRequest] = "Gonderilen istek gecersiz.",
        [LocalizationKeys.PasswordResetEmailSubject] = "Sifre Sifirlama",
        [LocalizationKeys.PasswordResetEmailGreeting] = "Merhaba {0},",
        [LocalizationKeys.PasswordResetEmailBody] = "Sifre sifirlama talebiniz alindi.",
        [LocalizationKeys.PasswordResetEmailToken] = "Sifirlama tokeni: {0}",
        [LocalizationKeys.PasswordResetEmailExpiry] = "Bu token 2 saat boyunca gecerlidir.",
        [LocalizationKeys.Forbidden] = "Bu kaynaga erisim yetkiniz yok.",
        [LocalizationKeys.PermissionGroupInUse] = "Bu yetki grubu bir veya daha fazla kullaniciya atanmis oldugu icin silinemez.",
        [LocalizationKeys.SystemPermissionGroupCannotBeDeleted] = "Sistem yetki gruplari silinemez.",
        [LocalizationKeys.StockSyncStarted] = "RII_STOCK sync isi {0} tarihinde basladi.",
        [LocalizationKeys.StockSyncAborted] = "RII_FN_STOK cagrisi basarisiz oldugu icin RII_STOCK sync durduruldu.",
        [LocalizationKeys.StockSyncSkipped] = "RII_FN_STOK sonuc dondurmedigi icin RII_STOCK sync atlandi.",
        [LocalizationKeys.StockSyncCompleted] = "RII_STOCK sync tamamlandi. olusan={0}, guncellenen={1}, hatali={2}, atlanan={3}, yinelenenPayload={4}.",
        [LocalizationKeys.StockSyncRecordFailed] = "RII_STOCK sync kaydi basarisiz. StokKodu: {0}",
        [LocalizationKeys.StockSyncFailureLogWriteFailed] = "RII_STOCK sync hata kaydi RII_HANGFIRE_JOB_LOGS tablosuna yazilamadi. StokKodu: {0}",
        [LocalizationKeys.CustomerSyncStarted] = "RII_CUSTOMERS sync isi {0} tarihinde basladi.",
        [LocalizationKeys.CustomerSyncAborted] = "RII_FN_CARI cagrisi basarisiz oldugu icin RII_CUSTOMERS sync durduruldu.",
        [LocalizationKeys.CustomerSyncSkipped] = "RII_FN_CARI sonuc dondurmedigi icin RII_CUSTOMERS sync atlandi.",
        [LocalizationKeys.CustomerSyncCompleted] = "RII_CUSTOMERS sync tamamlandi. olusan={0}, guncellenen={1}, yenidenAktiflesen={2}, hatali={3}, atlanan={4}, yinelenenPayload={5}.",
        [LocalizationKeys.CustomerSyncRecordFailed] = "RII_CUSTOMERS sync kaydi basarisiz. CariKodu: {0}",
        [LocalizationKeys.CustomerSyncFailureLogWriteFailed] = "RII_CUSTOMERS sync hata kaydi RII_HANGFIRE_JOB_LOGS tablosuna yazilamadi. CariKodu: {0}",
        [LocalizationKeys.HangfireJobFailed] = "Hangfire isi basarisiz. JobId: {0}, Is: {1}, RetryCount: {2}, Sebep: {3}",
        [LocalizationKeys.HangfireJobSucceeded] = "Hangfire isi basarili. JobId: {0}, Is: {1}, Gecikme: {2}, Sure: {3}",
        [LocalizationKeys.HangfireMovedToDeadLetter] = "Dead-letter kuyruguna tasindi.",
        [LocalizationKeys.HangfireSqlLogFailed] = "Hangfire is logu SQL kaydi basarisiz. JobId: {0}",
        [LocalizationKeys.HangfireDeadLetterCaptured] = "Dead-letter Hangfire isi yakalandi. JobId: {0}, JobName: {1}, Queue: {2}, RetryCount: {3}, Sebep: {4}, Exception: {5} - {6}",
        [LocalizationKeys.HangfireHeartbeatExecuted] = "UTS Hangfire heartbeat isi {0} tarihinde calisti.",
        [LocalizationKeys.UtsLogNotFound] = "UTS log kaydi bulunamadi.",
        [LocalizationKeys.UretimTarihiNotFound] = "Uretim tarihi kaydi bulunamadi."
    };

    public static string Get(string key, params object[] args)
    {
        var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("tr", StringComparison.OrdinalIgnoreCase)
            ? Tr
            : En;

        var template = culture.TryGetValue(key, out var value) ? value : key;
        return args.Length == 0 ? template : string.Format(template, args);
    }
}
