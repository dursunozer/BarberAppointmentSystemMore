# Barber Appointment System

[English README](README.md)

Barber Appointment System, berber salonu süreçlerini yönetmek için geliştirilmiş bir ASP.NET Core MVC web uygulamasıdır. Uygulama; müşteri kayıtları, çalışanlar, hizmetler, randevular ve role dayalı kullanıcı oturumları gibi temel salon yönetimi işlemlerini destekler. Projede Entity Framework Core, SQL Server LocalDB ve Admin/Müşteri/Çalışan rolleri için ayrı paneller kullanılmıştır.

## Özellikler

- **Admin, Müşteri ve Çalışan rolleri** için role dayalı giriş sistemi
- **Admin paneli** ile günlük kazanç ve çalışan uygunluk durumu görüntüleme
- **Müşteri yönetimi**: listeleme, ekleme, düzenleme, detay görüntüleme ve silme
- **Çalışan yönetimi**: listeleme, ekleme, düzenleme, detay görüntüleme ve silme
- **Hizmet yönetimi**: hizmet oluşturma, düzenleme, silme ve çalışanlara hizmet atama
- **Randevu alma sistemi**: müşteriler uygun çalışan, hizmet ve tarih seçerek randevu oluşturabilir
- **Çalışan uygunluk takibi**: randevu oluşturulduktan sonra ilgili çalışanın uygunluk durumu güncellenir
- **AI saç modeli önizleme**: müşteri bir fotoğraf yükleyerek harici bir saç modeli API’si üzerinden saç modeli sonucu oluşturabilir
- **ASP.NET Core MVC yapısı**: Controllers, Models, Views, Migrations ve statik dosyalar ile düzenli proje mimarisi

## Kullanılan Teknolojiler

- **C#**
- **ASP.NET Core MVC**
- **.NET 8**
- **Entity Framework Core**
- **SQL Server LocalDB**
- **Cookie Authentication**
- **Razor Views / HTML / CSS / JavaScript**
- **Swagger**
- **Harici Hairstyle API entegrasyonu**

## Proje Yapısı

```text
BarberAppointmentSystemMore/
├── Controllers/          # Admin, müşteri, çalışan, salon, hizmet, randevu ve AI controller dosyaları
├── Models/               # Entity modelleri ve DbContext
├── Views/                # Özelliklere göre ayrılmış Razor sayfaları
├── Migrations/           # Entity Framework Core veritabanı migration dosyaları
├── wwwroot/              # CSS, JS ve görseller gibi statik dosyalar
├── Program.cs            # Uygulama başlangıcı ve middleware ayarları
├── appsettings.json      # Veritabanı bağlantısı ve uygulama ayarları
└── BarberAppointmentSystem.csproj
```

## Ana Modüller

### Admin Paneli

Admin kullanıcıları müşteri, çalışan ve hizmet kayıtlarını yönetebilir. Dashboard ekranında günlük kazanç ve çalışanların uygunluk durumları görüntülenebilir.

### Müşteri Paneli

Müşteriler sisteme giriş yapabilir, kendi panellerine erişebilir ve uygun çalışan, hizmet ve tarih seçerek randevu oluşturabilir.

### Çalışan Paneli

Çalışan kullanıcıları role dayalı korumaya sahip kendi alanlarına erişebilir. Çalışan kayıtları CRUD işlemleriyle yönetilebilir.

### Randevu Sistemi

Randevu modülü; müşteri, çalışan, hizmet, tarih ve durum bilgileriyle randevu oluşturur. Randevu oluşturulduğunda seçilen çalışanın uygunluk durumu güncellenir.

### AI Saç Modeli Önizleme

AI modülü, müşterilerin bir fotoğraf yükleyerek harici RapidAPI saç modeli servisi üzerinden saç modeli önizlemesi almasını sağlar. Yüklenen dosya boyut ve uzantı kontrollerinden geçirilir.

## Kurulum

### Gereksinimler

Aşağıdaki araçların kurulu olması gerekir:

- Visual Studio 2022 veya daha yeni bir sürüm
- .NET 8 SDK
- SQL Server LocalDB veya SQL Server Express

### Adımlar

1. Depoyu klonlayın:

```bash
git clone https://github.com/dursunozer/BarberAppointmentSystemMore.git
cd BarberAppointmentSystemMore
```

2. Bağımlılıkları yükleyin:

```bash
dotnet restore
```

3. `appsettings.json` içindeki veritabanı bağlantısını kontrol edin:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=BarberAppointmentSystem;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

4. Veritabanı migration’larını uygulayın:

```bash
dotnet ef database update
```

5. Projeyi çalıştırın:

```bash
dotnet run
```

6. Terminalde görünen localhost adresini tarayıcıda açın.

## AI API Ayarı

Projede harici bir saç modeli API istemcisi bulunur. AI saç modeli özelliğini kullanmadan önce `Program.cs` içindeki örnek API anahtarını kendi RapidAPI anahtarınızla değiştirin:

```csharp
client.DefaultRequestHeaders.Add("x-rapidapi-key", "Your-Api-Key");
```

Güvenlik için API anahtarlarını doğrudan kaynak kodda tutmak yerine user secrets veya environment variables ile saklamanız önerilir.

## Geliştirilebilecek Noktalar

- API anahtarlarını ve hassas ayarları environment variable içine taşımak
- Şifreleri düz metin yerine hashlenmiş şekilde saklamak
- Randevu çakışmalarını saat aralığına göre daha detaylı kontrol etmek
- Unit ve integration testleri eklemek
- E-posta veya SMS randevu bildirimi eklemek
- Mobil uyumluluğu artırmak
- Admin dashboard ekranına daha detaylı raporlar eklemek

## Lisans

Projede şu an lisans dosyası bulunmamaktadır. Projenin nasıl kullanılmasını istediğinize göre uygun bir lisans ekleyebilirsiniz.
