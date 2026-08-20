# ECOS — Environmental Contract Oversight System

Ứng dụng desktop hỗ trợ quản lý quy trình hợp đồng quan trắc môi trường, được phát triển dưới dạng dự án học phần theo nghiệp vụ tham khảo từ doanh nghiệp.

## Trạng thái dự án

Các chức năng chính đã được cài đặt:

- Quản lý khách hàng, hợp đồng và chu kỳ hợp đồng.
- Lập kế hoạch, phân công và theo dõi các đợt quan trắc.
- Nhập kết quả, kiểm tra ngưỡng và xuất báo cáo.
- Cảnh báo tiến độ, gửi email và xác thực OTP.
- Trợ lý RAG tiếng Việt truy xuất văn bản quan trắc/QCVN có trang, URL, hiệu lực,
  checksum và phiên bản index trước khi gọi Gemini qua REST.
- Dự báo khả năng tái ký từ snapshot T-60 qua ML API; Gemini giải thích KPI nhưng
  không được thay đổi xác suất của model.
- Tìm kiếm bằng giọng nói với Whisper local và iFLYTEK tùy chọn.

Giới hạn hiện tại:

- Dữ liệu đi kèm là dữ liệu mẫu/synthetic, không phải dữ liệu vận hành thật của doanh nghiệp.
- Corpus RAG ban đầu mới bao phủ Thông tư 10/2021 và một số QCVN phổ biến; chưa
  bao phủ mọi quy chuẩn ngành/địa phương, giấy phép môi trường hay TCVN có bản quyền.
- Dự báo tái ký đã tích hợp vào chat nhưng model chỉ được huấn luyện/đánh giá trên
  dữ liệu synthetic, chưa được kiểm chứng trên dữ liệu doanh nghiệp.
- Đây chưa phải hệ thống production và không nên dùng đầu ra AI để ra quyết định môi trường hoặc kinh doanh.

## Công nghệ

| Thành phần | Công nghệ |
|---|---|
| Desktop UI | C#, .NET 8, WinForms |
| Kiến trúc ứng dụng | GUI / BLL / DAL |
| Cơ sở dữ liệu | SQL Server, stored procedures |
| Trợ lý AI | RAG sparse/hybrid, Gemini API qua REST, citations có checksum |
| Dự báo tái ký | FastAPI, scikit-learn HistGradientBoosting, snapshot T-60 |
| Giọng nói | Whisper.net; iFLYTEK IAT tùy chọn |
| Nhận diện khuôn mặt | Emgu CV |
| Báo cáo và thông báo | RDLC, PDF/Excel, SMTP |

## Cấu trúc repository

```text
.
├── QuanLyHopDongQuanTrac/
│   ├── GUI/                # WinForms và cấu hình chạy ứng dụng
│   ├── BLL/                # Nghiệp vụ, Gemini, email và speech
│   ├── DAL/                # Truy cập SQL Server
│   ├── DTO/                # DTO và bộ nạp cấu hình
│   ├── 3010.sql            # Schema, stored procedures và dữ liệu demo
│   └── QuanLyHopDongQuanTrac.sln
├── database/migrations/    # Migration bổ sung, chạy sau script database chính
├── docs/                   # Quy trình nghiệp vụ đích và tài liệu portfolio
├── ml/synthetic_data/      # Pipeline sinh dữ liệu sự kiện và snapshot T-60 cho ML
└── README.md
```

## Chạy project ở máy local

### 1. Yêu cầu

- Windows và Visual Studio 2022.
- .NET 8 SDK.
- SQL Server và SQL Server Management Studio.
- Git.

### 2. Clone repository

```powershell
git clone https://github.com/CNPM-CK/Ecos-environmental-monitoring-system.git
cd Ecos-environmental-monitoring-system
```

### 3. Khởi tạo database

Tạo database `QuanLyHopDongQuanTrac`, sau đó chạy script:

```text
QuanLyHopDongQuanTrac/3010.sql
```

Script có chứa dữ liệu demo. Hãy kiểm tra nội dung trước khi chạy trên một SQL Server có dữ liệu cần giữ lại.

Chạy tiếp migration idempotent để lưu audit dự báo tái ký:

```text
database/migrations/20260819_ai_renewal_prediction.sql
```

### 4. Cấu hình local

Repository không cung cấp API key hoặc mật khẩu dùng chung. Mỗi người chạy project phải sử dụng credential của chính mình.

Sao chép file mẫu:

```powershell
Copy-Item QuanLyHopDongQuanTrac/GUI/appsettings.local.example.json QuanLyHopDongQuanTrac/GUI/appsettings.local.json
```

