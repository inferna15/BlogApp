# BlogApp

`BlogApp`, kullanıcıların gönderi (post), kategori ve yorum oluşturabildiği modern bir blog platformunun backend servisidir. Bu proje, **.NET 9** üzerinde, sürdürülebilir, test edilebilir ve ölçeklenebilir yazılım geliştirme prensipleri göz önünde bulundurularak, **Clean Architecture** (Temiz Mimari) kullanılarak tasarlanmıştır.

*Not: Bu proje şu an için sadece backend servislerini içermektedir. Yakın zamanda modern bir frontend teknolojisi (React, Vue, Blazor vb.) ile arayüzü geliştirilecektir.*

## Temel Özellikler

- **Kullanıcı Yönetimi:** Kayıt olma, giriş yapma ve rol bazlı yetkilendirme (Admin, User).
- **Kategoriler:** Yöneticilerin gönderileri sınıflandırmak için kategori oluşturması.
- **Gönderiler (Posts):** Kullanıcıların belirli kategoriler altında zengin içerikli gönderiler oluşturması, güncellemesi ve silmesi.
- **Yorumlar (Comments):** Kullanıcıların gönderilere yorum yapabilmesi.
- **Güvenlik:** JWT (JSON Web Token) ile korunan güvenli endpoint'ler.

## Mimari ve Tasarım Desenleri

Projenin temelini, sorumlulukların net bir şekilde ayrıldığı **Clean Architecture** oluşturur. İş mantığı, dış bağımlılıklardan (veritabanı, arayüz, harici servisler) tamamen izole edilmiştir.

- **Clean Architecture:** Proje, `Domain`, `Application`, `Infrastructure` ve `Presentation` olmak üzere dört ana katmana ayrılmıştır. Tüm bağımlılıklar merkeze (Domain) doğrudur.
- **CQRS (Command Query Responsibility Segregation):** Uygulama mantığı, veri değiştiren işlemler (Command'ler) ve veri okuyan işlemler (Query'ler) olarak ikiye ayrılmıştır. Bu, sistemin daha performanslı ve yönetilebilir olmasını sağlar.
- **MediatR Tasarım Deseni:** `Command` ve `Query`'leri, bu istekleri işleyen `Handler`'lardan ayırmak için MediatR kütüphanesi kullanılmıştır. Bu, esnek ve az bağımlı bir yapı sağlar.

## Kullanılan Teknolojiler ve Kütüphaneler

- **Framework:** .NET 9
- **API:** ASP.NET Core Web API
- **Veritabanı:** Entity Framework Core 9
- **Veritabanı Sağlayıcısı:** SQLite (Yerel geliştirme için)
- **Kimlik Doğrulama ve Yetkilendirme:** ASP.NET Core Identity, JWT Bearer Tokens
- **CQRS Implementasyonu:** MediatR
- **Validasyon:** FluentValidation
- **API Dokümantasyonu:** Microsoft.AspNetCore.OpenApi (.NET 9) ve Scalar Arayüzü

## API Endpoint'leri

`(Yetki Gerekli)` olarak işaretlenen endpoint'ler geçerli bir JWT Bearer token gerektirir.

### Auth
- `POST /api/login` - Kullanıcı girişi yapar ve JWT döndürür.
- `POST /api/register` - Yeni bir kullanıcı hesabı oluşturur.

### Kategoriler (Categories)
- `GET /api/categories` - Tüm kategorileri listeler.
- `POST /api/categories` - Yeni bir kategori oluşturur. `(Admin Yetkisi Gerekli)`
- `GET /api/categories/{id}` - Belirtilen ID'ye sahip tek bir kategoriyi getirir.
- `PUT /api/categories/{id}` - Belirtilen kategoriyi günceller. `(Admin Yetkisi Gerekli)`
- `DELETE /api/categories/{id}` - Belirtilen kategoriyi siler. `(Admin Yetkisi Gerekli)`
- `GET /api/categories/{category_id}/posts` - Belirtilen kategoriye ait tüm gönderileri listeler.

