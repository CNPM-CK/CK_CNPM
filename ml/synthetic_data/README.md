# ECOS synthetic renewal data

Bộ sinh này tạo dữ liệu giả lập cho bài toán dự đoán khả năng khách hàng ký **một hợp đồng kế tiếp** với ECOS. Dữ liệu được sinh theo chuỗi sự kiện, không tạo trực tiếp một dòng feature rồi gán nhãn bằng ngưỡng.

## Quy tắc nghiệp vụ

- Một khách hàng có thể có nhiều hợp đồng.
- Một hợp đồng chứa toàn bộ các đợt quan trắc thuộc phạm vi của hợp đồng đó.
- Một đợt quan trắc không phải một hợp đồng mới.
- Hợp đồng kế tiếp là một bản ghi hợp đồng mới và liên kết với hợp đồng trước bằng `previous_contract_id`/`successor_contract_id`.
- `T` là ngày kết thúc hợp đồng hiện tại.
- Mỗi hợp đồng chỉ có một snapshot chính thức tại `T - 60 ngày`.
- Snapshot chỉ tổng hợp các sự kiện có ngày hoàn thành không muộn hơn ngày snapshot.
- Nhãn bằng `1` khi khách hàng thực sự ký hợp đồng kế tiếp trong cửa sổ từ sau snapshot đến `T + 90 ngày`.
- Nếu cửa sổ quan sát chưa kết thúc thì label để trống và bản ghi chỉ nằm trong `prediction_snapshots.csv`, không được đưa vào training.

```text
Bắt đầu hợp đồng ─── sự kiện quan trắc ─── T-60 ─── 60 ngày cuối ─── T ─── T+90
                                           │                              │
                                      tạo snapshot                  chốt label
```

## Vì sao nhãn không phải một công thức lộ thiên?

Pipeline tạo theo thứ tự:

1. Sinh hồ sơ khách hàng và các đặc điểm tiềm ẩn như nhu cầu tuân thủ, độ nhạy giá, mức trung thành và biến động kinh doanh.
2. Sinh hợp đồng và các đợt quan trắc theo thời gian.
3. Sinh trễ hạn, thời gian xử lý, sự cố, báo cáo sửa lại, phản hồi và các cú sốc vận hành có tương quan.
4. Tổng hợp feature từ dữ liệu quan sát được trước `T-60`.
5. Tiếp tục mô phỏng 60 ngày cuối mà model không được nhìn thấy.
6. Khách hàng chọn một trong ba kết quả `renewed`, `switched_provider`, `paused_service` bằng một quá trình quyết định ngẫu nhiên có nhiễu, cạnh tranh và biến động tương lai.
7. Label được suy ra từ việc có hay không có hợp đồng kế tiếp, không được gán trực tiếp từ `average_delay_days` hoặc `completion_rate`.

Các biến tiềm ẩn và sự kiện sau snapshot không có trong `training_dataset.csv`. Vì thế hai hợp đồng có feature gần giống nhau vẫn có thể nhận kết quả khác nhau.

## Chạy generator

Yêu cầu Python 3.10 trở lên, không cần cài package ngoài.

```powershell
cd ml\synthetic_data
python generate.py
```

Chạy với config/output khác:

```powershell
python generate.py --config config.json --output output
```

Chạy test:

```powershell
python -m unittest discover -s tests -v
```

## File đầu ra

| File | Mục đích |
|---|---|
| `customers.csv` | Khách hàng synthetic; tên không đại diện doanh nghiệp thật. |
| `contracts.csv` | Chuỗi hợp đồng, liên kết hợp đồng trước/sau và kết quả thực tế. |
| `monitoring_rounds.csv` | Các đợt quan trắc cùng ngày kế hoạch, ngày trả kết quả và sự cố. |
| `prediction_snapshots.csv` | Một snapshot `T-60` cho mọi hợp đồng, kể cả hợp đồng chưa có label. |
| `training_dataset.csv` | Chỉ những snapshot đã quan sát đủ `T+90`, dùng để train. |
| `feature_columns.json` | Danh sách feature chính thức và cột tuyệt đối không đưa vào model. |
| `generation_report.json` | Tỷ lệ tái ký, cold-start, missing data và phân phối chính. |
| `manifest.json` | Seed, config và SHA-256 của từng artifact. |

Output CSV được `.gitignore` để tránh làm repository phình to. Có thể tái tạo chính xác bằng `config.json` và seed đã lưu.

## Feature và cold-start

Feature model mặc định chỉ dùng thông tin có thể suy ra từ schema ECOS hiện tại:

- Thời hạn và tần suất hợp đồng.
- Số đợt dự kiến, đến hạn, đã hoàn thành và đang quá hạn tại cutoff.
- Tỷ lệ hoàn thành, đúng hạn, độ trễ và thời gian xử lý.
- Thống kê 90 ngày gần nhất.
- Số hợp đồng trước, thời gian quan hệ và thống kê lịch sử khách hàng.
- Cờ cho biết feature hiện tại/lịch sử có sẵn hay không.

Hợp đồng đầu tiên có `has_customer_history = 0`. Model vẫn sử dụng dữ liệu của hợp đồng hiện tại đến `T-60`. Nếu chưa có đợt hoàn thành, các thống kê tương ứng để trống và có `current_metrics_available = 0`.

## Chống leakage

Chỉ các cột trong `feature_columns.json` được đưa vào pipeline model. Không được dùng:

- `successor_contract_id`;
- `renewal_decision_date`;
- `renewal_outcome`;
- bất kỳ đợt quan trắc nào hoàn thành sau `snapshot_date`;
- file `latent_debug_DO_NOT_TRAIN.csv` nếu bật chế độ debug.

Train/validation/test được chia theo `snapshot_date`, không chia random. Khi tạo thêm nhiều snapshot cho một hợp đồng trong tương lai, phải group theo `contract_id` để tránh cùng hợp đồng xuất hiện ở nhiều split.

## Giới hạn bắt buộc phải công khai

Toàn bộ dữ liệu là synthetic và không chứa bản ghi vận hành của doanh nghiệp. Metric chỉ đo khả năng học lại thế giới do simulator tạo ra; nó không chứng minh khả năng tổng quát hóa sang khách hàng thật. `T-60` và cửa sổ label `T+90` là giả định nghiệp vụ của project, chưa được xác nhận bằng dữ liệu doanh nghiệp.

