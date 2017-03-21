using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;

namespace BinarySurveyPOC.Models
{
    public class Survey
    {
        [Key]
        public int SurveyID { get; set; }

        [Required]
        [StringLength(140)]
        public string SurveyQuestion { get; set; }

        public DbGeography Location { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime ExpiritionDate { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime AddDate { get; set; }

        public ICollection<SurveyResponse> SurveyResponses { get; set; }

    }
}