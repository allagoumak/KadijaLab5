using System.ComponentModel.DataAnnotations;

namespace Lab5.Model
{
    public enum Question
    {
        Earth, Computer
    }
    public class Prediction
    {
        public int PredictionId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [Display(Name = "Url")]
        public string Url { get; set; }

        [Display(Name = "Question")]
        public Question Question { get; set; }
    }
}
