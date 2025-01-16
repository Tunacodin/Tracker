using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utilities.Constants
{
    public class Messages
    {
        public const string AddSucceeded = "Ekleme işlemi başarılı";
        public const string CustomerNotFound = "Musteri bulunamadi";
        public const string CustomerCreated = "Musteri basariyla olusturuldu";
        public const string UpdateSucceeded = "Güncelleme işlemi başarılı";
        public const string DeleteSucceeded = "Silme işlemi başarılı";
        public const string Exist = "Kayıt zaten mevcut";
        public const string NotExist = "Kayıt bulunamadı";
        public const string NotExistCustomer = "Müşteri bulunamadı";
        public const string SaveFail = "Kayıt sırasında bir hata oluştu";
        public const string Fail = "İşlem sırasında bir hata oluştu";
        public const string LoginFail = "Hatalı e-posta veya şifre";
        public const string IdFail = "Hatalı veya eksik ID";
        public const string BalanceFail = "Hesabınızda yeterli bakiye bulunmamakta";
        public const string LoginSucceeded = "Giriş başarılı";
        public const string OtpNeeded = "OTP için mesajlarınızı kontrol edin";
        public const string NeedVerify = "Doğrulama gerekli";
        public const string EmailRequired = "E-posta adresi zorunludur";
        public const string SelectedCustomerDoesNotHaveAccounts =
            "Seçili müşterinin seçilen hesapları bulunmamaktadır";
        public const string SearchFailed = "Arama işlemi başarısız";
        public const string ValidationFailed = "Validasyon hatası";

        public static string InvalidOrExpiredCode = "Geçersiz veya süresi dolmuş doğrulama kodu.";
        public static string LoginSuccess  = "Giriş işlemi başarılı.";
        public static string VerificationCodeSent = "Doğrulama kodu e-posta adresinize gönderildi.";
        public static string SendEmailAsync = "E-posta gönderim işlemi başarılı.";
    }
}
