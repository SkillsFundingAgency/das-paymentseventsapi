﻿using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FastMember;
using Ploeh.AutoFixture;
using SFA.DAS.Provider.Events.Api.IntegrationTestsV2.EntityBuilders.Customisations;
using SFA.DAS.Provider.Events.Api.IntegrationTestsV2.RawEntities;

namespace SFA.DAS.Provider.Events.Api.IntegrationTestsV2.DatabaseAccess
{
    public class DatabaseSetup
    {
        public async Task PopulateAllData()
        {
            Debug.WriteLine("Populating tables, this could take a while");

            var payments = new IEnumerable<ItPayment>[8];
            var fixture = new Fixture().Customize(new IntegrationTestCustomisation());
            Parallel.For(0, 8, i => payments[i] = fixture.CreateMany<ItPayment>(2500));

            TestData.Payments = payments.SelectMany(p => p).ToList();
            
            await BulkInsertPayments(TestData.Payments).ConfigureAwait(false);
        }

        public async Task BulkInsertPayments(List<ItPayment> payments)
        {
            using (var conn = DatabaseConnection.Connection())
            {
                await conn.OpenAsync().ConfigureAwait(false);
                using (var bcp = new SqlBulkCopy(conn))
                using (var reader = ObjectReader.Create(payments, "Id", "EventId", "EarningEventId", 
                    "FundingSourceEventId", "RequiredPaymentEventId", "EventTime", "JobId", "DeliveryPeriod",
                    "CollectionPeriod", "AcademicYear", "Ukprn", "LearnerReferenceNumber",
                    "LearnerUln", "PriceEpisodeIdentifier", "Amount",
                    "LearningAimReference", "LearningAimProgrammeType", "LearningAimStandardCode", 
                    "LearningAimFrameworkCode", "LearningAimPathwayCode", "LearningAimFundingLineType", 
                    "ContractType", "TransactionType", "FundingSource", "IlrSubmissionDateTime", 
                    "SfaContributionPercentage", "AgreementId",  "AccountId", "TransferSenderAccountId", "CreationDate",
                    "EarningsStartDate", "EarningsPlannedEndDate", "EarningsActualEndDate",
                    "EarningsCompletionStatus", "EarningsCompletionAmount",
                    "EarningsInstalmentAmount", "EarningsNumberOfInstalments", "LearningStartDate",
                    "ApprenticeshipId", "ApprenticeshipPriceEpisodeId", "ApprenticeshipEmployerType", 
                    "ReportingAimFundingLineType", "NonPaymentReason", "DuplicateNumber"
                ))
                {
                    bcp.DestinationTableName = "[Payments2].[Payment]";
                    await bcp.WriteToServerAsync(reader).ConfigureAwait(false);
                }
            }
        }
    }
}