namespace DMWeb_REST.Models
{
    public class Group_Mailbox
    {
        public class DelegatesObject
        {
            public string DelegateAddress { get; set; }
            public string Created { get; set; }
        }

        public class ShowDelegatesResponse
        {
            public DelegatesObject[] Delegates { get; set; }
        }

        public class GroupBoxObject
        {
            public string GroupBoxAddress { get; set; }
            public string Created { get; set; }
        }

        public class ShowGroupBoxesResponse
        {
            public GroupBoxObject[] GroupBox { get; set; }
        }

        public class AddDelegateRequest
        {
            public string DelegateAddress { get; set; }
        }

        public class GetGroupInboxMIDsRequest
        {
            public bool MustHaveAttachments { get; set; }
        }

        public class GetGroupInboxMIDsResponse
        {
            public int[] MessageIds { get; set; }
        }
        
        public class GetGroupMessageSummariesRequest
        {
            public int LastMessageIdReceived { get; set; }
        }
        public class SummariesObject
        {
            public int MessageId { get; set; }
            public string Subject { get; set; }
            public string Created { get; set; }
            public string FromAddress { get; set; }
            public string ToAddress { get; set; }
            public int Size { get; set; }
            public MessageStatusCodes MessageStatus { get; set; }
        }

        public class GetGroupMessageSummariesResponse
        {
            public string MoreMessagesAvailable { get; set; }
            public SummariesObject[] Summaries { get; set; }
        }

        public class GroupInboxResponse
        {
            public string MoreMessagesAvailable { get; set; }
            public SummariesObject[] Summaries { get; set; }
        }

        public class GetGroupInboxResponse
        {
            public string MoreMessagesAvailable { get; set; }
            public SummariesObject[] Summaries { get; set; }
        }

        public class GetGroupInboxUnreadResponse
        {
            public string MoreMessagesAvailable { get; set; }
            public SummariesObject[] Summaries { get; set; }
        }
    }
}
