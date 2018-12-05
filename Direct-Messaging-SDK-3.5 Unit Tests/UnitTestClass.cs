using System.IO;
using System.Threading;
using System.Net;
using System.Reflection;
using NUnit.Framework;
using DMWeb_REST;
using DMWeb_REST.Models;

namespace Messaging_Library.TestFixtures.UnitTestClass
{
    public class Context
    {
        public static DMWeb dmWeb = new DMWeb("https://ssl.dmhisp.com/SecureMessagingAPI");
        public static string folderId;
        public static string trackSentFID;
        public static int sendDeleteMID;
        public static int moveMID;
        public static string mimeMessageId;
         
        public static string userName= "";
        public static string password = "";
    }

    [TestFixture]
    public class AccountTests
    {
        static string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static string rootPath = Directory.GetParent(assemblyFolder).Parent.Parent.FullName;
        private static string _consolidatedPath = Path.Combine(rootPath, "Unit Test Documents");
        private string _messageDataPath = Path.Combine(_consolidatedPath, "MessageData.txt");
        private string _testDataPath = Path.Combine(_consolidatedPath, "Test.txt");

        [Test, Order(1)]
        [Category("LogOn")] 
        [Category("No Session Key")]
        public void LogOnEmptyFieldsTest()
        {
            try
            {
                Context.dmWeb.Account.LogOn(new Account.LogOn { });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("500"));
            }
        }

