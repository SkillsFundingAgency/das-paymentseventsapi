﻿using SFA.DAS.Provider.Events.Submission.Domain;
using SFA.DAS.Provider.Events.Submission.Domain.Data;

namespace SFA.DAS.Provider.Events.Submission.Infrastructure.Data
{
    public class DcfsSubmissionEventRepository : SqlServerRepository, ISubmissionEventRepository
    {
        public DcfsSubmissionEventRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void StoreSubmissionEvent(SubmissionEvent @event)
        {
            if (@event.Id < 1)
            {
                @event.Id = QuerySingle<int>("SELECT ISNULL(MAX(Id),0) FROM Submissions.SubmissionEvents") + 1;
            }

            Execute("INSERT INTO Submissions.SubmissionEvents " +
                    "(Id,IlrFileName,FileDateTime,SubmittedDateTime,ComponentVersionNumber,UKPRN,ULN,LearnRefNumber,AimSeqNumber," +
                    "PriceEpisodeIdentifier,StandardCode,ProgrammeType,FrameworkCode,PathwayCode,ActualStartDate,PlannedEndDate," +
                    "ActualEndDate,OnProgrammeTotalPrice,CompletionTotalPrice,NINumber) " +
                    "VALUES " +
                    "(@Id,@IlrFileName,@FileDateTime,@SubmittedDateTime,@ComponentVersionNumber,@UKPRN,@ULN,@LearnRefNumber,@AimSeqNumber,@" +
                    "PriceEpisodeIdentifier,@StandardCode,@ProgrammeType,@FrameworkCode,@PathwayCode,@ActualStartDate,@PlannedEndDate,@" +
                    "ActualEndDate,@OnProgrammeTotalPrice,@CompletionTotalPrice,@NINumber)",
                    @event);
        }
    }
}