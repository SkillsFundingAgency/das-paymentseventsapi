﻿using System.Threading.Tasks;
using SFA.DAS.Provider.Events.Api.Types;
using SFA.DAS.Provider.Events.Application.Data.Entities;

namespace SFA.DAS.Provider.Events.Application.Repositories
{
    public interface IPaymentRepository
    {
        Task<PageOfResults<PaymentEntity>> GetPayments(
            int page, int pageSize, 
            string employerAccountId= null, 
            int? collectionPeriodYear = null, 
            int? collectionPeriodMonth = null, 
            long? ukprn = null);
    }
}