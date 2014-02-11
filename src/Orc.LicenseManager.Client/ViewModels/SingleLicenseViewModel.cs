// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleLicenseViewModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.ViewModels
{
    using System.Diagnostics;
    using Catel.Data;
    using Catel.IoC;
    using Catel.MVVM;
    using Models;
    using Services;

    /// <summary>
    /// View model for a single License.
    /// </summary>
    public class SingleLicenseViewModel : ViewModelBase
    {
        #region Constants
        /// <summary>
        /// Register the FailureOccured property so it is known in the class.
        /// </summary>
        public static readonly PropertyData FailureOccuredProperty = RegisterProperty("FailureOccured", typeof (bool), null);

        /// <summary>
        /// The single license model property
        /// </summary>
        public static readonly PropertyData SingleLicenseModelProperty = RegisterProperty("SingleLicenseModel", typeof (SingleLicenseModel));
        #endregion

        #region Fields
        private readonly ILicenseService _licenseService;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleLicenseViewModel"/> class.
        /// </summary>
        /// <param name="singleLicenseModel">The single license model.</param>
        public SingleLicenseViewModel(SingleLicenseModel singleLicenseModel)
        {
            _licenseService = DependencyResolver.Resolve<ILicenseService>(); // TODO: Ask geert how to fix this or this is oke?

            SingleLicenseModel = singleLicenseModel;
            WebsiteClick = new Command(OnWebsiteClickExecute);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the WebsiteClick command.
        /// </summary>
        public Command WebsiteClick { get; private set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        public bool FailureOccured
        {
            get { return GetValue<bool>(FailureOccuredProperty); }
            set
            {
                if (value == true)
                {
                    FailureMessage = _licenseService.GetValidationError().Message;
                    FailureSolution = _licenseService.GetValidationError().HowToResolve;
                }
                SetValue(FailureOccuredProperty, value);
            }
        }


        /// <summary>
        /// Gets the failure message.
        /// </summary>
        /// <value>
        /// The failure message.
        /// </value>
        public string FailureMessage { get; private set; }


        /// <summary>
        /// Gets the failure solution.
        /// </summary>
        /// <value>
        /// The failure solution.
        /// </value>
        public string FailureSolution { get; private set; }

        [Model]
        [Catel.Fody.Expose("Title")]
        [Catel.Fody.Expose("Website")]
        [Catel.Fody.Expose("Key")]
        [Catel.Fody.Expose("WebsiteIsSet")]
        private SingleLicenseModel SingleLicenseModel
        {
            get { return GetValue<SingleLicenseModel>(SingleLicenseModelProperty); }
            set { SetValue(SingleLicenseModelProperty, value); }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method to invoke when the WebsiteClick command is executed.
        /// </summary>
        private void OnWebsiteClickExecute()
        {
            Process.Start(SingleLicenseModel.Website);
        }

        /// <summary>
        /// Is fired upon pressing the Ok button! Saves the data, closes the window.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if successful; otherwise <c>false</c>.
        /// </returns>
        protected override bool Save()
        {
            FailureOccured = !_licenseService.ValidateLisence(SingleLicenseModel.Key);
            return !FailureOccured;
        }
        #endregion
    }
}