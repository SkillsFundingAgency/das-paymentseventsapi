﻿using AutoMapper;
using SFA.DAS.Provider.Events.Api.Types;
using SFA.DAS.Provider.Events.Domain;
using SFA.DAS.Provider.Events.Infrastructure.Mapping;
using CalendarPeriod = SFA.DAS.Provider.Events.Api.Types.CalendarPeriod;
using FundingSource = SFA.DAS.Provider.Events.Api.Types.FundingSource;
using NamedCalendarPeriod = SFA.DAS.Provider.Events.Api.Types.NamedCalendarPeriod;
using Payment = SFA.DAS.Provider.Events.Domain.Payment;
using TransactionType = SFA.DAS.Provider.Events.Api.Types.TransactionType;

namespace SFA.DAS.Provider.Events.Api.Plumbing.Mapping
{
    public static class AutoMapperConfigurationFactory
    {
        public static MapperConfiguration CreateMappingConfig()
        {
            return new MapperConfiguration(cfg =>
            {
                DomainAutoMapperConfiguration.AddDomainMappings(cfg);

                cfg.CreateMap<Domain.CalendarPeriod, CalendarPeriod>();
                cfg.CreateMap<Domain.NamedCalendarPeriod, NamedCalendarPeriod>();
                cfg.CreateMap<Payment, Types.Payment>()
                    .ForMember(dst => dst.FundingSource, opt => opt.Ignore())
                    .ForMember(dst => dst.TransactionType, opt => opt.Ignore())
                    .AfterMap((src, dst) =>
                    {
                        dst.FundingSource = (FundingSource)(int)src.FundingSource;
                        dst.TransactionType = (TransactionType)(int)src.TransactionType;
                    });
                cfg.CreateMap<Domain.PageOfResults<Payment>, Types.PageOfResults<Types.Payment>>();

                cfg.CreateMap<Period, PeriodEnd>()
                    .ForMember(dst => dst.CalendarPeriod, opt => opt.Ignore())
                    .ForMember(dst => dst.ReferenceData, opt => opt.Ignore())
                    .ForMember(dst => dst.Links, opt => opt.Ignore())
                    .AfterMap((src, dst) =>
                    {
                        dst.CalendarPeriod = new CalendarPeriod
                        {
                            Month = src.CalendarMonth,
                            Year = src.CalendarYear
                        };
                        dst.ReferenceData = new ReferenceDataDetails
                        {
                            AccountDataValidAt = src.AccountDataValidAt,
                            CommitmentDataValidAt = src.CommitmentDataValidAt
                        };
                    });



                cfg.CreateMap<Domain.PageOfResults<Domain.SubmissionEvent>, Types.PageOfResults<Types.SubmissionEvent>>();
                cfg.CreateMap<Domain.SubmissionEvent, Types.SubmissionEvent>();


                cfg.CreateMap<Domain.PageOfResults<Domain.DataLockEvent>, Types.PageOfResults<Types.DataLockEvent>>();
                cfg.CreateMap<Domain.DataLockEvent, Types.DataLockEvent>()
                    .ForMember(dst => dst.EventSource, opt => opt.Ignore())
                    .ForMember(dst => dst.Status, opt => opt.Ignore())
                    .AfterMap((src, dst) =>
                    {
                        dst.EventSource = (Types.EventSource)(int)src.EventSource;
                        dst.Status = (Types.EventStatus)(int)src.Status;
                    });
                cfg.CreateMap<Domain.DataLockEventError, Types.DataLockEventError>();
                cfg.CreateMap<Domain.DataLockEventPeriod, Types.DataLockEventPeriod>();
                cfg.CreateMap<Domain.DataLockEventApprenticeship, Types.DataLockEventApprenticeship>();

                cfg.CreateMap<Types.DataLockEvent, ObsoleteModels.DataLockEventV1>()
                    .ForMember(dst => dst.IlrPriceEffectiveDate, opt => opt.MapFrom(src => src.IlrPriceEffectiveFromDate));
                cfg.CreateMap<Types.DataLockEventApprenticeship, ObsoleteModels.DataLockEventApprenticeshipV1>()
                   .ForMember(dst => dst.Version, opt => opt.MapFrom(
                       src => src.Version.Contains("-") ? long.Parse(src.Version.Split('-')[0]) : long.Parse(src.Version)));

                cfg.CreateMap<Types.DataLockEventPeriod, ObsoleteModels.DataLockEventPeriodV1>()
                  .ForMember(dst => dst.ApprenticeshipVersion, opt => opt.MapFrom(
                      src => src.ApprenticeshipVersion.Contains("-") ? long.Parse(src.ApprenticeshipVersion.Split('-')[0]).ToString() : src.ApprenticeshipVersion));
            });
        }

     
    }
}