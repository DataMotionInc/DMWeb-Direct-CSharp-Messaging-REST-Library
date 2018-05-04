using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Direct_Messaging_SDK_461.Models
{
    public class Messaging
    {
        /// <summary>
        /// Structure for payload to retrieve messageIDs
        /// </summary>
        public class GetInboxMIDRequest
        {
            public int FolderId { get; set; }
            public int MessageListFilter { get; set; }
            public bool MustHaveAttachments { get; set; }
        }

        /// <summary>
        /// Structure for the InboxMID response body
        /// </summary>
        public class GetInboxMIDResponse
        {
            public int[] MessageIds { get; set; }
        }

        /// <summary>
        /// Structure for payload to retrieve message summary
        /// </summary>
        public class GetMessageSummariesRequest
        {
            public int FolderId { get; set; }
            public int LastMessageIDReceived { get; set; }
        }

        /// <summary>
        /// Structure for the Summaries object
        /// </summary>
        public class SummariesAndUnreadMessage
        {
            public int AttachmentCount { get; set; }
            public string createTimeString { get; set; }
            public int FolderId { get; set; }
            public int MessageId { get; set; }
            public int MessageSize { get; set; }
            public bool Read { get; set; }
            public int MessageStatus { get; set; }
            public string SenderAddress { get; set; }
            public string Subject { get; set; }
        }

        /// <summary>
        /// Structure for the MessageSummariesresponse body
        /// </summary>
        public class GetMessageSummaries
        {
            public bool MoreMessagesAvailable { get; set; }
            public SummariesAndUnreadMessage[] Summaries { get; set; }
        }

        /// <summary>
        /// Structure for the UnreadMessages response body
        /// </summary>
        public class GetUnreadMessages
        {
            public bool MoreMessagesAvailable { get; set; }
            public SummariesAndUnreadMessage[] Summaries { get; set; }
        }
        /// <summary>
        /// Structure for payload to search inbox
        /// </summary>
        public class SearchInbox
        {
            public string Filter { get; set; }
            public int FolderId { get; set; }
            public bool GetInboxUnReadOnly { get; set; }
            public bool GetRetractedMsgs { get; set; }
            public string OrderBy { get; set; }
            public bool OrderDesc { get; set; }
            public int PageNum { get; set; }
            public int PageSize { get; set; }
        }

        /// <summary>
        /// Structure for the PageDetails response object used in SearchInboxResponse
        /// </summary> 
        public class PageDetails
        {
            public int CurrentPage { get; set; }
            public int FolderId { get; set; }
            public string OrderBy { get; set; }
            public int PageSize { get; set; }
            public int TotalMessages { get; set; }
            public int TotalPages { get; set; }
        }

        /// <summary>
        /// Structure for the Results response object used in SearchInboxResponse
        /// </summary>
        public class Results
        {
            public string CreateTime { get; set; }
            public int LastAction { get; set; }
            public int MessageId { get; set; }
            public int MessageSize { get; set; }
            public int MessageStatusId { get; set; }
            public string PasswordHint { get; set; }
            public bool Read { get; set; }
            public int MessageStatus { get; set; }
            public bool ReadConfirmation { get; set; }
            public string SenderEmail { get; set; }
            public int SenderId { get; set; }
            public string Subject { get; set; }
        }

        /// <summary>
        /// Structure for SearchInbox response body
        /// </summary>
        public class SearchInboxResponse
        {
            public PageDetails PageDetails { get; set; }
            public Results[] Results { get; set; }
        }

        public class MetadataSecurityEnvelope
        {
            public string Checksum { get; set; }
            public string HashAlgorithm { get; set; }
            public int Status { get; set; }
            public string StatusDescription { get; set; }
        }

        public class Recipient
        {

            public int ChecksumValidated { get; set; }
            public bool Delivered { get; set; }
            public string DeliveredDate { get; set; }
            public bool Downloaded { get; set; }
            public string DownloadedDate { get; set; }
            public string Email { get; set; }
        }
        public class MetadataAttachmentTracking
        {
            public Recipient[] Recipients { get; set; }
        }

        public class MetadataAttachmentSize
        {
            public string StdString { get; set; }
        }
        public class MetadataAttachment
        {
            public int AttachmentId { get; set; }
            public string FileName { get; set; }
            public MetadataSecurityEnvelope SecurityEnvelope { get; set; }
            public MetadataAttachmentSize Size { get; set; }
            public MetadataAttachmentTracking Tracking { get; set; }
        }

        public class Tracking
        {
            public string DateOpened { get; set; }
            public string Email { get; set; }
            public string MessageStatusDescription { get; set; }
            public int MessageStatus { get; set; }
            public string ReceiverField { get; set; }
        }
        public class MetadataResponse
        {
            public MetadataAttachment[] Attachments { get; set; }
            public string ExpirationDate { get; set; }
            public int MessageId { get; set; }
            public int MessageSize { get; set; }
            public MetadataSecurityEnvelope SecurityEnvelope { get; set; }
            public Tracking[] Tracking { get; set; }
        }

        /// <summary>
        /// Structure for attachments
        /// </summary>
        public class AttachmentsBody
        {
            public string AttachmentBase64 { get; set; }
            public string ContentType { get; set; }
            public string FileName { get; set; }
        }

        /// <summary>
        /// Structure to create a message payload to get a message
        /// </summary>
        public class GetMessage
        {
            public List<string> To = new List<string>();
            public string From { get; set; }

            public List<string> Cc = new List<string>();

            public List<string> Bcc = new List<string>();
            public string Subject { get; set; }
            public string CreateTime { get; set; }

            public List<AttachmentsBody> Attachments = new List<AttachmentsBody>();

            public string HtmlBody { get; set; }
            public string TextBody { get; set; }
        }

        public class SendMessageResponse
        {
            public int MessageId { get; set; }
        }

        /// <summary>
        /// Structure to create a message payload to send
        /// </summary>
        public class SendMessage
        {
            public List<string> To = new List<string>();
            public string From { get; set; }

            public List<string> Cc = new List<string>();

            public List<string> Bcc = new List<string>();
            public string Subject { get; set; }
            public string CreateTime { get; set; }

            public List<AttachmentsBody> Attachments = new List<AttachmentsBody>();

            public string HtmlBody { get; set; }
            public string TextBody { get; set; }
        }
        /// <summary>
        /// Structure for moving, deleting, and retracting a message
        /// </summary>
        public class MessageOperations
        {
            public int MessageId { get; set; }
            public int DestinationFolderId { get; set; }
            public int NewFolderId { get; set; }
            public string Results { get; set; }
        }

        public class MoveMessage
        {
            public int DestinationFolderId { get; set; }
        }
        public class MimeMessageRequestandResponse
        {
            public string MimeMessage { get; set; }
            public int MessageId { get; set; }
        }
    }
}
