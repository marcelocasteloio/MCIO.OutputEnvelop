namespace MCIO.OutputEnvelop.Samples.SampleApi.Domain.Entities;

public class Customer
{
    // Fields
    public static readonly string IdShouldRequiredMessageCode = $"{nameof(Customer)}.{nameof(Id)}.Should.Required";
    public static readonly string IdShouldRequiredMessageDescription = $"{nameof(Customer)} {nameof(Id)} should required";

    public static readonly string NameShouldRequiredMessageCode = $"{nameof(Customer)}.{nameof(Name)}.Should.Required";
    public static readonly string NameShouldRequiredMessageDescription = $"{nameof(Customer)} {nameof(Name)} should required";

    public static readonly int NameMaxLength = 250;
    public static readonly string NameShouldLessThanMaxLengthMessageCode = $"{nameof(Customer)}.{nameof(Name)}.Should.LessThan";
    public static readonly string NameShouldLessThanMaxLengthMessageDescription = $"{nameof(Customer)} {nameof(Name)} should less than {NameMaxLength} characters";

    public static readonly string EmailShouldRequiredMessageCode = $"{nameof(Customer)}.{nameof(Email)}.Should.Required";
    public static readonly string EmailShouldRequiredMessageDescription = $"{nameof(Customer)} {nameof(Email)} should required";

    public static readonly int EmailMaxLength = 2048;
    public static readonly string EmailShouldLessThanMaxLengthMessageCode = $"{nameof(Customer)}.{nameof(Email)}.Should.LessThan";
    public static readonly string EmailShouldLessThanMaxLengthMessageDescription = $"{nameof(Customer)} {nameof(Email)} should less than {EmailMaxLength} characters";

    public static readonly int BirthDateMaxAge = 150;
    public static readonly string BirthDateShouldLessThanMaxAgeMessageCode = $"{nameof(Customer)}.Age.Should.LessThan";
    public static readonly string BirthDateShouldLessThanMaxAgeMessageDescription = $"{nameof(Customer)} age should less than {BirthDateMaxAge}";

    // Properties
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public DateOnly? BirthDate { get; private set; }

    // Constructors
    private Customer() { }
    private Customer(Guid id, string name, string email, DateOnly? birthDate)
    {
        Id = id;
        Name = name;
        Email = email;
        BirthDate = birthDate;
    }

    // Public Methods
    public static OutputEnvelop<Customer?> RegisterNew(string name, string email, DateOnly? birthDate)
    {
        // Process
        var customer = new Customer();

        var processOutputEnvelop = OutputEnvelop.Create(
            customer.GenerateNewId(),
            customer.SetName(name),
            customer.SetEmail(email),
            customer.SetBirthDate(birthDate)
        );

        // Return
        return OutputEnvelop<Customer?>.Create(
            output: processOutputEnvelop.IsSuccess ? customer : null,
            processOutputEnvelop
        );
    }
    public static OutputEnvelop<Customer> CreateFromExistingInfos(Guid id, string name, string email, DateOnly? birthDate)
    {
        return OutputEnvelop<Customer>.CreateSuccess(
            output: new Customer(id, name, email, birthDate)    
        );
    }

    // Private Methods
    private OutputEnvelop SetId(Guid id)
    {
        // Validate
        if (id == Guid.Empty)
            return OutputEnvelop.CreateError(IdShouldRequiredMessageCode, IdShouldRequiredMessageDescription);

        // Process
        Id = id;

        // Return
        return OutputEnvelop.CreateSuccess();
    }
    private OutputEnvelop GenerateNewId()
    {
        return SetId(Guid.NewGuid());
    }

    private OutputEnvelop SetName(string name)
    {
        // Validate
        if (string.IsNullOrWhiteSpace(name))
            return OutputEnvelop.CreateError(NameShouldRequiredMessageCode, NameShouldRequiredMessageDescription);
        else if(name.Length > NameMaxLength)
            return OutputEnvelop.CreateError(NameShouldLessThanMaxLengthMessageCode, NameShouldLessThanMaxLengthMessageDescription);

        // Process
        Name = name;

        // Return
        return OutputEnvelop.CreateSuccess();
    }

    private OutputEnvelop SetEmail(string email)
    {
        // Validate
        if (string.IsNullOrWhiteSpace(email))
            return OutputEnvelop.CreateError(EmailShouldRequiredMessageCode, EmailShouldRequiredMessageDescription);
        else if (email.Length > EmailMaxLength)
            return OutputEnvelop.CreateError(EmailShouldLessThanMaxLengthMessageCode, EmailShouldLessThanMaxLengthMessageDescription);

        // Process
        Email = email;

        // Return
        return OutputEnvelop.CreateSuccess();
    }

    private OutputEnvelop SetBirthDate(DateOnly? birthDate)
    {
        // Validate
        if (birthDate is not null)
        {
            var age = DateTime.Now.Date.Year - birthDate.Value.Year;

            if (DateTime.Now.Month < birthDate.Value.Month)
                age--;
            else if (DateTime.Now.Month == birthDate.Value.Month && DateTime.Now.Day < birthDate.Value.Day)
                age--;

            if (age > BirthDateMaxAge)
                return OutputEnvelop.CreateError(BirthDateShouldLessThanMaxAgeMessageCode, BirthDateShouldLessThanMaxAgeMessageDescription);
        }

        // Process
        BirthDate = birthDate;

        // Return
        return OutputEnvelop.CreateSuccess();
    }
}
