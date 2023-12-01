using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using MCIO.OutputEnvelop.Benchmarks.Interfaces;
using MCIO.OutputEnvelop.Models;

namespace MCIO.OutputEnvelop.Benchmarks.OutputEnvelopBenchs;

[SimpleJob(RunStrategy.Throughput, launchCount: 1)]
[HardwareCounters(
    HardwareCounter.CacheMisses,
    HardwareCounter.Timer,
    HardwareCounter.TotalCycles,
    HardwareCounter.TotalIssues,
    HardwareCounter.BranchMispredictions,
    HardwareCounter.BranchInstructions
)]
[MemoryDiagnoser]
public class UsingInDomainEntityBenchmark
    : IBenchmark
{
    // Public Methods
    [Benchmark(Baseline = true)]
    public (CustomerWithoutOutputEnvelop?, IEnumerable<string> messages) SuccessRegisterNewCustomerWithoutOutputEnvelop()
    {
        return CustomerWithoutOutputEnvelop.RegisterNew(
            name: "Marcelo",
            email: "contato@marcelocastelo.io",
            birthDate: DateOnly.FromDateTime(DateTime.Now.AddYears(-Customer.BirthDateMaxAge))
        );
    }
    [Benchmark]
    public OutputEnvelop<Customer?> SuccessRegisterNewCustomer()
    {
        return Customer.RegisterNew(
            name: "Marcelo",
            email: "contato@marcelocastelo.io",
            birthDate: DateOnly.FromDateTime(DateTime.Now.AddYears(-Customer.BirthDateMaxAge))
        );
    }

    [Benchmark()]
    public (CustomerWithoutOutputEnvelop?, IEnumerable<string> messages) ErrorOnRegisterNewCustomerWithoutOutputEnvelop()
    {
        return CustomerWithoutOutputEnvelop.RegisterNew(
            name: null!,
            email: null!,
            birthDate: DateOnly.MinValue
        );
    }
    [Benchmark]
    public OutputEnvelop<Customer?> ErrorOnRegisterNewCustomer()
    {
        return Customer.RegisterNew(
            name: null!,
            email: null!,
            birthDate: DateOnly.MinValue
        );
    }

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

        public static readonly int BirthDateMinAge = 0;
        public static readonly string BirthDateShouldGreaterThanMinAgeMessageCode = $"{nameof(Customer)}.Age.Should.GreaterThan";
        public static readonly string BirthDateShouldGreaterThanMinAgeMessageDescription = $"{nameof(Customer)} age should greater than {BirthDateMinAge}";

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
            else if (name.Length > NameMaxLength)
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

                if(age < BirthDateMinAge)
                    return OutputEnvelop.CreateError(BirthDateShouldGreaterThanMinAgeMessageCode, BirthDateShouldGreaterThanMinAgeMessageDescription);
                else if (age > BirthDateMaxAge)
                    return OutputEnvelop.CreateError(BirthDateShouldLessThanMaxAgeMessageCode, BirthDateShouldLessThanMaxAgeMessageDescription);
            }

            // Process
            BirthDate = birthDate;

            // Return
            return OutputEnvelop.CreateSuccess();
        }
    }

    public class CustomerWithoutOutputEnvelop
    {
        // Fields
        public static readonly string IdShouldRequiredMessage = $"{IdShouldRequiredMessageCode}: {IdShouldRequiredMessageDescription}";
        public static readonly string IdShouldRequiredMessageCode = $"{nameof(CustomerWithoutOutputEnvelop)}.{nameof(Id)}.Should.Required";
        public static readonly string IdShouldRequiredMessageDescription = $"{nameof(CustomerWithoutOutputEnvelop)} {nameof(Id)} should required";

        public static readonly string NameShouldRequiredMessage = $"{NameShouldRequiredMessageCode}: {NameShouldRequiredMessageDescription}";
        public static readonly string NameShouldRequiredMessageCode = $"{nameof(CustomerWithoutOutputEnvelop)}.{nameof(Name)}.Should.Required";
        public static readonly string NameShouldRequiredMessageDescription = $"{nameof(CustomerWithoutOutputEnvelop)} {nameof(Name)} should required";

        public static readonly int NameMaxLength = 250;
        public static readonly string NameShouldLessThanMaxLengthMessage = $"{NameShouldLessThanMaxLengthMessageCode}: {NameShouldLessThanMaxLengthMessageDescription}";
        public static readonly string NameShouldLessThanMaxLengthMessageCode = $"{nameof(CustomerWithoutOutputEnvelop)}.{nameof(Name)}.Should.LessThan";
        public static readonly string NameShouldLessThanMaxLengthMessageDescription = $"{nameof(CustomerWithoutOutputEnvelop)} {nameof(Name)} should less than {NameMaxLength} characters";

        public static readonly string EmailShouldRequiredMessage = $"{EmailShouldRequiredMessageCode}: {EmailShouldRequiredMessageDescription}";
        public static readonly string EmailShouldRequiredMessageCode = $"{nameof(CustomerWithoutOutputEnvelop)}.{nameof(Email)}.Should.Required";
        public static readonly string EmailShouldRequiredMessageDescription = $"{nameof(CustomerWithoutOutputEnvelop)} {nameof(Email)} should required";

        public static readonly int EmailMaxLength = 2048;
        public static readonly string EmailShouldLessThanMaxLengthMessage = $"{EmailShouldLessThanMaxLengthMessageCode}: {EmailShouldLessThanMaxLengthMessageDescription}";
        public static readonly string EmailShouldLessThanMaxLengthMessageCode = $"{nameof(CustomerWithoutOutputEnvelop)}.{nameof(Email)}.Should.LessThan";
        public static readonly string EmailShouldLessThanMaxLengthMessageDescription = $"{nameof(CustomerWithoutOutputEnvelop)} {nameof(Email)} should less than {EmailMaxLength} characters";

        public static readonly int BirthDateMinAge = 0;
        public static readonly string BirthDateShouldGreaterThanMinAgeMessage = $"{BirthDateShouldGreaterThanMinAgeMessageCode}: {BirthDateShouldGreaterThanMinAgeMessageDescription}";
        public static readonly string BirthDateShouldGreaterThanMinAgeMessageCode = $"{nameof(CustomerWithoutOutputEnvelop)}.Age.Should.GreaterThan";
        public static readonly string BirthDateShouldGreaterThanMinAgeMessageDescription = $"{nameof(CustomerWithoutOutputEnvelop)} age should greater than {BirthDateMinAge}";

        public static readonly int BirthDateMaxAge = 150;
        public static readonly string BirthDateShouldLessThanMaxAgeMessage = $"{BirthDateShouldLessThanMaxAgeMessageCode}: {BirthDateShouldLessThanMaxAgeMessageDescription}";
        public static readonly string BirthDateShouldLessThanMaxAgeMessageCode = $"{nameof(CustomerWithoutOutputEnvelop)}.Age.Should.LessThan";
        public static readonly string BirthDateShouldLessThanMaxAgeMessageDescription = $"{nameof(CustomerWithoutOutputEnvelop)} age should less than {BirthDateMaxAge}";

        // Properties
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public DateOnly? BirthDate { get; private set; }

        // Constructors
        private CustomerWithoutOutputEnvelop() { }
        private CustomerWithoutOutputEnvelop(Guid id, string name, string email, DateOnly? birthDate)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
        }

        // Public Methods
        public static (CustomerWithoutOutputEnvelop?, IEnumerable<string> messages) RegisterNew(string name, string email, DateOnly? birthDate)
        {
            // Process
            var customer = new CustomerWithoutOutputEnvelop();

            var messages = new List<string>(capacity: 4);

            var generateNewIdMessage = customer.GenerateNewId();
            var setNameMessage = customer.SetName(name);
            var setEmailMessage = customer.SetEmail(email);
            var setBirthDateMessage = customer.SetBirthDate(birthDate);

            if(!string.IsNullOrEmpty(generateNewIdMessage))
                messages.Add(generateNewIdMessage);

            if (!string.IsNullOrEmpty(setNameMessage))
                messages.Add(setNameMessage);

            if (!string.IsNullOrEmpty(setEmailMessage))
                messages.Add(setEmailMessage);

            if (!string.IsNullOrEmpty(setBirthDateMessage))
                messages.Add(setBirthDateMessage);

            // Return
            return (customer, messages);
        }

        // Private Methods
        private string? SetId(Guid id)
        {
            // Validate
            if (id == Guid.Empty)
                return IdShouldRequiredMessage;

            // Process
            Id = id;

            // Return
            return null;
        }
        private string? GenerateNewId()
        {
            return SetId(Guid.NewGuid());
        }

        private string? SetName(string name)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(name))
                return NameShouldRequiredMessage;
            else if (name.Length > NameMaxLength)
                return NameShouldLessThanMaxLengthMessage;

            // Process
            Name = name;

            // Return
            return null;
        }

        private string? SetEmail(string email)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(email))
                return EmailShouldRequiredMessage;
            else if (email.Length > EmailMaxLength)
                return EmailShouldLessThanMaxLengthMessage;

            // Process
            Email = email;

            // Return
            return null;
        }

        private string? SetBirthDate(DateOnly? birthDate)
        {
            // Validate
            if (birthDate is not null)
            {
                var age = DateTime.Now.Date.Year - birthDate.Value.Year;

                if (DateTime.Now.Month < birthDate.Value.Month)
                    age--;
                else if (DateTime.Now.Month == birthDate.Value.Month && DateTime.Now.Day < birthDate.Value.Day)
                    age--;

                if (age < BirthDateMinAge)
                    return BirthDateShouldGreaterThanMinAgeMessage;
                else if (age > BirthDateMaxAge)
                    return BirthDateShouldLessThanMaxAgeMessage;
            }

            // Process
            BirthDate = birthDate;

            // Return
            return null;
        }
    }
}
