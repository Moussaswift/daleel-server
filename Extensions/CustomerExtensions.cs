using daleel.Entities;

public static class CustomerExtensions
{
    public static void ApplyUpdate(this Customer customer, CustomerUpdateModel model)
    {
        // Update only non-null properties
        if (model.FullName != null) customer.FullName = model.FullName;
        if (model.Company != null) customer.Company = model.Company;
        if (model.Type.HasValue) customer.Type = model.Type.Value;
        if (model.PhotoURL != null) customer.PhotoURL = model.PhotoURL;

        // Update ContactInfo if provided
        if (model.ContactInfo != null)
        {
            customer.ContactInfo ??= new ContactInfo { CustomerId = customer.Id };
            customer.ContactInfo.ApplyUpdate(model.ContactInfo);
        }

        // Update AddressInfo if provided
        if (model.AddressInfo != null)
        {
            customer.AddressInfo ??= new AddressInfo { CustomerId = customer.Id };
            customer.AddressInfo.ApplyUpdate(model.AddressInfo);
        }
    }

    public static void ApplyUpdate(this ContactInfo contactInfo, ContactInfoUpdateModel model)
    {
        if (model.EmailAddress != null) contactInfo.EmailAddress = model.EmailAddress;
        if (model.HomePhone != null) contactInfo.HomePhone = model.HomePhone;
        if (model.WorkPhone != null) contactInfo.WorkPhone = model.WorkPhone;
    }

    public static void ApplyUpdate(this AddressInfo addressInfo, AddressInfoUpdateModel model)
    {
        if (model.StreetAddress != null) addressInfo.StreetAddress = model.StreetAddress;
        if (model.AptNumber != null) addressInfo.AptNumber = model.AptNumber;
        if (model.City != null) addressInfo.City = model.City;
        if (model.State != null) addressInfo.State = model.State;
        if (model.ZipCode != null) addressInfo.ZipCode = model.ZipCode;
        if (model.Country != null) addressInfo.Country = model.Country;
    }
} 