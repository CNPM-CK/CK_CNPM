# Quy trình nghiệp vụ quan trắc và ranh giới RAG của ECOS

Tài liệu này là thiết kế đích để ECOS tiến từ bài toán quản lý hợp đồng sang một
workflow quan trắc có thể audit. Căn cứ quy trình ban đầu là Thông tư
10/2021/TT-BTNMT về kỹ thuật quan trắc, QA/QC và quản lý dữ liệu; ngưỡng môi
trường nằm trong từng QCVN. Hồ sơ/giấy phép của cơ sở và phạm vi được phép của
đơn vị quan trắc vẫn là đầu vào bắt buộc.

## Nguyên tắc phân quyền cho AI

```text
Workflow + phân quyền + validation có cấu trúc
    quyết định trạng thái, trường bắt buộc và người phê duyệt

Logic so sánh xác định
    so kết quả với đúng bộ giới hạn đã được người có thẩm quyền chọn và khóa

RAG + Gemini
    tìm, trích dẫn, giải thích nguồn và chỉ ra dữ kiện còn thiếu
```

RAG không được tự chọn QCVN/cột áp dụng, không tự chuyển trạng thái hồ sơ và
không kết luận cơ sở tuân thủ. Điều này đặc biệt quan trọng với QCVN 14:2025 và
QCVN 40:2025 vì còn phụ thuộc đối tượng, lưu lượng, nguồn tiếp nhận/phân vùng,
mốc hồ sơ pháp lý, giấy phép và điều khoản chuyển tiếp.

## Luồng nghiệp vụ đích

| Giai đoạn | Dữ liệu/chứng từ bắt buộc | Kiểm soát chính | RAG hỗ trợ |
|---|---|---|---|
| 1. Tiếp nhận và rà phạm vi | Hợp đồng; mục tiêu; loại cơ sở; giấy phép môi trường; quyết định ĐTM; nguồn thải/nguồn tiếp nhận; phạm vi dịch vụ | Xác định đơn vị làm lấy mẫu, thử nghiệm hay cả hai; người nghiệp vụ duyệt phạm vi | Tìm điều khoản, tạo checklist thông tin còn thiếu; không tự quyết định văn bản áp dụng |
| 2. Thiết kế chương trình/QAPP | Ma trận mẫu; vị trí/toạ độ; thông số; tần suất; phương pháp; QA/QC; lịch; nhân sự/phòng thử nghiệm | Duyệt kế hoạch trước khi triển khai; kiểm tra phương pháp và phạm vi năng lực | Truy xuất yêu cầu lập chương trình, QA/QC, phương pháp và tần suất |
| 3. Chuẩn bị hiện trường | Phiếu công tác; thiết bị và hiệu chuẩn; chai/lọ; hoá chất bảo quản; nhãn; biểu mẫu; phương tiện | Không cho bắt đầu nếu thiết bị/hiệu chuẩn/vật tư hoặc phân công chưa đủ | Sinh checklist theo loại mẫu từ nguồn đã duyệt |
| 4. Lấy mẫu và QC hiện trường | Mã mẫu duy nhất; thời gian; người lấy; GPS; điều kiện hiện trường; phép đo tại chỗ; mẫu trắng/lặp; ảnh và biên bản | Trường bắt buộc, timestamp, audit người sửa; ghi nhận sai lệch | Giải thích thao tác và loại QC; không tạo hộ dữ liệu hiện trường |
| 5. Bảo quản, vận chuyển, giao nhận | Chất bảo quản; nhiệt độ; niêm phong; thời gian rời/nhận; người giao/nhận; tình trạng mẫu | Chain of custody liên tục; quá hạn hoặc sai điều kiện phải gắn nonconformance | Truy xuất yêu cầu bảo quản/giao nhận và đề xuất hành động kiểm tra |
| 6. Phân tích phòng thử nghiệm | Phiếu nhận mẫu; phương pháp; thiết bị; batch; đường chuẩn; QC; LOD/LOQ; kết quả gốc | Chỉ người được phân quyền nhập/duyệt; không sửa đè raw result; lưu phiên bản | Tìm phương pháp và yêu cầu QA/QC; không hợp thức hoá kết quả QC lỗi |
| 7. Rà soát kỹ thuật | Kết quả đã QC; bộ QCVN/cột đã chọn và lý do; giấy phép; sai lệch; người rà soát | So sánh bằng code xác định; khóa rule version; reviewer phê duyệt | Giải thích nguồn và cảnh báo thiếu căn cứ; không phát hành kết luận cuối |
| 8. Báo cáo, giao khách hàng, lưu hồ sơ | Báo cáo versioned; chữ ký/duyệt; biên bản giao; danh sách nơi nhận; hồ sơ gốc; lịch sử sửa | Chỉ bản Approved được phát hành; bản sửa phải tạo revision và audit trail | Tóm tắt kết quả đã duyệt, dẫn nguồn, hỗ trợ tìm hồ sơ |

