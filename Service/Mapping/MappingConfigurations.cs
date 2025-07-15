using Domain.Entities;
using Mapster;
using ServiceAbstraction.Contracts.Authentication;
using ServiceAbstraction.Contracts.Questions;

namespace Service.Mapping;
public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<QuestionRequest, Question>()
            .Map(dest => dest.Answers, src => src.Answers.Select(a => new Answer { Content = a }));

        config.NewConfig<RegisterRequest, ApplicationUser>()
           .Map(dest => dest.UserName , src => src.Email);
    }
}
