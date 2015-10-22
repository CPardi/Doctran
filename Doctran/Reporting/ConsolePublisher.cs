namespace Doctran.Reporting
{
    using System;
    using Helper;

    public class ConsolePublisher
    {
        public void AddErrorDescription(string description)
        {
            if (ErrorDescription != null)
            {
                throw new InvalidOperationException("Cannot specify more than one error description.");
            }

            if (WarningDescription != null)
            {
                throw new InvalidOperationException("Cannot specify both an error and a warning description.");
            }

            ErrorDescription = description;
        }

        public void AddWarningDescription(string description)
        {
            if (ErrorDescription != null)
            {
                throw new InvalidOperationException("Cannot specify both an error and a warning description.");
            }

            if (WarningDescription != null)
            {
                throw new InvalidOperationException("Cannot specify more than one warning description.");
            }

            WarningDescription = description;
        }

        public void AddReason(string reason)
        {
            if (Reason != null)
            {
                throw new InvalidOperationException("Cannot specify more than one error reason.");
            }

            Reason = reason;
        }

        public void AddLocation(string location)
        {
            if (Location != null)
            {
                throw new InvalidOperationException("Cannot specify more than one error location.");
            }

            Location = location;
        }

        public void AddMessage(string message)
        {
            if (Message != null)
            {
                throw new InvalidOperationException("Cannot specify more than one message.");
            }

            Message = message;
        }

        public void Publish()
        {
            // If we need a new line then add it.
            AddNewLine();

            // Check for a simple message first.
            if (Message != null)
            {
                Console.Write(Message);
                return;
            }

            // Give errors or warnings in standard form.
            var ttb = new TitledTextBuilder();
            ttb.Append(ErrorDescription != null ? "Error" : "Warning", ErrorDescription ?? WarningDescription);

            if (Reason != null)
            {
                ttb.Append("Reason", Reason);
            }

            if (Location != null)
            {
                ttb.Append("Location", Location);
            }

            Console.WriteLine(ttb.ToString());
        }

        private string WarningDescription { get; set; }

        private string ErrorDescription { get; set; }

        private string Reason { get; set; }

        private string Location { get; set; }

        private string Message { get; set; }

        private static void AddNewLine()
        {
            if (Console.CursorLeft != 0)
            {
                Console.WriteLine(string.Empty);
            }
        }
    }
}
