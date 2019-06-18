using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DMWeb_REST.Models;
using System.Net;

namespace DMWeb_REST
{
    public class DMWeb
    {
        public static HttpClient client = new HttpClient();
        public static string _baseUrl = "";
        public static string _sessionKey = "";

        public Messaging.SendMessage sendMessagePayload = new Messaging.SendMessage();
        public List<string> _base64 = new List<string>();

        public DMAccount Account = new DMAccount();
        public DMFolders Folders = new DMFolders();
        public DMMessage Message = new DMMessage();
        public DMGroupInbox GroupInbox = new DMGroupInbox();

        /// <summary>
        /// Default constructor that sets the _baseUrl to SecureMail
        /// </summary>
        public DMWeb()
        {
            _baseUrl = "https://ssl.dmhisp.com/SecureMessagingAPI";
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }

        /// <summary>
        /// Non-default constructor that allows the host URL to be changed
        /// </summary>
        /// <param name="url">The string of the destination URL</param>
        public DMWeb(string url)
        {
            _baseUrl = url;
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }
        public class DMAccount
        {
            /// <summary>
            /// Retrieve a sessionkey for the user
            /// </summary>
            /// <param name="model">Model of type AccountLogOn contains string UserIdOrEmail and string Password</param>
            /// <returns>HttpResponseMessage deserialized into AccountSessionKey object</returns>
            public async Task<string> LogOn(Account.LogOn model)
            {
                try
                {
                    client.DefaultRequestHeaders.Remove("X-Session-Key");

                    HttpResponseMessage response = await client.PostAsJsonAsync(_baseUrl + "/Account/Logon", model);
                    response.EnsureSuccessStatusCode();
                    _sessionKey = await response.Content.ReadAsStringAsync();

                    Account.AccountSessionKey temp = JsonConvert.DeserializeObject<Account.AccountSessionKey>(_sessionKey);
                    _sessionKey = temp.SessionKey;

                    client.DefaultRequestHeaders.Add("X-Session-Key", _sessionKey);

                    return temp.SessionKey;
                }
                catch (HttpRequestException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Displays the account details to the user
            /// </summary>
            /// <returns>HttpResponseMessage deserialized into AccountResponses object</returns>
            public async Task<Account.AccountDetails> Details()
            {
                string details = "";

                try
                {
                    HttpResponseMessage response = await client.GetAsync(_baseUrl + "/Account/Details");
                    response.EnsureSuccessStatusCode();
                    details = await response.Content.ReadAsStringAsync();
                    Account.AccountDetails temp = JsonConvert.DeserializeObject<Account.AccountDetails>(details);
                    return temp;
                }
                catch (HttpRequestException ex)
                {
                    //string errorMsg = JsonConvert.DeserializeObject<string>(details);
                    throw ex;
                }
            }

            /// <summary>
            /// Allows user to change their account's password
            /// </summary>
            /// <param name="model">Model of type AccountChangePassword contains string OldPassword and string NewPassword</param>
            /// <returns>HttpResponseMessage</returns>
            public async Task<string> ChangePassword(Account.ChangePassword model)
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(_baseUrl + "/Account/ChangePassword", model);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Removes the Session Key when the user chooses to log out and invalidates the supplied SessionKey on our Saas
            /// </summary>
            /// <returns>HttpResponseMessage</returns>
            public async Task<string> LogOut()
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(_baseUrl + "/Account/Logout", "");
                    response.EnsureSuccessStatusCode();
                    client.DefaultRequestHeaders.Remove("X-Session-Key");
                    _sessionKey = "";

                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException ex)
                {
                    throw ex;
                }
            }
        }

        public class DMFolders
        {
            /// <summary>
            /// Displays the details of a folder
            /// </summary>
            /// <returns>HttpResponseMessage deserialized into FolderResponses object</returns>
            public async Task<Folders.ListFolders> List()
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(_baseUrl + "/Folder/List");
                    response.EnsureSuccessStatusCode();
                    string stringFolders = await response.Content.ReadAsStringAsync();

                    Folders.ListFolders folderResponse = JsonConvert.DeserializeObject<Folders.ListFolders>(stringFolders);

                    return folderResponse;
                }
                catch (HttpRequestException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Creates a folder of the chosen type and name by the user
            /// </summary>
            /// <param name="model">Model of type Folder contains string FolderName and int FolderType</param>
            /// <returns>FolderID as a string</returns>
            public async Task<string> Create(Folders.Create model)
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(_baseUrl + "/Folder/", model);
                    response.EnsureSuccessStatusCode();

                    string responseString = await response.Content.ReadAsStringAsync();

                    Folders.FolderResponse fid = JsonConvert.DeserializeObject<Folders.FolderResponse>(responseString);

                    return fid.FolderId.ToString();
                }

