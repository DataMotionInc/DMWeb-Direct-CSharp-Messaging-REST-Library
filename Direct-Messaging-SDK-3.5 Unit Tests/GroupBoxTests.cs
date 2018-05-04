using System.Net;
using System.IO;
using System.Threading;
using System.Reflection;
using NUnit.Framework;
using Direct_Messaging_SDK_3._5;
using Direct_Messaging_SDK_3._5.Models;

namespace Direct_Messaging_SDK_3._5_Unit_Tests
{
    [TestFixture]
    public class GroupBoxTests
    {
        public class Context
        {
            public static Direct_Messaging_SDK_35 Direct = new Direct_Messaging_SDK_35();
            public static string folderId;
            public static string trackSentFID;
            public static int sendDeleteMID;
            public static int moveMID;
            public static string mimeMessageId;
            public static string delegateAddress = "";

            public static string userName = "";
            public static string password = "";
        }

        static string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static string rootPath = Directory.GetParent(assemblyFolder).Parent.Parent.FullName;
        private static string _consolidatedPath = Path.Combine(rootPath, "Unit Test Documents");
        private string _messageDataPath = Path.Combine(_consolidatedPath, "MessageData.txt");
        private string _testDataPath = Path.Combine(_consolidatedPath, "Test.txt");

        [Test, Order(1)]
        [Category("Show Delegates")]
        [Category("No Session Key")]
        public void ShowDelegatesNoSessionKeyTest()
        {
            try
            {
                Context.Direct.GroupInbox.ShowDelegates();
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(2)]
        [Category("Show Group Boxes")]
        [Category("No Session Key")]
        public void ShowGroupBoxesNoSessionKey()
        {
            try
            {
                Context.Direct.GroupInbox.ShowGroupBoxes();
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(3)]
        [Category("Add Delegate")]
        [Category("No Session Key")]
        public void AddDelegateCorrectEmailNoSessionKey()
        {
            try
            {
                Context.Direct.GroupInbox.AddDelegate(new Group_Mailbox.AddDelegateRequest { DelegateAddress = Context.delegateAddress });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(4)]
        [Category("Add Delegate")]
        [Category("No Session Key")]
        public void AddDelegateIncorrectEmailNoSessionKeyTest()
        {
            try
            {
                Context.Direct.GroupInbox.AddDelegate(new Group_Mailbox.AddDelegateRequest { DelegateAddress = Context.delegateAddress });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(5)]
        [Category("Delete Delegate")]
        [Category("No Session Key")]
        public void DeleteDelegateCorrectEmailNoSessionKey()
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(_messageDataPath);

                string str = lines[4];
                string[] linesplit = str.Split(':');
                Context.delegateAddress = linesplit[1];

                Context.Direct.GroupInbox.DeleteDelegate(Context.delegateAddress);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(6)]
        [Category("Delete Delegate")]
        [Category("No Session Key")]
        public void DeleteDelegateIncorrectEmailNoSessionKeyTest()
        {
            try
            {
                Context.Direct.GroupInbox.AddDelegate(new Group_Mailbox.AddDelegateRequest { DelegateAddress = Context.delegateAddress });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(7)]
        [Category("Get Group Inbox MID")]
        [Category("No Session Key")]
        public void GetGroupMIDAttachmentsNoSessionKey()
        {
            try
            {
                Context.Direct.GroupInbox.GetGroupInboxMessageIds(new Group_Mailbox.GetGroupInboxMIDsRequest { MustHaveAttachments = true });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(8)]
        [Category("Get Group Inbox MID")]
        [Category("No Session Key")]
        public void GetGroupMIDAttachmentsFalseNoSessionKey()
        {
            try
            {
                Context.Direct.GroupInbox.GetGroupInboxMessageIds(new Group_Mailbox.GetGroupInboxMIDsRequest { MustHaveAttachments = false });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(9)]
        [Category("Group Inbox")]
        [Category("No Session Key")]
        public void GroupInboxWithoutAfterNoSessionKey()
        {
            try
            {
                Context.Direct.GroupInbox.GroupInbox("");
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(10)]
        [Category("Group Inbox")]
        [Category("No Session Key")]
        public void GroupInboxWithAfterNoSessionKey()
        {
            try
            {
                Context.Direct.GroupInbox.GroupInbox("0");
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }


        [Test, Order(11)]
        [Category("Group Inbox")]
        [Category("No Session Key")]
        public void GroupInboxUnreadWithoutAfterNoSessionKey()
        {
            try
            {
                Context.Direct.GroupInbox.GetGroupInboxUnread("");
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(12)]
        [Category("Group Inbox")]
        [Category("No Session Key")]
        public void GroupInboxUnreadWithAfterNoSessionKey()
        {
            try
            {
                Context.Direct.GroupInbox.GetGroupInboxUnread("0");
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(13)]
        [Category("Get Group Message Summaries")]
        [Category("No Session Key")]
        public void GroupMessageSummariesLastMIDTrueNoSessionKey()
        {
            try
            {
                Context.Direct.GroupInbox.GetGroupMessageSummaries(new Group_Mailbox.GetGroupMessageSummariesRequest { LastMessageIdReceived = 0 });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(14)]
        [Category("Get Group Message Summaries")]
        [Category("No Session Key")]
        public void GroupMessageSummariesLastMIDFalseTestNoSessionKey()
        {
            try
            {
                Context.Direct.GroupInbox.GetGroupMessageSummaries(new Group_Mailbox.GetGroupMessageSummariesRequest { LastMessageIdReceived = 1856846819 });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(15)]
        [Category("LogOn")]
        public void LogOnPositiveTest()
        {
            string[] lines = System.IO.File.ReadAllLines(_messageDataPath);

            string str = lines[0];
            string[] linesplit = str.Split(':');
            Context.userName = linesplit[1];

            string str2 = lines[1];
            string[] linesplit2 = str2.Split(':');
            Context.password = linesplit2[1];

            string sessionKey = Context.Direct.Account.LogOn(new Account.LogOn { UserIdOrEmail = Context.userName, Password = Context.password });
            Assert.AreNotEqual(string.Empty, sessionKey);

            Thread.Sleep(4000);
        }

        //Order before DeleteDelegateCorrectEmailTest
        [Test, Order(16)]
        [Category("Add Delegate")]
        public void AddDelegateCorrectEmailTest()
        {
            string[] lines = System.IO.File.ReadAllLines(_messageDataPath);

            string str = lines[4];
            string[] linesplit = str.Split(':');
            Context.delegateAddress = linesplit[1];

            Context.Direct.GroupInbox.AddDelegate(new Group_Mailbox.AddDelegateRequest { DelegateAddress = Context.delegateAddress });
            Thread.Sleep(5000);
        }

        //Order after AddDelegateCorrectEmailTest
        [Test, Order(17)]
        [Category("Delete Delegate")]
        public void DeleteDelegateCorrectEmailTest()
        {
            Context.Direct.GroupInbox.DeleteDelegate(Context.delegateAddress);
        }
        [Test]
        [Category("Show Delegates")]
        public void ShowDelegatesPositiveTest()
        {
            Context.Direct.GroupInbox.ShowDelegates();

        }

        [Test]
        [Category("Show Group Boxes")]
        public void ShowGroupBoxesPositiveTest()
        {
            Context.Direct.GroupInbox.ShowGroupBoxes();
        }

        [Test]
        [Category("Add Delegate")]
        public void AddDelegateIncorrectEmailTest()
        {
            try
            {
                Context.Direct.GroupInbox.AddDelegate(new Group_Mailbox.AddDelegateRequest { DelegateAddress = Context.delegateAddress });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("Delete Delegate")]
        public void DeleteDelegateIncorrectEmailTest()
        {
            try
            {
                Context.Direct.GroupInbox.DeleteDelegate(Context.delegateAddress);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("Get Group Inbox MID")]
        public void GetGroupMIDAttachmentsTrueTest()
        {
            Context.Direct.GroupInbox.GetGroupInboxMessageIds(new Group_Mailbox.GetGroupInboxMIDsRequest { MustHaveAttachments = true });
        }

        [Test]
        [Category("Get Group Inbox MID")]
        public void GetGroupMIDAttachmentsFalseTest()
        {
            Context.Direct.GroupInbox.GetGroupInboxMessageIds(new Group_Mailbox.GetGroupInboxMIDsRequest { MustHaveAttachments = false });
        }

        [Test]
        [Category("Get Group Message Summaries")]
        public void GroupMessageSummariesLastMIDTrueTest()
        {
            Context.Direct.GroupInbox.GetGroupMessageSummaries(new Group_Mailbox.GetGroupMessageSummariesRequest { LastMessageIdReceived = 0 });
        }

        [Test]
        [Category("Get Group Message Summaries")]
        public void GroupMessageSummariesLastMIDFalseTest()
        {
            Context.Direct.GroupInbox.GetGroupMessageSummaries(new Group_Mailbox.GetGroupMessageSummariesRequest { LastMessageIdReceived = 1856846819 });
        }

        [Test]
        [Category("Group Inbox")]
        public void GroupInboxWithoutAfterTest()
        {
            Context.Direct.GroupInbox.GroupInbox("");
        }

        [Test]
        [Category("Group Inbox")]
        public void GroupInboxWithAfterTest()
        {
            Context.Direct.GroupInbox.GroupInbox("0");
        }

        [Test]
        [Category("Group Inbox")]
        public void GroupInboxUnreadWithoutAfterTest()
        {
            Context.Direct.GroupInbox.GetGroupInboxUnread("");
        }

        [Test]
        [Category("Group Inbox Unread")]
        public void GroupInboxUnreadWithAfterTest()
        {
            Context.Direct.GroupInbox.GetGroupInboxUnread("0");
        }
    }
}
