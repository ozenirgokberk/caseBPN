# Banking System Microservice Architecture

Bu proje, modern bir bankacılık sistemini mikroservis mimarisi kullanarak implemente eden bir uygulamadır. Sistem, müşteri yönetimi, para transferi ve bildirim işlemlerini ayrı servisler halinde yönetir.

## Sistem Mimarisi

Proje aşağıdaki mikroservisleri içerir:

### 1. API Gateway
- Tüm mikroservislere gelen istekleri yöneten ve yönlendiren ana giriş noktası
- Ocelot kullanılarak implement edilmiş routing ve load balancing özellikleri
- Port: 5000

### 2. User Management API
- Kullanır kayıt ve kimlik doğrulama işlemleri
- JWT tabanlı authentication
- Kullanıcı CRUD operasyonları
- Port: 5001

### 3. Customer Registration API
- Müşteri kayıt ve yönetim işlemleri
- Müşteri bilgilerinin validasyonu
- Müşteri CRUD operasyonları
- Port: 5002

### 4. Money Transfer API
- Para transfer işlemleri
- Transfer limit kontrolleri
- İşlem geçmişi takibi
- Port: 5003

### 5. Notification API
- Kullanıcılara bildirim gönderme servisi
- Email ve SMS bildirimleri
- Asenkron bildirim işleme
- Port: 5004

## Teknolojiler

- **.NET 8.0**: Ana geliştirme platformu
- **Entity Framework Core**: ORM ve veritabanı işlemleri
- **SQL Server**: Veritabanı
- **RabbitMQ**: Message broker
- **JWT**: Authentication
- **Ocelot**: API Gateway
- **Docker**: Konteynerizasyon
- **Swagger**: API dokümantasyonu

## Başlangıç

### Gereksinimler

- .NET 8.0 SDK
- SQL Server
- Docker
- RabbitMQ

### Kurulum

1. Repository'yi klonlayın:
```bash
git clone https://github.com/ozenirgokberk/caseBPN.git
```

2. Veritabanlarını oluşturun:
```bash
cd UserManagement.API
dotnet ef database update

cd ../CustomerRegistration.API
dotnet ef database update

cd ../MoneyTransfer.API
dotnet ef database update

cd ../Notification.API
dotnet ef database update
```

3. Docker ile çalıştırın:
```bash
docker-compose up -d
```

### API Endpoints

#### User Management API
- POST /api/auth/register - Yeni kullanıcı kaydı
- POST /api/auth/login - Kullanıcı girişi
- GET /api/users - Kullanıcı listesi
- GET /api/users/{id} - Kullanıcı detayı

#### Customer Registration API
- POST /api/customers - Yeni müşteri kaydı
- GET /api/customers - Müşteri listesi
- GET /api/customers/{id} - Müşteri detayı
- PUT /api/customers/{id} - Müşteri güncelleme

#### Money Transfer API
- POST /api/transfers - Para transferi
- GET /api/transfers - Transfer listesi
- GET /api/transfers/{id} - Transfer detayı

#### Notification API
- POST /api/notifications - Bildirim gönderme
- GET /api/notifications - Bildirim listesi

## Mimari Yapı

Proje Clean Architecture prensiplerine uygun olarak geliştirilmiştir:

```
├── Core
│   ├── Domain
│   │   ├── Entities
│   │   └── Interfaces
│   └── Application
│       ├── DTOs
│       ├── Interfaces
│       └── Services
├── Infrastructure
│   ├── Persistence
│   │   ├── Configurations
│   │   ├── Repositories
│   │   └── Context
│   └── Services
└── API
    ├── Controllers
    ├── Middleware
    └── Program.cs
```

## Güvenlik

- JWT tabanlı authentication
- Role-based authorization
- Input validation
- SQL injection koruması
- XSS koruması
- Rate limiting

## Monitoring ve Logging

- Structured logging
- Performance metrics
- Health checks
- Error tracking

## Katkıda Bulunma

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasına bakınız.

## İletişim

Gökberk Özenir - [@ozenirgokberk](https://github.com/ozenirgokberk)

Project Link: [https://github.com/ozenirgokberk/caseBPN](https://github.com/ozenirgokberk/caseBPN) 