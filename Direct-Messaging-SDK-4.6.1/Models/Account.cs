namespace DMWeb_REST.Models
{
    public class Account
    {
        /// <summary>
        /// Structure used to hold the SessionKey
        /// </summary>
        public class AccountSessionKey
        {
            public string SessionKey { get; set; }
        }

        /// <summary>
        /// Structure used for LogOn
        /// </summary>
        public class LogOn
        {
            public string UserIdOrEmail { get; set; }
            public string Password { get; set; }
        }

        /// <summary>
        /// Structure used for changing user password
        /// </summary>
        public class ChangePassword
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }

        /// <summary>
        /// Structure for the statistics object
        /// </summary>
        public class Statistics
        {
            public int AccountSize { get; set; }
            public int AvailableAccountSize { get; set; }
            public string DateCreated { get; set; }
            public string DateOfLastNotice { get; set; }
            public string DateOfLastVisit { get; set; }
            public string DatePasswordExpires { get; set; }
            public int TotalFilesInOutbox { get; set; }
            public int TotalFilesSent { get; set; }
            public int TotalMessagesInInbox { get; set; }
            public int TotalMessagesInOutbox { get; set; }
            public int TotalMessagesReceived { get; set; }
            public int TotalMessagesSent { get; set; }
            public int TotalUnreadMessagesInInbox { get; set; }
            public int TotalVisits { get; set; }
            public int UsedAccountSize { get; set; }
        }

        /// <summary>
        /// Structure for viewing account details
        /// </summary>
        public class AccountDetails
        {
            public string EmailAddress { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public Statistics Statistics { get; set; }
        }
    }
}
