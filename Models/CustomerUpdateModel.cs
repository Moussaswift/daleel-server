using daleel.Entities;

public class CustomerUpdateModel
{
    public string? FullName { get; set; }
    public string? Company { get; set; }
    public CustomerType? Type { get; set; }
    public string? PhotoURL { get; set; }
    public ContactInfoUpdateModel? ContactInfo { get; set; }
    public AddressInfoUpdateModel? AddressInfo { get; set; }
}

public class ContactInfoUpdateModel
{
    public string? EmailAddress { get; set; }
    public string? HomePhone { get; set; }
    public string? WorkPhone { get; set; }
}

public class AddressInfoUpdateModel
{
    public string? StreetAddress { get; set; }
    public string? AptNumber { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
} 