using Microsoft.Extensions.Options;
using SupportCore.App.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SupportCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using static SupportCore.App.Interfaces.RedmineService;
using System.IO;

namespace SupportCore.App.Interfaces
{
    public interface IRedmineService
    {
        Task<ReadmineIssue> ReadIssueAsync(string id);
        Task CreateIssueAsync(Tasks task);
    }
    public class RedmineService : IRedmineService
    {
        //private readonly EmailConfig ec;
        private readonly Context _context;
        private readonly string url;
        private readonly string _apikey;
        private readonly HttpClient httpClient;
        private readonly DataContractJsonSerializer serializer;
        private readonly HttpClientHandler handler;


        public RedmineService(Context context,string apikey)
        {
            _context = context;
            url = "https://redmine.swan.perm.ru/";
            _apikey = apikey;
            serializer = new DataContractJsonSerializer(typeof(Issue),
                new DataContractJsonSerializerSettings
                {
                    DateTimeFormat = new DateTimeFormat("yyyy-MM-dd'T'HH:mm:sszzz"),
                }
                );
            handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = delegate { return true; }
            };
            httpClient = new HttpClient(handler);
        }
        [DataContract]
        private class Issue
        {
            [DataMember]
            public issue issue { set; get; }
        }
        [DataContract]
        private class issue
        {
            [DataMember(EmitDefaultValue = false)]
            public int id { set; get; }
            [DataMember]
            public string subject { set; get; }
            [DataMember]
            public int project_id { set; get; }
            [DataMember]
            public string description { set; get; }
            [DataMember(EmitDefaultValue = false)]
            public DateTime updated_on { set; get; }
            [DataMember(EmitDefaultValue = false)]
            public string due_date { set; get; }
            [DataMember(EmitDefaultValue = false)]
            public status status { set; get; }
            [DataMember(EmitDefaultValue =false)]
            public List<journals> journals { set; get; }
            [DataMember(EmitDefaultValue = false)]
            public assigned_to assigned_to { set; get; }
        }
        [DataContract]
        private class status
        {
            [DataMember]
            public string name { set; get; }
        }
        [DataContract]
        private class journals
        {
            [DataMember]
            public string notes;
            [DataMember]
            public user user;
            [DataMember]
            public DateTime created_on;
        }
        [DataContract]
        private class user
        {
            [DataMember]
            public string name;
        }
        [DataContract]
        private class assigned_to
        {
            [DataMember]
            public string name;
        }

        public async Task<ReadmineIssue> ReadIssueAsync(string id){
  
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));                   
                    string url_issue = url + "issues/" + id + ".json?include=journals" + "&key=" + _apikey;
            var issue = serializer.ReadObject(await httpClient.GetStreamAsync(url_issue)) as Issue;
            var lastNote = issue.issue.journals.Last();
            ReadmineIssue readmineIssue = new ReadmineIssue
            {
                Id=url+"/issues/"+id,
                LastNote = lastNote.notes,
                LastNoteUser = lastNote.user.name,
                Status = issue.issue.status.name,
                AssignTo = issue.issue.assigned_to.name,
                DateNote= lastNote.created_on,
                DateUpdate=issue.issue.updated_on            
            };
            return readmineIssue;
        }
        public async Task CreateIssueAsync(Tasks task)
        {
            var issue = new Issue
            {
                issue = new issue
                {
                    subject = task.Title,
                    description = task.Body,
                    project_id = 57
                }
            };
       
            MemoryStream newIssue = new MemoryStream();
            serializer.WriteObject(newIssue, issue);
            newIssue.Position=0;
            StreamReader sr = new StreamReader(newIssue);
            //var s=sr.ReadToEnd();
            StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            string url_issue = url + "issues.json?key=" + _apikey;
            var result = await httpClient.PostAsync(url_issue, theContent);
            var sss = theContent.ReadAsStringAsync().Result;
        }
    }
}