        [Test, Order(2)]
        [Category("LogOn")]
        [Category("No Session Key")]
        public void LogOnOnlyUsernameOrIdFieldTest()
        {

            try
            {
                Context.dmWeb.Account.LogOn(new Account.LogOn { UserIdOrEmail = Context.userName, Password = "" });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("403"));
            }
        }

        [Test, Order(3)]
        [Category("LogOn")]
        [Category("No Session Key")]
        public void LogOnOnlyPasswordFieldTest()
        {
            Context.userName = "";

            try
            {
                Context.dmWeb.Account.LogOn(new Account.LogOn { UserIdOrEmail = "", Password = Context.password });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("403"));
            }
        }

        [Test, Order(4)]
        [Category("LogOn")]
        [Category("No Session Key")]
        public void LogOnInvalidFieldsNegativeTest()
        {
            try
            {
                Context.dmWeb.Account.LogOn(new Account.LogOn { UserIdOrEmail = Context.userName + "0", Password = Context.password + "0" });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("403"));
            }
        }

        [Test, Order(5)]
        [Category("LogOn")]
        [Category("No Session Key")]
        public void LogOnOnlyBadUsernameFieldTest()
        {
            try
            {
                Context.dmWeb.Account.LogOn(new Account.LogOn { UserIdOrEmail = Context.userName + "0", Password = Context.password });
            }
            catch (WebException ex)
            {
                //Returns "The Password field is required
                Assert.IsTrue(ex.Message.Contains("403"));
            }
        }

        [Test, Order(6)]
        [Category("LogOn")]
        [Category("No Session Key")]
        public void LogOnOnlyBadPasswordFieldTest()
        {
            try
            {
                Context.dmWeb.Account.LogOn(new Account.LogOn { UserIdOrEmail = Context.userName, Password = Context.password + "0" });
            }
            catch (WebException ex)
            {
                //Needs UserIdOrEmail message returned
                Assert.IsTrue(ex.Message.Contains("403"));
            }
        }

        [Test, Order(7)]
        [Category("Details")]
        [Category("No Session Key")]
        public void DisplayDetailsWithoutSessionKeyTest()
        {
            try
            {
                Context.dmWeb.Account.Details();
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(8)]
        [Category("ChangePassword")]
        [Category("No Session Key")]
        public void ChangePasswordWithoutSessionKeyTest()
        {
            try
            {
                Context.dmWeb.Account.ChangePassword(new Account.ChangePassword { OldPassword = "Test#password1!", NewPassword = "Test#password2!" });
            }
            catch (WebException ex)
            {
                //Call executes propery, but is false because OldPassword is not provided
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }
            
        [Test, Order(9)]
        [Category("LogOut")]
        [Category("No Session Key")]
        public void LogOutWithoutSessionKey()
        {
            try
            {
                Context.dmWeb.Account.LogOut();
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(10)]
        [Category("Create Folder")]
        [Category("No Session Key")]
        public void CreateNoSessionKeyTest()
        {
            try
            {
                Context.dmWeb.Folders.Create(new Folders.Create { FolderName = "Unit Test 3.5", FolderId = 0 });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(11)]
        [Category("Delete Folder")]
        [Category("No Session Key")]
        public void DeleteFolderWithoutSessionKey()
        {
            try
            {
                Context.dmWeb.Folders.Delete("85905");
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(12)]
        [Category("List Folders")]
        [Category("No Session Key")]
        public void ListFoldersWithOutSessionKey()
        {
            try
            {
                Context.dmWeb.Folders.List();
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(13)]
        [Category("Delete Message")]
        [Category("No Session Key")]
        public void DeleteMessageNoSessionKeyTest()
        {
            int MID = 36848566;

            try
            {
                Context.dmWeb.Message.Delete(MID.ToString(), permanentlyDeleteCheck: false);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(14)]
        [Category("Get Inbox MID")]
        [Category("No Session Key")]
        public void GetInboxMIDNoSessionKey()
        {
            try
            {
                Context.dmWeb.Message.GetInboxMessageIds(new Messaging.GetInboxMIDRequest { FolderId = 0, MessageListFilter = 0, MustHaveAttachments = false });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(15)]
        [Category("Get Message Metadata")]
        [Category("No Session Key")]
        public void GetMessageMetadataNoSessionKeyTest()
        {
            string MIDString = 22722701.ToString();

            try
            {
                Context.dmWeb.Message.GetMessageMetadata(MIDString);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(16)]
        [Category("Get Message Summaries")]
        [Category("No Session Key")]
        public void GetMessageSummariesNoSessionKeyTest()
        {
            try
            {
                Context.dmWeb.Message.GetMessageSummaries(new Messaging.GetMessageSummariesRequest { FolderId = 0, LastMessageIDReceived = 0 });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(17)]
        [Category("Get Message")]
        [Category("No Session Key")]
        public void GetMessageNoSessionKeyTest()
        {
            string MIDString = 22722701.ToString();

            try
            {
                Context.dmWeb.Message.Get(MIDString);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(18)]
        [Category("Get Mime Message")]
        [Category("No Session Key")]
        public void GetMimeMessageNoSessionKeyTest()
        {
            try
            {
                string messageId = 36896426.ToString();

                Context.dmWeb.Message.GetaMimeMessage(messageId);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(19)]
        [Category("Move Message")]
        [Category("No Session Key")]
        public void MoveMessagNoSessionKeyTest()
        {
            int MID = 36848566;
            int DFID = 2;

            try
            {
                Context.dmWeb.Message.Move(new Messaging.MoveMessageRequest { DestinationFolderId = DFID }, MID.ToString());
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(20)]
        [Category("Retract Message")]
        [Category("No Session Key")]
        public void RetractMessageNoSessionKeyTest()
        {
            int MID = 36860224;

            try
            {
                Context.dmWeb.Message.Retract(MID.ToString());
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(21)]
        [Category("Search Inbox")]
        [Category("No Session Key")]
        public void SearchInboxNoSessionKeyTest()
        {
            try
            {
                Context.dmWeb.Message.SearchInbox(new Messaging.SearchInbox { PageNum = 1, PageSize = 1 });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(22)]
        [Category("Send Message")]
        [Category("No Session Key")]

        public void SendMessageNoSessionKey()
        {
            string toAddress = "";
            try
            {
                Context.dmWeb.Message.Send(new Messaging.SendMessage { To = { toAddress } });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }
        [Test, Order(23)]
        [Category("Send Mime Message")]
        [Category("No Session Key")]

        public void SendMimeMessageNoSessionKeyTest()
        {
            string[] lines = System.IO.File.ReadAllLines(_messageDataPath);

            string str3 = lines[2];
            string[] linesplit3 = str3.Split(':');
            string toAddress = linesplit3[1]; 

            string str4 = lines[3];
            string[] linesplit4 = str4.Split(':');
            string fromAddress = linesplit4[1]; 

            try
            {
                Context.dmWeb.Message.SendMimeMessage("From: Delegate One <delegate1@customer.cmsafe.com>\r\nDate: Tue, 25 Sep 2018 16:53:16 -0400\r\nSubject: MIME Message Test\r\nMessage-Id: <24J3TDSOJ5U4.9POQ3IVXANX5@DellBlackTop>\r\nTo: \"delegate2@customer.cmsafe.com\" <delegate2@customer.cmsafe.com>\r\nCc: \r\nBcc: \r\nX-DateCreated: Tue, 25 Sep 2018 16:53:16 -0400\r\nMIME-Version: 1.0\r\nContent-Type: text/plain; charset=utf-8\r\n\r\nTest\r\n");
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }
        [Test, Order(24)]
        [Category("Get Unread Messages")]
        [Category("No Session Key")]
        public void GetUnreadMessagesNoSessionKeyTest()
        {
            try
            {
                Context.dmWeb.Message.GetUnreadMessages(LastMIDReceived: false, MID: 34174277.ToString());
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test, Order(25)]
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
            string sessionKey = Context.dmWeb.Account.LogOn(new Account.LogOn { UserIdOrEmail = Context.userName, Password = Context.password });
            Assert.AreNotEqual(string.Empty, sessionKey);

            //Thread.Sleep(2000);
        }

        [Test, Order(26)]
        [Category("Create Folder")]
        public void CreateFolderPositiveTest()
        {
            Context.folderId = Context.dmWeb.Folders.Create(new Folders.Create { FolderName = "UnitTest3.5", FolderType = 0 });
            Context.trackSentFID = Context.dmWeb.Folders.Create(new Folders.Create { FolderName = "TrackSent3.5", FolderType = 1 });

            Thread.Sleep(5000);
        }

        [Test, Order(30)]
        [Category("Delete Folder")]
        public void DeleteFolderWithSessionKey()
        {
            Context.dmWeb.Folders.Delete(Context.folderId);
            Context.dmWeb.Folders.Delete(Context.trackSentFID);
        }

        [Test, Order(28)]
        [Category("Send Message")]
        //Will send a message each time function is called
        public void SendMessagePositiveTest()
        {
            string[] lines = System.IO.File.ReadAllLines(_messageDataPath);

            string str3 = lines[2];
            string[] linesplit3 = str3.Split(':');
            string toAddress = linesplit3[1];

            Context.sendDeleteMID = Context.dmWeb.Message.Send(new Messaging.SendMessage { To = { toAddress }, Subject = "Positive test 3.5"});
            Thread.Sleep(5000);
        }

        [Test, Order(29)]
        [Category("Delete Message")]
        public void DeleteMessageTrueMIDFalseBoolTest()

        {
            Thread.Sleep(5000);
            Context.dmWeb.Message.Delete(Context.sendDeleteMID.ToString(), permanentlyDeleteCheck: false);
        }

        [Test, Order(27)]
        [Category("Move Message")]
        public void MoveMessagePositiveTest()
        {
            Thread.Sleep(5000);

            string[] lines = System.IO.File.ReadAllLines(_messageDataPath);

            string str3 = lines[2];
            string[] linesplit3 = str3.Split(':');
            string toAddress = linesplit3[1];

            Context.moveMID = Context.dmWeb.Message.Send(new Messaging.SendMessage { To = { toAddress }, Subject = "Move message test 3.5" });
            Thread.Sleep(5000);

            int DFID = int.Parse(Context.trackSentFID);

            Context.dmWeb.Message.Move(new Messaging.MoveMessageRequest { DestinationFolderId = DFID }, Context.moveMID.ToString());
        }

        [Test, Order(31)]
        [Category("Get Message")]
        public void GetMessagePositiveTest()
        {
            string[] lines = System.IO.File.ReadAllLines(_messageDataPath);

            string str3 = lines[2];
            string[] linesplit3 = str3.Split(':');
            string toAddress = linesplit3[1];

            int getMID = Context.dmWeb.Message.Send(new Messaging.SendMessage { To = { toAddress }, Subject = "Get message test 3.5" });
            Thread.Sleep(5000);

            Context.dmWeb.Message.Get(getMID.ToString());
        }

        [Test, Order(32)]
        [Category("Get Message Metadata")]
        public void GetMessageMetadataPositiveTest()
        {
            string[] lines = System.IO.File.ReadAllLines(_messageDataPath);

            string str3 = lines[2];
            string[] linesplit3 = str3.Split(':');
            string toAddress = linesplit3[1];

            int getMetadataMID = Context.dmWeb.Message.Send(new Messaging.SendMessage { To = { toAddress }, Subject = "Get message test 3.5" });
            Thread.Sleep(5000);

            Context.dmWeb.Message.GetMessageMetadata(getMetadataMID.ToString());
        }


        [Test, Order(33)]
        [Category("Send Mime Message")]
        public void SendMimeMessagePositiveTest()
        {
            string[] lines = System.IO.File.ReadAllLines(_messageDataPath);

            string str3 = lines[2];
            string[] linesplit3 = str3.Split(':');
            string toAddress = linesplit3[1];

            string str4 = lines[3];
            string[] linesplit4 = str4.Split(':');
            string fromAddress = linesplit4[1];

            Context.mimeMessageId = Context.dmWeb.Message.SendMimeMessage("From: Delegate One <delegate1@unittests2.direct.dmhisp.com>\r\nDate: Fri, 05 Oct 2018 16:39:35 -0400\r\nSubject: MIME MEssage Test\r\nMessage-Id: <R1MIRRXQM5U4.2NWKIF8VDB752@DellBlackTop>\r\nTo: \"delegate2@unittests2.direct.dmhisp.com\"\r\n\t<delegate2@unittests2.direct.dmhisp.com>\r\nCc: \r\nBcc: \r\nX-DateCreated: Fri, 5 Oct 2018 16:39:35 -0400\r\nMIME-Version: 1.0\r\nContent-Type: text/plain; charset=utf-8\r\n\r\nTest\r\n");
        }

        [Test, Order(34)]
        [Category("Get Mime Message")]
        public void GetMimeMessagePositiveTest()
        {
            Context.dmWeb.Message.GetaMimeMessage(Context.mimeMessageId);
        }

        [Test]
        [Category("Details")]
        public void DisplayDetailsWithSessionKeyTest()
        {
            Context.dmWeb.Account.Details();
        }

        [Test]
        [Category("ChangePassword")]
        [Ignore("Ignore test: Avoid changing password")]
        public void ChangePasswordPositiveTest()
        {
            Context.dmWeb.Account.ChangePassword(new Account.ChangePassword { OldPassword = Context.password, NewPassword = "temp#pass" });
        }

        [Test]
        [Category("ChangePassword")]
        public void ChangePasswordEmptyFieldsTest()
        {
            try
            {
                Context.dmWeb.Account.ChangePassword(new Account.ChangePassword { OldPassword = "", NewPassword = "" });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("ChangePassword")]
        public void ChangePasswordOnlyOldPasswordFieldTest()
        {
            try
            {
                Context.dmWeb.Account.ChangePassword(new Account.ChangePassword { OldPassword = Context.password, NewPassword = "" });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("ChangePassword")]
        public void ChangePasswordOnlyNewPasswordFieldTest()
        {
            try
            {
                Context.dmWeb.Account.ChangePassword(new Account.ChangePassword { OldPassword = "", NewPassword = "newtest#password" });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("ChangePassword")]
        public void ChangePasswordSamePasswordsTest()
        {
            try
            {
                Context.dmWeb.Account.ChangePassword(new Account.ChangePassword { OldPassword = Context.password, NewPassword = Context.password });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("ChangePassword")]
        //Will send message saying password is changed despite remaining the same
        public void ChangePasswordInvalidFieldsTest()
        {
            try
            {
                Context.dmWeb.Account.ChangePassword(new Account.ChangePassword { OldPassword = "False1!", NewPassword = "False2!" });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("ChangePassword")]
        public void ChangePasswordFalseOldPasswordOnlyTest()
        {
            try
            {
                Context.dmWeb.Account.ChangePassword(new Account.ChangePassword { OldPassword = "false", NewPassword = "" });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        //May be same as OnlyNewPasswordFieldTest()
        [Test]
        [Category("ChangePassword")]
        public void ChangePasswordFalseNewPasswordOnlyTest()
        {
            try
            {
                Context.dmWeb.Account.ChangePassword(new Account.ChangePassword { OldPassword = "", NewPassword = "false" });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("Create Folder")]
        public void CreateFolderEmptyFieldsTest()
        {
            try
            {
                Context.dmWeb.Folders.Create(new Folders.Create { });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("Create Folder")]
        public void CreateOnlyFolderNameTest()
        {
            try
            {
                string fid = Context.dmWeb.Folders.Create(new Folders.Create { FolderName = "Unit Test Folder Name Only 3.5" });
                Thread.Sleep(5000);
                Context.dmWeb.Folders.Delete(fid);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test]
        [Category("Create Folder")]
        public void CreateOnlyFolderTypeTest()
        {
            try
            {
                Context.dmWeb.Folders.Create(new Folders.Create { FolderType = 0 });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("Create Folder")]
        public void CreateFalseFolderIDTest()
        {
            try
            {
                Context.dmWeb.Folders.Create(new Folders.Create { FolderName = "Unit Test 3.5", FolderType = 15 });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("Delete Folder")]
        public void DeleteFolderInvalidFID()
        {
            try
            {
                Context.dmWeb.Folders.Delete("1234");
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }

        }

        [Test]
        [Category("List Folders")]
        public void ListFoldersWithSessionKey()
        {
            Context.dmWeb.Folders.List();
        }

        [Test]
        [Category("Delete Message")]
        public void DeleteMessageTrueMIDTrueBoolTest()
        {
            string[] lines = System.IO.File.ReadAllLines(_messageDataPath);

            string str3 = lines[2];
            string[] linesplit3 = str3.Split(':');
            string toAddress = linesplit3[1];

            int mid = Context.dmWeb.Message.Send(new Messaging.SendMessage { To = { toAddress }, Subject = "True bool: Delete Message 3.5" });
            Thread.Sleep(5000);

            Context.dmWeb.Message.Delete(mid.ToString(), permanentlyDeleteCheck: true);
        }

        [Test]
        [Category("Delete Message")]
        public void DeleteMessageFalseMIDFalseBoolTest()
        {
            int mid = 1561646;

            try
            {
                Context.dmWeb.Message.Delete(mid.ToString(), permanentlyDeleteCheck: false);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("403"));
            }
        }

        [Test]
        [Category("Delete Message")]
        public void DeleteMessageFalseMIDTrueBoolTest()
        {
            int MID = 36848563;
            try
            {
                Context.dmWeb.Message.Delete(MID.ToString(), permanentlyDeleteCheck: true);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("403"));
            }
        }

        [Test]
        [Category("Get Inbox MID")]
        public void GetInboxMIDNoFieldWithSessionKeyTest()
        {
            Context.dmWeb.Message.GetInboxMessageIds(new Messaging.GetInboxMIDRequest { });
        }

        [Test]
        [Category("Get Inbox MID")]
        public void GetInboxMIDAllFieldsTest()
        {
            Context.dmWeb.Message.GetInboxMessageIds(new Messaging.GetInboxMIDRequest { FolderId = 1, MessageListFilter = 0, MustHaveAttachments = false });
        }

        [Test]
        [Category("Get Inbox MID")]
        public void GetInboxMIDOnlyFIDTest()
        {
            Context.dmWeb.Message.GetInboxMessageIds(new Messaging.GetInboxMIDRequest { FolderId = 1 });
        }

        [Test]
        [Category("Get Inbox MID")]
        public void GetInboxMIDOnlyMLFTest()
        {
            Context.dmWeb.Message.GetInboxMessageIds(new Messaging.GetInboxMIDRequest { MessageListFilter = 1 });
        }

        [Test]
        [Category("Get Inbox MID")]
        public void GetInboxMIDOnlyMHATest()
        {
            Context.dmWeb.Message.GetInboxMessageIds(new Messaging.GetInboxMIDRequest { MustHaveAttachments = false });
        }

        [Test]
        [Category("Get Inbox MID")]
        public void GetInboxMIDFalseFIDTest()
        {
            try
            {
                Context.dmWeb.Message.GetInboxMessageIds(new Messaging.GetInboxMIDRequest { FolderId = 214 });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("Get Inbox MID")]
        public void GetInboxMIDFalseMLFTest()
        {
            try
            {
                Context.dmWeb.Message.GetInboxMessageIds(new Messaging.GetInboxMIDRequest { MessageListFilter = 12 });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("500"));
            }
        }

        [Test]
        [Category("Get Message Metadata")]
        public void GetMessageMetadataInvalidMIDTest()
        {
            string invalidMIDString = 15612.ToString();

            try
            {
                Context.dmWeb.Message.GetMessageMetadata(invalidMIDString);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("Get Message Summaries")]
        public void GetMessageSummariesPositiveTest()
        {
            Context.dmWeb.Message.GetMessageSummaries(new Messaging.GetMessageSummariesRequest { FolderId = 1, LastMessageIDReceived = 22722701 });
        }

        [Test]
        [Category("Get Message Summaries")]
        public void GetMessageSummariesOnlyFIDTest()
        {
            Context.dmWeb.Message.GetMessageSummaries(new Messaging.GetMessageSummariesRequest { FolderId = 1 });
        }

        [Test]
        [Category("Get Message Summaries")]
        public void GetMessageSummariesInvalidFieldsTest()
        {
            try
            {
                Context.dmWeb.Message.GetMessageSummaries(new Messaging.GetMessageSummariesRequest { FolderId = 12, LastMessageIDReceived = 12 });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("Get Message Summaries")]
        public void GetMessageSummariesFalseFIDTest()
        {
            try
            {
                Context.dmWeb.Message.GetMessageSummaries(new Messaging.GetMessageSummariesRequest { FolderId = 12 });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        public void GetMessageInvalidMIDTest()
        {
            string invalidMIDString = 15612.ToString();

            try
            {
                Context.dmWeb.Message.Get(invalidMIDString);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("403"));
            }
        }

        [Test]
        [Category("Get Mime Message")]
        public void GetMimeMessageFalseMIDTest()
        {
            try
            {
                string messageId = 36896182.ToString();

                Context.dmWeb.Message.GetaMimeMessage(messageId);
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("403"));
            }
        }

        [Test]
        [Category("Move Message")]
        public void MoveMessageMIDOnlyTest()
        {
            int MID = 36848566;

            try
            {
                Context.dmWeb.Message.Move(new Messaging.MoveMessageRequest { DestinationFolderId = 0 }, MID.ToString());
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("Move Message")]
        public void MoveMessageFIDOnlyTest()
        {
            int DFID = 1;

            try
            {
                Context.dmWeb.Message.Move(new Messaging.MoveMessageRequest { DestinationFolderId = DFID }, 0.ToString());
            }
            catch (WebException ex)
            {
                //400 status code because missing MID from model
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("Move Message")]
        public void MoveMessageTrueMIDFalseFIDTest()
        {
            int MID = 36848566;

            try
            {
                Context.dmWeb.Message.Move(new Messaging.MoveMessageRequest { DestinationFolderId = 15143 }, MID.ToString());
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("Move Message")]
        public void MoveMessageTrueFIDFalseMIDTest()
        {
            int MID = 36848541;
            int DFID = 1;

            try
            {
                Context.dmWeb.Message.Move(new Messaging.MoveMessageRequest { DestinationFolderId = DFID }, MID.ToString());
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("Move Message")]
        public void MoveMessageFalseMIDFalseFIDTest()
        {
            int MID = 36848541;
            int DFID = 15513;

            try
            {
                Context.dmWeb.Message.Move(new Messaging.MoveMessageRequest { DestinationFolderId = DFID }, MID.ToString());
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("Retract Message")]
        public void RetractMessagePositiveTest()
        {
            string[] lines = System.IO.File.ReadAllLines(_messageDataPath);

            string str3 = lines[2];
            string[] linesplit3 = str3.Split(':');
            string toAddress = linesplit3[1];


            int MID = Context.dmWeb.Message.Send(new Messaging.SendMessage { To = { toAddress }, Subject = "Retract Message Test 3.5" });

            Context.dmWeb.Message.Retract(MID.ToString());
        }

        [Test]
        [Category("Retract Message")]
        public void RetractMessageFalseMIDTest()
        {
            int MID = 36860224;

            try
            {
                Context.dmWeb.Message.Retract(MID.ToString());
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("Search Inbox")]
        public void SearchInboxPositiveTest()
        {
            Context.dmWeb.Message.SearchInbox(new Messaging.SearchInbox { PageNum = 1, PageSize = 1 });
        }

        [Test]
        [Category("Search Inbox")]
        public void SearchInboxOnlyPageNumTest()
        {
            try
            {
                Context.dmWeb.Message.SearchInbox(new Messaging.SearchInbox { PageNum = 1 });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("Search Inbox")]
        public void SearchInboxOnlyPageSizeTest()
        {
            try
            {
                Context.dmWeb.Message.SearchInbox(new Messaging.SearchInbox { PageSize = 1 });
            }
            catch (WebException ex)
            {
                //Does not return error because information is still returned
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test]
        [Category("Search Inbox")]
        public void SearchInboxFalsePageNumTest()
        {
            try
            {
                Context.dmWeb.Message.SearchInbox(new Messaging.SearchInbox { PageNum = 1561, PageSize = 1 });
            }
            catch (WebException ex)
            {
                //No exception caught because API automatically goes to the last page
                Assert.IsTrue(ex.Message.Contains("401"));
            }
        }

        [Test]
        [Category("Search Inbox")]
        public void SearchInboxFalsePageSizeTest()
        {
            try
            {
                Context.dmWeb.Message.SearchInbox(new Messaging.SearchInbox { PageNum = 1, PageSize = 1561 });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("Search Inbox")]
        public void SearchInboxNegativeTest()
        {
            try
            {
                Context.dmWeb.Message.SearchInbox(new Messaging.SearchInbox { PageNum = 1561, PageSize = 1561 });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("Search Inbox")]
        public void SearchInboxFalseFIDTest()
        {
            try
            {
                Context.dmWeb.Message.SearchInbox(new Messaging.SearchInbox { PageNum = 1, PageSize = 1, FolderId = 1565 });
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("Search Inbox")]
        public void SearchInboxGetInboxUnReadOnlyTrueTest()
        {
            Context.dmWeb.Message.SearchInbox(new Messaging.SearchInbox { PageNum = 1, PageSize = 1, GetInboxUnReadOnly = true });
        }

        [Test]
        [Category("Search Inbox")]
        public void SearchInboxGetInboxUnReadOnlyFalseTest()
        {
            Context.dmWeb.Message.SearchInbox(new Messaging.SearchInbox { PageNum = 1, PageSize = 1, GetInboxUnReadOnly = false });
        }

        [Test]
        [Category("Search Inbox")]
        public void SearchInboxGetRetractedMsgsTrueTest()
        {
            Context.dmWeb.Message.SearchInbox(new Messaging.SearchInbox { PageNum = 1, PageSize = 1, GetRetractedMsgs = true });
        }

        [Test]
        [Category("Search Inbox")]
        public void SearchInboxGetRetractedMsgsFalseTest()
        {
            Context.dmWeb.Message.SearchInbox(new Messaging.SearchInbox { PageNum = 1, PageSize = 1, GetRetractedMsgs = false });
        }

        [Test]
        [Category("Search Inbox")]
        public void SearchInboxOrderDescTrueTest()
        {
            Context.dmWeb.Message.SearchInbox(new Messaging.SearchInbox { PageNum = 1, PageSize = 1, OrderDesc = true });
        }

        [Test]
        [Category("Search Inbox")]
        public void SearchInboxOrderDescFalseTest()
        {
            Context.dmWeb.Message.SearchInbox(new Messaging.SearchInbox { PageNum = 1, PageSize = 1, OrderDesc = false });
        }

        [Test]
        [Category("Send Mime Message")]
        public void SendMimeMessageNoToAddressTest()
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(_messageDataPath);

                string str4 = lines[3];
                string[] linesplit4 = str4.Split(':');
                string fromAddress = linesplit4[1];

                Context.dmWeb.Message.SendMimeMessage("From: Delegate One <delegate1@customer.cmsafe.com>\r\nDate: Tue, 25 Sep 2018 16:54:48 -0400\r\nSubject: MIME Message Test\r\nMessage-Id: <3ECVXSSOJ5U4.8QLA3MWFPW1Q1@DellBlackTop>\r\nTo: \r\nCc: \r\nBcc: \r\nX-DateCreated: Tue, 25 Sep 2018 16:54:48 -0400\r\nMIME-Version: 1.0\r\nContent-Type: text/plain; charset=utf-8\r\n\r\nTest\r\n");
            }
            catch (WebException ex)
            {
                Assert.IsTrue(ex.Message.Contains("400"));
            }
        }

        [Test]
        [Category("Get Unread Messages")]
        public void GetUnreadMessagesLastMIDFalsePositiveTest()
        {
            Context.dmWeb.Message.GetUnreadMessages(LastMIDReceived: false, MID: 0.ToString());
        }

        [Test]
        [Category("Get Unread Messages")]
        public void GetUnreadMessagesLastMIDTrueMIDTrueTest()
        {
            Context.dmWeb.Message.GetUnreadMessages(LastMIDReceived: true, MID: 0.ToString());
        }

        [Test]
        [Category("Get Unread Messages")]
        public void GetUnreadMessagesLastMIDTrueMIDFalseTest()
        {
            try
            {
                Context.dmWeb.Message.GetUnreadMessages(LastMIDReceived: true, MID: 241.ToString());
            }
            catch (WebException ex)
            {
                //Does not return error
                //Instead MessageSummaries is empty
                Assert.IsTrue(ex.Message.Contains("403"));
            }
        }

        [OneTimeTearDown]
        [Category("LogOut")]
        public void LogOutWithSessionKey()
        {
            Context.dmWeb.Account.LogOut();
        }
    }
}