Nhánh ngoại lệ cần có ở mọi giai đoạn: `Rejected`, `Nonconformance`, `Resampling`
và `Cancelled`; phải ghi lý do, người xử lý, thời điểm và liên kết đến hồ sơ thay thế.

## State machine đề xuất cho một đợt quan trắc

```text
Draft
  → ScopeReviewed
  → ProgramApproved
  → ReadyForField
  → Sampled
  → ReceivedByLaboratory
  → InAnalysis
  → TechnicalReview
  → Approved
  → Delivered
  → Closed
```

Mỗi transition phải là command nghiệp vụ có transaction, quyền thực hiện,
validation trường bắt buộc và audit event; không cho UI cập nhật trực tiếp một số
trạng thái tuỳ ý. `Approved` phải khóa kết quả và bộ quy chuẩn/cột đã dùng; chỉnh
sau đó phải tạo revision.

## Đối chiếu với schema ECOS hiện tại

Đã có nền tảng:

- `KhachHang` → `HopDong` → nhiều `DotQuanTrac`.
- `Dot_Nen`, `Dot_Nen_Ts` lưu vị trí, nền mẫu, thông số và phương pháp dự kiến.
- `KetQuaHeader`, `KetQuaNenMau`, `KetQuaChiTiet` lưu kết quả và báo cáo.
- Dự báo tái ký dùng snapshot T−60 độc lập với workflow kỹ thuật.

Khoảng trống trước khi gọi là workflow doanh nghiệp hoàn chỉnh:

- Chưa có hồ sơ pháp lý của cơ sở và quyết định chọn quy chuẩn/cột có version.
- Chưa có QAPP/approval, thiết bị-hiệu chuẩn, field record và mẫu QC có cấu trúc.
- Chưa có chain of custody, điều kiện bảo quản/vận chuyển và phiếu nhận mẫu.
- Chưa có laboratory batch/QC/raw result, nonconformance và resampling workflow.
- Trạng thái hiện tại còn thô; chưa có transition audit và separation of duties.
- `KetQuaChiTiet.qcvn` là text, chưa đủ để tái lập chính xác bộ giới hạn đã dùng.

Vì các khoảng trống này liên quan trực tiếp đến cách doanh nghiệp thật phân vai,
không nên tự suy đoán rồi nhồi thêm cột vào `3010.sql`. Migration workflow chỉ nên
được chốt sau buổi xác nhận nghiệp vụ ngắn với người dùng/doanh nghiệp.

## RAG đã triển khai trong phiên bản này

- Corpus manifest có nguồn, hiệu lực, chủ đề, URL hồ sơ và ghi chú áp dụng.
- Downloader có URL dự phòng, kiểm tra PDF, số trang, số hiệu văn bản và SHA-256.
- Chunk theo trang/heading; response luôn có trang PDF, URL, checksum và index version.
- Sparse retrieval chạy không cần key; có thể build hybrid bằng Gemini Embedding 2.
- Bộ lọc thời điểm áp dụng và câu hỏi ngoài phạm vi.
- Gemini nhận evidence được phân cách, bị cấm làm theo chỉ dẫn trong evidence và bị
  kiểm tra mã citation; ECOS tự gắn danh sách URL sau câu trả lời.
- Nếu RAG lỗi hoặc không có nguồn, ECOS không fallback sang câu trả lời pháp lý
  không căn cứ.

## Nguồn chính thức ban đầu

- [Thông tư 10/2021/TT-BTNMT](https://vanban.chinhphu.vn/default.aspx?docid=203741&pageid=27160)
- [Hồ sơ ban hành nhóm QCVN 2023](https://vbpl.vn/TW/Pages/vbpq-thuoctinh.aspx?ItemID=159586)
- [Thông tư 05/2025/TT-BTNMT và QCVN 14:2025](https://congbao.chinhphu.vn/van-ban/thong-tu-so-05-2025-tt-btnmt-44496.htm)
- [Thông tư 06/2025/TT-BTNMT và QCVN 40:2025](https://congbao.chinhphu.vn/van-ban/thong-tu-so-06-2025-tt-btnmt-44497.htm)

Trạng thái hiệu lực trong manifest là snapshot review ngày 19/08/2026, không phải
cơ sở dữ liệu pháp luật cập nhật theo thời gian thực.
