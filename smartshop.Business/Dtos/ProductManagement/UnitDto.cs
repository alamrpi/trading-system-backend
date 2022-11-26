namespace smartshop.Business.Dtos
{
    public class UnitDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string? Comments { get; set; }

        public IList<UnitVariationDto> UnitVariations { get; set; }
    }
}