                catch (HttpRequestException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Deletes a folder of the user's choice
            /// </summary>
            /// <param name="FolderID">string FolderId</param>
            /// <returns>HttpResponseMessage</returns>
            public async Task<string> Delete(string FolderID)
            {
                try
                {
                    HttpResponseMessage response = await client.DeleteAsync(_baseUrl + "/Folder/" + FolderID);
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException ex)
                {
                    throw ex;
                }
            }
        }

        public class DMGroupInbox
        {
            public async Task<Group_Mailbox.ShowDelegatesResponse> ShowDelegates()
            {
                HttpResponseMessage response = await client.GetAsync(_baseUrl + "/Folder/Delegates");
                response.EnsureSuccessStatusCode();
                string responseString = await response.Content.ReadAsStringAsync();

                Group_Mailbox.ShowDelegatesResponse delegatesObject = JsonConvert.DeserializeObject<Group_Mailbox.ShowDelegatesResponse>(responseString);

                return delegatesObject;
            }

            public async Task<Group_Mailbox.ShowGroupBoxesResponse> ShowGroupBoxes()
            {
                HttpResponseMessage response = await client.GetAsync(_baseUrl + "/Folder/GroupBox");
                response.EnsureSuccessStatusCode();
                string responseString = await response.Content.ReadAsStringAsync();

                Group_Mailbox.ShowGroupBoxesResponse groupBoxObject = JsonConvert.DeserializeObject<Group_Mailbox.ShowGroupBoxesResponse>(responseString);

                return groupBoxObject;
            }

            public async Task<string> AddDelegate(Group_Mailbox.AddDelegateRequest model)
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(_baseUrl + "/Folder/Delegates", model);
                response.EnsureSuccessStatusCode();
                string responseString = await response.Content.ReadAsStringAsync();

                return responseString;
            }

            public async Task<string> DeleteDelegate(string delegateEmail)
            {
                HttpResponseMessage response = await client.DeleteAsync(_baseUrl + "/Folder/" + delegateEmail + "/Delegates");
                response.EnsureSuccessStatusCode();
                string responseString = await response.Content.ReadAsStringAsync();

                return responseString;
            }

            public async Task<Group_Mailbox.GetGroupInboxMIDsResponse> GetGroupInboxMessageIds(Group_Mailbox.GetGroupInboxMIDsRequest model)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(_baseUrl + "/Message/GetGroupInboxMessageIds", model);
                response.EnsureSuccessStatusCode();
                string responseString = await response.Content.ReadAsStringAsync();

                Group_Mailbox.GetGroupInboxMIDsResponse midObject = JsonConvert.DeserializeObject<Group_Mailbox.GetGroupInboxMIDsResponse>(responseString);

                return midObject;
            }

            public async Task<Group_Mailbox.GetGroupMessageSummariesResponse> GetGroupMessageSummaries(Group_Mailbox.GetGroupMessageSummariesRequest model)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(_baseUrl + "/Message/GetGroupMessageSummaries", model);
                response.EnsureSuccessStatusCode();
                string responseString = await response.Content.ReadAsStringAsync();

                Group_Mailbox.GetGroupMessageSummariesResponse summariesObject = JsonConvert.DeserializeObject<Group_Mailbox.GetGroupMessageSummariesResponse>(responseString);

