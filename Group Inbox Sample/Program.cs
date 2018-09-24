using System;
using DMWeb_REST;
using DMWeb_REST.Models;

namespace Group_Inbox_Sample
{
    class Program
    {

        static void Main(string[] args)
        {
            DMWeb direct = new DMWeb();

            Account.LogOn user = new Account.LogOn();
            user.UserIdOrEmail = "";
            user.Password = "";

            //Session Key
            Console.WriteLine("**********************************************************");
            Console.WriteLine("Session Key:\n");

            string sessionKey = direct.Account.LogOn(user).GetAwaiter().GetResult();
            Console.WriteLine("SessionKey: " + sessionKey);
            Console.WriteLine();

            //Show Delegates
            Console.WriteLine("**********************************************************");
            Console.WriteLine("Show Delegates:\n");
            Group_Mailbox.ShowDelegatesResponse delegatesList = direct.GroupInbox.ShowDelegates().GetAwaiter().GetResult();
            int delegatesLength = delegatesList.Delegates.Length;
            
            for (int i = 0; i < delegatesLength; i++)
            {
                Console.WriteLine("Delegate Address: " + delegatesList.Delegates[i].DelegateAddress);
                Console.WriteLine("Created Time: " + delegatesList.Delegates[i].Created);
                Console.WriteLine();
            }

            //Show Group Boxes
            Console.WriteLine("**********************************************************");
            Console.WriteLine("Show Group Boxes:\n");
            Group_Mailbox.ShowGroupBoxesResponse groupBoxes = direct.GroupInbox.ShowGroupBoxes().GetAwaiter().GetResult();
            int groupBoxLength = groupBoxes.GroupBox.Length;

            for (int i = 0; i < groupBoxLength; i++)
            {
                Console.WriteLine("Group Box Address: " + groupBoxes.GroupBox[i].GroupBoxAddress);
                Console.WriteLine("Time Delegate was Added: " + groupBoxes.GroupBox[i].Created);
                Console.WriteLine();
            }

            Group_Mailbox.AddDelegateRequest newDelegate = new Group_Mailbox.AddDelegateRequest();
            newDelegate.DelegateAddress = "delegate3@customer.cmsafe.com";


            //Add delegate
            direct.GroupInbox.AddDelegate(newDelegate).GetAwaiter().GetResult();

            //Remove delegate
            direct.GroupInbox.DeleteDelegate("delegate3@customer.cmsafe.com").GetAwaiter().GetResult();


            //Get Group Inbox MIDs
            Console.WriteLine("**********************************************************");
            Console.WriteLine("Get Group Inbox MIDS:\n");

            Group_Mailbox.GetGroupInboxMIDsRequest groupInboxMIDRequest = new Group_Mailbox.GetGroupInboxMIDsRequest();
            groupInboxMIDRequest.MustHaveAttachments = false;

            Group_Mailbox.GetGroupInboxMIDsResponse groupInboxMIDResponse = direct.GroupInbox.GetGroupInboxMessageIds(groupInboxMIDRequest).GetAwaiter().GetResult();
            int groupInboxMIDLength = groupInboxMIDResponse.MessageIds.Length;

            Console.WriteLine("MIDS:");
            for (int i = 0; i < groupInboxMIDLength; i++)
            {
                Console.WriteLine(groupInboxMIDResponse.MessageIds[i]);
            }

            //Get Group Message Summaries
            Console.WriteLine("**********************************************************");
            Console.WriteLine("Get Group Message Summaries:\n");
            Group_Mailbox.GetGroupMessageSummariesRequest groupMessageRequest = new Group_Mailbox.GetGroupMessageSummariesRequest();
            groupMessageRequest.LastMessageIdReceived = 0;

            Group_Mailbox.GetGroupMessageSummariesResponse groupMessageResponse = direct.GroupInbox.GetGroupMessageSummaries(groupMessageRequest).GetAwaiter().GetResult();
            int groupMessageLength = groupMessageResponse.Summaries.Length;

            Console.WriteLine("More Messages Available: " + groupMessageResponse.MoreMessagesAvailable + "\n");
            for (int i = 0; i < groupMessageLength; i++)
            {
                Console.WriteLine("MessageId: " + groupMessageResponse.Summaries[i].MessageId);
                Console.WriteLine("Subject: " + groupMessageResponse.Summaries[i].Subject);
                Console.WriteLine("Created Time: " + groupMessageResponse.Summaries[i].Created);
                Console.WriteLine("From Address: " + groupMessageResponse.Summaries[i].FromAddress);
                Console.WriteLine("To Address: " + groupMessageResponse.Summaries[i].ToAddress);
                Console.WriteLine("Size: " + groupMessageResponse.Summaries[i].Size);
                Console.WriteLine("Message Status: " + groupMessageResponse.Summaries[i].MessageStatus);
                Console.WriteLine();
            }

            //Get Group Inbox
            Console.WriteLine("**********************************************************");
            Console.WriteLine("Get Group Inbox:\n");

            Group_Mailbox.GroupInboxResponse groupInboxResponse = direct.GroupInbox.GroupInbox("").GetAwaiter().GetResult();
            int groupInboxResponseLength = groupInboxResponse.Summaries.Length;

            Console.WriteLine("More Messages Available: " + groupInboxResponse.MoreMessagesAvailable + "\n");
            for(int i = 0; i < groupInboxResponseLength; i++)
            {
                Console.WriteLine("MessageId: " + groupInboxResponse.Summaries[i].MessageId);
                Console.WriteLine("Subject: " + groupInboxResponse.Summaries[i].Subject);
                Console.WriteLine("Created Time: " + groupInboxResponse.Summaries[i].Created);
                Console.WriteLine("From Address: " + groupInboxResponse.Summaries[i].FromAddress);
                Console.WriteLine("To Address: " + groupInboxResponse.Summaries[i].ToAddress);
                Console.WriteLine("Size: " + groupInboxResponse.Summaries[i].Size);
                Console.WriteLine("Message Status: " + groupInboxResponse.Summaries[i].MessageStatus);
                Console.WriteLine();
            }

            //Get Group Inbox Unread
            Console.WriteLine("**********************************************************");
            Console.WriteLine("Get Group Inbox Unread:\n");

            Group_Mailbox.GetGroupInboxUnreadResponse groupInboxUnreadResponse = direct.GroupInbox.GetGroupInboxUnread("").GetAwaiter().GetResult();
            int groupInboxUnreadResponseLength = groupInboxUnreadResponse.Summaries.Length;

            Console.WriteLine("More Messages Available: " + groupInboxUnreadResponse.MoreMessagesAvailable + "\n");
            for (int i = 0; i < groupInboxUnreadResponseLength; i++)
            {
                Console.WriteLine("MessageId: " + groupInboxUnreadResponse.Summaries[i].MessageId);
                Console.WriteLine("Subject: " + groupInboxUnreadResponse.Summaries[i].Subject);
                Console.WriteLine("Created Time: " + groupInboxUnreadResponse.Summaries[i].Created);
                Console.WriteLine("From Address: " + groupInboxUnreadResponse.Summaries[i].FromAddress);
                Console.WriteLine("To Address: " + groupInboxUnreadResponse.Summaries[i].ToAddress);
                Console.WriteLine("Size: " + groupInboxUnreadResponse.Summaries[i].Size);
                Console.WriteLine("Message Status: " + groupInboxUnreadResponse.Summaries[i].MessageStatus);
                Console.WriteLine();
            }
        }
    }
}
