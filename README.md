# SmartTrash.API

Sử dụng Visual Studio để mở project: File SmartTrash.API.sln

Trước khi chạy BE, mọi người mở file appsettings.json và sửa lại phần ConnectionString.database như sau:
- "Server=localhost;Port={port};Database={tên db};Uid=root;Pwd={mật khẩu}".
- Các phần để trong dấu ngoặc nhọn được thay đổi theo properties localhost của mọi người và xóa dấu ngoặc nhọn đi.

Hiện tại, tên Database đang để mặc định là smart_trash, cần tạo sẵn một database smart_trash trước khi chạy file sql.

Các api cần chú thích:

Các api chung: 
- GET/api/v1/{entity}: Lấy toàn bộ thông tin của tất cả entity.
- GET/api/v1/{entity}/id: Lấy thông tin của entity theo id đầu vào
- POST/api/v1/{entity}: Thêm 1 bản ghi entity
- PUT/api/v1/{entity}/{id}: Sửa bản ghi theo id đầu vào
- DELETE/api/v1/{entity}/{id}: Xóa bản ghi theo id đầu vào

Các entity hiện tại đang có:
- Garage: Thông tin garage.
- GarbageTruck: Thông tin xe thu gom.
- Notification: Thông báo.
- RecycleBin: Thông tin thùng rác.
- RouteNode: Thông tin về khoảng cách, mức độ ưu tiên giữa thùng rác và xe.
- User: Thông tin người dùng

Khi chạy BE, một tab SwaggerUI sẽ xuất hiện với url: "https://localhost:{port}" với port như chúng mình đang chạy là 7145

VD: Để lấy dữ liệu toàn bộ thùng rác gọi theo url: "https://localhost:7145/api/v1/RecycleBin" với http method là GET

Garbagetruck: 
- GET/api/v1/Garbagetruck/str: api thực hiện kiểm tra các thùng rác, nếu đầy sẽ tự động gửi xe đi thu gom rác bằng việc cập nhật trực tiếp một chuỗi các id của thùng rác cần thu theo thứ tự. Các id sẽ được cập nhật vào cột RecyclebinListID. Bên FE sẽ chạy api này liên tục sau 1 khoảng thời gian nào đấy, coi như là cập nhật so với thời gian thực, sau đó lấy dữ liệu RecyclebinListID của xe để hiển thị ra map.

RecycleBin: 
- POST/api/v1/RecycleBin/clearTrash: api thực hiện cập nhật toàn bộ độ đầy các thùng rác đầu vào về 0 (thu gom xong). Đầu vào là một chuỗi các id thùng rác ngăn cách nhau bởi dấu ";". Bên FE sau khi demo đường đi của xe có thể gọi api này để cập nhật các thùng rác mà xe đã đi qua.

UPDATE API TÌM ĐƯỜNG:
- Proc_GarbageTruck_NextWay: Store database cập nhật vị trí hiện tại của xe theo thùng rác tiếp theo, xóa thùng rác đó khỏi id list, trả về danh sách xe thu gom.
- Proc_RecycleBin_GetFull: Store database lấy những thùng rác không bị hỏng, không đang đổ, sức chứa > 80%.
- Lấy kho gần nhất với xe tại vị trí hiện tại, nếu là kho ban đầu thì không thêm, nếu khác thì cập nhật thêm id kho phụ.
- Cập nhật lại api key để dùng api tìm đường.
