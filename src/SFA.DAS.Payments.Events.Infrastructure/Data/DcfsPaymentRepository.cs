﻿using System;
using System.Threading.Tasks;
using SFA.DAS.Payments.Events.Domain.Data;
using SFA.DAS.Payments.Events.Domain.Data.Entities;

namespace SFA.DAS.Payments.Events.Infrastructure.Data
{
    public class DcfsPaymentRepository : DcfsRepository, IPaymentRepository
    {
        private const string Source = "Payments.Payments p INNER JOIN PaymentsDue.RequiredPayments rp ON p.RequiredPaymentId = rp.Id";
        private const string Columns = "CAST(p.PaymentId as varchar(36)) [Id], "
                                     + "rp.CommitmentId [ApprenticeshipId], "
                                     + "rp.CommitmentVersionId [ApprenticeshipVersion], "
                                     + "rp.Ukprn, "
                                     + "rp.Uln, "
                                     + "rp.AccountId [EmployerAccountId], "
                                     + "rp.AccountVersionId [EmployerAccountVersion], "
                                     + "p.DeliveryMonth [DeliveryPeriodMonth], "
                                     + "p.DeliveryYear [DeliveryPeriodYear], "
                                     + "p.CollectionPeriodName [CollectionPeriodId], "
                                     + "p.CollectionPeriodMonth, "
                                     + "p.CollectionPeriodYear, "
                                     + "rp.IlrSubmissionDateTime [EvidenceSubmittedOn], "
                                     + "p.FundingSource, "
                                     + "p.TransactionType, "
                                     + "p.Amount, "
                                     + "rp.StandardCode, "
                                     + "rp.FrameworkCode, "
                                     + "rp.ProgrammeType, "
                                     + "rp.PathwayCode, "
                                     + "rp.ApprenticeshipContractType [ContractType]";
        private const string CountColumn = "COUNT(p.PaymentId)";
        private const string Pagination = "ORDER BY p.CollectionPeriodYear, p.CollectionPeriodMonth OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY";

        public async Task<PageOfEntities<PaymentEntity>> GetPayments(int page, int pageSize)
        {
            return await GetPageOfPayments(string.Empty, page, pageSize);
        }

        public async Task<PageOfEntities<PaymentEntity>> GetPaymentsForPeriod(int collectionPeriodYear, int collectionPeriodMonth, int page, int pageSize)
        {
            var whereClause = $"WHERE p.CollectionPeriodYear = {collectionPeriodYear} AND p.CollectionPeriodMonth = {collectionPeriodMonth}";

            return await GetPageOfPayments(whereClause, page, pageSize);
        }

        public async Task<PageOfEntities<PaymentEntity>> GetPaymentsForAccount(string employerAccountId, int page, int pageSize)
        {
            var whereClause = $"WHERE rp.AccountId = '{employerAccountId.Replace("'", "''")}'";

            return await GetPageOfPayments(whereClause, page, pageSize);
        }

        public async Task<PageOfEntities<PaymentEntity>> GetPaymentsForAccountInPeriod(string employerAccountId, int collectionPeriodYear, int collectionPeriodMonth,
            int page, int pageSize)
        {
            var whereClause = $"WHERE rp.AccountId = '{employerAccountId.Replace("'", "''")}' AND p.CollectionPeriodYear = {collectionPeriodYear} AND p.CollectionPeriodMonth = {collectionPeriodMonth}";

            return await GetPageOfPayments(whereClause, page, pageSize);
        }


        private async Task<PageOfEntities<PaymentEntity>> GetPageOfPayments(string whereClause, int page, int pageSize)
        {
            var numberOfPages = await GetNumberOfPages(whereClause, pageSize);

            var payments = await GetPayments(whereClause, page, pageSize);

            return new PageOfEntities<PaymentEntity>
            {
                PageNumber = page,
                TotalNumberOfPages = numberOfPages,
                Items = payments
            };
        }
        private async Task<PaymentEntity[]> GetPayments(string whereClause, int page, int pageSize)
        {
            var command = $"SELECT {Columns} FROM {Source} {whereClause} {Pagination}";

            var offset = (page - 1) * pageSize;
            return await Query<PaymentEntity>(command, new { offset, pageSize });
        }
        private async Task<int> GetNumberOfPages(string whereClause, int pageSize)
        {
            var command = $"SELECT {CountColumn} FROM {Source} {whereClause}";
            var count = await QuerySingle<int>(command);

            return (int)Math.Ceiling(count / (float)pageSize);
        }
    }
}
