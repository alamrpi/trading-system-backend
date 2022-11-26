
namespace smartshop.Business.ViewModels
{
    public class CategoryViewModel
    {
        public int GroupId { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; }
    }
}
