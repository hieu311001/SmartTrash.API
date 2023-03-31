namespace SmartTrash.API.Enum
{
    public enum RecycleBinStatus
    {
        /// <summary>
        /// Chưa đầy
        /// </summary>
        NotFull = 0,
        /// <summary>
        /// Đầy
        /// </summary>
        Full = 1,
        /// <summary>
        /// Hỏng
        /// </summary>
        Broken = 2,
        /// <summary>
        /// Đang đổ rác
        /// </summary>
        TakingOut = 3,
    }
}
