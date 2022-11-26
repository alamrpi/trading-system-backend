using smartshop.Business.Dtos;
using smartshop.Common.Dto;

namespace smartshop.Api.Test.MoqData.HumanResources
{
    public static class DesignationMockData
    {
        public static PaginationResponse<DesignationDto> Gets()
        {
            return new PaginationResponse<DesignationDto>()
            {
                Rows = new List<DesignationDto>()
                {
                    new DesignationDto(){ Id = 1, Name = "Software Engineer", Priority = 1, Descriptions = ""},
                    new DesignationDto(){ Id = 2, Name = "Sr. Software Engineer", Priority = 2, Descriptions = ""},
                    new DesignationDto(){ Id = 3, Name = "Jr Software Engineer", Priority = 3, Descriptions = ""},
                    new DesignationDto(){ Id = 4, Name = "Support Software Engineer", Priority = 4, Descriptions = ""},
                },
                TotalRows = 4
            };
           
        }
    }
}
