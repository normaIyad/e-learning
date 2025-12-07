using Course.DAL.DTO.Responce;
using Course.DAL.Models;
using Mapster;

namespace Course.Bll.Service.Class
{
    public class MapsterConfig
    {
        public static void RegisterMappings ()
        {
            TypeAdapterConfig<QuestionOption, QustionOptionsDto>
       .NewConfig()
       .Map(dest => dest.OptionText, src => src.OptionText);

        }
    }
}
