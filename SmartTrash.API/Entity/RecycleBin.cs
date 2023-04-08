﻿using SmartTrash.API.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTrash.API.Entity
{
    [Table("recyclebin")]
    public class RecycleBin
    {
        [Key]
        public Guid RecycleBinID { get; set; }

        [Required]
        public RecycleBinStatus RecyclebinStatus { get; set; }

        [Required]
        public int Usages { get; set; }

        [Required]
        public int DaySinceLastCollection { get; set; }

        [Required]
        public string? Location { get; set; }

    }
}
