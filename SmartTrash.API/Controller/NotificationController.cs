﻿using Dapper;
using Microsoft.AspNetCore.Mvc;
using SmartTrash.API.Entity;

namespace SmartTrash.API.Controller
{
    public class NotificationController : BaseController<Notification>
    {
        public NotificationController(IConfiguration configuration) : base(configuration)
        {
        }

        [HttpGet("notifyTruck")]
        public IActionResult CreateNotification()
        {
            // Chuẩn bị câu lệnh 
            var notify = $"Proc_Recyclebin_GetAll";
            var allNotify = $"Proc_Notification_GetAll";

            // Thực hiện gọi vào db để chạy câu lệnh 
            var resultFull = mySqlConnection.Query<RecycleBin>(notify, commandType: System.Data.CommandType.StoredProcedure);
            var resultNotify = mySqlConnection.Query<Notification>(allNotify, commandType: System.Data.CommandType.StoredProcedure);

            List<RecycleBin> recycleBins = (List<RecycleBin>)resultFull;
            List<Notification> notifys = (List<Notification>)resultNotify;
            List<Notification> notifications = new List<Notification>();

            foreach (RecycleBin recycleBin in recycleBins)
            {
                if (recycleBin.RecyclebinStatus == Enum.RecycleBinStatus.Broken)
                {
                    var notification = new Notification()
                    {
                        NotificationID = new Guid(),
                        NotificationType = Enum.NotificationType.RecycleBinBroken,
                        NotificationName = "Thùng rác hỏng",
                        RecycleBinID = recycleBin.RecycleBinID
                    };
                    notifications.Add(notification);
                }
                else if (recycleBin.RecyclebinStatus == Enum.RecycleBinStatus.Full)
                {
                    var notification = new Notification()
                    {
                        NotificationID = new Guid(),
                        NotificationType = Enum.NotificationType.RecycleBinFull,
                        NotificationName = "Thùng rác đầy",
                        RecycleBinID = recycleBin.RecycleBinID
                    };
                    notifications.Add(notification);
                }
            }

            List<Notification> notificationClone = notifications;

            for (var i = 0; i < notifications.Count; i++)
            {
                for (var j = 0; j < notifys.Count; j++)
                {
                    if (notifys[j].RecycleBinID == notifications[i].RecycleBinID)
                    {
                        notificationClone.RemoveAt(i);
                    }
                }
            }

            notifications = notificationClone;

            var insertedRow = 0;
            using (var transaction = mySqlConnection.BeginTransaction())
            {
                try
                {
                    var sqlCommand = $"Proc_Notification_Insert";
                    foreach (var notification in notifications)
                    {
                        insertedRow += mySqlConnection.Execute(sqlCommand, param: notification, transaction: transaction, commandType: System.Data.CommandType.StoredProcedure);
                    }
                    if (insertedRow == notifications.Count)
                    {
                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }
                finally
                {
                    if (mySqlConnection != null && mySqlConnection.State != System.Data.ConnectionState.Closed)
                    {
                        mySqlConnection.Close();
                    }
                }
            }

            // Xử lý kết quả trả về ở db
            if (insertedRow != null)
            {
                return StatusCode(StatusCodes.Status200OK, insertedRow);
            }
            else
            {

                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }


        /// <summary>
        /// Clear các thông báo thùng rác đã được thu gom hoặc sửa
        /// </summary>
        /// <param name="ids">Chuỗi id các thùng rác, ngăn cách bởi dấu ; </param>
        /// <returns></returns>
        [HttpDelete("notify")]
        public IActionResult ClearNotification([FromBody] string ids)
        {
            // Tách dữ liệu id từ chuỗi ids:
            List<string> selectedIds = ids.Split(';').ToList();
            // Chuẩn bị câu lệnh 
            var getCommand = $"Proc_Notification_ClearNotify";
            // Chuẩn bị tham số đầu vào cho câu lệnh sql
            var parameters = new DynamicParameters();

            var whereClause = "";
            foreach (var id in selectedIds)
            {
                if (id.Equals(selectedIds.Last()))
                {
                    whereClause += $"'{id}'";
                }
                else
                {
                    whereClause += $"'{id}', ";
                }
            }
            whereClause = '"' + whereClause + '"';

            parameters.Add("$NotificationID", whereClause);

            // Thực hiện gọi vào db để chạy câu lệnh 
            var result = mySqlConnection.Execute(getCommand, parameters, commandType: System.Data.CommandType.StoredProcedure);
            // Xử lý kết quả trả về ở db
            if (result != null)
            {
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {

                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }
    }
}
