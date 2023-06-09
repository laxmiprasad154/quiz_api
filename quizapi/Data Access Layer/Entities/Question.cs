﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;




namespace quizapi.Data_Access_Layer.Entities
{
    public class Question
    {
        

        [Key]
        public int QnId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string QnInWords { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Option1 { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Option2 { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Option3 { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Option4 { get; set; }
        public int Answer { get; set; }

        
        

    }
}

