using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BinarySurveyPOC.Models
{
    public class SurveyResponse
    {
        public int SurveyResponseID { get; set; }
        public int SurveyId { get; set; }

        [Required]
        public bool Response { get; set; }

        [ForeignKey("SurveyId")]
        public Survey Survey { get; set; }

        [Required]
        public DateTime AddDate { get; set; }
    }
}