using Domain.Entities;
using Mapster;
using Shared.Contracts.Authentication;
using Shared.Contracts.Questions;
using Shared.Contracts.Users;

namespace Service.Mapping;
public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<QuestionRequest, Question>()
            .Map(dest => dest.Answers, src => src.Answers.Select(a => new Answer { Content = a }));

        config.NewConfig<RegisterRequest, ApplicationUser>()
           .Map(dest => dest.UserName, src => src.Email);

        config.NewConfig<CreateUserRequest, ApplicationUser>()
           .Map(dest => dest.UserName, src => src.Email)
           .Map(dest => dest.NormalizedUserName, src => src.Email.ToUpper())
           .Map(dest => dest.EmailConfirmed, src => true);

        config.NewConfig<UpdateUserRequest, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Email)
            .Map(dest => dest.NormalizedUserName, src => src.Email.ToUpper());


        config.NewConfig<(ApplicationUser user, IList<string> roles), UserResponse>()
         .Map(dest => dest, src => src.user)
         .Map(dest => dest.Roles, src => src.roles);
    }
}
