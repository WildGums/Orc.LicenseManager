namespace Orc.LicenseManager
{
    using System;
    using Catel;
    using Catel.Data;
    using Catel.Logging;

    public class MachineLicenseValidationService : IMachineLicenseValidationService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IIdentificationService _identificationService;

        public MachineLicenseValidationService(IIdentificationService identificationService)
        {
            ArgumentNullException.ThrowIfNull(identificationService);

            _identificationService = identificationService;

            Threshold = 1;
        }

        public int Threshold { get; set; }

        public IValidationContext Validate(string machineIdToValidate)
        {
            Argument.IsNotNullOrWhitespace(() => machineIdToValidate);

            var validationContext = new ValidationContext();

            Log.Debug("Retrieving machine id");

            var machineId = _identificationService.GetMachineId();

            Log.Debug("Validating machine id '{0}' against expected machine id '{1}'", machineId, machineIdToValidate);

            var machineSplitted = machineId.Split(new[] { LicenseElements.IdentificationSeparator }, StringSplitOptions.None);
            var expectedSplitter = machineIdToValidate.Split(new[] { LicenseElements.IdentificationSeparator }, StringSplitOptions.None);

            if (machineSplitted.Length != expectedSplitter.Length)
            {
                var error = "The number of items inside the license differ too much, assuming machine ids do not match";
                Log.Error(error);
                validationContext.Add(BusinessRuleValidationResult.CreateError(error));

                return validationContext;
            }

            var invalidEntries = 0;

            for (var i = 0; i < machineSplitted.Length; i++)
            {
                if (!string.Equals(expectedSplitter[i], machineSplitted[i], StringComparison.OrdinalIgnoreCase))
                {
                    invalidEntries++;
                }
            }

            if (invalidEntries > Threshold)
            {
                var error = string.Format("{0} values are not equal, not accepting the machine id, maximum threshold is '{1}'", invalidEntries, Threshold);
                Log.Error(error);
                validationContext.Add(BusinessRuleValidationResult.CreateError(error));

                return validationContext;
            }

            if (invalidEntries > 0)
            {
                var warning = string.Format("One of the values is not equal, but we have a threshold of {0} so accepting machine id", Threshold);
                Log.Warning(warning);

                validationContext.Add(BusinessRuleValidationResult.CreateWarning(warning));
            }

            return validationContext;
        }
    }
}
