﻿using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using DMWeb_REST.Models;
using System.Security.Authentication;

namespace DMWeb_REST
{
    /// <summary>
    /// Class functions
    /// </summary>
    public class DMWeb
    {
        public static WebClient client = new WebClient();
        public static string _baseUrl = "";
        public static string _sessionKey = "";
        
        public Messaging.SendMessage sendMessagePayload = new Messaging.SendMessage();
        public List<string> _base64 = new List<string>();

        public DMAccount Account = new DMAccount();
        public DMFolders Folders = new DMFolders();
        public DMMessage Message = new DMMessage();
        public DMGroupInbox GroupInbox = new DMGroupInbox();

        public const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
        public const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;

        /// <summary>
        /// Default constructor that sets the _baseUrl to SecureMail
        /// </summary>
        public DMWeb()
        {
            _baseUrl = "https://ssl.dmhisp.com/SecureMessagingAPI";
            ServicePointManager.SecurityProtocol = Tls12;
        }

        /// <summary>
        /// Non-default constructor that allows the host URL to be changed
        /// </summary>
        /// <param name="url">The string of the destination URL</param>
        public DMWeb(string url)
        {
            _baseUrl = url;
            ServicePointManager.SecurityProtocol = Tls12;
        }
        public class DMAccount
        {
            /// <summary>
            /// Retrieve a sessionkey for the user
            /// </summary>
            /// <param name="model">Model of type AccountLogOn contains string UserIdOrEmail and string Password</param>
            /// <returns>HttpResponseMessage deserialized into AccountSessionKey object</returns>
            public string LogOn(Account.LogOn model)
            {
                string jsonString = JsonConvert.SerializeObject(model);
                byte[] jsonByteArray = Encoding.UTF8.GetBytes(jsonString);

                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Remove("X-Session-Key");

                try
                {
                    string response = Encoding.UTF8.GetString(client.UploadData(_baseUrl + "/Account/Logon", "POST", jsonByteArray));

                    Account.AccountSessionKey temp = JsonConvert.DeserializeObject<Account.AccountSessionKey>(response);
                    _sessionKey = temp.SessionKey;

                    client.Headers.Add("X-Session-Key", _sessionKey);

                    return temp.SessionKey;
                }
                catch (WebException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Displays the account details to the user
            /// </summary>
            /// <returns>HttpResponseMessage deserialized into AccountResponses object</returns>
            public Account.AccountDetails Details()
            {
                client.Headers.Add("Content-Type", "application/json");

                try
                {
                    string response = client.DownloadString(_baseUrl + "/Account/Details");
                    Account.AccountDetails temp = JsonConvert.DeserializeObject<Account.AccountDetails>(response);
                    return temp;
                }
                catch(WebException ex)
                {
                    throw ex;  
                }
            }

            /// <summary>
            /// Allows user to change their account's password
            /// </summary>
            /// <param name="model">Model of type AccountChangePassword contains string OldPassword and string NewPassword</param>
            /// <returns>HttpResponseMessage</returns>
            public string ChangePassword(Account.ChangePassword model)
            {
                string jsonString = JsonConvert.SerializeObject(model);
                byte[] jsonByteArray = Encoding.UTF8.GetBytes(jsonString);

                client.Headers.Add("Content-Type", "application/json");

                try
                {
                    string response = Encoding.UTF8.GetString(client.UploadData(_baseUrl + "/Account/ChangePassword", "POST", jsonByteArray));

                    return response;
                }
                catch (WebException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Removes the Session Key when the user chooses to log out and invalidates the supplied SessionKey on our Saas
            /// </summary>
            /// <returns>HttpResponseMessage</returns>
            public string LogOut()
            {
                string jsonString = JsonConvert.SerializeObject("");
                byte[] jsonByteArray = Encoding.UTF8.GetBytes(jsonString);

                client.Headers.Add("Content-Type", "application/json");

                try
                {
                    string response = Encoding.UTF8.GetString(client.UploadData(_baseUrl + "/Account/Logout", "POST", jsonByteArray));
                    client.Headers.Remove("X-Session-Key");
                    _sessionKey = "";

                    return response;
                }
                catch (WebException ex)
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
            public Folders.ListFolders List()
            {
                client.Headers.Add("Content-Type", "application/json");

                try
                {
                    string response = client.DownloadString(_baseUrl + "/Folder/List");
                    Folders.ListFolders folderResponse = JsonConvert.DeserializeObject<Folders.ListFolders>(response);

                    return folderResponse;
                }
                catch (WebException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Creates a folder of the chosen type and name by the user
            /// </summary>
            /// <param name="model">Model of type Folder contains string FolderName and int FolderType</param>
            /// <returns>FolderID as a string</returns>
            public string Create(Folders.Create model)
            {
                string jsonString = JsonConvert.SerializeObject(model);
                byte[] jsonByteArray = Encoding.UTF8.GetBytes(jsonString);

                client.Headers.Add("Content-Type", "application/json");

                try
                {
                    string response = Encoding.UTF8.GetString(client.UploadData(_baseUrl + "/Folder", jsonByteArray));
                    Folders.FolderResponse fid = JsonConvert.DeserializeObject<Folders.FolderResponse>(response);

                    return fid.FolderId.ToString();
                }

                catch (WebException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Deletes a folder of the user's choice
            /// </summary>
            /// <param name="FolderID">string FolderId</param>
            /// <returns>HttpResponseMessage</returns>
            public string Delete(string FolderID)
            {
                string jsonString = JsonConvert.SerializeObject("");
                byte[] jsonByteArray = Encoding.UTF8.GetBytes(jsonString);

                client.Headers.Add("Content-Type", "application/json");

                try
                {
                    string response = Encoding.UTF8.GetString(client.UploadData(_baseUrl + "/Folder/" + FolderID, "DELETE", jsonByteArray));

                    return response;
                }
                catch (WebException ex)
                {
                    throw ex;
                }
            }
        }

        public class DMGroupInbox
        {
            public Group_Mailbox.ShowDelegatesResponse ShowDelegates()
            {
                client.Headers.Add("Content-Type", "application/json");

                string response = client.DownloadString(_baseUrl + "/Folder/Delegates");
                Group_Mailbox.ShowDelegatesResponse delegatesObject = JsonConvert.DeserializeObject<Group_Mailbox.ShowDelegatesResponse>(response);

                return delegatesObject;
            }

            public Group_Mailbox.ShowGroupBoxesResponse ShowGroupBoxes()
            {
                client.Headers.Add("Content-Type", "application/json");

                string response = client.DownloadString(_baseUrl + "/Folder/GroupBox");

                Group_Mailbox.ShowGroupBoxesResponse groupBoxObject = JsonConvert.DeserializeObject<Group_Mailbox.ShowGroupBoxesResponse>(response);

                return groupBoxObject;
            }

            public string AddDelegate(Group_Mailbox.AddDelegateRequest model)
            {
                string jsonString = JsonConvert.SerializeObject(model);
                byte[] jsonByteArray = Encoding.UTF8.GetBytes(jsonString);

                client.Headers.Add("Content-Type", "application/json");

                string response = Encoding.UTF8.GetString(client.UploadData(_baseUrl + "/Folder/Delegates", "PUT", jsonByteArray));

                return response;
            }

            public string DeleteDelegate(string delegateEmail)
            {
                string jsonString = JsonConvert.SerializeObject("");
                byte[] jsonByteArray = Encoding.UTF8.GetBytes(jsonString);

                client.Headers.Add("Content-Type", "application/json");

                string response = Encoding.UTF8.GetString(client.UploadData(_baseUrl + "/Folder/" + delegateEmail + "/Delegates", "DELETE", jsonByteArray));

                return response;
            }

            public Group_Mailbox.GetGroupInboxMIDsResponse GetGroupInboxMessageIds(Group_Mailbox.GetGroupInboxMIDsRequest model)
            {
                string jsonString = JsonConvert.SerializeObject(model);
                byte[] jsonByteArray = Encoding.UTF8.GetBytes(jsonString);

                client.Headers.Add("Content-Type", "application/json");

                string response = Encoding.UTF8.GetString(client.UploadData(_baseUrl + "/Message/GetGroupInboxMessageIds", "POST", jsonByteArray));

                Group_Mailbox.GetGroupInboxMIDsResponse midObject = JsonConvert.DeserializeObject<Group_Mailbox.GetGroupInboxMIDsResponse>(response);

                return midObject;

            }

            public Group_Mailbox.GetGroupMessageSummariesResponse GetGroupMessageSummaries(Group_Mailbox.GetGroupMessageSummariesRequest model)
            {
                string jsonString = JsonConvert.SerializeObject(model);
                byte[] jsonByteArray = Encoding.UTF8.GetBytes(jsonString);

                client.Headers.Add("Content-Type", "application/json");

                string response = Encoding.UTF8.GetString(client.UploadData(_baseUrl + "/Message/GetGroupMessageSummaries", "POST", jsonByteArray));

                Group_Mailbox.GetGroupMessageSummariesResponse summariesObject = JsonConvert.DeserializeObject<Group_Mailbox.GetGroupMessageSummariesResponse>(response);

                return summariesObject;
            }

            public Group_Mailbox.GroupInboxResponse GroupInbox(string mid)
            {
                client.Headers.Add("Content-Type", "application/json");

                if (mid != "")
                {
                    string response = client.DownloadString(_baseUrl + "/Message/GroupInbox?After=" + mid);

                    Group_Mailbox.GroupInboxResponse groupInboxObject = JsonConvert.DeserializeObject<Group_Mailbox.GroupInboxResponse>(response);

                    return groupInboxObject;
                }
                else
                {
                    string response = client.DownloadString(_baseUrl + "/Message/GroupInbox");

                    Group_Mailbox.GroupInboxResponse groupInboxObject = JsonConvert.DeserializeObject<Group_Mailbox.GroupInboxResponse>(response);

                    return groupInboxObject;
                }
            }

            public Group_Mailbox.GetGroupInboxUnreadResponse GetGroupInboxUnread(string mid)
            {
                client.Headers.Add("Content-Type", "application/json");

                if (mid != "")
                {
                    string response = client.DownloadString(_baseUrl + "/Message/GroupInbox/Unread?After=" + mid);

                    Group_Mailbox.GetGroupInboxUnreadResponse groupInboxUnreadObject = JsonConvert.DeserializeObject<Group_Mailbox.GetGroupInboxUnreadResponse>(response);

                    return groupInboxUnreadObject;
                }
                else
                {
                    string response = client.DownloadString(_baseUrl + "/Message/GroupInbox/Unread");

                    Group_Mailbox.GetGroupInboxUnreadResponse groupInboxUnreadObject = JsonConvert.DeserializeObject<Group_Mailbox.GetGroupInboxUnreadResponse>(response);

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
            public Messaging.GetInboxMIDResponse GetInboxMessageIds(Messaging.GetInboxMIDRequest model)
            {
                string jsonString = JsonConvert.SerializeObject(model);
                byte[] jsonByteArray = Encoding.UTF8.GetBytes(jsonString);

                client.Headers.Add("Content-Type", "application/json");

                try
                {
                    string response = Encoding.UTF8.GetString(client.UploadData(_baseUrl + "/Message/GetInboxMessageIds", "POST", jsonByteArray));

                    Messaging.GetInboxMIDResponse inboxResponse = JsonConvert.DeserializeObject<Messaging.GetInboxMIDResponse>(response);

                    return inboxResponse;
                }
                catch(WebException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Displays the MessageSummaries of a message
            /// </summary>
            /// <param name="model">Model of type GetMessageSummariesRequest contains int FolderId and int LastMessageIDReceived</param>
            /// <returns>HttpResponseMessage deserialized into SummariesResponseBody object</returns>
            public Messaging.GetMessageSummaries GetMessageSummaries(Messaging.GetMessageSummariesRequest model)
            {
                string jsonString = JsonConvert.SerializeObject(model);
                byte[] jsonByteArray = Encoding.UTF8.GetBytes(jsonString);

                client.Headers.Add("Content-Type", "application/json");

                try
                {
                    string response = Encoding.UTF8.GetString(client.UploadData(_baseUrl + "/Message/GetMessageSummaries", "POST", jsonByteArray));

                    Messaging.GetMessageSummaries summariesResponse = JsonConvert.DeserializeObject<Messaging.GetMessageSummaries>(response);

                    return summariesResponse;
                }
                catch(WebException ex)
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
            public Messaging.GetUnreadMessages GetUnreadMessages(bool LastMIDReceived, string MID)
            {
                client.Headers.Add("Content-Type", "application/json");


                if (LastMIDReceived == false)
                {
                    try
                    {
                        string response = client.DownloadString(_baseUrl + "/Message/Inbox/Unread");

                        Messaging.GetUnreadMessages unreadResponse = JsonConvert.DeserializeObject<Messaging.GetUnreadMessages>(response);

                        return unreadResponse;
                    }
                    catch(WebException ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    try
                    {
                        string response = client.DownloadString(_baseUrl + "/Message/Inbox/Unread?After=" + MID);

                        Messaging.GetUnreadMessages unreadResponse = JsonConvert.DeserializeObject<Messaging.GetUnreadMessages>(response);

                        return unreadResponse;
                    }
                    catch(WebException ex)
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
            public Messaging.SearchInboxResponse SearchInbox(Messaging.SearchInbox model)
            {
                string jsonString = JsonConvert.SerializeObject(model);
                byte[] jsonByteArray = Encoding.UTF8.GetBytes(jsonString);

                client.Headers.Add("Content-Type", "application/json");

                try
                {
                    string response = Encoding.UTF8.GetString(client.UploadData(_baseUrl + "/Message/Inbox/Search", "POST", jsonByteArray));

                    Messaging.SearchInboxResponse searchInboxResponseObject = JsonConvert.DeserializeObject<Messaging.SearchInboxResponse>(response);

                    return searchInboxResponseObject;
                }
                catch (WebException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Gets the metadata of a selected message
            /// </summary>
            /// <param name="MessageId"></param>
            /// <returns>MetadataResponse object</returns>
            public Messaging.MetadataResponse GetMessageMetadata(string MessageId)
            {
                client.Headers.Add("Content-Type", "application/json");
                
                try
                {
                    string response = client.DownloadString(_baseUrl + "/Message/" + MessageId + "/Metadata");

                    Messaging.MetadataResponse messageMetadata = JsonConvert.DeserializeObject<Messaging.MetadataResponse>(response);

                    return messageMetadata;
                }
                catch(WebException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Used to retract a message
            /// </summary>
            /// <param name="model">Model contains string messageId</param>
            /// <returns>HttpResponseMessage(null if successful) </returns>
            public string Retract(string messageId)
            {
                string jsonString = JsonConvert.SerializeObject("");
                byte[] jsonByteArray = Encoding.UTF8.GetBytes(jsonString);

                client.Headers.Add("Content-Type", "application/json");

                try
                {
                    string response = Encoding.UTF8.GetString(client.UploadData(_baseUrl + "/Message/" + messageId + "/Retract", "POST", jsonByteArray));

                    return response;
                }
                catch (WebException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Used to move a message
            /// </summary>
            /// <param name="model">Model contains string messageId</param>
            /// <returns>HttpResponseMessage(null if successful)</returns>
            public string Move(Messaging.MoveMessageRequest model, string messageId)
            {
                string jsonString = JsonConvert.SerializeObject(model);
                byte[] jsonByteArray = Encoding.UTF8.GetBytes(jsonString);

                client.Headers.Add("Content-Type", "application/json");

                try
                {
                    string response = Encoding.UTF8.GetString(client.UploadData(_baseUrl + "/Message/" + messageId + "/Move", "POST", jsonByteArray));

                    return response;
                }
                catch (WebException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Used to send a message
            /// </summary>
            /// <param name="model">Model contains multiple parameters</param>
            /// <returns>MessageID as an integer</returns>
            public int Send(Messaging.SendMessage model)
            {
                string jsonString = JsonConvert.SerializeObject(model);
                byte[] jsonByteArray = Encoding.UTF8.GetBytes(jsonString);

                client.Headers.Add("Content-Type", "application/json");

                try
                {
                    string response = Encoding.UTF8.GetString(client.UploadData(_baseUrl + "/Message", "POST", jsonByteArray));

                    Messaging.SendMessageResponse mid = JsonConvert.DeserializeObject<Messaging.SendMessageResponse>(response);

                    return mid.MessageId;
                }
                catch (WebException ex)
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
            public string Delete(string mid, bool permanentlyDeleteCheck)
            {
                string jsonString = JsonConvert.SerializeObject("");
                byte[] jsonByteArray = Encoding.UTF8.GetBytes(jsonString);

                client.Headers.Add("Content-Type", "application/json");

                string messageId = mid;
                if (permanentlyDeleteCheck == true)
                {
                    try
                    {
                        string response = Encoding.UTF8.GetString(client.UploadData(_baseUrl + "/Message/" + messageId + "?Permanently=true", "DELETE", jsonByteArray));

                        return response;
                    }
                    catch (WebException ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    try
                    {
                        string response = Encoding.UTF8.GetString(client.UploadData(_baseUrl + "/Message/" + messageId, "DELETE", jsonByteArray));

                        return response;
                    }
                    catch (WebException ex)
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
            public Messaging.GetMessage Get(string messageID)
            {
                client.Headers.Add("Content-Type", "application/json");

                try
                {
                    string response = client.DownloadString(_baseUrl + "/Message/" + messageID);

                    Messaging.GetMessage messageResponse = JsonConvert.DeserializeObject<Messaging.GetMessage>(response);

                    return messageResponse;
                }
                catch (WebException ex)
                {
                    throw ex;
                }
            }

            /// <summary>
            /// Retrieves a Mime Message
            /// </summary>
            /// <param name="messageId"></param>
            /// <returns>MimeMessageRequestandResponse object</returns>
            public Messaging.GetMimeMessageResponse GetaMimeMessage(string messageId)
            {
                client.Headers.Add("Content-Type", "application/json");

                try
                {
                    string response = client.DownloadString(_baseUrl + "/Message/" + messageId + "/Mime");
                    Messaging.GetMimeMessageResponse mimeMessage = JsonConvert.DeserializeObject<Messaging.GetMimeMessageResponse>(response);

                    return mimeMessage;
                }
                catch (WebException ex)
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
            public string SendMimeMessage(string mimeString)
            {
                client.Headers.Add("Content-Type", "application/json");

                Messaging.SendMimeMessageRequest mimeMessageObject = new Messaging.SendMimeMessageRequest();
                mimeMessageObject.MimeMessage = mimeString;

                string jsonString = JsonConvert.SerializeObject(mimeMessageObject);
                byte[] jsonByteArray = Encoding.UTF8.GetBytes(jsonString);

                try
                {
                    string response = Encoding.UTF8.GetString(client.UploadData(_baseUrl + "/Message/Mime", "POST", jsonByteArray));

                    Messaging.SendMimeMessageResponse mid = JsonConvert.DeserializeObject<Messaging.SendMimeMessageResponse>(response);

                    return mid.MessageId.ToString();
                }
                catch (WebException ex)
                {
                    throw ex;
                }
            }
        }
    }
}