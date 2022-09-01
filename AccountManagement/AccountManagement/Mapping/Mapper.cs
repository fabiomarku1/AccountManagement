using AccountManagement.Data;
using AccountManagement.Data.DTO;
using AutoMapper;

namespace AccountManagement.Mapping
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<ClientDto, Client>();
            CreateMap<Client, ClientDto>();

            //CreateMap<Client, ClientDto>().ReverseMap();
        }
    }
}
