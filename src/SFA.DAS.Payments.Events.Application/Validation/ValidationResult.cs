﻿namespace SFA.DAS.Payments.Events.Application.Validation
{
    public class ValidationResult
    {
        public string[] ValidationMessages { get; set; }

        public bool IsValid
        {
            get { return ValidationMessages == null || ValidationMessages.Length == 0; }
        }
    }
}
