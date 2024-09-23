using Teram.Framework.Core.Service;
using Teram.Module.Authentication.Entities;
using Microsoft.Extensions.Logging;
using System;

namespace Teram.Module.Authentication.Logic
{
    public class TokenPersistanceService : BasePersistenceService<Token, IIdentityUnitOfWork>, IBasePersistenceService<Token, IIdentityUnitOfWork>
    {
        public TokenPersistanceService(IIdentityUnitOfWork unitOfWork, ILogger<Token> logger, IServiceProvider serviceProvider) : base(unitOfWork, serviceProvider)
        {
        }
    }

    public class TokenParameterPersistanceService : BasePersistenceService<TokenParameter, IIdentityUnitOfWork>, IBasePersistenceService<TokenParameter, IIdentityUnitOfWork>
    {
        public TokenParameterPersistanceService(IIdentityUnitOfWork unitOfWork, ILogger<Token> logger, IServiceProvider serviceProvider) : base(unitOfWork, serviceProvider)
        {
        }
    }
}
