﻿using System.Collections.Generic;
using SFA.DAS.Provider.Events.Api.IntegrationTestsV2.RawEntities;

namespace SFA.DAS.Provider.Events.Api.IntegrationTestsV2
{
    class TestData
    {
        public static List<ItPayment> Payments { get; set; }
        public static List<ItEarning> Earnings { get; set; }
        public static List<ItRequiredPayment> RequiredPayments { get; set; }
        public static List<ItSubmissionEvent> SubmissionEvents { get; set; }
        public static List<ItTransfer> Transfers { get; set; }
        public static List<ItSubmissionEvent> SubmissionEventsForUln { get; set; }
    }
}