Sau đó chỉnh `appsettings.local.json`:

- `ConnectionStrings:DefaultConnection`: connection string SQL Server của máy đang chạy.
- `Gemini:ApiKey`: Gemini API key của người chạy.
- `Gemini:Model`: model Gemini dùng để giải thích.
- `RenewalPrediction:BaseUrl`: địa chỉ service `ai-taiky-api`, mặc định
  `http://127.0.0.1:8000/`.
- `RenewalPrediction:TimeoutSeconds`: timeout khi ECOS gọi ML API.
- `EnvironmentalKnowledge:BaseUrl`: địa chỉ RAG API; mặc định dùng cùng
  `ai-taiky-api` tại `http://127.0.0.1:8000/`.
- `EnvironmentalKnowledge:TimeoutSeconds`: timeout truy xuất kho tri thức.
- `Speech:IAT:*`: tùy chọn; nếu bỏ trống, chức năng giọng nói có thể fallback sang Whisper local.
- `Smtp:OtpAndReports:*`: tài khoản SMTP dùng cho OTP và gửi báo cáo.
- `Smtp:Notifications:*`: tài khoản SMTP riêng dùng cho email cảnh báo.

`appsettings.local.json` đã được `.gitignore` loại trừ. Không commit file này, không gửi nó qua chat và không đóng gói nó vào bản public.

Ứng dụng cũng hỗ trợ biến môi trường. Biến môi trường có độ ưu tiên cao hơn file JSON:

| Cấu hình | Biến môi trường |
|---|---|
| SQL Server | `ECOS_ConnectionStrings__DefaultConnection` |
| Gemini | `ECOS_Gemini__ApiKey` |
| Gemini model | `ECOS_Gemini__Model` |
| ML API URL | `ECOS_RenewalPrediction__BaseUrl` |
| ML API timeout | `ECOS_RenewalPrediction__TimeoutSeconds` |
| RAG API URL | `ECOS_EnvironmentalKnowledge__BaseUrl` |
| RAG API timeout | `ECOS_EnvironmentalKnowledge__TimeoutSeconds` |
| iFLYTEK App ID | `ECOS_Speech__IAT__AppId` |
| iFLYTEK API key | `ECOS_Speech__IAT__ApiKey` |
| iFLYTEK API secret | `ECOS_Speech__IAT__ApiSecret` |
| SMTP OTP/report address | `ECOS_Smtp__OtpAndReports__FromAddress` |
| SMTP OTP/report password | `ECOS_Smtp__OtpAndReports__Password` |
| SMTP notification address | `ECOS_Smtp__Notifications__FromAddress` |
| SMTP notification password | `ECOS_Smtp__Notifications__Password` |

Thứ tự ưu tiên cấu hình là:

```text
environment variables > appsettings.local.json > appsettings.json
```

`appsettings.json` chỉ chứa cấu hình mặc định không nhạy cảm và giá trị secret rỗng.

### 5. Chuẩn bị và chạy ECOS AI API

Đặt repository `ai-taiky-api` cạnh repository ECOS, tạo môi trường Python theo
README của service, rồi chạy:

```powershell
cd ..\ai-taiky-api
.\.venv\Scripts\Activate.ps1
python -m pip install -r requirements-dev.txt
python scripts\download_knowledge_sources.py
python scripts\build_knowledge_index.py
uvicorn api:app --host 0.0.0.0 --port 8000
```

Hai lệnh knowledge tạo sparse RAG, không cần Gemini key. Muốn tạo hybrid index:

```powershell
$env:ECOS_GEMINI_API_KEY="YOUR_OWN_GEMINI_API_KEY"
python scripts\build_knowledge_index.py --with-embeddings
```

Kiểm tra trước khi mở ECOS:

- `http://localhost:8000/health` trả `model_ready: true`.
- `http://localhost:8000/v1/knowledge/status` trả `ready: true`, cùng
  `index_version`, `source_count` và `chunk_count`.

### 6. Chạy ứng dụng

Mở `QuanLyHopDongQuanTrac/QuanLyHopDongQuanTrac.sln`, đặt project `GUI` làm Startup Project và chạy bằng `F5`.

Trong cửa sổ chat, có thể dùng:

```text
Dự báo khả năng tái ký của KH001
Phân tích tái ký hợp đồng HD001
Quy trình lấy, bảo quản và giao nhận mẫu nước mặt gồm những bước nào?
QCVN 08 dùng để đánh giá nước mặt hay giới hạn xả nước thải?
```

