using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Master.Microservice.Models{
    public class Currency
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string FName { get; set; }
        public int StatusID { get; set; }
    }



}
