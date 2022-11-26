namespace smartshop.Business.Dtos
{
    public class DropdownDto
    {
        public string Text { get; set; }
        public object Value { get; set; }
        public DropdownDto(string text, object value)
        {
            Text = text;
            Value = value;
        }
        public DropdownDto()
        {

        }
    }
}