Với mã khách hàng, ECOS chọn hợp đồng có ngày kết thúc mới nhất. Trước ngày T-60,
trợ lý chỉ thông báo ngày đủ điều kiện và không gọi model. Từ T-60 trở đi, ECOS lấy
snapshot đã khóa hoặc dựng snapshot T-60 ở lần dự báo đầu tiên, gọi ML API, dùng
Gemini giải thích các KPI tổng hợp và lưu kết quả theo `snapshotId + modelVersion`.
Nếu Gemini lỗi, kết quả ML vẫn được hiển thị cùng phần giải thích dự phòng xác định.

Với câu hỏi nghiệp vụ môi trường, ECOS gọi RAG trước. Gemini chỉ nhận các đoạn đã
truy xuất, phải dẫn `[S#]`; ECOS tự gắn URL văn bản, trang PDF và index version.
Nếu không có nguồn hoặc RAG lỗi, hệ thống không fallback sang câu trả lời pháp lý
không có căn cứ. Thiết kế workflow doanh nghiệp và các gap của schema hiện tại nằm
tại [`docs/ECOS_MONITORING_WORKFLOW_RAG.md`](docs/ECOS_MONITORING_WORKFLOW_RAG.md).

## Quản lý key khi chia sẻ hoặc triển khai

### Người khác clone để chạy thử

Người clone tự tạo key Gemini của họ và đặt vào `appsettings.local.json` hoặc biến môi trường. Chủ repository không chia sẻ key cá nhân.

### Triển khai cho khách hàng

Không đặt Gemini key trong source, file cài đặt WinForms hoặc gửi trực tiếp cho người dùng cuối. Secret nằm trong desktop client có thể bị trích xuất dù được mã hóa.

Kiến trúc production dự kiến:

```text
ECOS WinForms → ECOS AI backend (ML + RAG) → Gemini API
```

Gemini key được giữ tại backend bằng secret manager hoặc biến môi trường của server. WinForms xác thực với backend bằng tài khoản/token riêng; backend chịu trách nhiệm phân quyền, giới hạn quota, ghi log và thu hồi quyền truy cập. Với triển khai on-premise, khách hàng tự cấp key của họ vào secret store của server, không đưa key vào máy người dùng.

## Lưu ý bảo mật

- Key từng xuất hiện trong Git phải được xem là đã lộ và cần thu hồi/rotate tại nhà cung cấp.
- Xóa key khỏi phiên bản hiện tại không xóa nó khỏi lịch sử Git.
- Không dùng credential production cho dữ liệu demo hoặc bản build portfolio.
- Trước khi public repository, nên quét secret và xem lại toàn bộ lịch sử commit.

## Sinh dữ liệu cho mô hình tái ký

Pipeline tại [`ml/synthetic_data`](ml/synthetic_data/README.md) mô phỏng chuỗi khách hàng → hợp đồng → nhiều đợt quan trắc, sau đó tạo đúng một snapshot tại `T-60` cho mỗi hợp đồng. Nhãn được suy ra từ việc xuất hiện hợp đồng kế tiếp trong cửa sổ đến `T+90`, có nhiễu và các yếu tố tiềm ẩn thay vì được gán trực tiếp từ một ngưỡng feature.

```powershell
cd ml\synthetic_data
python generate.py
python -m unittest discover -s tests -v
```

Toàn bộ output là synthetic. Các metric huấn luyện sau này chỉ phản ánh khả năng học trên simulator, không chứng minh hiệu quả với dữ liệu doanh nghiệp thật.

## Luồng dự báo tái ký trong ECOS

```text
Chat KHxxx/HDxxx
        ↓
SQL Server lấy snapshot đã khóa hoặc dựng feature chỉ từ dữ liệu đến T-60
        ↓
ai-taiky-api trả xác suất, ngưỡng, model version và evidence level
        ↓
Gemini chỉ giải thích KPI; ECOS tự hiển thị xác suất cố định từ ML
        ↓
AI_RenewalPrediction lưu snapshot JSON và kết quả để audit
```

Service hiện đóng gói một `HistGradientBoostingClassifier`. Artifact hiện tại đạt
test ROC-AUC 0.885 và average precision 0.925 trên 5.153 snapshot synthetic có nhãn.
Các con số này phải luôn được mô tả kèm phạm vi dữ liệu synthetic.

## Nhóm phát triển

Nhóm 19 — STech:

| Thành viên | Vai trò |
|---|---|
| Nguyễn Hoàng Sơn | Project Manager, Tester |
| Phan Đức Tài | Business Analyst, Tester |
| Tôn Quốc Thái | Developer, Designer |
| Trần Quang Thái | Developer, Designer |
| Phan Trí Tâm | Developer, Designer |
