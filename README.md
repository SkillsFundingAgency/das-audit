# das-audit
Generic auditing component for DAS systems

## The data model
The structure of an audit message is as follows:

* **AffectedEntity** - [REQUIRED] The primary object within the application domain that has been affected by the action being audited. There are 2 properties to set. **Type**, which is the type of object affected such as the BankAccount; and **Id**, which is the identifier of the object affected.
* **Category** - [REQUIRED] The type of action / change that has occured, such as _UPDATE_ or _APPROVE_. This allows later quering by type of action, so should be granular enough to be easily filterable but not to granular as to be unusable.
* **Description** - [REQUIRED] Human readable description of the change. Sensitive information should either not be stored in here or suitably obfuscated.
* **Source** - [REQUIRED] The source system where the action has occured. There are 3 properties to set. **System**, which is the overall system/application name. **Component**, which is the component name within the system; and **Version**, which is the version of the component.
* **ChangedProperties** - Which is a list of properties which have changed as a result of the action. Each property change consists of **PropertyName** and **NewValue**.
* **ChangeAt** - [REQUIRED] The UTC date and time that the action occured.
* **ChangedBy** - The actor that invoked the action. There are 3 properties to set. **Id** which is the unique identifier for the user (such as the IDAMS identifier). **EmailAddress**, which is the email address for the user at the time of the action; and **OriginIpAddress**, which is the IP address to action occured from.
* **RelatedEntities** - A list of entities that are related to the affected entity. Each related entity has a **Type** and **Id** as per the AffectedEntity.

So for example, in a situation where a user request to transfer money out of their bank account and audit message may be raised that looks like:

```json
{
  "AffectedEntity": {
    "Type": "BankAccount",
    "Id": "112233/12345678"
  },
  "Category": "TRANSFER",
  "Description": "Outward transfer of £2821.12 to 44***2/12****91 requested by Bertie Banker",
  "Source": {
    "System": "CustomerOnlineBanking",
    "Component": "TransferProcessor",
    "Version": "4.5.12341"
  },
  "ChangedProperties": [
    {
      "PropertyName": "Balance",
      "NewValue": "3569841.25"
    }
  ],
  "ChangeAt": "2017-01-25T12:34:28Z",
  "ChangedBy": {
    "Id": "BANKUSER001",
    "EmailAddress": "bertie.banker@thebankinggroup.com",
    "OriginIpAddress": "98.21.23.102"
  },
  "RelatedEntities": [
    {
      "Type": "Branch",
      "Id": "112233"
    },
    {
      "Type": "FundSource",
      "Id": "Cash"
    }
  ]
}
```

## The client
There is a .NET client available on Nuget that wraps sending audit messages to the service. It can be added by running the following in package manage console:

```powershell
Install-Package SFA.DAS.Audit.Client
```

There is a _IAuditApiClient_, implemented by _AuditApiClient_ that can be used to write audit messages as follows:

```csharp
var configuration = new AuditApiConfiguration();
// Initialise configuration properties here

var message = new AuditMessage();
// Initialise message properties here

var client = new AuditApiClient(configuration);
await client.Audit(message);
```

There are helper methods on _PropertyUpdate_ to help set properties in consistent formats. For example:

```csharp
message.ChangedProperties = new List<PropertyUpdate>
{
  PropertyUpdate.FromDateTime("ExpiryTime", DateTime.Now),
  PropertyUpdate.FromDecimal("Balance", 3569841.25)
};
```

The package also contains a AuditMessageFactory to help build messages pre-populated with consistent values (such as ChangedAt set to DateTime.UtcNow).

To consume the factory, you need an instance of the factory to call the build message. For example:

```csharp
var messageFactory = new AuditMessageFactory();
var message = messageFactory.Build();
// message is not populated with ChangeAt as DateTime.UtcNow
```

You can also register other builder functions with the factory. For example, to pre-populate theSource property you can do the following:

```csharp
AuditMessageFactory.RegisterBuilder(message =>
{
  var name = typeof(MvcApplication).Assembly.GetName();

  message.Source = new Audit.Types.Source
  {
    System = "CustomerOnlineBanking",
    Component = name.Name,
    Version = name.Version.ToString()
  };
});
```

The builder registration is static, and should be done during application startup. Now messages will also be pre-populated with the Source property when the Build method is called.

## The client - for web applications
If the audit is being consumed from a web application then a supplementary nuget package is available the builds on the main package to add user information.

You can add the package by using the following:

```powershell
Install-Package SFA.DAS.Audit.Client.Web
```

You then need to register the builders with the factory in application startup:

```csharp
// These lines are optional. The default claims are:
//    UserIdClaim = ClaimTypes.NameIdentifier;
//    UserEmailClaim = ClaimTypes.Email;
WebMessageBuilders.UserIdClaim = DasClaimTypes.Id;
WebMessageBuilders.UserEmailClaim = DasClaimTypes.Email;

// This line registers the builder
WebMessageBuilders.Register();
```

This will now pre-populate ChangedBy.OriginIpAddress from the HttpContext. It will also set the ChangedBy.Id and ChangedBy.EmailAddress if a claims identity is available.