### Gönderiler (Posts)
- `GET /api/posts` - Tüm gönderileri listeler (sayfalama eklenebilir).
- `POST /api/posts` - Yeni bir gönderi oluşturur. `(Yetki Gerekli)`
- `PUT /api/posts/{id}` - Belirtilen gönderiyi günceller. `(Yetki ve Sahiplik Kontrolü Gerekli)`
- `DELETE /api/posts/{id}` - Belirtilen gönderiyi siler. `(Yetki ve Sahiplik/Admin Kontrolü Gerekli)`
- `GET /api/posts/{post_id}/comments` - Belirtilen gönderiye ait tüm yorumları listeler.

### Yorumlar (Comments)
- `POST /api/comments` - Bir gönderiye yeni bir yorum ekler. `(Yetki Gerekli)`
- `PUT /api/comments/{id}` - Belirtilen yorumu günceller. `(Yetki ve Sahiplik Kontrolü Gerekli)`
- `DELETE /api/comments/{id}` - Belirtilen yorumu siler. `(Yetki ve Sahiplik/Admin Kontrolü Gerekli)`

### Kullanıcılar (Users)
- `GET /api/users/{user_id}/posts` - Belirtilen kullanıcıya ait tüm gönderileri listeler.

## Projeyi Çalıştırma (Getting Started)

### Gerekli Araçlar

- .NET 9 SDK
- Visual Studio 2022 Preview veya Visual Studio Code

### Kurulum Adımları

1.  **Projeyi Klonlayın:**
    ```sh
    git clone [https://github.com/inferna15/BlogApp.git](https://github.com/inferna15/BlogApp.git)
    cd BlogApp
    ```

2.  **`appsettings.json` Dosyasını Yapılandırın:**
    `BlogApp.Presentation` projesinin içindeki `appsettings.Development.json` dosyasını oluşturun veya düzenleyin. JWT `Secret` anahtarını **kesinlikle değiştirin.**
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Data Source=../BlogApp.db"
      },
      "JwtSettings": {
        "Secret": "BU_KISIM_COK_GIZLI_VE_TAHMIN_EDILEMEZ_UZUN_BIR_ANAHTAR_OLMALI_EN_AZ_32_KARAKTER",
        "Issuer": "[https://api.blogapp.com](https://api.blogapp.com)",
        "Audience": "[https://webapp.blogapp.com](https://webapp.blogapp.com)",
        "ExpiryMinutes": 60
      }
    }
    ```

3.  **Veritabanını Oluşturun (Migration):**
    Projenin veritabanı şemasını oluşturmak için çözümün ana dizininde bir terminal açın ve aşağıdaki komutları çalıştırın:
    ```sh
    # dotnet-ef aracının yüklü olduğundan emin olun: dotnet tool install --global dotnet-ef
    dotnet ef database update --startup-project BlogApp.Presentation
    ```
    Bu komut, `BlogApp.db` adında bir SQLite veritabanı dosyası oluşturacak ve başlangıç verilerini (Admin kullanıcısı, kategoriler vb.) ekleyecektir.

4.  **Uygulamayı Çalıştırın:**
    ```sh
    dotnet run --project BlogApp.Presentation
    ```
    Uygulama varsayılan olarak `https://localhost:7122` ve `http://localhost:5012` gibi portlarda başlayacaktır.

## API'yi Test Etme

1.  Uygulama çalıştıktan sonra tarayıcınızda, API dokümantasyonu için yapılandırılan adrese gidin (örn: `https://localhost:7122/docs`).
2.  Scalar arayüzü karşınıza çıkacaktır.
3.  `POST /api/register` endpoint'ini kullanarak yeni bir kullanıcı kaydedin.
4.  `POST /api/login` endpoint'ini kullanarak giriş yapın ve dönen `token`'ı kopyalayın.
5.  Scalar arayüzünün üst kısmındaki **"Authentication"** bölümüne tıklayın ve kopyaladığınız token'ı "Bearer Token" alanına yapıştırın.
6.  Artık `[Authorize]` ile korunan (yanında kilit ikonu olan) endpoint'lere başarılı bir şekilde istek atabilirsiniz.

---
