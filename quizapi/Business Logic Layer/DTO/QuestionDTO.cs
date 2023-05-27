

using System.ComponentModel.DataAnnotations;

namespace quizapi.Business_Logic_Layer.DTO
{
    public class QuestionDTO
    {

        public int QnId{ get; set; }
        [Required]
        public string QnInWords { get; set; }


        [Required]
        public string Option1 { get; set; }
        [Required]
        public string Option2 { get; set; }
        [Required]
        public string Option3 { get; set; }
        [Required]
        public string Option4 { get; set; }
        [Required]

        public int Answer { get; set; }
        

        

       

    }
}