                return summariesObject;
            }

            public async Task<Group_Mailbox.GroupInboxResponse> GroupInbox(string mid)
            {
                if (mid != "")
                {
                    HttpResponseMessage response = await client.GetAsync(_baseUrl + "/Message/GroupInbox?After=" + mid);
                    response.EnsureSuccessStatusCode();
                    string responseString = await response.Content.ReadAsStringAsync();

                    Group_Mailbox.GroupInboxResponse groupInboxObject = JsonConvert.DeserializeObject<Group_Mailbox.GroupInboxResponse>(responseString);

                    return groupInboxObject;
                }
                else
                {
                    HttpResponseMessage response = await client.GetAsync(_baseUrl + "/Message/GroupInbox");
                    response.EnsureSuccessStatusCode();
                    string responseString = await response.Content.ReadAsStringAsync();

                    Group_Mailbox.GroupInboxResponse groupInboxObject = JsonConvert.DeserializeObject<Group_Mailbox.GroupInboxResponse>(responseString);

                    return groupInboxObject;
                }
            }

            public async Task<Group_Mailbox.GetGroupInboxUnreadResponse> GetGroupInboxUnread(string mid)
            {
                if (mid != "")
                {
                    HttpResponseMessage response = await client.GetAsync(_baseUrl + "/Message/GroupInbox/Unread?After=" + mid);
                    response.EnsureSuccessStatusCode();
                    string responseString = await response.Content.ReadAsStringAsync();

                    Group_Mailbox.GetGroupInboxUnreadResponse groupInboxUnreadObject = JsonConvert.DeserializeObject<Group_Mailbox.GetGroupInboxUnreadResponse>(responseString);

                    return groupInboxUnreadObject;
                }
                else
                {
                    HttpResponseMessage response = await client.GetAsync(_baseUrl + "/Message/GroupInbox/Unread");
                    response.EnsureSuccessStatusCode();
                    string responseString = await response.Content.ReadAsStringAsync();

                    Group_Mailbox.GetGroupInboxUnreadResponse groupInboxUnreadObject = JsonConvert.DeserializeObject<Group_Mailbox.GetGroupInboxUnreadResponse>(responseString);

                    return groupInboxUnreadObject;
                }
            }
        }
        public class DMMessage
        {
            /// <summary>
            /// Displays the MessageIds in the inbox
            /// </summary>
            /// <param name="model"></param>
            /// <returns>GetInboxMIDResponse object</returns>
            public async Task<Messaging.GetInboxMIDResponse> GetInboxMessageIds(Messaging.GetInboxMIDRequest model)
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(_baseUrl + "/Message/GetInboxMessageIds", model);
                    response.EnsureSuccessStatusCode();
                    string messageIdsString = await response.Content.ReadAsStringAsync();

                    Messaging.GetInboxMIDResponse inboxResponse = JsonConvert.DeserializeObject<Messaging.GetInboxMIDResponse>(messageIdsString);

                    return inboxResponse;
                }
                catch (HttpRequestException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Displays the MessageSummaries of a message
            /// </summary>
            /// <param name="model">Model of type GetMessageSummariesRequest contains int FolderId and int LastMessageIDReceived</param>
            /// <returns>HttpResponseMessage deserialized into SummariesResponseBody object</returns>
            public async Task<Messaging.GetMessageSummaries> GetMessageSummaries(Messaging.GetMessageSummariesRequest model)
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(_baseUrl + "/Message/GetMessageSummaries", model);
                    response.EnsureSuccessStatusCode();
                    string summariesString = await response.Content.ReadAsStringAsync();

                    Messaging.GetMessageSummaries summariesResponse = JsonConvert.DeserializeObject<Messaging.GetMessageSummaries>(summariesString);

                    return summariesResponse;
                }
                catch (HttpRequestException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Displays the unread messages for a user
            /// </summary>
            /// <param name="LastMIDReceived">Receive only messages created since the ID reference</param>
            /// <param name="MID">MessageID</param>
            /// <returns>GetMessageSummariesResponse object</returns>
            public async Task<Messaging.GetUnreadMessages> GetUnreadMessages(bool LastMIDReceived, string MID)
            {
                if (LastMIDReceived == false)
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(_baseUrl + "/Message/Inbox/Unread");
                        string unreadResponseString = await response.Content.ReadAsStringAsync();
                        response.EnsureSuccessStatusCode();

                        Messaging.GetUnreadMessages unreadResponse = JsonConvert.DeserializeObject<Messaging.GetUnreadMessages>(unreadResponseString);

                        return unreadResponse;
                    }
                    catch (HttpRequestException ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(_baseUrl + "/Message/Inbox/Unread?After=" + MID);
                        string unreadResponseString = await response.Content.ReadAsStringAsync();
                        response.EnsureSuccessStatusCode();

                        Messaging.GetUnreadMessages unreadResponse = JsonConvert.DeserializeObject<Messaging.GetUnreadMessages>(unreadResponseString);
                        return unreadResponse;
                    }
                    catch (HttpRequestException ex)
                    {
                        throw ex;
                    }
                }
            }
            /// <summary>
            /// Searches the user's inbox, based on filter parameters
            /// </summary>
            /// <param name="model"></param>
            /// <returns>searchInboxResponse object</returns>
            public async Task<Messaging.SearchInboxResponse> SearchInbox(Messaging.SearchInbox model)
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(_baseUrl + "/Message/Inbox/Search", model);
                    string searchInboxResponseString = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();

                    Messaging.SearchInboxResponse searchInboxResponseObject = JsonConvert.DeserializeObject<Messaging.SearchInboxResponse>(searchInboxResponseString);

                    return searchInboxResponseObject;
                }
                catch (HttpRequestException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Gets the metadata of a selected message
            /// </summary>
            /// <param name="MessageId"></param>
            /// <returns>MetadataResponse object</returns>
            public async Task<Messaging.MetadataResponse> GetMessageMetadata(string MessageId)
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(_baseUrl + "/Message/" + MessageId + "/Metadata");
                    string messageMetadataString = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();

                    Messaging.MetadataResponse messageMetadata = JsonConvert.DeserializeObject<Messaging.MetadataResponse>(messageMetadataString);

                    return messageMetadata;
                }
                catch (HttpRequestException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Used to retract a message
            /// </summary>
            /// <param name="messageId">The messageId of the message being retracted</param>
            /// <returns>HttpResponseMessage(null if successful) </returns>
            public async Task<string> Retract(string messageId)
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(_baseUrl + "/Message/" + messageId + "/Retract", "");
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Used to move a message
            /// </summary>
            /// <param name="model">MoveMessageRequest model</param>
            /// <param name="messageId">The messageId being moved</param>
            /// <returns>HttpResponseMessage(null if successful)</returns>
            public async Task<string> Move(Messaging.MoveMessageRequest model, string messageId)
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(_baseUrl + "/Message/" + messageId + "/Move", model);
                    response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Used to send a message
            /// </summary>
            /// <param name="model">Model contains multiple parameters</param>
            /// <returns>MessageID as an integer</returns>
            public async Task<int> Send(Messaging.SendMessage model)
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(_baseUrl + "/Message/", model);
                    response.EnsureSuccessStatusCode();

                    string responseString = await response.Content.ReadAsStringAsync();

                    Messaging.SendMessageResponse mid = JsonConvert.DeserializeObject<Messaging.SendMessageResponse>(responseString);

                    return mid.MessageId;
                }
                catch (HttpRequestException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Used to delete a message
            /// </summary>
            /// <param name="model">Model contains messageId</param>
            /// <param name="permanentlyDeleteCheck">Boolean value used to delete a message permanently or to trash</param>
            /// <returns>HttpResponseMessage</returns>
            public async Task<string> Delete(string mid, bool permanentlyDeleteCheck)
            {
                string messageId = mid;
                if (permanentlyDeleteCheck == true)
                {
                    try
                    {
                        HttpResponseMessage response = await client.DeleteAsync(_baseUrl + "/Message/" + messageId + "?Permanently=true");
                        response.EnsureSuccessStatusCode();

                        return await response.Content.ReadAsStringAsync();
                    }
                    catch (HttpRequestException ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    try
                    {
                        HttpResponseMessage response = await client.DeleteAsync(_baseUrl + "/Message/" + messageId);
                        response.EnsureSuccessStatusCode();

                        return await response.Content.ReadAsStringAsync();
                    }
                    catch (HttpRequestException ex)
                    {
                        throw ex;
                    }
                }
            }

            /// <summary>
            /// Used to display a specific message
            /// </summary>
            /// <param name="messageID">string messageID of selected message</param>
            /// <returns>GetMessage object</returns>
            public async Task<Messaging.GetMessage> Get(string messageID)
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(_baseUrl + "/Message.svc/" + messageID);
                    response.EnsureSuccessStatusCode();
                    string messageString = await response.Content.ReadAsStringAsync();

                    Messaging.GetMessage messageResponse = JsonConvert.DeserializeObject<Messaging.GetMessage>(messageString);

                    return messageResponse;
                }
                catch (HttpRequestException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Retrieves a Mime Message
            /// </summary>
            /// <param name="messageId"></param>
            /// <returns>MimeMessageRequestandResponse object</returns>
            public async Task<Messaging.GetMimeMessageResponse> GetaMimeMessage(string messageId)
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(_baseUrl + "/Message/" + messageId + "/Mime");
                    string mimeString = await response.Content.ReadAsStringAsync();
                    response.EnsureSuccessStatusCode();

                    Messaging.GetMimeMessageResponse mimeMessage = JsonConvert.DeserializeObject<Messaging.GetMimeMessageResponse>(mimeString);

                    return mimeMessage;
                }
                catch (HttpRequestException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Send a Mime message
            /// </summary>
            /// <param name="model"></param>
            /// <param name="location"></param>
            /// <returns>Mime MessageID as a string</returns>
            public async Task<string> SendMimeMessage(string messageString)
            {
                Messaging.SendMimeMessageRequest mimeMessageObject = new Messaging.SendMimeMessageRequest();
                mimeMessageObject.MimeMessage = messageString;

                try
                {
                    HttpResponseMessage response = await client.PostAsJsonAsync(_baseUrl + "/Message/Mime", mimeMessageObject);
                    response.EnsureSuccessStatusCode();
                    string responseString = await response.Content.ReadAsStringAsync();

                    Messaging.SendMimeMessageResponse mid = JsonConvert.DeserializeObject<Messaging.SendMimeMessageResponse>(responseString);

                    return mid.MessageId.ToString();
                }
                catch (HttpRequestException ex)
                {
                    throw ex;
                }
            }
        }
    }
}
