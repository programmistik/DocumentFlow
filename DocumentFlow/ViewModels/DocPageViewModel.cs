using DocumentFlow.Models;
using DocumentFlow.Services;
using DocumentFlow.Views;
using DocumentFlow.ModalWindows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFlow.ViewModels
{
    public class DocPageViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IMessageService messageService;
        private readonly AppDbContext db;

        private User CurrentUser { get; set; }
        private Employee CurrentEmployee { get; set; }
        private bool IsNew { get; set; }
        private User CreatedBy { get; set; }
        private Employee CreatedByEmp { get; set; }
        private Document CurrentDocument { get; set; }
        private MyFile CurrentFile { get; set; }
        private bool EditFileMode { get; set; }
        private DocumentState StateBeforeOpen { get; set; }

        private DateTime docData;
        public DateTime DocData { get => docData; set => Set(ref docData, value); }

        private string docNumber;
        public string DocNumber { get => docNumber; set => Set(ref docNumber, value); }

        //private Document currentDoc;
        //public Document CurrentDoc { get => currentDoc; set => Set(ref currentDoc, value); }

        private string createdByName;
        public string CreatedByName { get => createdByName; set => Set(ref createdByName, value); }

        private string docCompany;
        public string DocCompany { get => docCompany; set => Set(ref docCompany, value); }

        private ObservableCollection<DocumentType> docTypeCollection;
        public ObservableCollection<DocumentType> DocTypeCollection { get => docTypeCollection; set => Set(ref docTypeCollection, value); }

        private DocumentType docType;
        public DocumentType DocType { get => docType; set => Set(ref docType, value); }

        private ObservableCollection<DocumentState> docStatusCollection;
        public ObservableCollection<DocumentState> DocStatusCollection { get => docStatusCollection; set => Set(ref docStatusCollection, value); }

        private DocumentState docStatus;
        public DocumentState DocStatus { get => docStatus; set => Set(ref docStatus, value); }

        private DateTime docDocumentDate;
        public DateTime DocDocumentDate { get => docDocumentDate; set => Set(ref docDocumentDate, value); }

        private Organization org;
        public Organization Org { get => org; set => Set(ref org, value); }

        private ObservableCollection<Organization> orgCollection;
        public ObservableCollection<Organization> OrgCollection { get => orgCollection; set => Set(ref orgCollection, value); }

        private ExternalContact contact;
        public ExternalContact Contact { get => contact; set => Set(ref contact, value); }

        private ObservableCollection<ExternalContact> contactCollection;
        public ObservableCollection<ExternalContact> ContactCollection { get => contactCollection; set => Set(ref contactCollection, value); }

        private ObservableCollection<Currency> currCollection;
        public ObservableCollection<Currency> CurrCollection { get => currCollection; set => Set(ref currCollection, value); }

        private Currency currency;
        public Currency Currency { get => currency; set => Set(ref currency, value); }

        private string fileName;
        public string FileName { get => fileName; set => Set(ref fileName, value); }

        private string addNewFilePath;
        public string AddNewFilePath { get => addNewFilePath; set => Set(ref addNewFilePath, value); }

        private string fileComment;
        public string FileComment { get => fileComment; set => Set(ref fileComment, value); }

        private string docComment;
        public string DocComment { get => docComment; set => Set(ref docComment, value); }

        private decimal docSum;
        public decimal DocSum { get => docSum; set => Set(ref docSum, value); }

        private string docInfoComment;
        public string DocInfoComment { get => docInfoComment; set => Set(ref docInfoComment, value); }

        private string addNewFileContent;
        public string AddNewFileContent { get => addNewFileContent; set => Set(ref addNewFileContent, value); }

        private string addFileContent;
        public string AddFileContent { get => addFileContent; set => Set(ref addFileContent, value); }

        private bool RoEditFile;
        public bool roEditFile { get => RoEditFile; set => Set(ref RoEditFile, value); }

        private bool RoBtnEditFile;
        public bool roBtnEditFile { get => RoBtnEditFile; set => Set(ref RoBtnEditFile, value); }

        private bool RoDocType;
        public bool roDocType { get => RoDocType; set => Set(ref RoDocType, value); }

        private ObservableCollection<MyFile> docFiles;
        public ObservableCollection<MyFile> DocFiles { get => docFiles; set => Set(ref docFiles, value); }

        private ObservableCollection<TaskProcess> processCollection;
        public ObservableCollection<TaskProcess> ProcessCollection { get => processCollection; set => Set(ref processCollection, value); }

        public DocPageViewModel(INavigationService navigationService,
                                   IMessageService messageService,
                                      AppDbContext db)
        {
            this.navigationService = navigationService;
            this.messageService = messageService;
            this.db = db;

            Messenger.Default.Register<NotificationMessage<User>>(this, OnHitUser);
            Messenger.Default.Register<NotificationMessage<Document>>(this, OnHitIt);
        }

        private void OnHitUser(NotificationMessage<User> usr)
        {
            if (usr.Notification == "SendCurrentUser")
            {
                CurrentUser = usr.Content;
                CurrentEmployee = db.Employees.Where(e => e.UserId == CurrentUser.Id).Single();
            }
        }

        private void OnHitIt(NotificationMessage<Document> doc)
        {
            if (doc.Notification == "NewDocument")
            {
                IsNew = true;
                CurrentDocument = doc.Content;                
                DocData = DateTime.Now;
                CreatedBy = CurrentUser;
                CreatedByEmp = db.Employees.Where(e => e.UserId == CreatedBy.Id).Single();
                CreatedByName = CreatedByEmp.Name + " " + CreatedByEmp.Surname;
                DocCompany = CreatedByEmp.Company.CompanyName;
                DocTypeCollection = new ObservableCollection<DocumentType>(db.DocumentTypes);
                DocStatusCollection = new ObservableCollection<DocumentState>(db.DocumentStates);
                OrgCollection = new ObservableCollection<Organization>(db.Organizations);
                ContactCollection = new ObservableCollection<ExternalContact>(db.ExternalContacts);
                DocStatus = DocStatusCollection.Where(s => s.DocStateName == "New").Single();
                DocStatus.IsSelectable = true;
                CurrCollection = new ObservableCollection<Currency>(db.Currencies);
                AddNewFileContent = "Add new file";
                AddFileContent = "Add";
                roEditFile = false;
                roBtnEditFile = true;
                DocFiles = new ObservableCollection<MyFile>();
                ProcessCollection = new ObservableCollection<TaskProcess>();
                DocDocumentDate = DateTime.Now;
                EditFileMode = false;
                StateBeforeOpen = DocStatus;
                roDocType = false;
            }
            else if(doc.Notification == "CurrentDocument")
            {
                IsNew = false;
                CurrentDocument = doc.Content;
                DocData = CurrentDocument.DocDate;
                DocNumber = CurrentDocument.DocNumber;
                CreatedBy = CurrentDocument.CreatedBy;
                CreatedByEmp = db.Employees.Where(e => e.UserId == CreatedBy.Id).Single();
                CreatedByName = CreatedByEmp.Name + " " + CreatedByEmp.Surname;
                DocCompany = CurrentDocument.Company.CompanyName;
                DocType = CurrentDocument.DocumentType;
                DocStatus = CurrentDocument.DocumentState;
                DocDocumentDate = CurrentDocument.DocInfoDate;
                Org = CurrentDocument.Organization;
                Contact = CurrentDocument.InfoContact;
                DocSum = CurrentDocument.DocSum;
                Currency = CurrentDocument.DocCurrency;
                DocInfoComment = CurrentDocument.DocInfoComment;
                DocFiles = new ObservableCollection<MyFile>(CurrentDocument.myFiles);
                ProcessCollection = new ObservableCollection<TaskProcess>(CurrentDocument.myProcesses);

                DocTypeCollection = new ObservableCollection<DocumentType>(db.DocumentTypes);
                DocStatusCollection = new ObservableCollection<DocumentState>(db.DocumentStates);
                OrgCollection = new ObservableCollection<Organization>(db.Organizations);
                ContactCollection = new ObservableCollection<ExternalContact>(db.ExternalContacts);
                CurrCollection = new ObservableCollection<Currency>(db.Currencies);

                if (DocStatus.DocStateName == "New")
                {
                    if (CreatedBy == CurrentUser) 
                    {
                        DocStatus.IsSelectable = true;
                        DocStatusCollection.Where(s => s.DocStateName == "In progress").Single().IsSelectable = true;
                        DocStatusCollection.Where(s => s.DocStateName == "Rejected").Single().IsSelectable = true;

                        AddNewFileContent = "Add new file";
                        AddFileContent = "Add";
                        roEditFile = false;
                        roBtnEditFile = true;
                        EditFileMode = false;
                    }
                    else
                    {

                    }
                }
                else
                {
                    if (DocStatus.DocStateName == "In progress")
                        if (ProcessCollection.Last().TaskUser == CurrentEmployee)
                        {
                            DocStatusCollection.Where(s => s.DocStateName == "In progress").Single().IsSelectable = true;
                        }
                }
                StateBeforeOpen = DocStatus;
                roDocType = true;
            }
        }


        private RelayCommand selectionChangedCommand;
        public RelayCommand SelectionChangedCommand => selectionChangedCommand ?? (selectionChangedCommand = new RelayCommand(
                () =>
                {
                    if (Org != null)
                        ContactCollection = new ObservableCollection<ExternalContact>(db.ExternalContacts.Where(c => c.Organization.Id == Org.Id)); 
                }
            ));

        private RelayCommand<ExternalContact> contact_SelectionChangedCommand;
        public RelayCommand<ExternalContact> Contact_SelectionChangedCommand => contact_SelectionChangedCommand ?? (contact_SelectionChangedCommand = new RelayCommand<ExternalContact>(
               param =>
                {
                    if (param != null)
                        Org = param.Organization;
                }
            ));

        private RelayCommand addNewFileCommand;
        public RelayCommand AddNewFileCommand => addNewFileCommand ?? (addNewFileCommand = new RelayCommand(
                async() =>
                {
                    if (IsNew == true)
                    {
                        var res = messageService.ShowYesNo("Save this document?");
                        if (res)
                        {
                            if (DocType == null)
                            {
                                messageService.ShowError("Document type can't be empty.");
                                return;
                            }

                            CurrentDocument.DocDate = DocData;
                            CurrentDocument.CreatedBy = CreatedBy;
                            CurrentDocument.Company = CreatedByEmp.Company;
                            CurrentDocument.Comment = DocComment;
                            CurrentDocument.DocumentType = DocType;
                            CurrentDocument.DocumentState = DocStatus;
                            CurrentDocument.DocInfoDate = DocDocumentDate;
                            CurrentDocument.Organization = Org;
                            CurrentDocument.InfoContact = Contact;
                            CurrentDocument.DocSum = DocSum;
                            CurrentDocument.DocCurrency = Currency;
                            CurrentDocument.DocInfoComment = DocInfoComment;
                           

                            var docCount = db.Documents.Count() + 1;
                            StringBuilder strNumber = new StringBuilder(docCount.ToString());

                            while (strNumber.Length < 6)
                            {
                                strNumber = strNumber.Insert(0,"0");
                            }

                            strNumber = strNumber.Insert(0, CurrentDocument.DocumentType.DocTypeAcc);
                            DocNumber = strNumber.ToString();
                            CurrentDocument.DocNumber = DocNumber;

                            db.Documents.Add(CurrentDocument);
                            await db.SaveChangesAsync();
                            IsNew = false;
                            roDocType = true;
                            DocStatusCollection.Where(s => s.DocStateName == "In progress").Single().IsSelectable = true;
                            DocStatusCollection.Where(s => s.DocStateName == "Rejected").Single().IsSelectable = true;
                        }
                        else
                        {
                            messageService.ShowInfo("You must save document before add files.");
                            return;
                        }
                    }

                    if (EditFileMode == false)
                    {
                        if (!string.IsNullOrEmpty(FileName) && !string.IsNullOrEmpty(AddNewFilePath))
                        {
                            //var icon = Icon.ExtractAssociatedIcon(AddNewFilePath);
                            //FileIcon = Bitmap.FromHicon(icon.Handle);


                            // (!) Copy new file to fileserver
                            var newPath = db.Constants.FirstOrDefault().DocPath +"\\" + Guid.NewGuid().ToString() + Path.GetExtension(AddNewFilePath);

                            try
                            {
                                File.Copy(AddNewFilePath, newPath, false);
                            }
                            catch (Exception ex)
                            {
                                messageService.ShowError(ex.Message);
                                throw;
                            }

                            var newFile = new MyFile
                            {
                                FileName = FileName,
                                FileUri = newPath,
                                Doc = CurrentDocument,
                                FileComment = FileComment
                            };

                            CurrentDocument.myFiles.Add(newFile);
                            DocFiles.Add(newFile);

                            await db.SaveChangesAsync();

                            FileName = "";
                            AddNewFilePath = "";
                            FileComment = "";

                        }
                        else
                        {
                            messageService.ShowError("You should select a file and fill FileName field");
                        }
                    }
                    else
                    {
                        CurrentFile.FileComment = FileComment;
                        await db.SaveChangesAsync();
                        CurrentFile = null;
                        EditFileMode = false;

                        FileName = "";
                        AddNewFilePath = "";
                        FileComment = "";
                        AddNewFileContent = "Add new file";
                        AddFileContent = "Add";
                        roEditFile = false;
                        roBtnEditFile = true;
                    }
                    
                }
            ));

        private RelayCommand<MyFile> editFileCommand;
        public RelayCommand<MyFile> EditFileCommand => editFileCommand ?? (editFileCommand = new RelayCommand<MyFile>(
               param =>
               {
                   CurrentFile = param;
                   EditFileMode = true;

                   AddNewFileContent = "Edit file comment";
                   AddFileContent = "Save";
                   roEditFile = true;
                   roBtnEditFile = false;
                   FileName = param.FileName;
                   AddNewFilePath = "File server";
                   FileComment = param.FileComment;
               }
            ));

        private RelayCommand<MyFile> viewFileCommand;
        public RelayCommand<MyFile> ViewFileCommand => viewFileCommand ?? (viewFileCommand = new RelayCommand<MyFile>(
               param =>
               {
                   var path = param.FileUri;
                   var ext = Path.GetExtension(path);
                   var tempPath = Path.GetTempPath() + param.FileName + ext;

                   try
                   {
                       File.Copy(path, tempPath, true);
                   }
                   catch (Exception ex)
                   {
                       messageService.ShowError(ex.Message);
                       throw;
                   }
                   

                   Process.Start(tempPath);
               }
            ));

        private RelayCommand newFilePathCommand;
        public RelayCommand NewFilePathCommand
        {
            get => newFilePathCommand ?? (newFilePathCommand = new RelayCommand(
                () =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "All files (*.*)|*.*";
                    string Link = null;

                    if (openFileDialog.ShowDialog() == true)
                    {
                        Link = openFileDialog.FileName;
                    }

                    if (!string.IsNullOrEmpty(Link))
                    {
                        AddNewFilePath = Link;
                    }
                }
            ));
        }

        private RelayCommand backCommand;
        public RelayCommand BackCommand => backCommand ?? (backCommand = new RelayCommand(
                () =>
                {
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    navigationService.Navigate<DocumentsPageView>();
                }
            ));

        private RelayCommand okCommand;
        public RelayCommand OkCommand => okCommand ?? (okCommand = new RelayCommand(
                async() =>
                {
                    if (IsNew == true)
                    {
                        if (DocType == null)
                        {
                            messageService.ShowError("Document type can't be empty.");
                            return;
                        }

                        CurrentDocument.DocDate = DocData;
                        CurrentDocument.CreatedBy = CreatedBy;
                        CurrentDocument.Company = CreatedByEmp.Company;
                        CurrentDocument.Comment = DocComment;
                        CurrentDocument.DocumentType = DocType;
                        CurrentDocument.DocumentState = DocStatus;
                        CurrentDocument.DocInfoDate = DocDocumentDate;
                        CurrentDocument.Organization = Org;
                        CurrentDocument.InfoContact = Contact;
                        CurrentDocument.DocSum = DocSum;
                        CurrentDocument.DocCurrency = Currency;
                        CurrentDocument.DocInfoComment = DocInfoComment;


                        var docCount = db.Documents.Count() + 1;
                        StringBuilder strNumber = new StringBuilder(docCount.ToString());

                        while (strNumber.Length < 6)
                        {
                            strNumber = strNumber.Insert(0, "0");
                        }

                        strNumber = strNumber.Insert(0, CurrentDocument.DocumentType.DocTypeAcc);
                        DocNumber = strNumber.ToString();
                        CurrentDocument.DocNumber = DocNumber;

                        db.Documents.Add(CurrentDocument);
                        await db.SaveChangesAsync();
                        IsNew = false;
                        roDocType = true;
                        DocStatusCollection.Where(s => s.DocStateName == "In progress").Single().IsSelectable = true;
                        DocStatusCollection.Where(s => s.DocStateName == "Rejected").Single().IsSelectable = true;
                    }
                    else
                    {
                        // if State changes
                        if(StateBeforeOpen != DocStatus)
                        {
                            var InProgress = DocStatusCollection.Where(s => s.DocStateName == "In progress").Single();
                            var done = DocStatusCollection.Where(s => s.DocStateName == "Done").Single();
                            if (DocStatus == InProgress)
                            {
                                // Check all fields is not null
                                // at least present one file
                                // at least one direction
                                var ErrorMsg = new StringBuilder("This fields in document can't be empty:\n");
                                var ok = true;
                                if(string.IsNullOrEmpty(DocDocumentDate.ToString()))
                                {
                                    ok = false;
                                    ErrorMsg.Append("Date in short info about document\n");
                                }
                                if (Org == null)
                                {
                                    ok = false;
                                    ErrorMsg.Append("Organization\n");
                                }
                                if (Contact == null)
                                {
                                    ok = false;
                                    ErrorMsg.Append("Contact\n");
                                }
                                if (DocSum == 0.0m)
                                {
                                    ok = false;
                                    ErrorMsg.Append("Sum\n");
                                }
                                if (Currency == null)
                                {
                                    ok = false;
                                    ErrorMsg.Append("Currency\n");
                                }
                                if (ok)
                                    ErrorMsg.Clear();

                                if(DocFiles.Count() == 0)
                                {
                                    ok = false;
                                    ErrorMsg.Append("You didn't attach any file!\n");
                                }

                                if (ProcessCollection.Count() == 0)
                                {
                                    ok = false;
                                    ErrorMsg.Append("You didn't start business process!\n");
                                }

                                if (ok == false)
                                {
                                    ErrorMsg.Append("Changes not saved.");
                                    messageService.ShowError(ErrorMsg.ToString());
                                    return;
                                }

                            }
                            else if(DocStatus == done)
                            {

                            }
                        }
                        
                        CurrentDocument.Comment = DocComment;
                        
                        CurrentDocument.DocumentState = DocStatus;
                        CurrentDocument.DocInfoDate = DocDocumentDate;
                        CurrentDocument.Organization = Org;
                        CurrentDocument.InfoContact = Contact;
                        CurrentDocument.DocSum = DocSum;
                        CurrentDocument.DocCurrency = Currency;
                        CurrentDocument.DocInfoComment = DocInfoComment;

                        await db.SaveChangesAsync();

                       
                    }
                    // save history
                    var newHistoryItem = new History
                    {
                        EditionData = DateTime.Now,
                        WhoEdited = CurrentUser.GoogleAccount,
                        ClassName = "Document",
                        ObjectId = CurrentDocument.Id,
                        ObjectJson = JsonConvert.SerializeObject(CurrentDocument, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        })
                    };

                    db.Histories.Add(newHistoryItem);
                    await db.SaveChangesAsync();

                    //close page
                    Messenger.Default.Send(new NotificationMessage<User>(CurrentUser, "SendCurrentUser"));
                    navigationService.Navigate<DocumentsPageView>();
                }
            ));

        private RelayCommand addNewProcessCommand;
        public RelayCommand AddNewProcessCommand => addNewProcessCommand ?? (addNewProcessCommand = new RelayCommand(
                () =>
                {
                    var win = new AddEditProcessWindow(null, new List<Department>(db.Departments), 
                        new List<Employee>(db.Employees), 
                        new List<DocumentState>(db.DocumentStates));

                    win.ShowDialog();
                    if (win.DataContext is AddEditProcessViewModel)
                    {
                        var dc = win.DataContext as AddEditProcessViewModel;
                        if(dc.Process != null)
                        {
                            dc.Process.StartDate = DateTime.Now;
                            dc.Process.StartUser = CurrentUser;
                            dc.Process.StateDate = DateTime.Now;
                            dc.Process.Doc = CurrentDocument;
                            
                            ProcessCollection.Add(dc.Process);
                            db.MyProcesses.Add(dc.Process);
                        }
                    }
                }
            ));

        private RelayCommand<TaskProcess> doubleClickCommand;
        public RelayCommand<TaskProcess> DoubleClickCommand => doubleClickCommand ?? (doubleClickCommand = new RelayCommand<TaskProcess>(
        param =>
        {
            if(param.State.DocStateName == "Done")
            {
                // Open read only window
                var win = new AddEditProcessWindow(param, null, null, null);
                
                win.ShowDialog();
            }
            else
            {
                if(param.StartUser == CurrentUser)
                {
                    if (param.State.DocStateName == "New")
                    {
                        // full edit exp. state
                        var win = new AddEditProcessWindow(param, new List<Department>(db.Departments),
                        new List<Employee>(db.Employees),
                        null);

                        win.ShowDialog();
                    }
                   
                }
                if(param.TaskUser == null)
                {
                    if (param.Department == CurrentEmployee.Department && CurrentEmployee.HeadOfDep == true)
                    {
                        // edit resp. person only
                        var win = new AddEditProcessWindow(param, null,
                        new List<Employee>(db.Employees),
                        null);

                        win.ShowDialog();
                    }
                }
                if (param.TaskUser == CurrentEmployee)
                {
                    //edit state only
                    var win = new AddEditProcessWindow(param, null,
                        null,
                        new List<DocumentState>(db.DocumentStates));

                    win.ShowDialog();
                }
            }
        }));

    }
}
