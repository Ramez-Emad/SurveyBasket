using Domain.RepositoriesContracts;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service;
public class ServiceManager(IUnitOfWork _unitOfWork) : IServiceManager
{

    private IPollService? _pollService;
    public IPollService PollService => _pollService ??= new PollService(_unitOfWork);

}
