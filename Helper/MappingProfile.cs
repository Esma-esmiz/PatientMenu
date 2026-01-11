using AutoMapper;


public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<NewPatientDto, Patient>();
        CreateMap<NewItemDto, MenuItem>();
    }
}
