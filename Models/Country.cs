using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Master.Microservice.Models
{ 
public partial class Country
{

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name_en { get; set; }
    public string? Name_ar { get; set; }
    public virtual Currency? CurrencyCode { get; set; }
    public int Status { get; set; }

}
}