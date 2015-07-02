# Orc.LicenseManager

[![Join the chat at https://gitter.im/WildGums/Orc.LicenseManager](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/WildGums/Orc.LicenseManager?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

![License](https://img.shields.io/github/license/wildgums/orc.licensemanager.svg)
![NuGet downloads](https://img.shields.io/nuget/dt/orc.licensemanager.client.svg)
![Version](https://img.shields.io/nuget/v/orc.licensemanager.client.svg)
![Pre-release version](https://img.shields.io/nuget/vpre/orc.licensemanager.client.svg)

This library makes it very easy to manage licenses for commercial software.

# Client validation

Internally the license manager uses <a href="https://github.com/dnauck/Portable.Licensing" target="_blank">Portable.Licensing</a>. This is a wrapper around both the server and client to make it very easy to implement license validations.

## The basics

To check a license on the client, we need the public key of the application. It is very important to keep the private key private on the server.

It is a good practice to create a *License* class to contain all this information:

    public static class License
    {
		// The public key of the product
        public const string ApplicationId = "MIIBKjCB4wYHKoZI.....";

        public const string LicenseServer = "https://www.myserver.com/api/license";
    }

## Validating a license locally

The first thing that needs to be done is to validate locally. If the license is not valid, the software will automatically show a license dialog.

	if (!await _simpleLicenseService.Validate(License.ApplicationId, "My Product", "/MyProduct;component/Resources/Images/logo_0128.png", "In order to use this software, a license is required.")())
	{
		// License is not valid, exit software
	}

## Validating a license online

It is also possible to check the license on the server as well.

	if (!await _simpleLicenseService.ValidateOnServer(License.LicenseServer, License.ApplicationId, "My Product", "/MyProduct;component/Resources/Images/logo_0128.png", "In order to use this software, a license is required.")())
	{
		// License is not valid, exit software
	}

## Validating the number of licenses used on the network

The software can detect the number of licenses used on the current network. It does this by sending UDP broadcast packages over the network with the license signature. All software using the specified license will reply with the IP and the date/time they started using the license. This way it's possible to list all usages and kick out the latest user.

### Initialize

The easiest way to use the *INetworkLicenseService* is to initialize it with a polling value. Then it will automatically raise events when a validation has occurred.

	_networkLicenseService.Validated += OnNetworkLicenseValidated;
	await _networkLicenseService.Initialize(TimeSpan.FromSeconds(30));

Then you can handle the result of the validation in the event handler:

	private async void OnNetworkLicenseValidated(object sender, NetworkValidatedEventArgs e)
	{
	    var validationResult = e.ValidationResult;
	    if (!validationResult.IsValid)
	    {
	        var latestUsage = (from usage in validationResult.CurrentUsers
	                            orderby usage.StartDateTime descending
	                            select usage).First();
	
	        if (string.Equals(_networkLicenseService.ComputerId, latestUsage.ComputerId))
	        {
	            await _messageService.Show(string.Format("License is invalid, using '{0}' of '{1}' licenses. You are the latest user, your software will be shut down", validationResult.CurrentUsers.Count, validationResult.MaximumConcurrentUsers));                    
	        }
	        else
	        {
	            await _messageService.Show(string.Format("License is invalid, using '{0}' of '{1}' licenses. The latest user is '{2}' with ip '{3}', you can continue working", validationResult.CurrentUsers.Count, validationResult.MaximumConcurrentUsers, latestUsage.ComputerId, latestUsage.Ip));
	        }
	    }
	}
 
### Manually validating the network

To manually perform a check, use the following code:

	var validationResult = await _networkLicenseService.Validate();

# Server validation

[server documentation must be written yet]
